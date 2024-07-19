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

[assembly: RegisterWebPageRoute(NutritionPage.CONTENT_TYPE_NAME, typeof(DancingGoatNutritionController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatNutritionController : Controller
    {
        private readonly NutritionPageRepository nutritionPageRepository;
        // private readonly ContactRepository contactRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;

        private readonly ArticlePageRepository articlePageRepository;
        private readonly ArticlesSectionRepository articlesSectionRepository;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly TeamRepository teamRepository;

        public DancingGoatNutritionController(NutritionPageRepository nutritionPageRepository, ArticlePageRepository articlePageRepository,
            CafeRepository cafeRepository, IPreferredLanguageRetriever currentLanguageRetriever, IWebPageDataContextRetriever webPageDataContextRetriever, ArticlesSectionRepository articlesSectionRepository, IWebPageUrlRetriever urlRetriever, TeamRepository teamRepository)
        {
            this.nutritionPageRepository = nutritionPageRepository;
            this.articlePageRepository = articlePageRepository;
            this.cafeRepository = cafeRepository;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.articlesSectionRepository = articlesSectionRepository;
            this.urlRetriever = urlRetriever;
            this.teamRepository = teamRepository;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;

            var nutritionPage = await nutritionPageRepository.GetNutritionPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

            var model = await GetIndexViewModel(nutritionPage, cancellationToken);

            return View(model);
        }


        private async Task<NutritionIndexViewModel> GetIndexViewModel(NutritionPage nutritionPage, CancellationToken cancellationToken)
        {
            var languageName = currentLanguageRetriever.Get();
            var cafes = await cafeRepository.GetCompanyCafes(4, languageName, cancellationToken);
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var articlesSection = await articlesSectionRepository.GetArticlesSection(5, languageName, HttpContext.RequestAborted);
            var teams = await teamRepository.GetCompanyTeams(4, languageName, cancellationToken);

            var articleViewModels = new List<ArticleViewModel>();

            if (articlesSection != null){
            var articles = await articlePageRepository.GetArticles(articlesSection.SystemFields.WebPageItemTreePath, languageName, true, cancellationToken: HttpContext.RequestAborted);
                foreach (var article in articles)
                {
                    var articleModel = await ArticleViewModel.GetViewModel(article, urlRetriever, languageName);
                    articleViewModels.Add(articleModel);
                }
            }

            var teamViewModels = teams.Select(TeamViewModel.GetViewModel).ToList();



            return new NutritionIndexViewModel
            {
                // CompanyContact = ContactViewModel.GetViewModel(contact),
                CompanyCafes = GetCompanyCafesModel(cafes),
                WebPage = nutritionPage,
                Content = nutritionPage.Content,
                Title = nutritionPage.Title,
                Articles = articleViewModels,
                Teams = teamViewModels
            };
        }


        private List<CafeViewModel> GetCompanyCafesModel(IEnumerable<Products> cafes)
        {
            return cafes.Select(cafe => CafeViewModel.GetViewModel(cafe)).ToList();
        }
    }
}
