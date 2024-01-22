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

    public PullDatabaseItemController(ILogger<PullDatabaseItemController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
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

        //     var foundDatabaseItem = context.DatabaseItems.SingleOrDefault(o => o.UniqueId == request.DatabaseItem.UniqueId &&
        //     o.Loginid == id);

        //     if (foundDatabaseItem == null)
        //     {
        //         context.DatabaseItems.Add(request.DatabaseItem);
        //         context.SaveChanges();
        //     }
        //     else
        //     {
        //         if (request.DatabaseItem != null && (foundDatabaseItem.LastModified > request.DatabaseItem.LastModified ||
        //         foundDatabaseItem.Version > request.DatabaseItem.Version) && !request.Force)
        //         {
        //             return BadRequest(_localizer["NotMostRecentVersion"]);
        //         }

        //         //we are clear to force the server to be the same as the client 

        //         if (request.DatabaseItem != null && (foundDatabaseItem.LastModified < request.DatabaseItem.LastModified ||
        //         foundDatabaseItem.Version < request.DatabaseItem.Version))
        //         {
        //             foundDatabaseItem.LastModified = request.DatabaseItem.LastModified;
        //             foundDatabaseItem.Version = request.DatabaseItem.Version;
        //             foundDatabaseItem.Order = request.DatabaseItem.Order;
        //             foundDatabaseItem.Text= request.DatabaseItem.Text;
        //             foundDatabaseItem.Letters= request.DatabaseItem.Letters;
        //             foundDatabaseItem.Image= request.DatabaseItem.Image;
        //             foundDatabaseItem.Sound= request.DatabaseItem.Sound;
        //             context.SaveChanges();
        //         }
        //     }
        // }

        return Ok();
    }
}