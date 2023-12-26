using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;


namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public class GoogleController : ControllerBase
{
    private readonly ILogger<GoogleController> _logger;

    public GoogleController(ILogger<GoogleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Challenge(new AuthenticationProperties { IsPersistent=true, RedirectUri = "https://awesum.app/Login" }, "Google");
    }
}
