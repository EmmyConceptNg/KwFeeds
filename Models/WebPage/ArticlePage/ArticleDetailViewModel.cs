using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record ArticleDetailViewModel(string Title,  string Summary, string Text,  Guid Guid, bool IsSecured, string Url)
        : IWebPageBasedViewModel
    {
        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }


        /// <summary>
        /// Validates and maps <see cref="AboutPage"/> to a <see cref="ArticleDetailViewModel"/>.
        /// </summary>
        public static async Task<ArticleDetailViewModel> GetViewModel(AboutPage aboutPage, string languageName, ArticlePageRepository articlePageRepository, IWebPageUrlRetriever urlRetriever)
        {
            

          

            var url = await urlRetriever.Retrieve(aboutPage, languageName);

            return new ArticleDetailViewModel(
                aboutPage.ArticleTitle,
                aboutPage.ArticlePageSummary,
                aboutPage.ArticlePageText,
                aboutPage.SystemFields.ContentItemGUID,
                aboutPage.SystemFields.ContentItemIsSecured,
                url.RelativePath)
            {
                WebPage = aboutPage
            };
        }
    }
}
