using System.Collections.Generic;

namespace DancingGoat.Models
{
    /// <summary>
    /// Custom code for page of type <see cref="SingleNewsPage"/>.
    /// </summary>
    public partial class SingleNewsPage : INewsPage
    {
        /// <inheritdoc />
        IEnumerable<INewsFields> INewsPage.RelatedItem { get => RelatedItem; }
    }
}
