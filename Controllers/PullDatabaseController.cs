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
public partial class PullDatabaseController : ControllerBase
{
    private readonly ILogger<PullDatabaseController> _logger;
    private readonly IStringLocalizer _localizer;

    public PullDatabaseController(ILogger<PullDatabaseController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullDatabaseRequest request)
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
        PullDatabaseResponse response = new PullDatabaseResponse();
        Database? foundDatabase = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundDatabase = context.Databases.SingleOrDefault(o => o.Loginid == id && o.Id == request.Database.Id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyDatabases"]);
            }
        }
        else
        {
            foundFollower = context.Followers.FirstOrDefault(o =>
            o.FollowerLoginId == id && o.LeaderAppId == request.AppId &&
            (o.FollowerDatabaseGroup == null || o.DatabaseId == request.Database.Id));
            if (foundFollower is not null)
            {
                foundDatabase = context.Databases.SingleOrDefault(o => o.Id == request.Database.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindDatabaseFollower"]);
            }
        }

        if (foundDatabase == null)
        {
            return BadRequest(_localizer["DatabaseNotFound"]);
        }

        if (foundDatabase.Deleted == true)
        {
            return BadRequest(_localizer["DatabaseDeleted"]);
        }

        if (foundDatabase.LastModified > request.Database.LastModified ||
        foundDatabase.Version > request.Database.Version)
        {
            response.Database = foundDatabase;
            response.Types = context.DatabaseTypes.Where(o => o.DatabaseId == request.Database.Id)
            .Select(o => new DatabaseType()
            {
                Id = o.Id,
                Version = o.Version,
                LastModified = o.LastModified
            }).ToList();
        }

        return Ok(response);
    }
}