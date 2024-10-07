﻿using DancingGoat.Models;
using DancingGoat.ViewComponents;

using Microsoft.Extensions.DependencyInjection;

namespace DancingGoat
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Injects DG services into the IoC container.
        /// </summary>
        public static void AddDancingGoatServices(this IServiceCollection services)
        {
            AddViewComponentServices(services);
            AddRepositories(services);

            services.AddSingleton<ICurrentWebsiteChannelPrimaryLanguageRetriever, CurrentWebsiteChannelPrimaryLanguageRetriever>();
        }


        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<SocialLinkRepository>();
            services.AddSingleton<ContactRepository>();
            services.AddSingleton<HomePageRepository>();
            services.AddSingleton<ArticlePageRepository>();
            services.AddSingleton<ArticlesSectionRepository>();
            services.AddSingleton<ConfirmationPageRepository>();
            services.AddSingleton<ImageRepository>();
            services.AddSingleton<CafeRepository>();
            services.AddSingleton<NavigationItemRepository>();
            services.AddSingleton<ContactsPageRepository>();
            services.AddSingleton<PrivacyPageRepository>();
            services.AddSingleton<LandingPageRepository>();
            services.AddSingleton<ProductSectionRepository>();
            services.AddSingleton<ProductPageRepository>();
            services.AddSingleton<ProductRepository>();
            services.AddSingleton<FeedCastSectionRepository>();
            services.AddSingleton<FeedCastPageRepository>();
            services.AddSingleton<FeedCastRepository>();
            services.AddSingleton<NewsSectionRepository>();
            services.AddSingleton<NewsPageRepository>();
            services.AddSingleton<NewsRepository>();
            services.AddSingleton<TechnologyPageRepository>();
            services.AddSingleton<NutritionPageRepository>();
            services.AddSingleton<TraffordGoldPageRepository>();
            services.AddSingleton<TeamRepository>();
            services.AddSingleton<TestimonialRepository>();
            services.AddSingleton<TestimonialPageRepository>();
        }


        private static void AddViewComponentServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationService>();
        }
    }
}
