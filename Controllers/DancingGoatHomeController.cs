using System.Threading.Tasks;
using CMS.Websites;
using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWebPageRoute(HomePage.CONTENT_TYPE_NAME, typeof(DancingGoatHomeController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatHomeController : Controller
    {
        private readonly HomePageRepository _homePageRepository;
        private readonly ProductPageRepository _productPageRepository;
        private readonly IWebPageUrlRetriever _urlRetriever;
        private readonly IWebPageDataContextRetriever _webPageDataContextRetriever;

        public DancingGoatHomeController(
            HomePageRepository homePageRepository,
            ProductPageRepository productPageRepository,
            IWebPageUrlRetriever urlRetriever,
            IWebPageDataContextRetriever webPageDataContextRetriever)
        {
            _homePageRepository = homePageRepository;
            _productPageRepository = productPageRepository;
            _urlRetriever = urlRetriever;
            _webPageDataContextRetriever = webPageDataContextRetriever;
        }

        public async Task<IActionResult> Index()
        {
            var webPage = _webPageDataContextRetriever.Retrieve().WebPage;

            var homePage = await _homePageRepository.GetHomePage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);
            if (homePage == null)
            {
                return NotFound();
            }

            var viewModel = await HomePageViewModel.GetViewModel(homePage, _urlRetriever, webPage.LanguageName, _productPageRepository);
            return View(viewModel);
        }
    }
}
