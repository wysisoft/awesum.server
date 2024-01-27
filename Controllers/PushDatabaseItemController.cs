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
public partial class PushDatabaseItemController : ControllerBase
{
    private readonly ILogger<PushDatabaseItemController> _logger;
    private readonly IStringLocalizer _localizer;

    public PushDatabaseItemController(ILogger<PushDatabaseItemController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpPost]
    [RequestSizeLimit(5_242_880)]
    public ActionResult Post(PushDatabaseItemRequest request)
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

        var context = new AwesumContext();
        PushDatabaseItemResponse response = new PushDatabaseItemResponse();
        DatabaseItem? foundLeaderDatabaseItem = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundLeaderDatabaseItem = context.DatabaseItems.SingleOrDefault(o => o.Loginid == id
                && o.Id == request.DatabaseItem.Id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyLoginDatabaseItems"]);
            }
        }
        else
        {
            foundFollower = context.Followers.FirstOrDefault(o => o.FollowerLoginId == id
            && o.LeaderAppId == request.DatabaseItem.AppId);
            if (foundFollower is not null)
            {
                foundLeaderDatabaseItem = context.DatabaseItems.SingleOrDefault(o =>
                o.Id == request.DatabaseItem.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindDatabaseItemAsFollower"]);
            }
        }

        if (foundLeaderDatabaseItem == null)
        {
            return BadRequest(_localizer["DatabaseItemNotFound"]);
        }

        if (!request.Force && foundLeaderDatabaseItem.LastModified >= request.DatabaseItem.LastModified ||
        foundLeaderDatabaseItem.Version >= request.DatabaseItem.Version)
        {
            response.RequiresForce = true;
            return Ok(response);
        }

        if (request.Force || foundLeaderDatabaseItem.LastModified < request.DatabaseItem.LastModified ||
        foundLeaderDatabaseItem.Version < request.DatabaseItem.Version)
        {
            context.DatabaseItems.Update(request.DatabaseItem);
        }

        return Ok(response);
    }
}