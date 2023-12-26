using System.Security.Claims;
using System.Text.Json;
using awesum.server.Model;
using csharp;
using System.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public class DeleteAllDataController : ControllerBase
{
    private readonly ILogger<DeleteAllDataController> _logger;
    private readonly IStringLocalizer _localizer;

    public DeleteAllDataController(ILogger<DeleteAllDataController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {

        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]

    public ActionResult Get(string manualId)
    {
        App app = new App();
        var context = new AwesumContext();
        string email = "", id = "";

        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

            app = context.Apps.SingleOrDefault(o => o.Loginid == id);
            if (app == null)
            {
                return BadRequest();
            }
        }

        return Ok();
    }
}
