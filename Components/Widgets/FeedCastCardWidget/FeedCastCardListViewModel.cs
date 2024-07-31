using System.Collections.Generic;
using System.Linq;

using DancingGoat.Models;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for FeedCast card widget.
    /// </summary>
    public class FeedCastCardListViewModel
    {
        /// <summary>
        /// Collection of FeedCasts.
        /// </summary>
        public IEnumerable<FeedCastCardViewModel> FeedCasts { get; set; }


        /// <summary>
        /// Gets ViewModels for <paramref name="feedcasts"/>.
        /// </summary>
        /// <param name="feedcasts">Collection of feedcasts.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static FeedCastCardListViewModel GetViewModel(IEnumerable<IFeedCastFields> feedcasts)
        {
            var feedcastModels = new List<FeedCastCardViewModel>();

            foreach (var feedcast in feedcasts.Where(feedcast => feedcast != null))
            {
                var feedcastModel = FeedCastCardViewModel.GetViewModel(feedcast);
                feedcastModels.Add(feedcastModel);
            }

            return new FeedCastCardListViewModel
            {
                FeedCasts = feedcastModels
            };
        }
    }
}
