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
public partial class PushAppController : ControllerBase
{
    private readonly ILogger<PushAppController> _logger;
    private readonly IStringLocalizer _localizer;

    public PushAppController(ILogger<PushAppController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache)
    {

        _logger = logger;
        _localizer = (localizerFactory as TxtFileStringLocalizerFactory).Create2(typeof(SharedResources), cache);
    }

    [HttpGet]
    public ActionResult Get(PullAppRequest request)
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(_localizer["AuthenticationIsRequired"]);
        }

        App foundLeaderApp = new App();

        PullAppResponse response = new PullAppResponse();

        var context = new AwesumContext();

        string email = "", id = "";

        var appLevelFollow = true;

        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToDictionary(o => o.Type);
            email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();

            foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);

            if (foundLeaderApp is null)
            {
                var follower = context.Followers.SingleOrDefault(o => o.FollowerLoginId == id
                && o.LeaderAppId == request.AppId);
                if (follower is not null)
                {
                    foundLeaderApp = context.Apps.SingleOrDefault(o => o.Id == follower.LeaderAppId);
                    //appLevelFollow = follower;
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

            // var foundDatabase = context.Databases.SingleOrDefault(o => o.Loginid == foundLeaderApp.Loginid && o.Id == request.DatabaseId);

            // if (foundLeaderApp.LastModified > request.lastModified ||
            // foundLeaderApp.Version > request.Version)
            // {
            //     response.App = foundLeaderApp;
            //     response.Databases = context.Databases.Where(o => o.Loginid == foundLeaderApp.Loginid).Select(o => o.Id).ToList();
            // }
        }

        return Ok(response);
    }
}
