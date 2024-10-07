using System;
using System.Collections.Generic;
using System.Linq;

namespace DancingGoat.Models
{
    
    public record TestimonialViewModel(string JobTitle, string HeroBannerImageUrl, string HeroBannerShortDescription, string Content, string Name)
    {
        /// <summary>
        /// Validates and maps <see cref="Testimonial"/> to a <see cref="TestimonialViewModel"/>.
        /// </summary>
        public static TestimonialViewModel GetViewModel(Testimonial testimonialContentItem)
        {
            if (testimonialContentItem == null)
            {
                return null;
            }

            var bannerImage = testimonialContentItem.Image.FirstOrDefault();

            return new TestimonialViewModel(
                testimonialContentItem.JobTitle,
                bannerImage?.ImageFile.Url,
                bannerImage?.ImageShortDescription,
                testimonialContentItem.Content,
                testimonialContentItem.Name
            );
           
        }
    }
}
