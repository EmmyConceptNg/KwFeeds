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
    public class NewsSectionRepository : ContentRepositoryBase
    {
        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;


        public NewsSectionRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache, ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever)
            : base(websiteChannelContext, executor, cache)
        {
            this.linkedItemsDependencyRetriever = linkedItemsDependencyRetriever;
        }


        public async Task<NewsSection> GetNewsSection(int id, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(id, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(NewsSection), id, languageName);

            var result = await GetCachedQueryResult<NewsSection>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<NewsSection> newsSection, CancellationToken cancellationToken)
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var newSection in newsSection)
            {
                dependencyCacheKeys.UnionWith(GetDependencyCacheKeys(newSection));
            }

            dependencyCacheKeys.Add(CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID));

            return Task.FromResult<ISet<string>>(dependencyCacheKeys);
        }


        private static IEnumerable<string> GetDependencyCacheKeys(NewsSection newsSection)
        {
            if (newsSection == null)
            {
                return Enumerable.Empty<string>();
            }

            var cacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", newsSection.SystemFields.WebPageItemID.ToString() }, false),
                CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byguid", newsSection.SystemFields.WebPageItemGUID.ToString() }, false),
            };

            return cacheKeys;
        }


        private ContentItemQueryBuilder GetQueryBuilder(int id, string languageName)
        {
            return new ContentItemQueryBuilder()
                .ForContentType(NewsSection.CONTENT_TYPE_NAME,
                config =>
                    config
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                        .Where(where => where.WhereEquals(nameof(WebPageFields.WebPageItemID), id))
                        .TopN(1))
                .InLanguage(languageName);
        }
    }
}
