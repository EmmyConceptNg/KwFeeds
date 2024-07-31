using System.Linq;

using DancingGoat.Models;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for FeedCast card widget.
    /// </summary>
    public class FeedCastCardViewModel
    {
        /// <summary>
        /// Card heading.
        /// </summary>
        public string Heading { get; set; }


        /// <summary>
        /// Card background image path.
        /// </summary>
        public string ImagePath { get; set; }


        /// <summary>
        /// Card text.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Gets ViewModel for <paramref name="feedcast"/>.
        /// </summary>
        /// <param name="feedcast">feedcast.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static FeedCastCardViewModel GetViewModel(IFeedCastFields feedcast)
        {
            if (feedcast == null)
            {
                return null;
            }

            return new FeedCastCardViewModel
            {
                Heading = feedcast.FeedCastFieldsName,
                ImagePath = feedcast.FeedCastFieldsImage.FirstOrDefault()?.ImageFile.Url,
                Text = feedcast.FeedCastFieldsShortDescription
            };
        }
    }
}
