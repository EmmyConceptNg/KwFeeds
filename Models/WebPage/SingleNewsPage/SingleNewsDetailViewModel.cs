using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;

namespace DancingGoat.Models
{
    public record SingleNewsDetailViewModel(string Name, string Description, string ShortDescription,  string ImageUrl, IEnumerable<Tag> ArticleType, IEnumerable<Tag> TypeOfStock, IEnumerable<Tag> StockAge, IEnumerable<Tag> ProductionStage, IEnumerable<Tag> InformationSource, IEnumerable<Tag> ArticleLength)
    {
        /// <summary>
        /// Maps <see cref="SingleNewsPage"/> to a <see cref="SingleNewsDetailViewModel"/>.
        /// </summary>
        public async static Task<SingleNewsDetailViewModel> GetViewModel(SingleNewsPage singleNewsPage, string languageName, ITaxonomyRetriever taxonomyRetriever)
        {
            var singleNews = singleNewsPage.RelatedItem.FirstOrDefault();
            var image = singleNews.NewsFieldsImage.FirstOrDefault();

            return new SingleNewsDetailViewModel(
                singleNews.NewsFieldsName,
                singleNews.NewsFieldsDescription,
                singleNews.NewsFieldsShortDescription,
                image?.ImageFile.Url,
                await taxonomyRetriever.RetrieveTags(singleNews.ArticleType.Select(taste => taste.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleNews.TypeOfStock.Select(processing => processing.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleNews.StockAge.Select(processing => processing.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleNews.ProductionStage.Select(processing => processing.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleNews.InformationSource.Select(processing => processing.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleNews.ArticleLength.Select(processing => processing.Identifier), languageName)
            );
        }
    }
}
