using System.Linq;

namespace DancingGoat.Models
{
    public record SingleProductViewModel(string Name, string Description, string ShortDescription, string NutritionalInformation, string Ingredients, string FeedingGuidelines, string UsageTips, string HandlingInstructions, string HealthBenefits, string ImageUrl)
    {
        /// <summary>
        /// Validates and maps <see cref="SingleProduct"/> to a <see cref="SingleProductViewModel"/>.
        /// </summary>
        public static SingleProductViewModel GetViewModel(SingleProduct singleProduct)
        {
            if (singleProduct == null)
            {
                return null;
            }

            
            var image = singleProduct.ProductFieldsImage.FirstOrDefault();

            return new SingleProductViewModel(
                singleProduct.ProductFieldsName,
                singleProduct.ProductFieldsDescription,
                singleProduct.ProductFieldsShortDescription,
                singleProduct.ProductFieldNutritionalInformation,
                singleProduct.ProductFieldIngredients,
                singleProduct.ProductFieldFeedingGuidelines,
                singleProduct.ProductFieldUsageTips,
                singleProduct.ProductFieldHandlingInstructions,
                singleProduct.ProductFieldHealthBenefits,
                image?.ImageFile.Url
            );
        }
    }
}
