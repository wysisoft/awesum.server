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

    public PullDatabaseTypeController(ILogger<PullDatabaseTypeController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {

        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullDatabasType request)
    {
        // var followers = new List<Follower>();
        // if (!HttpContext.User.Identity.IsAuthenticated)
        // {
        //     return BadRequest(_localizer["AuthenticationIsRequired"]);
        // }
        // var context = new AwesumContext();
        // string email = "", id = "";
        // if (HttpContext.User.Identity.AuthenticationType == "Google")
        // {
        //     var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
        //     email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
        //     id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

        //     var foundDatabaseType = context.DatabaseTypes.SingleOrDefault(o => o.UniqueId == request.DatabaseType.UniqueId &&
        //     o.Loginid == id);

        //     if (foundDatabaseType == null)
        //     {
        //         context.DatabaseTypes.Add(request.DatabaseType);
        //         context.SaveChanges();
        //     }
        //     else
        //     {
        //         if (request.DatabaseType != null && (foundDatabaseType.LastModified > request.DatabaseType.LastModified ||
        //         foundDatabaseType.Version > request.DatabaseType.Version) && !request.Force)
        //         {
        //             return BadRequest(_localizer["NotMostRecentVersion"]);
        //         }

        //         //we are clear to force the server to be the same as the client 

        //         if (request.DatabaseType != null && (foundDatabaseType.LastModified < request.DatabaseType.LastModified ||
        //         foundDatabaseType.Version < request.DatabaseType.Version))
        //         {
        //             foundDatabaseType.LastModified = request.DatabaseType.LastModified;
        //             foundDatabaseType.Version = request.DatabaseType.Version;
        //             foundDatabaseType.Order = request.DatabaseType.Order;
        //             context.SaveChanges();
        //         }
        //     }
        // }

        return Ok();
    }
}
