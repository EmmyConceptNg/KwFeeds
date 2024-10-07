using System.Collections.Generic;

namespace DancingGoat.Models
{
    /// <summary>
    /// Custom code for page of type <see cref="SingleFeedCastPage"/>.
    /// </summary>
    public partial class SingleFeedCastPage : IFeedCastPage
    {
        /// <inheritdoc />
        IEnumerable<IFeedCastFields> IFeedCastPage.RelatedItem { get => RelatedItem; }
    }
}
