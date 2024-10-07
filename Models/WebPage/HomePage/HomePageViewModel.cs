using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;

namespace DancingGoat.Models
{
    public record HomePageViewModel(BannerViewModel Banner, IEnumerable<SingleProductViewModel> Products, WebPageRelatedItem ArticlesSection)
        : IWebPageBasedViewModel
    {
        public IWebPageFieldsSource WebPage { get; init; }

        public static async Task<HomePageViewModel> GetViewModel(HomePage home, IWebPageUrlRetriever urlRetriever, string languageName, ProductPageRepository productPageRepository)
        {
            if (home == null)
            {
                return null;
            }

             var productViewModels = home.Products.Select(SingleProductViewModel.GetViewModel);

            return new HomePageViewModel(
                BannerViewModel.GetViewModel(home.HomePageBanner.FirstOrDefault()),
                   productViewModels,
                // productViewModels,
                home.HomePageArticlesSection.FirstOrDefault())
            {
                WebPage = home
            };
        }
    }
}
