using System.Linq;
using System.Threading.Tasks;

using CMS.Websites;

namespace DancingGoat.Models
{
    public record NewsListItemViewModel(string Name, string Description, string ShortDescription, string ImagePath, string Url)
    {
        public static async Task<NewsListItemViewModel> GetViewModel(INewsPage newsPage, IWebPageUrlRetriever urlRetriever, string languageName)
        {
            var news = newsPage.RelatedItem.FirstOrDefault();
            var image = news.NewsFieldsImage.FirstOrDefault();
            

            var path = (await urlRetriever.Retrieve(newsPage, languageName)).RelativePath;

            return new NewsListItemViewModel(
                news.NewsFieldsName,
                news.NewsFieldsDescription,
                news.NewsFieldsShortDescription,
                image?.ImageFile.Url,
                path
            );
        }

        
    }
}
