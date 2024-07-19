using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Models
{
    
    public record TeamViewModel(string JobTitle, string HeroBannerImageUrl, string HeroBannerShortDescription, string Content, string Name)
    {
        /// <summary>
        /// Validates and maps <see cref="Team"/> to a <see cref="TeamViewModel"/>.
        /// </summary>
        public static TeamViewModel GetViewModel(Team teamContentItem)
        {
            if (teamContentItem == null)
            {
                return null;
            }

            var bannerImage = teamContentItem.TeamImage.FirstOrDefault();

            return new TeamViewModel(
                teamContentItem.JobTitle,
                bannerImage?.ImageFile.Url,
                bannerImage?.ImageShortDescription,
                teamContentItem.Content,
                teamContentItem.Name
            );
           
        }
    }
}
