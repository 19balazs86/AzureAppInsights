using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace AppInsightsForWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public WeatherForecastController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet("GimmeMore")]
    public async Task<IEnumerable<WeatherForecast>> GimmeMore([FromQuery] int randomInt)
    {
        _telemetryClient.TrackEvent(createEvenOddEventTelemetry(randomInt));

        await Task.Delay(Random.Shared.Next(0, 1000));

        return Enumerable.Range(1, 5).Select(_ => WeatherForecast.Create(randomInt));
    }

    [HttpGet("GimmeOne")]
    public async Task<WeatherForecast> GimmeOne([FromQuery] int randomInt)
    {
        _telemetryClient.TrackEvent(createEvenOddEventTelemetry(randomInt));

        await Task.Delay(Random.Shared.Next(0, 1000));

        return WeatherForecast.Create(randomInt);
    }

    private static EventTelemetry createEvenOddEventTelemetry(int randomInt)
    {
        string name = $"RandomInt" + (randomInt % 2 == 0 ? "Even" : "Odd");

        var eventTelemetry = new EventTelemetry(name);

        eventTelemetry.Properties.Add("RandomIntProp", randomInt.ToString());

        eventTelemetry.Metrics.Add("RandomIntMetrics", randomInt);

        return eventTelemetry;
    }
}

public sealed class WeatherForecast
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public int RandomInt { get; init; }
    public DateTime Date { get; init; }
    public int Temperature { get; init; }
    public string? Summary { get; init; }

    public static WeatherForecast Create(int randomIntParam)
    {
        return new WeatherForecast
        {
            RandomInt   = randomIntParam,
            Date        = DateTime.Now.AddDays(randomIntParam),
            Temperature = Random.Shared.Next(-20, 55),
            Summary     = _summaries[Random.Shared.Next(_summaries.Length)]
        };
    }
}
