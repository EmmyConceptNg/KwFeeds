using System.Collections.Generic;

using CMS.Websites;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a common news page model.
    /// </summary>
    public interface INewsPage : IWebPageFieldsSource
    {
        /// <summary>
        /// Get news related item.
        /// </summary>
        public IEnumerable<INewsFields> RelatedItem { get; }
    }
}
