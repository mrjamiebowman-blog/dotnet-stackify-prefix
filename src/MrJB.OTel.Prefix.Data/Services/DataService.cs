using Microsoft.Extensions.Logging;
using MrJB.OTel.Prefix.Data.Models;
using OpenTelemetry.Trace;

namespace MrJB.OTel.Prefix.Data.Services;

public class DataService : IDataService 
{
    private readonly Tracer _tracer;
    private readonly ILogger<IDataService> _logger;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public DataService(ILogger<IDataService> logger, Tracer tracer)
    {
        _logger = logger;
        _tracer = tracer;
    }

    public Task<WeatherForecast[]> GetDataAsync()
    {
        using var scope = _tracer.StartActiveSpan("DataService.GetDataAsync");

        var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        scope.End();

        return Task.FromResult(data);
    }
}