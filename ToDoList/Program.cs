using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Todos.Service.DI;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console(new RenderedCompactJsonFormatter(),LogEventLevel.Information)
    .WriteTo.File("Logs/Log-Error-.txt",LogEventLevel.Error,rollingInterval:RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.InitializeRepositories();
    builder.Services.InitializeServices();
    builder.Services.AddAutoMapperService();
    builder.Services.AddValidationService();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Host.UseSerilog();

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

    app.UseSerilogRequestLogging();

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
