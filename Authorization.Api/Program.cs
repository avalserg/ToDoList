using System.Text;
using Common.Repositories.Context;
using FluentValidation.AspNetCore;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;
using System.Text.Json.Serialization;
using Authorization.Service.DI;
using Common.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console(new RenderedCompactJsonFormatter(), LogEventLevel.Information)
    .WriteTo.File("Logs/Log-Error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application");


    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                ValidAudience = builder.Configuration["JwtOptions:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]!))

            };
        });
    builder.Services.AddTransient<ExceptionsHandlerMiddleware>();
    builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },

                new List<string>{}
            }
        });
    });
    builder.Services.InitializeRepositories();
    builder.Services.InitializeServices();
    //builder.Services.AddAutoMapperService();
    builder.Services.AddValidationService();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddTodosDatabase(builder.Configuration);



    builder.Host.UseSerilog();
    var app = builder.Build();
    app.UseExceptionsHandler();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}