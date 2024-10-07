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
    /// Represents a collection of news pages.
    /// </summary>
    public class NewsRepository : ContentRepositoryBase
    {
        private const string ArticleType = "ArticleType";
        private const string TypeOfStock = "TypeOfStock";
        private const string StockAge = "StockAge";
        private const string ProductionStage = "ProductionStage";
        private const string InformationSource = "InformationSource";
        private const string ArticleLength = "ArticleLength";



        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;
        private readonly IInfoProvider<TaxonomyInfo> taxonomyInfoProvider;


        /// <summary>
        /// Initializes new instance of <see cref="NewsRepository"/>.
        /// </summary>
        public NewsRepository(
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
        /// Returns list of <see cref="INewsFields"/> content items.
        /// </summary>
        public Task<IEnumerable<INewsFields>> GetNews(string languageName, IDictionary<string, TaxonomyViewModel> filter, bool includeSecuredItems = true, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(languageName, filter: filter);

            var options = new ContentQueryExecutionOptions
            {
                IncludeSecuredItems = includeSecuredItems
            };

            var filterCacheItemNameParts = filter.Values.Where(value => value != null && value.Tags != null).SelectMany(value => value.Tags.Where(tag => tag.IsChecked)).Select(id => id.Value.ToString()).Join("|");

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, languageName, includeSecuredItems, nameof(INewsFields), filterCacheItemNameParts);

            return GetCachedQueryResult<INewsFields>(queryBuilder, options, cacheSettings, (_, _) => GetDependencyCacheKeys(languageName), cancellationToken);
        }


        /// <summary>
        /// Returns list of <see cref="INewsFields"/> content items.
        /// </summary>
        public Task<IEnumerable<INewsFields>> GetNews(ICollection<Guid> newsGuids, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(languageName, newsGuids);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, languageName, nameof(INewsFields), newsGuids.Select(guid => guid.ToString()).Join("|"));

            return GetCachedQueryResult<INewsFields>(queryBuilder, new ContentQueryExecutionOptions(), cacheSettings, (_, _) => GetDependencyCacheKeys(languageName, newsGuids), cancellationToken);
        }


        private static ContentItemQueryBuilder GetQueryBuilder(string languageName, ICollection<Guid> newsGuids = null, IDictionary<string, TaxonomyViewModel> filter = null)
        {
            var baseBuilder = new ContentItemQueryBuilder().ForContentTypes(ct =>
                {
                    ct.OfReusableSchema(INewsFields.REUSABLE_FIELD_SCHEMA_NAME)
                      .WithContentTypeFields()
                      .WithLinkedItems(1);
                }).InLanguage(languageName);

            if (newsGuids != null)
            {
                baseBuilder.Parameters(query => query.Where(where => where.WhereIn(nameof(IContentQueryDataContainer.ContentItemGUID), newsGuids)));
            }

            if (filter == null || !filter.Any())
            {
                return baseBuilder;
            }
            return baseBuilder
                .Parameters(query => query.Where(where => where
                    .Where(async singleNewsWhere => singleNewsWhere
                        .WhereContainsTags(nameof(SingleNews.ArticleType), await GetSelectedTags(filter, ArticleType))
                        .WhereContainsTags(nameof(SingleNews.TypeOfStock), await GetSelectedTags(filter, TypeOfStock))
                        .WhereContainsTags(nameof(SingleNews.StockAge), await GetSelectedTags(filter, StockAge))
                        .WhereContainsTags(nameof(SingleNews.ProductionStage), await GetSelectedTags(filter, ProductionStage))
                        .WhereContainsTags(nameof(SingleNews.InformationSource), await GetSelectedTags(filter, InformationSource))
                        .WhereContainsTags(nameof(SingleNews.InformationSource), await GetSelectedTags(filter, InformationSource))
                        .WhereContainsTags(nameof(SingleNews.ArticleLength), await GetSelectedTags(filter, ArticleLength))

                )));
        }


        private static async Task<TagCollection> GetSelectedTags(IDictionary<string, TaxonomyViewModel> filter, string taxonomyName)
        {
            if (filter.TryGetValue(taxonomyName, out var taxonomy))
            {
                return await taxonomy.GetSelectedTags();
            }

            return null;
        }


        private async Task<ISet<string>> GetDependencyCacheKeys(string languageName, ICollection<Guid> newsGuids = null)
        {
            var dependencyCacheKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                CacheHelper.GetCacheItemName(null, WebsiteChannelInfo.OBJECT_TYPE, "byid", WebsiteChannelContext.WebsiteChannelID),
                CacheHelper.GetCacheItemName(null, "contentitem", "bycontenttype", SingleNews.CONTENT_TYPE_NAME, languageName),
                CacheHelper.GetCacheItemName(null, "contentitem", "bycontenttype", Grinder.CONTENT_TYPE_NAME, languageName),
                await GetTaxonomyTagsCacheDependencyKey(ArticleType),
                await GetTaxonomyTagsCacheDependencyKey(TypeOfStock),
                await GetTaxonomyTagsCacheDependencyKey(StockAge),
                await GetTaxonomyTagsCacheDependencyKey(ProductionStage),
                await GetTaxonomyTagsCacheDependencyKey(InformationSource),
                await GetTaxonomyTagsCacheDependencyKey(ArticleLength)
            };
            GetNewsPageDependencies(newsGuids, dependencyCacheKeys);

            return dependencyCacheKeys;
        }


        private static void GetNewsPageDependencies(ICollection<Guid> newsGuids, HashSet<string> dependencyCacheKeys)
        {
            if (newsGuids == null || !newsGuids.Any())
            {
                return;
            }

            foreach (var guid in newsGuids)
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
