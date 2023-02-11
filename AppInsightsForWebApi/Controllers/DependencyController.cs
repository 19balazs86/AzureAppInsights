﻿using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AppInsightsForWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DependencyController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public DependencyController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet("CallJsonPlaceholder")]
    public async Task CallJsonPlaceholder()
    {
        // Add an extra property to the request telemetry.
        var requestTelemetry = HttpContext.Features.Get<RequestTelemetry>();

        if (requestTelemetry != null)
            requestTelemetry.Properties["ExtraProperty"] = "Extra value";

        HttpClient httpClient = _clientFactory.CreateClient(Startup.ClientName);

        Response.ContentType = MediaTypeNames.Application.Json;

        // await using will call the IAsyncDisposable at the end of the method.
        await using Stream stream = await httpClient.GetStreamAsync("albums");

        await stream.CopyToAsync(Response.Body);
    }
}
