using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public class LogoutController : ControllerBase
{
    private readonly ILogger<LogoutController> _logger;

    public LogoutController(ILogger<LogoutController> logger)
    {
        _logger = logger;
    }

[HttpGet]

    public async Task<IActionResult> Get()
    {
        await HttpContext.SignOutAsync();
        return Redirect("CurrentUserInfo");
    }
}
