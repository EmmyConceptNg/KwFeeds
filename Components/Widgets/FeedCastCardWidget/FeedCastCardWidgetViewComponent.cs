using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(FeedCastCardWidgetViewComponent.IDENTIFIER, typeof(FeedCastCardWidgetViewComponent), "Feed Cast cards", typeof(FeedCastCardProperties), Description = "Displays feedcast.", IconClass = "icon-box")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for feedcast card widget.
    /// </summary>
    public class FeedCastCardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.FeedCastCardWidget";


        private readonly FeedCastRepository repository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;


        /// <summary>
        /// Creates an instance of <see cref="FeedCastCardWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Repository for retrieving FeedCasts.</param>
        /// <param name="currentLanguageRetriever">Retrieves preferred language name for the current request. Takes language fallback into account.</param>
        public FeedCastCardWidgetViewComponent(FeedCastRepository repository, IPreferredLanguageRetriever currentLanguageRetriever)
        {
            this.repository = repository;
            this.currentLanguageRetriever = currentLanguageRetriever;
        }


        public async Task<ViewViewComponentResult> InvokeAsync(FeedCastCardProperties properties)
        {
            var languageName = currentLanguageRetriever.Get();
            var selectedFeedCastGuids = properties.SelectedFeedCasts.Select(i => i.Identifier).ToList();
            var feedcasts = (await repository.GetFeedCasts(selectedFeedCastGuids, languageName))
                                            .OrderBy(p => selectedFeedCastGuids.IndexOf(((IContentItemFieldsSource)p).SystemFields.ContentItemGUID));
            var model = FeedCastCardListViewModel.GetViewModel(feedcasts);

            return View("~/Components/Widgets/FeedCastCardWidget/_FeedCastCardWidget.cshtml", model);
        }
    }
}
