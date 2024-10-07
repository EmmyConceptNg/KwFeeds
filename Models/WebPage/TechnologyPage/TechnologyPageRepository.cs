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
    public class TechnologyPageRepository : ContentRepositoryBase
    {
        /// <summary>
        /// Initializes new instance of <see cref="TechnologyPageRepository"/>.
        /// </summary>
        public TechnologyPageRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache)
            : base(websiteChannelContext, executor, cache)
        {
        }


        /// <summary>
        /// Returns <see cref="TechnologyPage"/> web page by ID and language name.
        /// </summary>
        public async Task<TechnologyPage> GetTechnologyPage(int webPageItemId, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(webPageItemId, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(TechnologyPage), webPageItemId, languageName);

            var result = await GetCachedQueryResult<TechnologyPage>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private ContentItemQueryBuilder GetQueryBuilder(int webPageItemId, string languageName)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(TechnologyPage.CONTENT_TYPE_NAME, config => config
                        .ForWebsite(WebsiteChannelContext.WebsiteChannelName, includeUrlPath: false)
                    .Where(where => where
                            .WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemID), webPageItemId))
                    .TopN(1))
                    .InLanguage(languageName);
        }


        private static Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<TechnologyPage> technologyPages, CancellationToken cancellationToken)
        {
            var dependencyCacheKeys = new HashSet<string>();

            var technologyPage = technologyPages.FirstOrDefault();
            if (technologyPage != null)
            {
                dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", technologyPage.SystemFields.WebPageItemID.ToString() }, false));
            }

            return Task.FromResult<ISet<string>>(dependencyCacheKeys);
        }
    }
}
