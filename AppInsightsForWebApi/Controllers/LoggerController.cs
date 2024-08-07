﻿using AppInsightsForWebApi.ServiceAndRepo;
using Microsoft.AspNetCore.Mvc;

namespace AppInsightsForWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class LoggerController : ControllerBase
{
    private readonly TestService _testService;

    private readonly ILogger<LoggerController> _logger;

    public LoggerController(TestService testService, ILogger<LoggerController> logger)
    {
        _testService = testService;
        _logger      = logger;
    }

    [HttpGet("DoSomeLog")]
    public async Task<IActionResult> DoSomeLog()
    {
        await Task.Delay(Random.Shared.Next(0, 1000));

        _logger.LogTrace("Trace log level message.");
        _logger.LogDebug("Debug log level message.");
        _logger.LogInformation("Information log level message.");
        _logger.LogWarning("Warning log level message.");

        try
        {
            _testService.CallRepositoryInModel();
        }
        catch (TestServiceException ex)
        {
            _logger.LogError(ex, "Failed to do something. {MyParams}", new { Param1 = 1, Param2 = "P2" });

            return new StatusCodeResult(500);
        }

        return Ok();
    }
}
