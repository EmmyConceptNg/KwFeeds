using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of news pages.
    /// </summary>
    public class NewsPageRepository : ContentRepositoryBase
    {
        private readonly IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever;


        /// <summary>
        /// Initializes new instance of <see cref="NewsPageRepository"/>.
        /// </summary>
        public NewsPageRepository(
            IWebsiteChannelContext websiteChannelContext,
            IContentQueryExecutor executor,
            IProgressiveCache cache,
            IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever)
            : base(websiteChannelContext, executor, cache)
        {
            this.webPageLinkedItemsDependencyRetriever = webPageLinkedItemsDependencyRetriever;
        }


        /// <summary>
        /// Returns list of <see cref="NewsPage"/> web pages.
        /// </summary>
        public async Task<IEnumerable<INewsPage>> GetNews(string treePath, string languageName, IEnumerable<INewsFields> linkedNews, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
        {
            if (!linkedNews.Any())
            {
                return Enumerable.Empty<INewsPage>();
            }

            var queryBuilder = GetQueryBuilder(treePath, languageName, linkedNews);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var linkedNewsCacheParts = linkedNews.Select(news => news.NewsFieldsName).Join("|");
            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, treePath, languageName, includeSecuredItems, nameof(INewsPage), linkedNewsCacheParts);

            return await GetCachedQueryResult<INewsPage>(queryBuilder, options, cacheSettings, GetDependencyCacheKeys, cancellationToken);
        }


        public async Task<NewsPageType> GetNews<NewsPageType>(string contentTypeName, int id, string languageName, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
            where NewsPageType : IWebPageFieldsSource, new()
        {
            var queryBuilder = GetQueryBuilder(id, languageName, contentTypeName);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(NewsPageType), id, languageName);

            var result = await GetCachedQueryResult<NewsPageType>(queryBuilder, options, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private ContentItemQueryBuilder GetQueryBuilder(string treePath, string languageName, IEnumerable<INewsFields> linkedNews)
        {
            return GetQueryBuilder(
                languageName,
                config => config
                    .Linking(nameof(INewsPage.RelatedItem), linkedNews.Select(linkedNews => ((IContentItemFieldsSource)linkedNews).SystemFields.ContentItemID))
                    .WithLinkedItems(2)
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Children(treePath)));
        }


        private static ContentItemQueryBuilder GetQueryBuilder(string languageName, Action<ContentTypeQueryParameters> configure = null)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(SingleNewsPage.CONTENT_TYPE_NAME, configure)
                    .ForContentType(GrinderPage.CONTENT_TYPE_NAME, configure)
                    .InLanguage(languageName);
        }


        private ContentItemQueryBuilder GetQueryBuilder(int id, string languageName, string contentTypeName)
        {
            return GetQueryBuilder(
                languageName,
                contentTypeName,
                config => config
                    .WithLinkedItems(2)
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                    .Where(where => where.WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemID), id)));
        }


        private static ContentItemQueryBuilder GetQueryBuilder(string languageName, string contentTypeName, Action<ContentTypeQueryParameters> configureQuery = null)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(contentTypeName, configureQuery)
                    .InLanguage(languageName);
        }


        private async Task<ISet<string>> GetDependencyCacheKeys<NewsPageType>(IEnumerable<NewsPageType> news, CancellationToken cancellationToken)
            where NewsPageType : IWebPageFieldsSource
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var _news in news)
            {
                dependencyCacheKeys.UnionWith(GetDependencyCacheKeys(_news));
            }

            dependencyCacheKeys.UnionWith(await webPageLinkedItemsDependencyRetriever.Get(news.Select(newsPage => newsPage.SystemFields.WebPageItemID), 1, cancellationToken));
            dependencyCacheKeys.Add(CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID));

            return dependencyCacheKeys;
        }


        private IEnumerable<string> GetDependencyCacheKeys(IWebPageFieldsSource news)
        {
            if (news == null)
            {
                return Enumerable.Empty<string>();
            }

            return new List<string>()
            {
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", news.SystemFields.WebPageItemID.ToString() }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", WebsiteChannelContext.WebsiteChannelName, "bypath", news.SystemFields.WebPageItemTreePath }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", WebsiteChannelContext.WebsiteChannelName, "childrenofpath", DataHelper.GetParentPath(news.SystemFields.WebPageItemTreePath) }, false),
                CacheHelper.GetCacheItemName(null, ContentLanguageInfo.OBJECT_TYPE, "all")
            };
        }
    }
}
