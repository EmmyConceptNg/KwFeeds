using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace DancingGoat.Models
{
    /// <summary>
    /// Represents a collection of feedcast pages.
    /// </summary>
    public class FeedCastRepository : ContentRepositoryBase
    {
        


        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;
        private readonly IInfoProvider<TaxonomyInfo> taxonomyInfoProvider;


        /// <summary>
        /// Initializes new instance of <see cref="FeedCastRepository"/>.
        /// </summary>
        public FeedCastRepository(
            IWebsiteChannelContext websiteChannelContext,
            IContentQueryExecutor executor,
            IProgressiveCache cache,
            ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever,
            IInfoProvider<TaxonomyInfo> taxonomyInfoProvider)
            : base(websiteChannelContext, executor, cache)
        {
            this.linkedItemsDependencyRetriever = linkedItemsDependencyRetriever;
            this.taxonomyInfoProvider = taxonomyInfoProvider;
        }


        /// <summary>
        /// Returns list of <see cref="IFeedCastFields"/> content items.
        /// </summary>
        public Task<IEnumerable<IFeedCastFields>> GetFeedCasts(string languageName, IDictionary<string, TaxonomyViewModel> filter, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(languageName, filter: filter);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var filterCacheItemNameParts = filter.Values.Where(value => value != null && value.Tags != null).SelectMany(value => value.Tags.Where(tag => tag.IsChecked)).Select(id => id.Value.ToString()).Join("|");

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, languageName, includeSecuredItems, nameof(IFeedCastFields), filterCacheItemNameParts);

            return GetCachedQueryResult<IFeedCastFields>(queryBuilder, options, cacheSettings, (_, _) => GetDependencyCacheKeys(languageName), cancellationToken);
        }


        /// <summary>
        /// Returns list of <see cref="IFeedCastFields"/> content items.
        /// </summary>
        public Task<IEnumerable<IFeedCastFields>> GetFeedCasts(ICollection<Guid> feedcastGuids, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(languageName, feedcastGuids);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, languageName, nameof(IFeedCastFields), feedcastGuids.Select(guid => guid.ToString()).Join("|"));

            return GetCachedQueryResult<IFeedCastFields>(queryBuilder, new ContentQueryExecutionOptions(), cacheSettings, (_, _) => GetDependencyCacheKeys(languageName, feedcastGuids), cancellationToken);
        }


        private static ContentItemQueryBuilder GetQueryBuilder(string languageName, ICollection<Guid> feedcastGuids = null, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var baseBuilder = new ContentItemQueryBuilder().ForContentTypes(ct =>
                {
                    ct.OfReusableSchema(IFeedCastFields.REUSABLE_FIELD_SCHEMA_NAME)
                      .WithContentTypeFields()
                      .WithLinkedItems(1);
                }).InLanguage(languageName);

            if (feedcastGuids != null)
            {
                baseBuilder.Parameters(query => query.Where(where => where.WhereIn(nameof(IContentQueryDataContainer.ContentItemGUID), feedcastGuids)));
            }
                return baseBuilder;
        }


        private static async Task<TagCollection> GetSelectedTags(IDictionary<string, TaxonomyViewModel> filter, string taxonomyName)
        {
            if (filter.TryGetValue(taxonomyName, out var taxonomy))
            {
                return await taxonomy.GetSelectedTags();
            }

            return null;
        }


        private async Task<ISet<string>> GetDependencyCacheKeys(string languageName, ICollection<Guid> feedcastGuids = null)
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID),
                CacheHelper.GetCacheItemName(null, "contentitem", "bycontenttype", SingleFeedCast.CONTENT_TYPE_NAME, languageName),
                CacheHelper.GetCacheItemName(null, "contentitem", "bycontenttype", Grinder.CONTENT_TYPE_NAME, languageName),
                
            };
            GetFeedCastPageDependencies(feedcastGuids, dependencyCacheKeys);

            return dependencyCacheKeys;
        }


        private static void GetFeedCastPageDependencies(ICollection<Guid> feedcastGuids, HashSet<string> dependencyCacheKeys)
        {
            if (feedcastGuids == null || !feedcastGuids.Any())
            {
                return;
            }

            foreach (var guid in feedcastGuids)
            {
                dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "contentitem", "byguid", guid.ToString() }, false));
            }
        }


        private async Task<string> GetTaxonomyTagsCacheDependencyKey(string taxonomyName)
        {
            var taxonomyID = (await taxonomyInfoProvider.GetAsync(taxonomyName)).TaxonomyID;
            return CacheHelper.GetCacheItemName(null, TaxonomyInfo.OBJECT_TYPE, "byid", taxonomyID, "children");
        }
    }
}
