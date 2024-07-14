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
public class GetCurrentUserInfoController : ControllerBase
{
    private readonly ILogger<GetCurrentUserInfoController> _logger;
    private readonly IStringLocalizer _localizer;

    private readonly AwesumContext _context;


    public GetCurrentUserInfoController(ILogger<GetCurrentUserInfoController> logger, IStringLocalizerFactory localizerFactory, IMemoryCache cache,
    AwesumContext context)
    {
        _context = context;
        _logger = logger;
        var txtFileStringLocalizerFactory = localizerFactory as TxtFileStringLocalizerFactory;
        if (txtFileStringLocalizerFactory == null)
            throw new System.Exception("localizerFactory is not TxtFileStringLocalizerFactory");
        _localizer = txtFileStringLocalizerFactory.Create2(typeof(SharedResources), cache);
    }

    [HttpPost]
    [RequestSizeLimit(5_242_880)]
    public ActionResult Post(App app)
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

        var followers = new List<Follower>();

        var context = _context;
        string email = "", id = "";
        if (HttpContext.User.Identity.AuthenticationType == "Google")
        {
            var claims = claimsIdentity.Claims.ToDictionary(o => o.Type);
            email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].Value.ToLower();
            id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"].Value.ToLower();
        }

        var foundLeaderApp = context.Apps.SingleOrDefault(o => o.Loginid == id);
        if (foundLeaderApp == null)
        {
            foundLeaderApp = app;

            foundLeaderApp.Loginid = id;
            foundLeaderApp.Email = email;

            context.Apps.Add(foundLeaderApp);

            context.SaveChanges();

            var paddedManualId = foundLeaderApp.Id.ToString().PadLeft(10, '0');
            foundLeaderApp.ManualId = paddedManualId.Substring(0, 3) + '-' +
            paddedManualId.Substring(3, 3) + '-' +
            paddedManualId.Substring(6, 4);
            context.SaveChanges();
        }

        if (foundLeaderApp.ManualId == null)
        {
            return BadRequest(_localizer["ManualIdIsNull"]);
        }

        if (HttpContext.User.Identity.AuthenticationType == null)
        {
            return BadRequest(_localizer["AuthenticationTypeIsNull"]);
        }

        return Ok(new GetCurrentUserInfoResponse()
        {
            ManualId = foundLeaderApp.ManualId,
            AuthenticationType = HttpContext.User.Identity.AuthenticationType,
            Email = email,
            Id = foundLeaderApp.Id,
            Followers = awesum.Awesum.LeaderOrFollowerRows(context.Followers, foundLeaderApp)
        });
    }
}
