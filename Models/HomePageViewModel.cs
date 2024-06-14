namespace KwFeeds.Models
{
    public class HomePageViewModel
    {
        public string Title { get; init; }
        public HomePageViewModel(KwHomePage homePage)
        {
            Title = homePage.Title;
        }
    }
}