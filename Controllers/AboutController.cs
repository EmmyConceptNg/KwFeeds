// using Microsoft.AspNetCore.Mvc;
// using System.Linq;
// using System.Threading.Tasks;
// using CMS.ContentEngine;
// using CMS.Websites;
// using Kentico.Content.Web.Mvc.Routing;
// using KwFeeds.Models;
// using System.Collections.Generic;
// using Microsoft.Extensions.Logging;
// using System;
// using KwFeeds;
// using KwFeeds.Controllers;

// [assembly: RegisterWebPageRoute(
//     contentTypeName: AboutPage.CONTENT_TYPE_NAME,
//     controllerType: typeof(AboutController),
//     WebsiteChannelNames = new[] { "KwFeeds" })]

// namespace KwFeeds.Controllers
// {
//     public class AboutController : Controller
//     {
//         private readonly IContentQueryExecutor executor;
//         private readonly ILogger<AboutController> logger;

//         public AboutController(IContentQueryExecutor executor, ILogger<AboutController> logger)
//         {
//             this.executor = executor;
//             this.logger = logger;
//         }

//         public async Task<IActionResult> Index()
//         {
//             try
//             {
//                 logger.LogInformation("Starting Index action.");

//                 // Fetch the AboutPage
//                 var query = new ContentItemQueryBuilder()
//                     .ForContentType(AboutPage.CONTENT_TYPE_NAME, config => config
//                         .ForWebsite("KwFeeds", PathMatch.Single("/About")));

//                 var pages = await executor.GetMappedWebPageResult<AboutPage>(query);
//                 logger.LogInformation("Pages fetched: {Count}", pages?.Count() ?? 0);

//                 var page = pages.FirstOrDefault();

//                 if (page == null)
//                 {
//                     logger.LogWarning("AboutPage not found.");
//                     return NotFound();
//                 }

//                 logger.LogInformation("AboutPage found. Title: {Title}",
//                     page.title);

//                 // Fetch all About items
//                 var productQuery = new ContentItemQueryBuilder()
//                     .ForContentType(About.CONTENT_TYPE_NAME, config => { });

//                 var allSections = await executor.GetMappedWebPageResult<About>(productQuery);
//                 logger.LogInformation("All products fetched: {Count}", allSections?.Count() ?? 0);

//                 // Create the view model for the view
//                 var viewModel = new AboutPageViewModel
//                 {
//                     Title = page.title,
//                     Sections = allSections.ToList()
//                 };

//                 logger.LogInformation("ViewModel created with {Count} products.", viewModel.Sections.Count);

//                 return View("Views/About/Index.cshtml", viewModel);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "An error occurred while fetching the product data.");
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
// }