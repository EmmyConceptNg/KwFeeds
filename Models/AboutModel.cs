using System;

namespace KwFeeds.Models
{
    public class AboutViewModel
    {
        public string Heading { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public AboutViewModel(About about)
        {
            if (about == null) throw new ArgumentNullException(nameof(about));

            Heading = about.Heading;
            Content = about.Content;
            ImageUrl = about.Image.Url; // Assuming Image has a Url property
        }
    }
}