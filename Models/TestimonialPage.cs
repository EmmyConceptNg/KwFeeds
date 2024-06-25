using System.Collections.Generic;

namespace KwFeeds.Models
{
    public class TestimonialPageViewModel
    {
        public string Title { get; init; }

        public List<Testimonial> Sections { get; set; } = new List<Testimonial>();

    }
}