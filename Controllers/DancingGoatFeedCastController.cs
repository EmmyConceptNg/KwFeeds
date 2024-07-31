using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Websites;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWebPageRoute(FeedCastsSection.CONTENT_TYPE_NAME, typeof(DancingGoatFeedCastController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatFeedCastController : Controller
    {
        private readonly FeedCastSectionRepository feedcastSectionRepository;
        private readonly FeedCastPageRepository feedcastPageRepository;
        private readonly FeedCastRepository feedcastRepository;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;


        public DancingGoatFeedCastController(
            FeedCastSectionRepository feedcastSectionRepository,
            FeedCastPageRepository feedcastPageRepository,
            FeedCastRepository feedcastRepository,
            IWebPageUrlRetriever urlRetriever,
            IPreferredLanguageRetriever currentLanguageRetriever,
            IWebPageDataContextRetriever webPageDataContextRetriever,
            ITaxonomyRetriever taxonomyRetriever)
        {
            this.feedcastSectionRepository = feedcastSectionRepository;
            this.feedcastPageRepository = feedcastPageRepository;
            this.feedcastRepository = feedcastRepository;
            this.urlRetriever = urlRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.taxonomyRetriever = taxonomyRetriever;
        }


        public async Task<IActionResult> Index()
        {
            var languageName = currentLanguageRetriever.Get();
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var feedcastsSection = await feedcastSectionRepository.GetFeedCastsSection(webPage.WebPageItemID, languageName, HttpContext.RequestAborted);

            var feedcasts = await GetFeedCasts(languageName, feedcastsSection);
            var listModel = new FeedCastListViewModel(feedcasts);

            return View(listModel); 
        }


        [HttpPost($"{{{WebPageRoutingOptions.LANGUAGE_ROUTE_VALUE_KEY}}}/{{controller}}/{{action}}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Filter(IDictionary<string, TaxonomyViewModel> filter)
        {
            var languageName = currentLanguageRetriever.Get();
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var feedcastsSection = await feedcastSectionRepository.GetFeedCastsSection(webPage.WebPageItemID, languageName, HttpContext.RequestAborted);

            var feedcasts = await GetFeedCasts(languageName, feedcastsSection, filter);
            return PartialView("FeedCastsList", feedcasts);
        }


        private async Task<IEnumerable<FeedCastListItemViewModel>> GetFeedCasts(string languageName, FeedCastsSection feedcastsSection, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var feedcasts = await feedcastRepository.GetFeedCasts(languageName, filter ?? new Dictionary<string, TaxonomyViewModel>(), cancellationToken: HttpContext.RequestAborted);
            var feedcastPages = await feedcastPageRepository.GetFeedCasts(feedcastsSection.SystemFields.WebPageItemTreePath, languageName, feedcasts, cancellationToken: HttpContext.RequestAborted);

            return feedcastPages.Select(feedcastPage => FeedCastListItemViewModel.GetViewModel(feedcastPage, urlRetriever, languageName).Result);
        }
    }
}
