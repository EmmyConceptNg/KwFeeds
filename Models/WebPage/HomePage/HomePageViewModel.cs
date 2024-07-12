using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record HomePageViewModel(BannerViewModel Banner, IEnumerable<CafeViewModel> Products, WebPageRelatedItem ArticlesSection)
        : IWebPageBasedViewModel
    {
        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }


        /// <summary>
        /// Validates and maps <see cref="HomePage"/> to a <see cref="HomePageViewModel"/>.
        /// </summary>
        public static HomePageViewModel GetViewModel(HomePage home)
        {
            if (home == null)
            {
                return null;
            }

            return new HomePageViewModel(
                BannerViewModel.GetViewModel(home.HomePageBanner.FirstOrDefault()),
                home.Products.Select(CafeViewModel.GetViewModel),
                home.HomePageArticlesSection.FirstOrDefault())
            {
                WebPage = home
            };
        }
    }
}
