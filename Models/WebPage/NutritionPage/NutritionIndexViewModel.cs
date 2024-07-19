using System.Collections.Generic;

using CMS.Websites;

namespace DancingGoat.Models
{
    public class NutritionIndexViewModel : IWebPageBasedViewModel
    {
        /// <summary>
        /// The company contact data.
        /// </summary>
        public ContactViewModel CompanyContact { get; set; }

        public string Title { get; init; }
        public string Content { get; init; }


        /// <summary>
        /// The company cafes data.
        /// </summary>
        public List<CafeViewModel> CompanyCafes { get; set; }


        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
        public IEnumerable<TeamViewModel> Teams { get; set; }
    }
}
