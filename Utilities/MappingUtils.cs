using System.Collections.Generic;
using System.Linq;
using KwFeeds;  // Ensure the correct namespace is being used

namespace KwFeeds.Utilities
{
    public static class MappingUtils
    {
        public static List<Models.AboutViewModel> MapAboutToViewModel(IEnumerable<About> aboutSections)
        {
            return aboutSections?.Select(static about => new Models.AboutViewModel(about)).ToList() ?? new List<Models.AboutViewModel>();
        }
    }
}