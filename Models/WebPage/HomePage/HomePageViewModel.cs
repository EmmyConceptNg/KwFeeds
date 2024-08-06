using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;

namespace DancingGoat.Models
{
    public record HomePageViewModel(BannerViewModel Banner, IEnumerable<ProductListItemViewModel> Products, WebPageRelatedItem ArticlesSection)
        : IWebPageBasedViewModel
    {
        public IWebPageFieldsSource WebPage { get; init; }

        public static async Task<HomePageViewModel> GetViewModel(HomePage home, IWebPageUrlRetriever urlRetriever, string languageName, ProductPageRepository productPageRepository)
        {
            if (home == null)
            {
                return null;
            }

            // Ensure the correct types in linkedProducts
            var linkedProducts = home.Products.SelectMany(p => p.RelatedItem).ToList();
            var productPages = await productPageRepository.GetProducts(home.SystemFields.WebPageItemTreePath, languageName, linkedProducts);

            // Filter to ensure only IProductPage types are processed
            var validProductPages = productPages.Where(page => page is IProductPage).Cast<IProductPage>();

            // Create the view models for valid product pages
            var productTasks = validProductPages.Select(productPage =>
                ProductListItemViewModel.GetViewModel(productPage, urlRetriever, languageName));
            var productViewModels = await Task.WhenAll(productTasks);

            return new HomePageViewModel(
                BannerViewModel.GetViewModel(home.HomePageBanner.FirstOrDefault()),
                productViewModels,
                home.HomePageArticlesSection.FirstOrDefault())
            {
                WebPage = home
            };
        }
    }
}
