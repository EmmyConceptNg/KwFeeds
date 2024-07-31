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
    /// Represents a collection of FeedCast pages.
    /// </summary>
    public class FeedCastPageRepository : ContentRepositoryBase
    {
        private readonly IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever;


        /// <summary>
        /// Initializes new instance of <see cref="FeedCastPageRepository"/>.
        /// </summary>
        public FeedCastPageRepository(
            IWebsiteChannelContext websiteChannelContext,
            IContentQueryExecutor executor,
            IProgressiveCache cache,
            IWebPageLinkedItemsDependencyAsyncRetriever webPageLinkedItemsDependencyRetriever)
            : base(websiteChannelContext, executor, cache)
        {
            this.webPageLinkedItemsDependencyRetriever = webPageLinkedItemsDependencyRetriever;
        }


        /// <summary>
        /// Returns list of <see cref="FeedCastPage"/> web pages.
        /// </summary>
        public async Task<IEnumerable<IFeedCastPage>> GetFeedCasts(string treePath, string languageName, IEnumerable<IFeedCastFields> linkedFeedCasts, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
        {
            if (!linkedFeedCasts.Any())
            {
                return Enumerable.Empty<IFeedCastPage>();
            }

            var queryBuilder = GetQueryBuilder(treePath, languageName, linkedFeedCasts);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var linkedFeedCastCacheParts = linkedFeedCasts.Select(feedcast => feedcast.FeedCastFieldsName).Join("|");
            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, treePath, languageName, includeSecuredItems, nameof(IFeedCastPage), linkedFeedCastCacheParts);

            return await GetCachedQueryResult<IFeedCastPage>(queryBuilder, options, cacheSettings, GetDependencyCacheKeys, cancellationToken);
        }


        public async Task<FeedCastPageType> GetFeedCast<FeedCastPageType>(string contentTypeName, int id, string languageName, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
            where FeedCastPageType : IWebPageFieldsSource, new()
        {
            var queryBuilder = GetQueryBuilder(id, languageName, contentTypeName);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(FeedCastPageType), id, languageName);

            var result = await GetCachedQueryResult<FeedCastPageType>(queryBuilder, options, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private ContentItemQueryBuilder GetQueryBuilder(string treePath, string languageName, IEnumerable<IFeedCastFields> linkedFeedCasts)
        {
            return GetQueryBuilder(
                languageName,
                config => config
                    .Linking(nameof(IFeedCastPage.RelatedItem), linkedFeedCasts.Select(linkedFeedCast => ((IContentItemFieldsSource)linkedFeedCast).SystemFields.ContentItemID))
                    .WithLinkedItems(2)
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Children(treePath)));
        }


        private static ContentItemQueryBuilder GetQueryBuilder(string languageName, Action<ContentTypeQueryParameters> configure = null)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(SingleFeedCastPage.CONTENT_TYPE_NAME, configure)
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


        private async Task<ISet<string>> GetDependencyCacheKeys<FeedCastPageType>(IEnumerable<FeedCastPageType> feedcasts, CancellationToken cancellationToken)
            where FeedCastPageType : IWebPageFieldsSource
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var feedcast in feedcasts)
            {
                dependencyCacheKeys.UnionWith(GetDependencyCacheKeys(feedcast));
            }

            dependencyCacheKeys.UnionWith(await webPageLinkedItemsDependencyRetriever.Get(feedcasts.Select(feedcastPage => feedcastPage.SystemFields.WebPageItemID), 1, cancellationToken));
            dependencyCacheKeys.Add(CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID));

            return dependencyCacheKeys;
        }


        private IEnumerable<string> GetDependencyCacheKeys(IWebPageFieldsSource feedcast)
        {
            if (feedcast == null)
            {
                return Enumerable.Empty<string>();
            }

            return new List<string>()
            {
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", feedcast.SystemFields.WebPageItemID.ToString() }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", WebsiteChannelContext.WebsiteChannelName, "bypath", feedcast.SystemFields.WebPageItemTreePath }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", WebsiteChannelContext.WebsiteChannelName, "childrenofpath", DataHelper.GetParentPath(feedcast.SystemFields.WebPageItemTreePath) }, false),
                CacheHelper.GetCacheItemName(null, ContentLanguageInfo.OBJECT_TYPE, "all")
            };
        }
    }
}
