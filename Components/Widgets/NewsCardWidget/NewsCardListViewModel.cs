using System.Collections.Generic;
using System.Linq;

using DancingGoat.Models;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for Product card widget.
    /// </summary>
    public class NewsCardListViewModel
    {
        /// <summary>
        /// Collection of news.
        /// </summary>
        public IEnumerable<NewsCardViewModel> News { get; set; }


        /// <summary>
        /// Gets ViewModels for <paramref name="news"/>.
        /// </summary>
        /// <param name="news">Collection of news.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static NewsCardListViewModel GetViewModel(IEnumerable<INewsFields> news)
        {
            var newsModels = new List<NewsCardViewModel>();

            foreach (var _news in news.Where(_news => _news != null))
            {
                var newsModel = NewsCardViewModel.GetViewModel(_news);
                newsModels.Add(newsModel);
            }

            return new NewsCardListViewModel
            {
                News = newsModels
            };
        }
    }
}
