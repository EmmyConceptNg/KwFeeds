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

[assembly: RegisterWebPageRoute(TechnologyPage.CONTENT_TYPE_NAME, typeof(DancingGoatTechnologyController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatTechnologyController : Controller
    {
        private readonly TechnologyPageRepository technologyPageRepository;
        // private readonly ContactRepository contactRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;

        private readonly ArticlePageRepository articlePageRepository;
        private readonly ArticlesSectionRepository articlesSectionRepository;
        private readonly IWebPageUrlRetriever urlRetriever;

        public DancingGoatTechnologyController(TechnologyPageRepository technologyPageRepository, ArticlePageRepository articlePageRepository,
            CafeRepository cafeRepository, IPreferredLanguageRetriever currentLanguageRetriever, IWebPageDataContextRetriever webPageDataContextRetriever, ArticlesSectionRepository articlesSectionRepository, IWebPageUrlRetriever urlRetriever)
        {
            this.technologyPageRepository = technologyPageRepository;
            this.articlePageRepository = articlePageRepository;
            this.cafeRepository = cafeRepository;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.articlesSectionRepository = articlesSectionRepository;
            this.urlRetriever = urlRetriever;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;

            var technologyPage = await technologyPageRepository.GetTechnologyPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

            var model = await GetIndexViewModel(technologyPage, cancellationToken);

            return View(model);
        }


        private async Task<TechnologyIndexViewModel> GetIndexViewModel(TechnologyPage technologyPage, CancellationToken cancellationToken)
        {
            var languageName = currentLanguageRetriever.Get();
            var cafes = await cafeRepository.GetCompanyCafes(4, languageName, cancellationToken);
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var articlesSection = await articlesSectionRepository.GetArticlesSection(5, languageName, HttpContext.RequestAborted);

            var articleViewModels = new List<ArticleViewModel>();

            if (articlesSection != null){
            var articles = await articlePageRepository.GetArticles(articlesSection.SystemFields.WebPageItemTreePath, languageName, true, cancellationToken: HttpContext.RequestAborted);
                foreach (var article in articles)
                {
                    var articleModel = await ArticleViewModel.GetViewModel(article, urlRetriever, languageName);
                    articleViewModels.Add(articleModel);
                }
            }
                

            return new TechnologyIndexViewModel
            {
                // CompanyContact = ContactViewModel.GetViewModel(contact),
                CompanyCafes = GetCompanyCafesModel(cafes),
                WebPage = technologyPage,
                Content = technologyPage.Content,
                Title = technologyPage.Title,
                Articles = articleViewModels
            };
        }


        private List<CafeViewModel> GetCompanyCafesModel(IEnumerable<Products> cafes)
        {
            return cafes.Select(cafe => CafeViewModel.GetViewModel(cafe)).ToList();
        }
    }
}
