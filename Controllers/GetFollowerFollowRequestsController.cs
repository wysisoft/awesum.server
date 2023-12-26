using System.Security.Claims;
using System.Text.Json;
using awesum.server.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public class GetFollowerFollowRequestsController : ControllerBase
{
    private readonly ILogger<GetFollowerFollowRequestsController> _logger;

    private readonly IStringLocalizer _localizer;

    public GetFollowerFollowRequestsController(ILogger<GetFollowerFollowRequestsController> logger
    , IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]

    public ActionResult Get()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(_localizer["AuthenticationIsRequired"]);
        }
        App foundFollowerApp = new App();
        var context = new AwesumContext();

        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
            var id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

            foundFollowerApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
            if (foundFollowerApp == null)
            {
                return BadRequest(_localizer["UnknownFollowerApp"].Value);
            }
        }

        return Ok(new GetFollowerFollowRequestsResponse(
    context.Followers.Where(o => o.FollowerAppId == foundFollowerApp.Id)
            ));

    }
}