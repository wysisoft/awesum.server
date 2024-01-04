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
public class GetLeaderFollowRequestsController : ControllerBase
{
    private readonly ILogger<GetLeaderFollowRequestsController> _logger;

    private readonly IStringLocalizer _localizer;

    public GetLeaderFollowRequestsController(ILogger<GetLeaderFollowRequestsController> logger
    , IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]

    public ActionResult Get(string leaderId)
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(_localizer["AuthenticationIsRequired"]);
        }
        App foundLeaderApp = new App();
        var context = new AwesumContext();

        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
            var id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

            foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
            if (foundLeaderApp == null)
            {
                return BadRequest(_localizer["UnknownFollowerApp"].Value);
            }
        }

        return Ok(new GetLeaderFollowRequestsResponse(
            awesum.awesum.LeaderOrFollowerRows(context.Followers, foundLeaderApp)
            ));
    }
}