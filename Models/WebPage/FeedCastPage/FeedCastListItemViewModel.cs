using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record FeedCastListItemViewModel(string Name, string Description, string ShortDescription, string ImagePath, string Url)
    {
        public static async Task<FeedCastListItemViewModel> GetViewModel(IFeedCastPage feedcastPage, IWebPageUrlRetriever urlRetriever, string languageName)
        {
            var feedcast = feedcastPage.RelatedItem.FirstOrDefault();
            var image = feedcast.FeedCastFieldsImage.FirstOrDefault();

            var path = (await urlRetriever.Retrieve(feedcastPage, languageName)).RelativePath;

            return new FeedCastListItemViewModel(
                feedcast.FeedCastFieldsName,
                feedcast.FeedCastFieldsDescription,
                feedcast.FeedCastFieldsShortDescription,
                image?.ImageFile.Url,
                path
            );
        }
    }
}
