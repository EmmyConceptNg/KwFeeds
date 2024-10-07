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

[assembly: RegisterWebPageRoute(NewsSection.CONTENT_TYPE_NAME, typeof(DancingGoatNewsController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatNewsController : Controller
    {
        private readonly NewsSectionRepository newsSectionRepository;
        private readonly NewsPageRepository newsPageRepository;
        private readonly NewsRepository newsRepository;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;


        public DancingGoatNewsController(
            NewsSectionRepository newsSectionRepository,
            NewsPageRepository newsPageRepository,
            NewsRepository newsRepository,
            IWebPageUrlRetriever urlRetriever,
            IPreferredLanguageRetriever currentLanguageRetriever,
            IWebPageDataContextRetriever webPageDataContextRetriever,
            ITaxonomyRetriever taxonomyRetriever)
        {
            this.newsSectionRepository = newsSectionRepository;
            this.newsPageRepository = newsPageRepository;
            this.newsRepository = newsRepository;
            this.urlRetriever = urlRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.taxonomyRetriever = taxonomyRetriever;
        }


        public async Task<IActionResult> Index()
        {
            var languageName = currentLanguageRetriever.Get();
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var newsSection = await newsSectionRepository.GetNewsSection(webPage.WebPageItemID, languageName, HttpContext.RequestAborted);

            var news = await GetNews(languageName, newsSection);

            var taxonomies = new Dictionary<string, TaxonomyViewModel>();
            var taxonomyNames = new List<string> { "ArticleType", "TypeOfStock",  "StockAge", "ProductionStage", "InformationSource", "ArticleLength" };
            foreach (var taxonomyName in taxonomyNames)
            {
                var taxonomy = await taxonomyRetriever.RetrieveTaxonomy(taxonomyName, languageName);
                if (taxonomy.Tags.Any())
                {
                    taxonomies.Add(taxonomyName, TaxonomyViewModel.GetViewModel(taxonomy));
                }
            }

            var listModel = new NewsListViewModel(news, taxonomies);

            return View(listModel);
        }


        [HttpPost($"{{{WebPageRoutingOptions.LANGUAGE_ROUTE_VALUE_KEY}}}/{{controller}}/{{action}}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Filter(IDictionary<string, TaxonomyViewModel> filter)
        {
            var languageName = currentLanguageRetriever.Get();
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var newsSection = await newsSectionRepository.GetNewsSection(webPage.WebPageItemID, languageName, HttpContext.RequestAborted);

            var news = await GetNews(languageName, newsSection, filter);
            return PartialView("NewsList", news);
        }


        private async Task<IEnumerable<NewsListItemViewModel>> GetNews(string languageName, NewsSection newsSection, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var news = await newsRepository.GetNews(languageName, filter ?? new Dictionary<string, TaxonomyViewModel>(), cancellationToken: HttpContext.RequestAborted);
            var newsPages = await newsPageRepository.GetNews(newsSection.SystemFields.WebPageItemTreePath, languageName, news, cancellationToken: HttpContext.RequestAborted);

            return newsPages.Select(newsPage => NewsListItemViewModel.GetViewModel(newsPage, urlRetriever, languageName).Result);
        }
    }
}
