using System.Linq;

using DancingGoat.Models;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// View model for News card widget.
    /// </summary>
    public class NewsCardViewModel
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
        /// Gets ViewModel for <paramref name="news"/>.
        /// </summary>
        /// <param name="news">News.</param>
        /// <returns>Hydrated ViewModel.</returns>
        public static NewsCardViewModel GetViewModel(INewsFields news)
        {
            if (news == null)
            {
                return null;
            }

            return new NewsCardViewModel
            {
                Heading = news.NewsFieldsName,
                ImagePath = news.NewsFieldsImage.FirstOrDefault()?.ImageFile.Url,
                Text = news.NewsFieldsShortDescription
            };
        }
    }
}
