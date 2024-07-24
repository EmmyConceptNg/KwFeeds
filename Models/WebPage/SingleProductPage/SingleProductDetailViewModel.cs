using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;

namespace DancingGoat.Models
{
    public record SingleProductDetailViewModel(string Name, string Description, string ShortDescription, string NutritionalInformation, string Ingredients, string FeedingGuidelines, string UsageTips, string HandlingInstructions, string HealthBenefits, string ImageUrl, IEnumerable<Tag> Tastes, IEnumerable<Tag> Processing)
    {
        /// <summary>
        /// Maps <see cref="SingleProductPage"/> to a <see cref="SingleProductDetailViewModel"/>.
        /// </summary>
        public async static Task<SingleProductDetailViewModel> GetViewModel(SingleProductPage singleProductPage, string languageName, ITaxonomyRetriever taxonomyRetriever)
        {
            var singleProduct = singleProductPage.RelatedItem.FirstOrDefault();
            var image = singleProduct.ProductFieldsImage.FirstOrDefault();

            return new SingleProductDetailViewModel(
                singleProduct.ProductFieldsName,
                singleProduct.ProductFieldsDescription,
                singleProduct.ProductFieldsShortDescription,
                singleProduct.ProductFieldNutritionalInformation,
                singleProduct.ProductFieldIngredients,
                singleProduct.ProductFieldFeedingGuidelines,
                singleProduct.ProductFieldUsageTips,
                singleProduct.ProductFieldHandlingInstructions,
                singleProduct.ProductFieldHealthBenefits,
                image?.ImageFile.Url,
                await taxonomyRetriever.RetrieveTags(singleProduct.SingleProductTastes.Select(taste => taste.Identifier), languageName),
                await taxonomyRetriever.RetrieveTags(singleProduct.SingleProductProcessing.Select(processing => processing.Identifier), languageName)
            );
        }
    }
}
