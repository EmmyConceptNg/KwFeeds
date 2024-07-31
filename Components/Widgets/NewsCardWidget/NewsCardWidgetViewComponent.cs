using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;

using DancingGoat.Models;
using DancingGoat.Widgets;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(NewsCardWidgetViewComponent.IDENTIFIER, typeof(NewsCardWidgetViewComponent), "News cards", typeof(NewsCardProperties), Description = "Displays news.", IconClass = "icon-box")]

namespace DancingGoat.Widgets
{
    /// <summary>
    /// Controller for news card widget.
    /// </summary>
    public class NewsCardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "DancingGoat.LandingPage.NewsCardWidget";


        private readonly NewsRepository repository;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;


        /// <summary>
        /// Creates an instance of <see cref="NewsCardWidgetViewComponent"/> class.
        /// </summary>
        /// <param name="repository">Repository for retrieving News.</param>
        /// <param name="currentLanguageRetriever">Retrieves preferred language name for the current request. Takes language fallback into account.</param>
        public NewsCardWidgetViewComponent(NewsRepository repository, IPreferredLanguageRetriever currentLanguageRetriever)
        {
            this.repository = repository;
            this.currentLanguageRetriever = currentLanguageRetriever;
        }


        public async Task<ViewViewComponentResult> InvokeAsync(NewsCardProperties properties)
        {
            var languageName = currentLanguageRetriever.Get();
            var selectedNewsGuids = properties.SelectedNews.Select(i => i.Identifier).ToList();
            var news = (await repository.GetNews(selectedNewsGuids, languageName))
                                            .OrderBy(p => selectedNewsGuids.IndexOf(((IContentItemFieldsSource)p).SystemFields.ContentItemGUID));
            var model = NewsCardListViewModel.GetViewModel(news);

            return View("~/Components/Widgets/NewsCardWidget/_NewsCardWidget.cshtml", model);
        }
    }
}
