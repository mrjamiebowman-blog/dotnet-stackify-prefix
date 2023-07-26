using Microsoft.AspNetCore.Mvc;
using MrJB.OTel.Prefix.Data.Models;
using MrJB.OTel.Prefix.Data.Services;
using OpenTelemetry.Trace;

namespace MrJb.OTel.Prefix.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    // logging
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly Tracer _tracer;

    // services
    private readonly IDataService _dataService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Tracer tracer, IDataService dataService)
    {
        // logging
        _logger = logger;
        _tracer = tracer;

        // services
        _dataService = dataService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        // start telemetry
        using var scope = _tracer.StartActiveSpan("WeatherForecastController.Get");

        // data
        var data = await _dataService.GetDataAsync();

        // end telemetry
        scope.End();

        return data;
    }
}