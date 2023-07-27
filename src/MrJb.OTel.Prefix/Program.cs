using MrJB.OTel.Prefix.Data;
using MrJB.OTel.Prefix.OTel;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics;

var resource = ResourceBuilder
    .CreateDefault()
    .AddService(OTel.ServiceName)
    .AddTelemetrySdk()
    .AddEnvironmentVariableDetector();

// stackify
StackifyLib.Utils.StackifyAPILogger.LogEnabled = true;
StackifyLib.Utils.StackifyAPILogger.OnLogMessage += StackifyAPILogger_OnLogMessage;

static void StackifyAPILogger_OnLogMessage(string data)
{
    Debug.WriteLine(data);
}

// logging
using var log = new LoggerConfiguration()
    .WriteTo.OpenTelemetry() // open telemetry
    .WriteTo.Console() // serilog
    //.WriteTo.Stackify() // stackify (Serilog Sink)
    .CreateLogger();
Log.Logger = log;

var builder = WebApplication.CreateBuilder(args);

// logging
builder.Host.UseSerilog((ctx, lc) =>
{
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// open telemetry
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(OTel.ServiceName));

// data service
builder.Services.AddCustomDataService();

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

// service provider
var serviceProvider = builder.Services.BuildServiceProvider();

// open telemetry
using (var provider = Sdk.CreateTracerProviderBuilder()
           .AddAppTracing(serviceProvider)
           .Build())
{
    using (OTel.Application.StartActivity("Seed Db"))
    {
        try
        {
            // seed db
            Log.Information("Seed Db");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to Seed Db");
            throw;
        }
    };

    app.Run();

    provider?.ForceFlush();
}
