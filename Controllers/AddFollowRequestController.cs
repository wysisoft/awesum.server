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
public class AddFollowRequestController : ControllerBase
{
    private readonly ILogger<AddFollowRequestController> _logger;

    private readonly IStringLocalizer _localizer;
private readonly AwesumContext _context;
    public AddFollowRequestController(ILogger<AddFollowRequestController> logger
    , IStringLocalizerFactory localizerFactory, IMemoryCache cache, AwesumContext context)
    {
        _context = context;
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpGet]

    public ActionResult Get(string leaderId)
    {
        if (HttpContext == null)
        {
            return BadRequest(_localizer["HttpContextIsNull"]);
        }

        if (HttpContext.User == null)
        {
            return BadRequest(_localizer["HttpContextUserIsNull"]);
        }

        if (HttpContext.User.Identity == null)
        {
            return BadRequest(_localizer["HttpContextUserIdentityIsNull"]);
        }

        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

        if (claimsIdentity == null)
        {
            return BadRequest(_localizer["HttpContextUserIdentityIsNotClaimsIdentity"]);
        }

        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(_localizer["AuthenticationIsRequired"]);
        }

        App? foundFollowerApp = new App();
        var context = _context;

        string email = "", id = "";
        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = claimsIdentity.Claims.ToDictionary(o => o.Type);
            email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();
        }

        foundFollowerApp = context.Apps.SingleOrDefault(o => o.Loginid == id);

        if (foundFollowerApp == null)
        {
            return BadRequest(_localizer["UnknownFollowerApp"].Value);
        }

        var foundLeaderApp = context.Apps.SingleOrDefault(o => o.ManualId == leaderId);
        if (foundLeaderApp == null)
        {
            return BadRequest(_localizer["UnknownLeaderApp"].Value);

        }

        var foundRequestToFollow = context.Followers.SingleOrDefault(o => o.LeaderAppId == foundLeaderApp.Id &&
        o.FollowerAppId == foundFollowerApp.Id);
        if (foundRequestToFollow == null)
        {
            context.Followers.Add(new Follower()
            {
                LeaderAppId = foundLeaderApp.Id,
                FollowerAppId = foundFollowerApp.Id,
                FollowerName = foundFollowerApp.Name,
                FollowerEmail = foundFollowerApp.Email
            });
            context.SaveChanges();

            return Ok(new AddFollowRequestResponse(
            awesum.Awesum.LeaderOrFollowerRows(context.Followers, foundFollowerApp)
            ));
        }
        else
        {
            return BadRequest(_localizer["FollowRequestAlreadyExists"].Value);
        }
    }
}
