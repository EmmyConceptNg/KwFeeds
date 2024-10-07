using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;
using System.Collections.Generic;
using System.Linq;
using CMS.Websites;

[assembly: RegisterWebPageRoute(SingleNewsPage.CONTENT_TYPE_NAME, typeof(DancingGoatSingleNewsController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME }, ActionName = nameof(DancingGoatSingleNewsController.Detail))]
namespace DancingGoat.Controllers
{
    public class DancingGoatSingleNewsController : Controller
    {
        private readonly NewsPageRepository newsPageRepository;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly NewsSectionRepository newsSectionRepository;
        private readonly NewsRepository newsRepository;

        public DancingGoatSingleNewsController(NewsPageRepository newsPageRepository,
            IWebPageDataContextRetriever webPageDataContextRetriever,
            IPreferredLanguageRetriever currentLanguageRetriever,
            ITaxonomyRetriever taxonomyRetriever,
            IWebPageUrlRetriever urlRetriever,
            NewsRepository newsRepository,
            NewsSectionRepository newsSectionRepository)
        {
            this.newsPageRepository = newsPageRepository;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.taxonomyRetriever = taxonomyRetriever;
            this.urlRetriever = urlRetriever;
            this.newsRepository = newsRepository;
            this.newsSectionRepository = newsSectionRepository;
        }

        public async Task<IActionResult> Detail()
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var languageName = currentLanguageRetriever.Get();
            var webPageItemId = webPageDataContextRetriever.Retrieve().WebPage.WebPageItemID;

            var singleNews = await newsPageRepository.GetNews<SingleNewsPage>(SingleNewsPage.CONTENT_TYPE_NAME, webPageItemId, languageName, cancellationToken: HttpContext.RequestAborted);

            var newsSection = await newsSectionRepository.GetNewsSection(42, languageName, HttpContext.RequestAborted);
            var news = await GetNews(languageName, newsSection);

            var viewModel = await SingleNewsDetailViewModel.GetViewModel(singleNews, languageName, taxonomyRetriever);

            var combinedModel = (ViewModel: viewModel, News: news);

            return View(combinedModel);
        }

        private async Task<IEnumerable<NewsListItemViewModel>> GetNews(string languageName, NewsSection newsSection, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var news = await newsRepository.GetNews(languageName, filter ?? new Dictionary<string, TaxonomyViewModel>(), cancellationToken: HttpContext.RequestAborted);
            var newsPages = await newsPageRepository.GetNews(newsSection.SystemFields.WebPageItemTreePath, languageName, news, cancellationToken: HttpContext.RequestAborted);

            return newsPages.Select(newsPage => NewsListItemViewModel.GetViewModel(newsPage, urlRetriever, languageName).Result);
        }
    }
}