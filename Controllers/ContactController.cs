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
    contentTypeName: ContactPage.CONTENT_TYPE_NAME,
    controllerType: typeof(KwContactController),
    WebsiteChannelNames = new[] { "KwFeeds" })]

namespace KwFeeds.Controllers
{
    public class KwContactController : Controller
    {
        private readonly IContentQueryExecutor executor;

        public KwContactController(IContentQueryExecutor executor)
        {
            this.executor = executor;
        }

        public async Task<IActionResult> Index()
        {
            var query = new ContentItemQueryBuilder()
                .ForContentType(ContactPage.CONTENT_TYPE_NAME, config => config
                    .ForWebsite("KwFeeds", PathMatch.Single("/Contact")));

            ContactPage page = (await executor.GetMappedWebPageResult<ContactPage>(query)).FirstOrDefault();
            return View("Views/Contact/Index.cshtml", new ContactPageViewModel(page));
        }
    }
}