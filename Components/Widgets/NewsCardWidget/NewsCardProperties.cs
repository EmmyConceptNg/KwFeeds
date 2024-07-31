using System.Collections.Generic;

using CMS.ContentEngine;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// News card widget properties.
    /// </summary>
    public class NewsCardProperties : IWidgetProperties
    {
        /// <summary>
        /// Selected news.
        /// </summary>
        [ContentItemSelectorComponent(typeof(NewsCardSchemaFilter), Label = "Selected news", Order = 1)]
        public IEnumerable<ContentItemReference> SelectedNews { get; set; } = new List<ContentItemReference>();
    }
}
