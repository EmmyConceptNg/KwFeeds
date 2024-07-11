using CMS.MediaLibrary;
using System.Collections.Generic;
using KwFeeds;

namespace KwFeeds.Models
{
    public class HomePageViewModel
    {
        public string Title { get; set; }
        public IEnumerable<HomePageBanner> HeroSection { get; set; }
        public IEnumerable<SingleProduct> ProductsOfTheMonth { get; set; }
        public IEnumerable<SingleProduct> Products { get; set; }
        public IEnumerable<About> About { get; set; }

        public HomePageViewModel(KwHomePage page)
        {
            Title = "Default Title"; // Adjust based on the correct property
            HeroSection = page.hero_section ?? new List<HomePageBanner>(); // Ensure it's not null
            ProductsOfTheMonth = page.products_of_the_month ?? new List<SingleProduct>(); // Ensure it's not null
            Products = page.products ?? new List<SingleProduct>(); // Ensure it's not null
            About = page.about ?? new List<About>(); // Ensure it's not null
        }

        public class AboutModel
        {
            public string Tag { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public IEnumerable<AssetRelatedItem> Image { get; set; }

            public AboutModel(About about)
            {
                Tag = about.tag;
                Title = about.title;
                Content = about.content;
                Image = about.image;
            }
        }
    }
}