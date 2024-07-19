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
    /// Represents a collection of cafes.
    /// </summary>
    public partial class TeamRepository : ContentRepositoryBase
    {
        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;


        public TeamRepository(
            IWebsiteChannelContext websiteChannelContext,
            IContentQueryExecutor executor,
            IProgressiveCache cache,
            ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever)
            : base(websiteChannelContext, executor, cache)
        {
            this.linkedItemsDependencyRetriever = linkedItemsDependencyRetriever;
        }

        /// <summary>
        /// Returns an enumerable collection of company cafes ordered by a position in the content tree.
        /// </summary>
        public async Task<IEnumerable<Team>> GetCompanyTeams(int count, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(count, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(TeamRepository), languageName, nameof(GetCompanyTeams), count);

            return await GetCachedQueryResult<Team>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);
        }

        private static ContentItemQueryBuilder GetQueryBuilder(int count, string languageName)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(Team.CONTENT_TYPE_NAME,
                        config => config
                            .WithLinkedItems(1)
                            .TopN(count))
                    .InLanguage(languageName);
        }


        private async Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<Team> teams, CancellationToken cancellationToken)
        {
            var teamIds = teams.Select(team => team.SystemFields.ContentItemID);
            var dependencyCacheKeys = (await linkedItemsDependencyRetriever.Get(teamIds, 1, cancellationToken))
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "contentitem", "bycontenttype", Team.CONTENT_TYPE_NAME }, false));

            return dependencyCacheKeys;
        }
    }
}
