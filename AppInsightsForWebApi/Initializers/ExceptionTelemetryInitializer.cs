using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsForWebApi.Initializers
{
  public class ExceptionTelemetryInitializer : ITelemetryInitializer
  {
    public void Initialize(ITelemetry telemetry)
    {
      if (!(telemetry is ExceptionTelemetry exceptionTelemetry)) return;

      var dataDictionary = new Dictionary<string, string>();

      destructuringData(exceptionTelemetry.Exception, dataDictionary);

      // These fields will be found in the customDimensions field in AppInsights.
      foreach (var item in dataDictionary)
        exceptionTelemetry.Properties.Add(item.Key, item.Value);
    }

    private static void destructuringData(Exception exception, Dictionary<string, string> dataDictionary)
    {
      if (exception is null) return;

      IDictionary data = exception.Data;

      if (data != null && data.Count > 0)
      {
        Dictionary<string, string> dataDic = data
          .Cast<DictionaryEntry>()
          .Where(x => x.Key is string && x.Value != null)
          .ToDictionary(x => x.Key as string, x => x.Value.ToString());

        foreach (var item in dataDic)
          dataDictionary.TryAdd(item.Key, item.Value);
      }

      destructuringData(exception.InnerException, dataDictionary);
    }
  }
}
