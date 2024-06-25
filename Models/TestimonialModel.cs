using System;

namespace KwFeeds.Models
{
    public class TestimonialViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public TestimonialViewModel(Testimonial testimonial)
        {
            if (testimonial == null) throw new ArgumentNullException(nameof(testimonial));

            Name = testimonial.Name;
            Title = testimonial.Title;
            Content = testimonial.Content;
            ImageUrl = testimonial.Image.Url; // Assuming Image has a Url property
        }
    }
}