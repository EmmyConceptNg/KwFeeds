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
    contentTypeName: TestimonialPage.CONTENT_TYPE_NAME,
    controllerType: typeof(TestimonialsController),
    WebsiteChannelNames = new[] { "KwFeeds" })]

namespace KwFeeds.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly IContentQueryExecutor executor;
        private readonly ILogger<TestimonialsController> logger;

        public TestimonialsController(IContentQueryExecutor executor, ILogger<TestimonialsController> logger)
        {
            this.executor = executor;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                logger.LogInformation("Starting Index action.");

                // Fetch the TestimonialPage
                var query = new ContentItemQueryBuilder()
                    .ForContentType(TestimonialPage.CONTENT_TYPE_NAME, config => config
                        .ForWebsite("KwFeeds", PathMatch.Single("/Testimonials")));

                var pages = await executor.GetMappedWebPageResult<TestimonialPage>(query);
                logger.LogInformation("Pages fetched: {Count}", pages?.Count() ?? 0);

                var page = pages.FirstOrDefault();

                if (page == null)
                {
                    logger.LogWarning("TestimonialPage not found.");
                    return NotFound();
                }

                logger.LogInformation("TestimonialPage found. Title: {Title}",
                    page.Title);

                // Fetch all Testimonial items
                var productQuery = new ContentItemQueryBuilder()
                    .ForContentType(Testimonial.CONTENT_TYPE_NAME, config => { });

                var allSections = await executor.GetMappedWebPageResult<Testimonial>(productQuery);
                logger.LogInformation("All products fetched: {Count}", allSections?.Count() ?? 0);

                // Create the view model for the view
                var viewModel = new TestimonialPageViewModel
                {
                    Title = page.Title,
                    Sections = allSections.ToList()
                };

                logger.LogInformation("ViewModel created with {Count} products.", viewModel.Sections.Count);

                return View("Views/Testimonials/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the product data.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}