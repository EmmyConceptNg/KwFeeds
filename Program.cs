using System;
using System.Threading.Tasks;
using DancingGoat;
using DancingGoat.Models;
using Kentico.Activities.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Membership;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using KwFeeds;
using DotNetEnv;

// Load environment variables
Env.Load();

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls |
                                                 System.Net.SecurityProtocolType.Tls11 |
                                                 System.Net.SecurityProtocolType.Tls12;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Kentico services
builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        DefaultSectionIdentifier = ComponentIdentifiers.SINGLE_COLUMN_SECTION,
        RegisterDefaultSection = false,
        ContentTypeNames = new[]
        {
            LandingPage.CONTENT_TYPE_NAME,
            ContactsPage.CONTENT_TYPE_NAME,
            ArticlePage.CONTENT_TYPE_NAME,
            KwHomePage.CONTENT_TYPE_NAME,
            ProductPage.CONTENT_TYPE_NAME,
            ContactPage.CONTENT_TYPE_NAME,
        }
    });

    features.UseWebPageRouting();
    features.UseEmailMarketing();
    features.UseEmailStatisticsLogging();
    features.UseActivityTracking();
});

// Configure routing options
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add localization and view services
builder.Services.AddLocalization()
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
    });

// Add Dancing Goat specific services
builder.Services.AddDancingGoatServices();
ConfigureMembershipServices(builder.Services);

WebApplication app = builder.Build();

// ** Call InitKentico before any middleware setup
try
{
    app.InitKentico();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Kentico Initialization Exception: {ex.Message}");
    Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
    return; // Exit if Kentico initialization fails
}

// Setup middleware in the correct order
app.UseStaticFiles();

try
{
    app.UseKentico(); // Detailed logging for Kentico middleware setup
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Kentico Middleware Exception: {ex.Message}");
    Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
    return; // Exit if Kentico middleware setup fails
}

app.UseCookiePolicy();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/error/{0}");

// Configure routing
app.Kentico().MapRoutes();

app.MapControllerRoute(
   name: "error",
   pattern: "error/{code}",
   defaults: new { controller = "HttpErrors", action = "Error" }
);

app.MapControllerRoute(
    name: DancingGoatConstants.DEFAULT_ROUTE_NAME,
    pattern: $"{{{WebPageRoutingOptions.LANGUAGE_ROUTE_VALUE_KEY}}}/{{controller}}/{{action}}",
    constraints: new
    {
        controller = DancingGoatConstants.CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
    }
);

app.MapControllerRoute(
    name: DancingGoatConstants.DEFAULT_ROUTE_WITHOUT_LANGUAGE_PREFIX_NAME,
    pattern: "{controller}/{action}",
    constraints: new
    {
        controller = DancingGoatConstants.CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
    }
);

app.Run();

static void ConfigureMembershipServices(IServiceCollection services)
{
    services.AddIdentity<ApplicationUser, NoOpApplicationRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 0;
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddUserStore<ApplicationUserStore<ApplicationUser>>()
    .AddRoleStore<NoOpApplicationRoleStore>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

    services.ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = new PathString("/account/login");
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            var factory = ctx.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = factory.GetUrlHelper(new ActionContext(ctx.HttpContext, new RouteData(ctx.HttpContext.Request.RouteValues), new ActionDescriptor()));
            var url = urlHelper.Action("Login", "Account") + new Uri(ctx.RedirectUri).Query;

            ctx.Response.Redirect(url);

            return Task.CompletedTask;
        };
    });

    services.AddAuthorization();
}
