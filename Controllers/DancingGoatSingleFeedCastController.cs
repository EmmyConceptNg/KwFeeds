using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;

[assembly: RegisterWebPageRoute(SingleFeedCastPage.CONTENT_TYPE_NAME, typeof(DancingGoatSingleFeedCastController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME }, ActionName = nameof(DancingGoatSingleFeedCastController.Detail))]

namespace DancingGoat.Controllers
{
    public class DancingGoatSingleFeedCastController : Controller
    {
        private readonly FeedCastPageRepository feedcastPageRepository;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;


        public DancingGoatSingleFeedCastController(FeedCastPageRepository feedcastPageRepository, 
            IWebPageDataContextRetriever webPageDataContextRetriever, 
            IPreferredLanguageRetriever currentLanguageRetriever
            )
        {
            this.feedcastPageRepository = feedcastPageRepository;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.currentLanguageRetriever = currentLanguageRetriever;
        }


        public async Task<IActionResult> Detail()
        {
            var languageName = currentLanguageRetriever.Get();
            var webPageItemId = webPageDataContextRetriever.Retrieve().WebPage.WebPageItemID;

            var singleFeedCast = await feedcastPageRepository.GetFeedCast<SingleFeedCastPage>(SingleFeedCastPage.CONTENT_TYPE_NAME, webPageItemId, languageName, cancellationToken: HttpContext.RequestAborted);

            return View(await SingleFeedCastDetailViewModel.GetViewModel(singleFeedCast));
        }
    }
}
