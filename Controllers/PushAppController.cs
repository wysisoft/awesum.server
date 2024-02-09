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
public partial class PushAppController : ControllerBase
{
    private readonly ILogger<PushAppController> _logger;
    private readonly IStringLocalizer _localizer;
private readonly AwesumContext _context;
    public PushAppController(ILogger<PushAppController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache,
    AwesumContext context)
    {
        _context = context;
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpPost]
    [RequestSizeLimit(5_242_880)]
    public ActionResult Post(PushAppRequest request)
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

        string email = "", id = "";
        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = claimsIdentity.Claims.ToDictionary(o => o.Type);
            email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();
        }

        var context = _context;
        PushAppResponse response = new PushAppResponse();
        App? foundLeaderApp = null;
        //Follower? foundFollower = null;

        // if (request.IsLeader)
        // {
            try
            {
                foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyLoginApps"]);
            }
        // }
        // else
        // {
        //     foundFollower = context.Followers.FirstOrDefault(o => o.FollowerLoginId == id
        //     && o.LeaderAppId == request.App.Id);
        //     if (foundFollower is not null)
        //     {
        //         foundLeaderApp = context.Apps.SingleOrDefault(o => o.Id == request.App.Id);
        //     }
        //     else
        //     {
        //         return BadRequest(_localizer["CouldNotFindAppAsFollower"]);
        //     }
        // }

        if (foundLeaderApp == null)
        {
            context.Apps.Add(request.App);
            return Ok(response);
        }

        if (foundLeaderApp.Deleted == true)
        {
            return BadRequest(_localizer["AppDeleted"]);
        }

        if (!request.Force && foundLeaderApp.LastModified >= request.App.LastModified ||
        foundLeaderApp.Version >= request.App.Version)
        {
            response.RequiresForce = true;
            return Ok(response);
        }

        if (request.Force || foundLeaderApp.LastModified < request.App.LastModified ||
        foundLeaderApp.Version < request.App.Version)
        {
            context.Apps.Update(request.App);
        }

        return Ok(response);
    }
}