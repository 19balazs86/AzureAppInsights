using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppInsightsForWorkerService
{
  public class Program
  {
    public const string ClientName = "jsonplaceholder";

    public static void Main(string[] args)
    {
      createHostBuilder(args).Build().Run();
    }

    private static IHostBuilder createHostBuilder(string[] args)
    {
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging);
    }

    private static void configureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
      if (hostContext.HostingEnvironment.IsDevelopment())
        services.AddSingleton<ITelemetryChannel>(new InMemoryChannel { DeveloperMode = true });

      services.AddHostedService<Worker>();

      services.AddApplicationInsightsTelemetryWorkerService();

      services.AddHttpClient(ClientName, client =>
        client.BaseAddress = new Uri("http://jsonplaceholder.typicode.com"));
    }

    private static void configureLogging(HostBuilderContext hostContext, ILoggingBuilder logging)
    {
      if (!hostContext.HostingEnvironment.IsDevelopment())
        logging.ClearProviders();
    }
  }
}
