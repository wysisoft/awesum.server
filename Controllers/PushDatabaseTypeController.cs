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
public partial class PushDatabaseTypeController : ControllerBase
{
    private readonly ILogger<PushDatabaseTypeController> _logger;
    private readonly IStringLocalizer _localizer;

    public PushDatabaseTypeController(ILogger<PushDatabaseTypeController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpPost]
    [RequestSizeLimit(5_242_880)]
    public ActionResult Post(PushDatabaseTypeRequest request)
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
        PushDatabaseTypeResponse response = new PushDatabaseTypeResponse();
        DatabaseType? foundLeaderDatabaseType = null;
        //Follower? foundFollower = null;

        // if (request.IsLeader)
        // {
        try
        {
            foundLeaderDatabaseType = context.DatabaseTypes.SingleOrDefault(o => o.Loginid == id
            && o.Id == request.DatabaseType.Id);
        }
        catch (System.InvalidOperationException)
        {
            return BadRequest(_localizer["TooManyLoginDatabaseTypes"]);
        }
        // }
        // else
        // {
        //     foundFollower = context.Followers.FirstOrDefault(o => o.FollowerLoginId == id
        //     && o.LeaderAppId == request.DatabaseType.AppId);
        //     if (foundFollower is not null)
        //     {
        //         foundLeaderDatabaseType = context.DatabaseTypes.SingleOrDefault(o => o.Id == request.DatabaseType.Id);
        //     }
        //     else
        //     {
        //         return BadRequest(_localizer["CouldNotFindDatabaseTypeAsFollower"]);
        //     }
        // }

        if (foundLeaderDatabaseType == null)
        {
            context.DatabaseTypes.Add(request.DatabaseType);
            return Ok(response);
        }

        if (!request.Force && foundLeaderDatabaseType.LastModified >= request.DatabaseType.LastModified ||
        foundLeaderDatabaseType.Version >= request.DatabaseType.Version)
        {
            response.RequiresForce = true;
            return Ok(response);
        }

        if (request.Force || foundLeaderDatabaseType.LastModified < request.DatabaseType.LastModified ||
        foundLeaderDatabaseType.Version < request.DatabaseType.Version)
        {
            context.DatabaseTypes.Update(request.DatabaseType);
        }

        return Ok(response);
    }
}