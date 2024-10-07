using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Models;

[assembly: RegisterWebPageRoute(SingleProductPage.CONTENT_TYPE_NAME, typeof(DancingGoatSingleProductController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME }, ActionName = nameof(DancingGoatSingleProductController.Detail))]

namespace DancingGoat.Controllers
{
    public class DancingGoatSingleProductController : Controller
    {
        private readonly ProductPageRepository productPageRepository;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly ITaxonomyRetriever taxonomyRetriever;


        public DancingGoatSingleProductController(ProductPageRepository productPageRepository, 
            IWebPageDataContextRetriever webPageDataContextRetriever, 
            IPreferredLanguageRetriever currentLanguageRetriever,
            ITaxonomyRetriever taxonomyRetriever)
        {
            this.productPageRepository = productPageRepository;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.currentLanguageRetriever = currentLanguageRetriever;
            this.taxonomyRetriever = taxonomyRetriever;
        }


        public async Task<IActionResult> Detail()
        {
            var languageName = currentLanguageRetriever.Get();
            var webPageItemId = webPageDataContextRetriever.Retrieve().WebPage.WebPageItemID;

            var singleProduct = await productPageRepository.GetProduct<SingleProductPage>(SingleProductPage.CONTENT_TYPE_NAME, webPageItemId, languageName, cancellationToken: HttpContext.RequestAborted);

            return View(await SingleProductDetailViewModel.GetViewModel(singleProduct, languageName, taxonomyRetriever));
        }
    }
}
