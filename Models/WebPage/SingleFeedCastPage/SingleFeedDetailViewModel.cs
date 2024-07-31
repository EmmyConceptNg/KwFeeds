using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;

namespace DancingGoat.Models
{
    public record SingleFeedCastDetailViewModel(string Name, string Description, string ShortDescription, string ImageUrl)
    {
        /// <summary>
        /// Maps <see cref="SingleFeedCastPage"/> to a <see cref="SingleFeedCastDetailViewModel"/>.
        /// </summary>
        public async static Task<SingleFeedCastDetailViewModel> GetViewModel(SingleFeedCastPage singleFeedCastPage)
        {
            var singleFeedCast = singleFeedCastPage.RelatedItem.FirstOrDefault();
            var image = singleFeedCast.FeedCastFieldsImage.FirstOrDefault();

            return new SingleFeedCastDetailViewModel(
                singleFeedCast.FeedCastFieldsName,
                singleFeedCast.FeedCastFieldsDescription,
                singleFeedCast.FeedCastFieldsShortDescription,
                image?.ImageFile.Url
            );
        }
    }
}
