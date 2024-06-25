using System.Collections.Generic;

namespace KwFeeds.Models
{
    public class ProductPageViewModel
    {
        public string Title { get; init; }
        public string PageHeaderContent { get; init; }

        public List<SingleProduct> Products { get; set; } = new List<SingleProduct>();


    }
}