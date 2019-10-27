using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace AppInsightsForWebApi.Initializers
{
  public class NotFoundTelemetryInitializer : ITelemetryInitializer
  {
    public const string NotFoundKey = "NotFound";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotFoundTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
      if (!(telemetry is RequestTelemetry requestTelemetry)) return;

      if (_httpContextAccessor.HttpContext.Items.ContainsKey(NotFoundKey))
        return; // If the NotFoundKey is present do not override the Success field.

      if (int.TryParse(requestTelemetry.ResponseCode, out int code) && code >= 400 && code < 500)
      {
        requestTelemetry.Success = true;

        // Allow us to filter these requests in the portal.
        requestTelemetry.Properties["Overridden400"] = "true";
      }
    }
  }
}
