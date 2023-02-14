using System.Reflection;

namespace AppInsightsForWebApi;

// https://andrewlock.net/converting-a-terminal-middleware-to-endpoint-routing-in-aspnetcore-3

public static class VersionEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapVersionEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
    {
        RequestDelegate pipeline = endpoints
            .CreateApplicationBuilder()
            .UseMiddleware<VersionMiddleware>()
            .Build();

        return endpoints
            .Map(pattern, pipeline)
            .WithDisplayName("Version number");
    }
}

public sealed class VersionMiddleware
{
    private readonly RequestDelegate _next;

    //private static readonly string _version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;
    private static readonly string _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public VersionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        context.Response.StatusCode = 200;

        await context.Response.WriteAsync(_version);

        // Do not invoke next middleware!
    }
}
