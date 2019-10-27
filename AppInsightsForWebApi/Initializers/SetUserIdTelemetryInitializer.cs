﻿using System.Security.Claims;
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
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User;

        if (user is null || !user.Identity.IsAuthenticated) return;

        telemetry.Context.User.Id = user.FindFirst(ClaimTypes.NameIdentifier).Value;

        // https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-send-user-context
        // https://eriksteinebach.com/2018/05/06/specific-user-application-insights-netcore
        // https://hajekj.net/2017/03/13/tracking-currently-signed-in-user-in-application-insights
      }
    }
  }
}
