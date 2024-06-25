using System;
using CMS.ContentEngine;
using CMS.Websites;

namespace KwFeeds
{
    [RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
    public partial class SingleProduct : IWebPageFieldsSource
    {
        /// <summary>
        /// Code name of the content type.
        /// </summary>
        public const string CONTENT_TYPE_NAME = "KwFeeds.SingleProduct";

        /// <summary>
        /// Represents system properties for a web page item.
        /// </summary>
        [SystemField]
        public WebPageFields SystemFields { get; set; } = new WebPageFields();

        public string Title { get; set; } = string.Empty;
        public string ProductLogo { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }

        public Guid Guid { get; set; }
    }
}