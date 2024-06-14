namespace KwFeeds.Models
{
    public class ContactPageViewModel
    {
        public string Title { get; init; }
        
        public string ContactFormContent { get; init; }
        
        public ContactPageViewModel(ContactPage contactPage)
        {
            Title = contactPage.Title;
            ContactFormContent = contactPage.ContactFormContent;
            
        }
    }
}