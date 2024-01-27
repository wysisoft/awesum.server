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
using System.Diagnostics;

namespace csharp.Controllers;

[ApiController]
[Route("[controller]")]
public partial class PullDatabaseUnitController : ControllerBase
{
    private readonly ILogger<PullDatabaseUnitController> _logger;
    private readonly IStringLocalizer _localizer;

    public PullDatabaseUnitController(ILogger<PullDatabaseUnitController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullDatabaseUnitRequest request)
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
        PullDatabaseUnitResponse response = new PullDatabaseUnitResponse();
        DatabaseUnit? foundDatabaseUnit = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundDatabaseUnit = context.DatabaseUnits.SingleOrDefault(o => o.Loginid == id
                && o.Id == request.DatabaseUnit.Id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyDatabaseUnits"]);
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
                foundDatabaseUnit = context.DatabaseUnits.SingleOrDefault(o => o.Id == request.DatabaseUnit.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindDatabaseUnit"]);
            }
        }

        if (foundDatabaseUnit == null)
        {
            return BadRequest(_localizer["DatabaseUnitNotFound"]);
        }

        if (foundDatabaseUnit.LastModified > request.DatabaseUnit.LastModified ||
        foundDatabaseUnit.Version > request.DatabaseUnit.Version)
        {
            response.DatabaseUnit = foundDatabaseUnit;
            response.Items = context.DatabaseItems.Where(o => o.UnitId == request.DatabaseUnit.Id)
            .Select(o => new DatabaseItem()
            {
                Id = o.Id,
                Version = o.Version,
                LastModified = o.LastModified
            }).ToList();
        }

        return Ok(response);
    }
}