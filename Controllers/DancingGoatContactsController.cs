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

[assembly: RegisterWebPageRoute(ContactsPage.CONTENT_TYPE_NAME, typeof(DancingGoatContactController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatContactController : Controller
    {
        private readonly ContactsPageRepository contactPageRepository;
        // private readonly ContactRepository contactRepository;
        private readonly CafeRepository cafeRepository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;

        private readonly ArticlePageRepository articlePageRepository;
        private readonly ArticlesSectionRepository articlesSectionRepository;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly ContactRepository contactRepository;

        public DancingGoatContactController(ContactsPageRepository contactPageRepository, ArticlePageRepository articlePageRepository,
            CafeRepository cafeRepository, IPreferredLanguageRetriever currentLanguageRetriever, IWebPageDataContextRetriever webPageDataContextRetriever, ArticlesSectionRepository articlesSectionRepository, IWebPageUrlRetriever urlRetriever, ContactRepository contactRepository)
        {
            this.contactPageRepository = contactPageRepository;
            this.articlePageRepository = articlePageRepository;
            this.cafeRepository = cafeRepository;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.articlesSectionRepository = articlesSectionRepository;
            this.urlRetriever = urlRetriever;
            this.contactRepository = contactRepository;
        }


        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;

            var contactPage = await contactPageRepository.GetContactsPage(webPage.WebPageItemID, webPage.LanguageName, HttpContext.RequestAborted);

            var model = await GetIndexViewModel(contactPage, cancellationToken);

            return View(model);
        }


        private async Task<ContactsIndexViewModel> GetIndexViewModel(ContactsPage contactPage, CancellationToken cancellationToken)
        {
            var languageName = currentLanguageRetriever.Get();
            var cafes = await cafeRepository.GetCompanyCafes(4, languageName, cancellationToken);
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var articlesSection = await articlesSectionRepository.GetArticlesSection(5, languageName, HttpContext.RequestAborted);
            var contacts = await contactRepository.GetCompanyContacts(4, languageName, cancellationToken);

            var articleViewModels = new List<ArticleViewModel>();

            if (articlesSection != null){
            var articles = await articlePageRepository.GetArticles(articlesSection.SystemFields.WebPageItemTreePath, languageName, true, cancellationToken: HttpContext.RequestAborted);
                foreach (var article in articles)
                {
                    var articleModel = await ArticleViewModel.GetViewModel(article, urlRetriever, languageName);
                    articleViewModels.Add(articleModel);
                }
            }

            var contactViewModels = contacts.Select(ContactViewModel.GetViewModel).ToList();



            return new ContactsIndexViewModel
            {
                // CompanyContact = ContactViewModel.GetViewModel(contact),
                CompanyCafes = GetCompanyCafesModel(cafes),
                WebPage = contactPage,
                Content = contactPage.Content,
                Articles = articleViewModels,
                Contacts = contactViewModels
            };
        }


        private List<CafeViewModel> GetCompanyCafesModel(IEnumerable<Products> cafes)
        {
            return cafes.Select(cafe => CafeViewModel.GetViewModel(cafe)).ToList();
        }
    }
}
