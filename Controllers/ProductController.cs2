using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc.Routing;
using KwFeeds.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
using KwFeeds;
using KwFeeds.Controllers;

[assembly: RegisterWebPageRoute(
    contentTypeName: ProductPage.CONTENT_TYPE_NAME,
    controllerType: typeof(KwProductController),
    WebsiteChannelNames = new[] { "KwFeeds" })]

namespace KwFeeds.Controllers
{
    public class KwProductController : Controller
    {
        private readonly IContentQueryExecutor executor;
        private readonly ILogger<KwProductController> logger;

        public KwProductController(IContentQueryExecutor executor, ILogger<KwProductController> logger)
        {
            this.executor = executor;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                logger.LogInformation("Starting Index action.");

                // Fetch the ProductPage
                var query = new ContentItemQueryBuilder()
                    .ForContentType(ProductPage.CONTENT_TYPE_NAME, config => config
                        .ForWebsite("KwFeeds", PathMatch.Single("/Product")));

                var pages = await executor.GetMappedWebPageResult<ProductPage>(query);
                logger.LogInformation("Pages fetched: {Count}", pages?.Count() ?? 0);

                var page = pages.FirstOrDefault();

                if (page == null)
                {
                    logger.LogWarning("ProductPage not found.");
                    return NotFound();
                }

                logger.LogInformation("ProductPage found. Title: {Title}, Description: {Description}",
                    page.Title, page.PageHeaderContent);

                // Fetch all SingleProduct items
                var productQuery = new ContentItemQueryBuilder()
                    .ForContentType(SingleProduct.CONTENT_TYPE_NAME, config => { });

                var allProducts = await executor.GetMappedWebPageResult<SingleProduct>(productQuery);
                logger.LogInformation("All products fetched: {Count}", allProducts?.Count() ?? 0);

                // Create the view model for the view
                var viewModel = new ProductPageViewModel
                {
                    Title = page.Title,
                    PageHeaderContent = page.PageHeaderContent,
                    Products = allProducts.ToList()
                };

                logger.LogInformation("ViewModel created with {Count} products.", viewModel.Products.Count);

                return View("Views/KwProducts/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the product data.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}