using System.Collections.Generic;

using CMS.ContentEngine;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// FeedCast card widget properties.
    /// </summary>
    public class FeedCastCardProperties : IWidgetProperties
    {
        /// <summary>
        /// Selected FeedCasts.
        /// </summary>
        [ContentItemSelectorComponent(typeof(FeedCastCardSchemaFilter), Label = "Selected FeedCasts", Order = 1)]
        public IEnumerable<ContentItemReference> SelectedFeedCasts { get; set; } = new List<ContentItemReference>();
    }
}
