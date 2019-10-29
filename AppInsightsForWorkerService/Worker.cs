using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppInsightsForWorkerService
{
  public class Worker : BackgroundService
  {
    private static readonly Random _random = new Random();

    private int _counter = 0;

    private readonly ILogger<Worker> _logger;
    private readonly TelemetryClient _telemetryClient;
    private readonly IHttpClientFactory _clientFactory;

    public Worker(ILogger<Worker> logger, TelemetryClient telemetryClient, IHttpClientFactory clientFactory)
    {
      _logger          = logger;
      _telemetryClient = telemetryClient;
      _clientFactory   = clientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      HttpClient httpClient = _clientFactory.CreateClient(Program.ClientName);

      while (!stoppingToken.IsCancellationRequested)
      {
        using IOperationHolder<RequestTelemetry> operation =
          _telemetryClient.StartOperation<RequestTelemetry>("WorkerOperation");

        _logger.LogInformation("Worker({counter}) running at: {time}", ++_counter, DateTime.Now);

        operation.Telemetry.Properties.Add("Counter", _counter.ToString());

        await Task.Delay(_random.Next(500, 1500), stoppingToken);

        try
        {
          if (_random.NextDouble() < 0.15)
            throw new Exception("My test Exception");

          if (_random.NextDouble() < 0.3)
            await callJsonPlaceholder(httpClient);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Something went wrong.");

          operation.Telemetry.Success      = false;
          operation.Telemetry.ResponseCode = "500";
        }
      }
    }

    private async Task callJsonPlaceholder(HttpClient httpClient)
    {
      _logger.LogInformation("CallJsonPlaceholder");

      using HttpResponseMessage response = await httpClient.GetAsync("albums");
    }
  }
}
