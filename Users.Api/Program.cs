using Common.Repositories.Context;
using FluentValidation.AspNetCore;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;
using Users.Service.DI;
using System.Text.Json.Serialization;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console(new RenderedCompactJsonFormatter(), LogEventLevel.Information)
    .WriteTo.File("Logs/Log-Error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application");


    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.InitializeRepositories();
    builder.Services.InitializeServices();
    builder.Services.AddAutoMapperService();
    builder.Services.AddValidationService();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddTodosDatabase(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

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