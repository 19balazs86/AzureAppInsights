using System;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsForWebApi.Initializers;
using AppInsightsForWebApi.Model;
using AppInsightsForWebApi.Repository;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppInsightsForWebApi
{
  public class Startup
  {
    private readonly IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
     if (_environment.IsDevelopment())
        services.AddSingleton<ITelemetryChannel>(new InMemoryChannel { DeveloperMode = true });

      services.AddApplicationInsightsTelemetry();

      // Remove PerformanceCollectorModule and EventCounterCollectionModule in Development.
      // https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#configuring-or-removing-default-telemetrymodules
      if (_environment.IsDevelopment())
      {
        Type[] moduleTypes = new Type[] { typeof(PerformanceCollectorModule), typeof(EventCounterCollectionModule) };

        foreach (var module in services.Where(t => moduleTypes.Contains(t.ImplementationType)).ToArray())
          services.Remove(module);
      }

      services.AddSingleton<ITelemetryInitializer, SetUserIdTelemetryInitializer>();
      services.AddSingleton<ITelemetryInitializer, NotFoundTelemetryInitializer>();
      services.AddSingleton<ITelemetryInitializer, ExceptionTelemetryInitializer>();

      services.AddHttpContextAccessor();

      services.AddControllers();

      services.AddSingleton<LoggerModel>();
      services.AddSingleton<LoggerRepository>();
    }

    public void Configure(IApplicationBuilder app, TelemetryConfiguration telemetryConfiguration)
    {
      if (_environment.IsDevelopment())
      {
        //telemetryConfiguration.DisableTelemetry = true;

        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();

        endpoints.MapFallback(pageNotFoundHandler);
      });
    }

    private static async Task pageNotFoundHandler(HttpContext context)
    {
      context.Items.Add(NotFoundTelemetryInitializer.NotFoundKey, true); // Use it in the NotFoundTelemetryInitializer.
      context.Response.StatusCode = 404;
      await context.Response.WriteAsync("The requested endpoint is not found.");
    }

  }
}
