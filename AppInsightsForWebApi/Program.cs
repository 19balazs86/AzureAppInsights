using AppInsightsForWebApi.Initializers;
using AppInsightsForWebApi.ServiceAndRepo;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using System.Security.Claims;

namespace AppInsightsForWebApi;

public static class Program
{
    public const string ClientName = "jsonplaceholder";

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        bool isDevelopment = builder.Environment.IsDevelopment();

        // Add services to the container
        {
            applyApplicationInsights(services, isDevelopment);

            services.AddControllers();

            services.AddSingleton<TestService>();
            services.AddSingleton<TestRepository>();

            services.AddHttpContextAccessor();

            services.AddHttpClient(ClientName, client => client.BaseAddress = new Uri("http://jsonplaceholder.typicode.com"));
        }

        WebApplication app = builder.Build();

        // Configure the request pipeline
        {
            if (isDevelopment)
            {
                // var telemetryConfiguration = app.Services.GetRequiredService<IOptions<TelemetryConfiguration>>().Value;
                // telemetryConfiguration.DisableTelemetry = true;

                app.UseDeveloperExceptionPage();
            }

            app.Use(injectFakeUser);

            app.UseAuthorization();

            app.MapVersionEndpoint("/version"); // .RequireAuthorization()

            app.MapControllers();

            app.MapFallback(pageNotFoundHandler);
        }

        app.Run();
    }

    private static void applyApplicationInsights(IServiceCollection services, bool isDevelopment)
    {
        services.AddApplicationInsightsTelemetry(options => options.DeveloperMode = isDevelopment);

        // https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#configuring-or-removing-default-telemetrymodules
        if (isDevelopment)
        {
            Type[] moduleTypes = [typeof(PerformanceCollectorModule), typeof(EventCounterCollectionModule)];

            foreach (var module in services.Where(t => moduleTypes.Contains(t.ImplementationType)).ToArray())
                services.Remove(module);
        }

        // --> Add: TelemetryInitializers
        services.AddSingleton<ITelemetryInitializer, SetUserIdTelemetryInitializer>();
        services.AddSingleton<ITelemetryInitializer, NotFoundTelemetryInitializer>();
        services.AddSingleton<ITelemetryInitializer, ExceptionTelemetryInitializer>();
    }

    private static async Task pageNotFoundHandler(HttpContext context)
    {
        context.Items.Add(NotFoundTelemetryInitializer.NotFoundKey, true); // Use it in the NotFoundTelemetryInitializer
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("The requested endpoint is not found.");
    }

    // For every request, configure a fake user
    private static Task injectFakeUser(HttpContext httpContext, Func<Task> next)
    {
        int userId = Random.Shared.Next(1, 6);

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name,           $"User#{userId}")
        ];

        var claimsIdentity = new ClaimsIdentity(claims, "FakeAuthType");

        httpContext.User = new ClaimsPrincipal(claimsIdentity);

        return next.Invoke();
    }
}
