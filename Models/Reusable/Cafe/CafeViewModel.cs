using System.Linq;

namespace DancingGoat.Models
{
    public record CafeViewModel(string Name, string Description, string NutritionalInformation, string Ingredients, string FeedingGuidelines, string UsageTips, string StorageInstructions, string PhotoPath, string PhotoShortDescription)
    {
        /// <summary>
        /// Maps <see cref=Cafe"/> to a <see cref="CafeViewModel"/>.
        /// </summary>
        public static CafeViewModel GetViewModel(Products cafe)
        {
            var cafePhoto = cafe.CafePhoto?.FirstOrDefault();
            return new CafeViewModel(
                cafe.Name,
                cafe.Description,
                cafe.NutritionalInformation,
                cafe.Ingredients,
                cafe.FeedingGuidelines,
                cafe.UsageTips,
                cafe.StorageInstructions,
                cafePhoto?.ImageFile.Url,
                cafePhoto?.ImageShortDescription
                );
        }
    }
}
