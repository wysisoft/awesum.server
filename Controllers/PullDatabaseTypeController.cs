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
public partial class PullDatabaseTypeController : ControllerBase
{
    private readonly ILogger<PullDatabaseTypeController> _logger;
    private readonly IStringLocalizer _localizer;
private readonly AwesumContext _context;

    public PullDatabaseTypeController(ILogger<PullDatabaseTypeController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache,
    AwesumContext context)
    {
        _context = context;
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullDatabaseTypeRequest request)
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
        PullDatabaseTypeResponse response = new PullDatabaseTypeResponse();
        DatabaseType? foundDatabaseType = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundDatabaseType = context.DatabaseTypes.SingleOrDefault(o => o.Loginid == id
                && o.Id == request.DatabaseType.Id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyDatabaseTypes"]);
            }
        }
        else
        {
            //find a follower row that entitles the user to pull this database
            foundFollower = context.Followers.FirstOrDefault(o =>
            o.FollowerLoginId == id && o.LeaderAppId == request.AppId &&
            (o.FollowerDatabaseGroup == "All" || o.DatabaseId == request.DatabaseId));
            if (foundFollower is not null)
            {
                foundDatabaseType = context.DatabaseTypes.SingleOrDefault(o => o.Id == request.DatabaseType.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindDatabaseType"]);
            }
        }

        if (foundDatabaseType == null)
        {
            return BadRequest(_localizer["DatabaseTypeNotFound"]);
        }

        if (foundDatabaseType.LastModified > request.DatabaseType.LastModified ||
        foundDatabaseType.Version > request.DatabaseType.Version)
        {
            response.DatabaseType = foundDatabaseType;
            response.Units = context.DatabaseUnits.Where(o => o.DatabaseTypeId == request.DatabaseType.Id)
            .Select(o => new DatabaseUnit()
            {
                Id = o.Id,
                Version = o.Version,
                LastModified = o.LastModified
            }).ToList();
        }

        return Ok(response);
    }
}
