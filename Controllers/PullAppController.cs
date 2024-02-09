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
public partial class PullAppController : ControllerBase
{
    private readonly ILogger<PullAppController> _logger;
    private readonly IStringLocalizer _localizer;
private readonly AwesumContext _context;
    public PullAppController(ILogger<PullAppController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache
    , AwesumContext context)
    {
        _context = context;
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullAppRequest request)
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
        PullAppResponse response = new PullAppResponse();
        App? foundLeaderApp = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyLoginApps"]);
            }
        }
        else
        {
            foundFollower = context.Followers.FirstOrDefault(o => o.FollowerLoginId == id
            && o.LeaderAppId == request.App.Id);
            if (foundFollower is not null)
            {
                foundLeaderApp = context.Apps.SingleOrDefault(o => o.Id == request.App.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindAppAsFollower"]);
            }
        }

        if (foundLeaderApp == null)
        {
            return BadRequest(_localizer["AppNotFound"]);
        }

        if (foundLeaderApp.Deleted == true)
        {
            return BadRequest(_localizer["AppDeleted"]);
        }

        if (foundLeaderApp.LastModified > request.App.LastModified ||
        foundLeaderApp.Version > request.App.Version)
        {
            List<Database> databases = new List<Database>();
            if (foundFollower is null || foundFollower.FollowerDatabaseGroup == "All")
            {
                databases = context.Databases.Where(
                                o => o.Loginid == foundLeaderApp.Loginid
                                ).Select(o => new Database()
                                {
                                    Id = o.Id,
                                    LastModified = o.LastModified,
                                    Version = o.Version
                                }).ToList();
            }
            else if (foundFollower.FollowerDatabaseGroup != "All")
            {
                databases =
                (from d in context.Databases
                 where d.Loginid == foundLeaderApp.Loginid &&
                  d.GroupName == foundFollower.FollowerDatabaseGroup
                 select new Database()
                 {
                     Id = d.Id,
                     LastModified = d.LastModified,
                     Version = d.Version
                 }).ToList();
            }

            response.App = foundLeaderApp;
            response.Databases = databases;
        }

        return Ok(response);
    }
}
