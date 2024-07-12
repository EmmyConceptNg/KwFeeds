using System;
using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record RelatedArticleViewModel(string Title, string TeaserUrl, string Summary, string Text, DateTime PublicationDate, Guid Guid, string Url)
    {
        /// <summary>
        /// Validates and maps <see cref="AboutPage"/> to a <see cref="RelatedArticleViewModel"/>.
        /// </summary>
        public static async Task<RelatedArticleViewModel> GetViewModel(AboutPage aboutPage, IWebPageUrlRetriever urlRetriever, string languageName)
        {
            var url = await urlRetriever.Retrieve(aboutPage, languageName);

            return new RelatedArticleViewModel
            (
                aboutPage.ArticleTitle,
                aboutPage.ArticlePageTeaser.FirstOrDefault()?.ImageFile.Url,
                aboutPage.ArticlePageSummary,
                aboutPage.ArticlePageText,
                aboutPage.ArticlePagePublishDate,
                aboutPage.SystemFields.ContentItemGUID,
                url.RelativePath
            );
        }
    }
}
