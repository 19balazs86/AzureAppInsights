using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections;

namespace AppInsightsForWebApi.Initializers;

public sealed class ExceptionTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not ExceptionTelemetry exceptionTelemetry) return;

        // These fields will be found in the customDimensions field in AppInsights.
        destructuringData(exceptionTelemetry.Exception, exceptionTelemetry.Properties);
    }

    private static void destructuringData(Exception? exception, IDictionary<string, string> telemetryProperties)
    {
        if (exception is null) return;

        IDictionary data = exception.Data;

        if (data is { Count: > 0 })
        {
            foreach ((object key, object? value) in data.Cast<DictionaryEntry>())
            {
                if (key is string keyString)
                {
                    telemetryProperties!.TryAdd(keyString, value?.ToString() ?? string.Empty);
                }
            }
        }

        destructuringData(exception.InnerException, telemetryProperties);
    }
}
