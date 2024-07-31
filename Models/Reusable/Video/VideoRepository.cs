using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites.Routing;

namespace DancingGoat.Models
{
    public class VideoRepository : ContentRepositoryBase
    {
        public VideoRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache)
            : base(websiteChannelContext, executor, cache)
        {
        }


        /// <summary>
        /// Returns <see cref="Video"/> content item.
        /// </summary>
        public async Task<Video> GetVideo(Guid videoGuid, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(videoGuid, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(VideoRepository), nameof(GetVideo), languageName, videoGuid);

            var result = await GetCachedQueryResult<Video>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);

            return result.FirstOrDefault();
        }


        private static ContentItemQueryBuilder GetQueryBuilder(Guid videoGuid, string languageName)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(Video.CONTENT_TYPE_NAME,
                        config => config
                                .TopN(1)
                                .Where(where => where.WhereEquals(nameof(IContentQueryDataContainer.ContentItemGUID), videoGuid)))
                    .InLanguage(languageName);
        }


        private static Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<Video> videos, CancellationToken cancellationToken)
        {
            var dependencyCacheKeys = new HashSet<string>();

            var video = videos.FirstOrDefault();

            if (video != null)
            {
                dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "contentitem", "byid", video.SystemFields.ContentItemID.ToString() }, false));
            }

            return Task.FromResult<ISet<string>>(dependencyCacheKeys);
        }
    }
}
