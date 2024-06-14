using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;

using KwFeeds;
using KwFeeds.Models;
using KwFeeds.Controllers;

[assembly: RegisterWebPageRoute(
    contentTypeName: KwHomePage.CONTENT_TYPE_NAME,
    controllerType: typeof(KwHomeController),
    WebsiteChannelNames = new[] { "KwFeeds" })]

namespace KwFeeds.Controllers
{
    public class KwHomeController : Controller
    {
        private readonly IContentQueryExecutor executor;

        public KwHomeController(IContentQueryExecutor executor)
        {
            this.executor = executor;
        }

        public async Task<IActionResult> Index()
        {
            var query = new ContentItemQueryBuilder()
                                // Scopes the query to pages of the MEDLAB.Home content type
                                .ForContentType(KwHomePage.CONTENT_TYPE_NAME,
                                config => config
                                    // Retrieves the page with the /Home tree path under the MEDLABClinicPages website channel
                                    .ForWebsite("KwFeeds", PathMatch.Single("/Home")));

            // Executes the query and stores the data in the generated 'Home' class
            KwHomePage page = (await executor.GetMappedWebPageResult<KwHomePage>(query)).FirstOrDefault();

            // Passes the home page content to the view using HomePageViewModel
            return View("Views/KwHome/Index.cshtml", new HomePageViewModel(page));
        }
    }
}