using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace AppInsightsForWebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly Random _random = new Random();

    private readonly TelemetryClient _telemetryClient;

    public WeatherForecastController(TelemetryClient telemetryClient)
    {
      _telemetryClient = telemetryClient;
    }

    [HttpGet("GimmeMore")]
    public async Task<IEnumerable<WeatherForecast>> GimmeMore([FromQuery] int randomInt)
    {
      _telemetryClient.TrackEvent(createEvenOddEventTelemetry(randomInt));

      await Task.Delay(_random.Next(0, 1000));

      return Enumerable.Range(1, 5).Select(_ => WeatherForecast.Create(randomInt));
    }

    [HttpGet("GimmeOne")]
    public async Task<WeatherForecast> GimmeOne([FromQuery] int randomInt)
    {
      _telemetryClient.TrackEvent(createEvenOddEventTelemetry(randomInt));

      await Task.Delay(_random.Next(0, 1000));

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

  public class WeatherForecast
  {
    private static readonly Random _random = new Random();

    private static readonly string[] _summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public int RandomInt { get; set; }
    public DateTime Date { get; set; }
    public int Temperature { get; set; }
    public string Summary { get; set; }

    public static WeatherForecast Create(int randomIntParam) => new WeatherForecast
    {
      RandomInt   = randomIntParam,
      Date        = DateTime.Now.AddDays(randomIntParam),
      Temperature = _random.Next(-20, 55),
      Summary     = _summaries[_random.Next(_summaries.Length)]
    };
  }
}
