using System.Collections.Generic;

using CMS.Websites;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a common feedcast page model.
    /// </summary>
    public interface IFeedCastPage : IWebPageFieldsSource
    {
        /// <summary>
        /// Get FeedCast related item.
        /// </summary>
        public IEnumerable<IFeedCastFields> RelatedItem { get; }
    }
}
