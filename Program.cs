using awesum.server.Model;
using csharp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AwesumContext>(
    opt => opt.UseNpgsql("Name=ConnectionStrings:postgres"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins("https://awesum.app:8443","https://awesum.app", "https://accounts.google.com");
                      });
});


// Add services to the container.

builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{

})
.AddMicrosoftAccount(options =>
{
    options.ClientId = "29e26e2-4767-492d-95ff-2e745aa1af7e";
    options.ClientSecret = "4IL8Q~RilDMy1pXkOfMxux9mGyH8b4C5N8t3_b3F";
    options.SaveTokens = true;
})
.AddGoogle(options =>
{
    options.ClientId = "270679439922-dq20tpv4tpvdbd0glr7evgm252a91lpk.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.SaveTokens = true;
    //     options.Events.OnAccessDenied = async ctx =>
    //     {

    //     };
    //     options.Events.OnCreatingTicket = async ctx =>
    //     {
    //     };
    //     options.Events.OnRedirectToAuthorizationEndpoint = async ctx =>
    //     {
    //     };
    //     options.Events.OnRemoteFailure = async ctx =>
    // {
    // };
    //     options.Events.OnTicketReceived = async ctx =>
    //     {
    //     };
});


builder.Services.AddControllers();

builder.Services.AddSpaStaticFiles(configuration =>
   {
       configuration.RootPath = "clientapp/dist";
   });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IStringLocalizerFactory>(sp => new TxtFileStringLocalizerFactory( builder.Environment.WebRootPath + "/Resources"));

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();    
    app.UseSwagger();
    app.UseSwaggerUI();
}

 

app.UseHttpsRedirection();

app.UseAuthentication();


var supportedCultures = new[] { "en","en-US", "en-GB" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

//app.MapControllers();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
              name: "default",
              pattern: "{controller}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "clientapp";
        if (app.Environment.IsDevelopment())
        {
            spa.UseProxyToSpaDevelopmentServer("http://localhost:8080/");
        }
    });



app.Run();