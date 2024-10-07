using System.Collections.Generic;

namespace DancingGoat.Models
{
    public record FeedCastListViewModel(IEnumerable<FeedCastListItemViewModel> Items)
    {
    }
}
