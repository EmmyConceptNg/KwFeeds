﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Models
{
    // public record EventViewModel(string Title, string HeroBannerImageUrl, string HeroBannerShortDescription, string PromoText, DateTime Date, string Location, IEnumerable<string> Coffees)
    public record EventViewModel(string Title, string HeroBannerImageUrl, string HeroBannerShortDescription, string PromoText, DateTime Date, string Location)
    {
        /// <summary>
        /// Validates and maps <see cref="Event"/> to a <see cref="EventViewModel"/>.
        /// </summary>
        public static EventViewModel GetViewModel(Event eventContentItem)
        {
            if (eventContentItem == null)
            {
                return null;
            }

            var bannerImage = eventContentItem.EventHeroBannerImage.FirstOrDefault();
            var cafe = eventContentItem.EventCafe?.FirstOrDefault();

            return new EventViewModel(
                eventContentItem.EventTitle,
                bannerImage?.ImageFile.Url,
                bannerImage?.ImageShortDescription,
                eventContentItem.EventPromoText,
                eventContentItem.EventDate,
                cafe?.Name
            );
            // return new EventViewModel(
            //     eventContentItem.EventTitle,
            //     bannerImage?.ImageFile.Url,
            //     bannerImage?.ImageShortDescription,
            //     eventContentItem.EventPromoText,
            //     eventContentItem.EventDate,
            //     cafe?.Name,
            //     cafe?.CafeCuppingOffer.Select(coffee => coffee.ProductFieldsName)
            // );
        }
    }
}
