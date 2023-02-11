using Microsoft.AspNetCore.Mvc;

namespace AppInsightsForWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class NotFoundController : ControllerBase
{
    [HttpGet("NotFoundResponse")]
    public IActionResult NotFoundResponse() => NotFound("The controller return with NotFound.");
}
