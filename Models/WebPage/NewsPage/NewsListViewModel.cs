using System.Collections.Generic;

namespace DancingGoat.Models
{
    public record NewsListViewModel(IEnumerable<NewsListItemViewModel> Items, Dictionary<string, TaxonomyViewModel> Filter)
    {
    }
}
