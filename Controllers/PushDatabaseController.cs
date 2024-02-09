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
public partial class PushDatabaseController : ControllerBase
{
    private readonly ILogger<PushDatabaseController> _logger;
    private readonly IStringLocalizer _localizer;
    private readonly AwesumContext _context;

    public PushDatabaseController(ILogger<PushDatabaseController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache, AwesumContext context)
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
    public ActionResult Post(PushDatabaseRequest request)
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
        PushDatabaseResponse response = new PushDatabaseResponse();
        Database? foundLeaderDatabase = null;
        //Follower? foundFollower = null;

        // if (request.IsLeader)
        // {
        try
        {
            foundLeaderDatabase = context.Databases.SingleOrDefault(o => o.Loginid == id
            && o.Id == request.Database.Id);
        }
        catch (System.InvalidOperationException)
        {
            return BadRequest(_localizer["TooManyLoginDatabases"]);
        }
        // }
        // else
        // {
        //     foundFollower = context.Followers.FirstOrDefault(o => o.FollowerLoginId == id
        //     && o.LeaderAppId == request.Database.AppId);
        //     if (foundFollower is not null)
        //     {
        //         foundLeaderDatabase = context.Databases.SingleOrDefault(o => o.Id == request.Database.Id);
        //     }
        //     else
        //     {
        //         return BadRequest(_localizer["CouldNotFindDatabaseAsFollower"]);
        //     }
        // }

        if (foundLeaderDatabase == null)
        {
            context.Databases.Add(request.Database);
            return Ok(response);
        }

        if (foundLeaderDatabase.Deleted == true)
        {
            return BadRequest(_localizer["DatabaseDeleted"]);
        }

        if (!request.Force && foundLeaderDatabase.LastModified >= request.Database.LastModified ||
        foundLeaderDatabase.Version >= request.Database.Version)
        {
            response.RequiresForce = true;
            return Ok(response);
        }

        if (request.Force || foundLeaderDatabase.LastModified < request.Database.LastModified ||
        foundLeaderDatabase.Version < request.Database.Version)
        {
            context.Databases.Update(request.Database);
        }

        return Ok(response);
    }
}