using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace MrJb.OTel.Prefix.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly Tracer _tracer;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Tracer tracer)
    {
        // logging
        _logger = logger;

        // tracer
        _tracer = tracer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        using var scope = _tracer.StartActiveSpan("WeatherForecastController.Get");
        
        // Your logic here
        var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        scope.End();

        return data;
    }
}