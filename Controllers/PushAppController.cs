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

    public PullAppController(ILogger<PullAppController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {

        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpPost]
    [RequestSizeLimit(5_242_880)]
    public ActionResult Post(PushAppRequest request)
    {
        var followers = new List<Follower>();
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(_localizer["AuthenticationIsRequired"]);
        }
        App foundLeaderApp = new App();
        var context = new AwesumContext();
        string email = "", id = "";
        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
            email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

            foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
            if (foundLeaderApp == null)
            {
                return BadRequest(_localizer["AppNotFound"]);
            }

            if (foundLeaderApp.Deleted == true)
            {
                return BadRequest(_localizer["AppDeleted"]);
            }

            if (request.App != null && (foundLeaderApp.LastModified > request.App.LastModified ||
            foundLeaderApp.Version > request.App.Version) && !request.Force)
            {
                return BadRequest(_localizer["NotMostRecentVersion"]);
            }

            //we are clear to force the server to be the same as the client for the leader app

            if (request.App != null && (foundLeaderApp.LastModified < request.App.LastModified ||
            foundLeaderApp.Version < request.App.Version))
            {
                foundLeaderApp.LastModified = request.App.LastModified;
                foundLeaderApp.Version = request.App.Version;
                foundLeaderApp.Name = request.App.Name;
                context.SaveChanges();
            }
        }

        return Ok();
    }
}