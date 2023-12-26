using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public class GetCurrentProcessIDController : ControllerBase
{
    private readonly ILogger<GetCurrentProcessIDController> _logger;

    public GetCurrentProcessIDController(ILogger<GetCurrentProcessIDController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(Process.GetCurrentProcess().Id);
    }
}
