using AppInsightsForWebApi.Initializers;
using AppInsightsForWebApi.ModelAndRepo;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using System.Security.Claims;

namespace AppInsightsForWebApi;

public class Startup
{
    public const string ClientName = "jsonplaceholder";

    private readonly bool _isDevelopment;

    public Startup(IWebHostEnvironment environment)
    {
        _isDevelopment = environment.IsDevelopment();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_isDevelopment)
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel { DeveloperMode = true });

        services.AddApplicationInsightsTelemetry();

        // https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#configuring-or-removing-default-telemetrymodules
        if (_isDevelopment)
        {
            Type[] moduleTypes = new Type[] { typeof(PerformanceCollectorModule), typeof(EventCounterCollectionModule) };

            foreach (var module in services.Where(t => moduleTypes.Contains(t.ImplementationType)).ToArray())
                services.Remove(module);
        }

        // Add TelemetryInitializers
        services.AddSingleton<ITelemetryInitializer, SetUserIdTelemetryInitializer>();
        services.AddSingleton<ITelemetryInitializer, NotFoundTelemetryInitializer>();
        services.AddSingleton<ITelemetryInitializer, ExceptionTelemetryInitializer>();

        services.AddHttpContextAccessor();

        services.AddControllers();

        services.AddSingleton<LoggerModel>();
        services.AddSingleton<LoggerRepository>();

        services.AddHttpClient(ClientName, client => client.BaseAddress = new Uri("http://jsonplaceholder.typicode.com"));
    }

    public void Configure(IApplicationBuilder app, TelemetryConfiguration telemetryConfiguration)
    {
        if (_isDevelopment)
        {
            //telemetryConfiguration.DisableTelemetry = true;

            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.Use(injectFakeUser);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapVersionEndpoint("/version"); // .RequireAuthorization() .RequireCors("AllowAllHosts");

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

    private static Task injectFakeUser(HttpContext httpContext, Func<Task> next)
    {
        int userId = Random.Shared.Next(1, 6);

        IEnumerable<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, $"User#{userId}")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "FakeAuthType");

        httpContext.User = new ClaimsPrincipal(claimsIdentity);

        return next.Invoke();
    }
}
