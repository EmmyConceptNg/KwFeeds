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
    public class FeedCastSectionRepository : ContentRepositoryBase
    {
        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;


        public FeedCastSectionRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache, ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever)
            : base(websiteChannelContext, executor, cache)
        {
            this.linkedItemsDependencyRetriever = linkedItemsDependencyRetriever;
        }


        public async Task<FeedCastsSection> GetFeedCastsSection(int id, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(id, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(FeedCastsSection), id, languageName);

            var result = await GetCachedQueryResult<FeedCastsSection>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<FeedCastsSection> feedcastsSection, CancellationToken cancellationToken)
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var feedcastSection in feedcastsSection)
            {
                dependencyCacheKeys.UnionWith(GetDependencyCacheKeys(feedcastSection));
            }

            dependencyCacheKeys.Add(CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID));

            return Task.FromResult<ISet<string>>(dependencyCacheKeys);
        }


        private static IEnumerable<string> GetDependencyCacheKeys(FeedCastsSection feedcastsSection)
        {
            if (feedcastsSection == null)
            {
                return Enumerable.Empty<string>();
            }

            var cacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", feedcastsSection.SystemFields.WebPageItemID.ToString() }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byguid", feedcastsSection.SystemFields.WebPageItemGUID.ToString() }, false),
            };

            return cacheKeys;
        }


        private ContentItemQueryBuilder GetQueryBuilder(int id, string languageName)
        {
            return new ContentItemQueryBuilder()
                .ForContentType(FeedCastsSection.CONTENT_TYPE_NAME,
                config =>
                    config
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereEquals(nameof(WebPageFields.WebPageItemID), id))
                        .TopN(1))
                .InLanguage(languageName);
        }
    }
}
