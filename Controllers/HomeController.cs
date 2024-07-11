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
                                .ForContentType(KwHomePage.CONTENT_TYPE_NAME,
                                config => config
                                    .ForWebsite("KwFeeds", PathMatch.Single("/Home")));

            var result = await executor.GetMappedWebPageResult<KwHomePage>(query);
            var page = result.FirstOrDefault();

            if (page == null)
            {
                return NotFound();
            }

            var viewModel = new HomePageViewModel(page);
            return View("Index", viewModel);
        }
    }
}

