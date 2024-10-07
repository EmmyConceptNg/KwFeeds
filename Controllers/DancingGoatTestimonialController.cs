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

[assembly: RegisterWebPageRoute(TestimonialPage.CONTENT_TYPE_NAME, typeof(DancingGoatTestimonialController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatTestimonialController : Controller
    {
        private readonly TestimonialPageRepository testimonialPageRepository;
        // private readonly ContactRepository contactRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;

        private readonly ArticlePageRepository articlePageRepository;
        private readonly ArticlesSectionRepository articlesSectionRepository;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly TestimonialRepository testimonialRepository;

        public DancingGoatTestimonialController(TestimonialPageRepository testimonialPageRepository, ArticlePageRepository articlePageRepository,
            CafeRepository cafeRepository, IPreferredLanguageRetriever currentLanguageRetriever, IWebPageDataContextRetriever webPageDataContextRetriever, ArticlesSectionRepository articlesSectionRepository, IWebPageUrlRetriever urlRetriever, TestimonialRepository testimonialRepository)
        {
            this.testimonialPageRepository = testimonialPageRepository;
            this.articlePageRepository = articlePageRepository;
            this.cafeRepository = cafeRepository;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.articlesSectionRepository = articlesSectionRepository;
            this.urlRetriever = urlRetriever;
            this.testimonialRepository = testimonialRepository;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;

            var testimonialPage = await testimonialPageRepository.GetTestimonialPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

            var model = await GetIndexViewModel(testimonialPage, cancellationToken);

            return View(model);
        }


        private async Task<TestimonialIndexViewModel> GetIndexViewModel(TestimonialPage testimonialPage, CancellationToken cancellationToken)
        {
            var languageName = currentLanguageRetriever.Get();
            var cafes = await cafeRepository.GetCompanyCafes(4, languageName, cancellationToken);
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var articlesSection = await articlesSectionRepository.GetArticlesSection(5, languageName, HttpContext.RequestAborted);
            var testimonials = await testimonialRepository.GetCompanyTestimonials(4, languageName, cancellationToken);

            var articleViewModels = new List<ArticleViewModel>();

            if (articlesSection != null){
            var articles = await articlePageRepository.GetArticles(articlesSection.SystemFields.WebPageItemTreePath, languageName, true, cancellationToken: HttpContext.RequestAborted);
                foreach (var article in articles)
                {
                    var articleModel = await ArticleViewModel.GetViewModel(article, urlRetriever, languageName);
                    articleViewModels.Add(articleModel);
                }
            }

            var testimonialViewModels = testimonials.Select(TestimonialViewModel.GetViewModel).ToList();



            return new TestimonialIndexViewModel
            {
                // CompanyContact = ContactViewModel.GetViewModel(contact),
                CompanyCafes = GetCompanyCafesModel(cafes),
                WebPage = testimonialPage,
                Content = testimonialPage.Content,
                Title = testimonialPage.Title,
                Articles = articleViewModels,
                Testimonials = testimonialViewModels
            };
        }


        private List<CafeViewModel> GetCompanyCafesModel(IEnumerable<Products> cafes)
        {
            return cafes.Select(cafe => CafeViewModel.GetViewModel(cafe)).ToList();
        }
    }
}
