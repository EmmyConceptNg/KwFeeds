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
    public class TraffordGoldPageRepository : ContentRepositoryBase
    {
        /// <summary>
        /// Initializes new instance of <see cref="TraffordGoldPageRepository"/>.
        /// </summary>
        public TraffordGoldPageRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache)
            : base(websiteChannelContext, executor, cache)
        {
        }


        /// <summary>
        /// Returns <see cref="TraffordGoldPage"/> web page by ID and language name.
        /// </summary>
        public async Task<TraffordGoldPage> GetTraffordGoldPage(int webPageItemId, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(webPageItemId, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(TraffordGoldPage), webPageItemId, languageName);

            var result = await GetCachedQueryResult<TraffordGoldPage>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private ContentItemQueryBuilder GetQueryBuilder(int webPageItemId, string languageName)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(TraffordGoldPage.CONTENT_TYPE_NAME, config => config
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, includeUrlPath: false)
                    .Where(where => where
                            .WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemID), webPageItemId))
                    .TopN(1))
                    .InLanguage(languageName);
        }


        private static Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<TraffordGoldPage> traffordgoldPages, CancellationToken cancellationToken)
        {
            var dependencyCacheKeys = new HashSet<string>();

            var traffordgoldPage = traffordgoldPages.FirstOrDefault();
            if (traffordgoldPage != null)
            {
                dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", traffordgoldPage.SystemFields.WebPageItemID.ToString() }, false));
            }

            return Task.FromResult<ISet<string>>(dependencyCacheKeys);
        }
    }
}
