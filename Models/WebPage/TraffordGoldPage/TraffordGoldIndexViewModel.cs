using System.Collections.Generic;
using CMS.Websites;
using System.Linq;

namespace DancingGoat.Models
{
    public class TraffordGoldIndexViewModel : IWebPageBasedViewModel
    {
        /// <summary>
        /// The company contact data.
        /// </summary>
        public ContactViewModel CompanyContact { get; set; }

        public string Title { get; init; }
        public string Content { get; init; }

        // Get first or default image
        private IEnumerable<Image> _images;
        public IEnumerable<Image> Images
        {
            get => _images;
            set
            {
                _images = value;
                FirstImage = _images?.FirstOrDefault();
            }
        }
        public Image FirstImage { get; private set; }

        // Get first or default video
        private IEnumerable<Video> _videos;
        public IEnumerable<Video> Videos
        {
            get => _videos;
            set
            {
                _videos = value;
                FirstVideo = _videos?.FirstOrDefault();
            }
        }
        public Video FirstVideo { get; private set; }

        /// <summary>
        /// The company cafes data.
        /// </summary>
        public List<CafeViewModel> CompanyCafes { get; set; }

        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
        public IEnumerable<NewsListItemViewModel> NewsItems { get; set; }
    }
}