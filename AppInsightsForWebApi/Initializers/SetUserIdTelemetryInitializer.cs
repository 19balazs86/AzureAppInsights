using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace AppInsightsForWebApi.Initializers
{
  // Difference between telemetry processors and telemetry initializers
  // https://docs.microsoft.com/en-ie/azure/azure-monitor/app/api-filtering-sampling#itelemetryprocessor-and-itelemetryinitializer
  public class SetUserIdTelemetryInitializer : ITelemetryInitializer
  {
    private readonly Random _random = new Random();

    private readonly IHttpContextAccessor _httpContextAccessor;

    public SetUserIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
      if (telemetry is TraceTelemetry     ||
          telemetry is RequestTelemetry   ||
          telemetry is ExceptionTelemetry ||
          telemetry is DependencyTelemetry)
      {
        //string userName = _httpContextAccessor.HttpContext?.User.Identity.Name;

        //if (string.IsNullOrWhiteSpace(userName)) return;

        //telemetry.Context.User.Id = userName;

        telemetry.Context.User.Id = $"User#{_random.Next(1, 6)}";
      }
    }
  }
}
