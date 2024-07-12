using System;
using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record ArticleViewModel(string Title, string TeaserUrl, string Summary, string Text, DateTime PublicationDate, Guid Guid, bool IsSecured, string Url)
    {
        /// <summary>
        /// Validates and maps <see cref="AboutPage"/> to a <see cref="ArticleViewModel"/>.
        /// </summary>
        public static async Task<ArticleViewModel> GetViewModel(AboutPage aboutPage, IWebPageUrlRetriever urlRetriever, string languageName)
        {
            var teaser = aboutPage.ArticlePageTeaser.FirstOrDefault();

            var url = await urlRetriever.Retrieve(aboutPage, languageName);

            return new ArticleViewModel(
                aboutPage.ArticleTitle,
                teaser?.ImageFile.Url,
                aboutPage.ArticlePageSummary,
                aboutPage.ArticlePageText,
                aboutPage.ArticlePagePublishDate,
                aboutPage.SystemFields.ContentItemGUID,
                aboutPage.SystemFields.ContentItemIsSecured,
                url.RelativePath
            );
        }
    }
}
