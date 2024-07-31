using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record ArticleDetailViewModel(string Title, string TeaserUrl, string Summary, string Text,  Guid Guid, bool IsSecured, string Url, IEnumerable<RelatedArticleViewModel> RelatedArticles)
        : IWebPageBasedViewModel
    {
        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }


        /// <summary>
        /// Validates and maps <see cref="AboutPage"/> to a <see cref="ArticleDetailViewModel"/>.
        /// </summary>
        public static async Task<ArticleDetailViewModel> GetViewModel(AboutPage aboutPage, string languageName, ArticlePageRepository articlePageRepository, IWebPageUrlRetriever urlRetriever)
        {
            var teaser = aboutPage.ArticlePageTeaser.FirstOrDefault();

            var relatedArticles = await articlePageRepository
                .GetArticles(aboutPage.ArticleRelatedArticles.Select(article => article.WebPageGuid).ToList(), languageName);

            var relatedArticlesViewModels = new List<RelatedArticleViewModel>();

            foreach (var relatedArticle in relatedArticles)
            {
                relatedArticlesViewModels.Add(await RelatedArticleViewModel.GetViewModel(relatedArticle, urlRetriever, languageName));
            }

            var url = await urlRetriever.Retrieve(aboutPage, languageName);

            return new ArticleDetailViewModel(
                aboutPage.ArticleTitle,
                teaser?.ImageFile.Url,
                aboutPage.ArticlePageSummary,
                aboutPage.ArticlePageText,
                aboutPage.SystemFields.ContentItemGUID,
                aboutPage.SystemFields.ContentItemIsSecured,
                url.RelativePath,
                relatedArticlesViewModels)
            {
                WebPage = aboutPage
            };
        }
    }
}
