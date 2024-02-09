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
public partial class PullDatabaseItemController : ControllerBase
{
    private readonly ILogger<PullDatabaseItemController> _logger;
    private readonly IStringLocalizer _localizer;

private readonly AwesumContext _context;
    public PullDatabaseItemController(ILogger<PullDatabaseItemController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache,
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
    public ActionResult Get(PullDatabaseItemRequest request)
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
        PullDatabaseItemResponse response = new PullDatabaseItemResponse();
        DatabaseItem? foundDatabaseItem = null;
        Follower? foundFollower = null;

        if (request.IsLeader)
        {
            try
            {
                foundDatabaseItem = context.DatabaseItems.SingleOrDefault(o => o.Loginid == id
            && request.DatabaseItem.Id == o.Id);
            }
            catch (System.InvalidOperationException)
            {
                return BadRequest(_localizer["TooManyDatabaseItems"]);
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
                foundDatabaseItem = context.DatabaseItems.SingleOrDefault(o => o.Id == request.DatabaseItem.Id);
            }
            else
            {
                return BadRequest(_localizer["CouldNotFindDatabaseItem"]);
            }
        }

        if (foundDatabaseItem == null)
        {
            return BadRequest(_localizer["DatabaseItemNotFound"]);
        }

        if (foundDatabaseItem.LastModified > request.DatabaseItem.LastModified ||
        foundDatabaseItem.Version > request.DatabaseItem.Version)
        {
            response.DatabaseItem = foundDatabaseItem;
        }

        return Ok(response);
    }
}