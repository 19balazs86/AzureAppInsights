using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Security.Claims;

namespace AppInsightsForWebApi.Initializers;

// Difference between telemetry processors and telemetry initializers
// https://docs.microsoft.com/en-ie/azure/azure-monitor/app/api-filtering-sampling#itelemetryprocessor-and-itelemetryinitializer
public sealed class SetUserIdTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SetUserIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is TraceTelemetry ||
            telemetry is RequestTelemetry ||
            telemetry is ExceptionTelemetry ||
            telemetry is DependencyTelemetry)
        {
            Claim? userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is not null)
            {
                telemetry.Context.User.Id = userIdClaim.Value;
            }

            // https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-send-user-context
            // https://eriksteinebach.com/2018/05/06/specific-user-application-insights-netcore
            // https://hajekj.net/2017/03/13/tracking-currently-signed-in-user-in-application-insights
        }
    }
}
