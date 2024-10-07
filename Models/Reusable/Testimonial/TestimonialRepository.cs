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
    public partial class TestimonialRepository : ContentRepositoryBase
    {
        private readonly ILinkedItemsDependencyAsyncRetriever linkedItemsDependencyRetriever;


        public TestimonialRepository(
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
        public async Task<IEnumerable<Testimonial>> GetCompanyTestimonials(int count, string languageName, CancellationToken cancellationToken = default)
        {
            var queryBuilder = GetQueryBuilder(count, languageName);

            var cacheSettings = new CacheSettings(5, WebsiteChannelContext.WebsiteChannelName, nameof(TestimonialRepository), languageName, nameof(GetCompanyTestimonials), count);

            return await GetCachedQueryResult<Testimonial>(queryBuilder, null, cacheSettings, GetDependencyCacheKeys, cancellationToken);
        }

        private static ContentItemQueryBuilder GetQueryBuilder(int count, string languageName)
        {
            return new ContentItemQueryBuilder()
                    .ForContentType(Testimonial.CONTENT_TYPE_NAME,
                        config => config
                            .WithLinkedItems(1)
                            .TopN(count))
                    .InLanguage(languageName);
        }


        private async Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<Testimonial> testimonilas, CancellationToken cancellationToken)
        {
            var testimonialIds = testimonilas.Select(testimonial => testimonial.SystemFields.ContentItemID);
            var dependencyCacheKeys = (await linkedItemsDependencyRetriever.Get(testimonialIds, 1, cancellationToken))
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "contentitem", "bycontenttype", Testimonial.CONTENT_TYPE_NAME }, false));

            return dependencyCacheKeys;
        }
    }
}
