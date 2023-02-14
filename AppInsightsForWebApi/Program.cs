namespace AppInsightsForWebApi;

public sealed class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder
                    .ConfigureLogging(configureLogging)
                    .UseStartup<Startup>());
    }

    private static void configureLogging(WebHostBuilderContext hostContext, ILoggingBuilder logging)
    {
        if (!hostContext.HostingEnvironment.IsDevelopment())
            logging.ClearProviders();

        // ApplicationInsightsLoggerProvider
        //Guid instrumentationKey;

        //if (!Guid.TryParse(hostContext.Configuration["ApplicationInsights:InstrumentationKey"], out instrumentationKey))
        //  if (!Guid.TryParse(hostContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"], out instrumentationKey))
        //    throw new NullReferenceException("ApplicationInsights InstrumentationKey is missing from the Configuration.");

        //logging.AddApplicationInsights(instrumentationKey.ToString());
    }
}
