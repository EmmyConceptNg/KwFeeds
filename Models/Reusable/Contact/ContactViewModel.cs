using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Models
{
    public record ContactViewModel(string LocationName, string HeroBannerImageUrl, string HeroBannerShortDescription, string About, string ContactName, string Address, string Phone, string Email, string Title)
    {
        /// <summary>
        /// Validates and maps <see cref="Contact"/> to a <see cref="ContactViewModel"/>.
        /// </summary>
        public static ContactViewModel GetViewModel(Contact contactContentItem)
        {
            if (contactContentItem is null)
            {
                return null;
            }

             var bannerImage = contactContentItem.Image.FirstOrDefault();

            return new ContactViewModel(
                contactContentItem.LocationName,
                 bannerImage?.ImageFile.Url,
                bannerImage?.ImageShortDescription,
                contactContentItem.About,
                contactContentItem.ContactName,
                contactContentItem.Address,
                contactContentItem.Phone,
                contactContentItem.Email,
                contactContentItem.Title
            );
        }
    }
}
