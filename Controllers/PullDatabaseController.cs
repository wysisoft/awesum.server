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
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullDatabaseItem request)
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


        //     var follower = context.Followers.SingleOrDefault(o => o.FollowerLoginId == id && o.LeaderAppId == request.AppId);


        //     var foundDatabase = context.Databases.SingleOrDefault(o => o.Id == request.DatabaseId &&
        //     o.Loginid == id);

        //     if (foundDatabase == null)
        //     {
        //         context.Databases.Add(request.Database);
        //         context.SaveChanges();
        //     }
        //     else
        //     {
        //         if (request.Database != null && (foundDatabase.LastModified > request.Database.LastModified ||
        //         foundDatabase.Version > request.Database.Version) && !request.Force)
        //         {
        //             return BadRequest(_localizer["NotMostRecentVersion"]);
        //         }

        //         //we are clear to force the server to be the same as the client 

        //         if (request.Database != null && (foundDatabase.LastModified < request.Database.LastModified ||
        //         foundDatabase.Version < request.Database.Version))
        //         {
        //             foundDatabase.LastModified = request.Database.LastModified;
        //             foundDatabase.Version = request.Database.Version;
        //             foundDatabase.Name = request.Database.Name;
        //             foundDatabase.Order = request.Database.Order;
        //             context.SaveChanges();
        //         }
        //     }
        // }

        return Ok();
    }
}

public class PullDatabaseItem
{
public int AppId { get; set; }
public int DatabaseId { get; set; }
}