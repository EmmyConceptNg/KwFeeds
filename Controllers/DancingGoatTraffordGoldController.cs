using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.Websites;
using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWebPageRoute(TraffordGoldPage.CONTENT_TYPE_NAME, typeof(DancingGoatTraffordGoldController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatTraffordGoldController : Controller
    {
        private readonly TraffordGoldPageRepository traffordgoldPageRepository;
        // private readonly ContactRepository contactRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;

        private readonly ArticlePageRepository articlePageRepository;
        private readonly ArticlesSectionRepository articlesSectionRepository;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly TeamRepository teamRepository;

        private readonly NewsSectionRepository newsSectionRepository;
        private readonly NewsRepository newsRepository;
        private readonly NewsPageRepository newsPageRepository;


        public DancingGoatTraffordGoldController(TraffordGoldPageRepository traffordgoldPageRepository, ArticlePageRepository articlePageRepository,
            CafeRepository cafeRepository, IPreferredLanguageRetriever currentLanguageRetriever, IWebPageDataContextRetriever webPageDataContextRetriever, ArticlesSectionRepository articlesSectionRepository, IWebPageUrlRetriever urlRetriever, TeamRepository teamRepository, NewsRepository newsRepository,
            NewsSectionRepository newsSectionRepository, NewsPageRepository newsPageRepository)
        {
            this.traffordgoldPageRepository = traffordgoldPageRepository;
            this.articlePageRepository = articlePageRepository;
            this.cafeRepository = cafeRepository;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.articlesSectionRepository = articlesSectionRepository;
            this.urlRetriever = urlRetriever;
            this.teamRepository = teamRepository;
            this.newsRepository = newsRepository;
            this.newsSectionRepository = newsSectionRepository;
            this.newsPageRepository = newsPageRepository;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;

            var traffordgoldPage = await traffordgoldPageRepository.GetTraffordGoldPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

            var model = await GetIndexViewModel(traffordgoldPage, cancellationToken);

            return View(model);
        }


        private async Task<TraffordGoldIndexViewModel> GetIndexViewModel(TraffordGoldPage traffordgoldPage, CancellationToken cancellationToken)
        {
            var languageName = currentLanguageRetriever.Get();
            var cafes = await cafeRepository.GetCompanyCafes(4, languageName, cancellationToken);
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
          


            var newsSection = await newsSectionRepository.GetNewsSection(42, languageName, HttpContext.RequestAborted);
            var news = await GetNews(languageName, newsSection);



            return new TraffordGoldIndexViewModel
            {
                // CompanyContact = ContactViewModel.GetViewModel(contact),
               
                WebPage = traffordgoldPage,
                Content = traffordgoldPage.Content,
                Title = traffordgoldPage.Title,
                Images = traffordgoldPage.Image, // Renamed to plural "Images"
                Videos = traffordgoldPage.Video, // Added property assignment for videos
                NewsItems = news

            };
        }

        private async Task<IEnumerable<NewsListItemViewModel>> GetNews(string languageName, NewsSection newsSection, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var news = await newsRepository.GetNews(languageName, filter ?? new Dictionary<string, TaxonomyViewModel>(), cancellationToken: HttpContext.RequestAborted);
            var newsPages = await newsPageRepository.GetNews(newsSection.SystemFields.WebPageItemTreePath, languageName, news, cancellationToken: HttpContext.RequestAborted);

            return newsPages.Select(newsPage => NewsListItemViewModel.GetViewModel(newsPage, urlRetriever, languageName).Result);
        }
    }
}
