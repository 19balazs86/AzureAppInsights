namespace AppInsightsForWorkerService;

public static class Program
{
    public const string ClientName = "jsonplaceholder";

    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        bool isDevelopment = builder.Environment.IsDevelopment();

        var services = builder.Services;

        // Add services to the container
        {
            services.AddHostedService<Worker>();

            services.AddApplicationInsightsTelemetryWorkerService(options => options.DeveloperMode = isDevelopment);

            services.AddHttpClient(ClientName, client => client.BaseAddress = new Uri("http://jsonplaceholder.typicode.com"));
        }

        builder.Build().Run();
    }
}
