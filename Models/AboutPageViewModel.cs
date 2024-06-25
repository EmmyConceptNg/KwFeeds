using System.Collections.Generic;

namespace KwFeeds.Models
{
    public class AboutPageViewModel
    {
        public string Title { get; init; }

        public List<About> Sections { get; set; } = new List<About>();

    }
}