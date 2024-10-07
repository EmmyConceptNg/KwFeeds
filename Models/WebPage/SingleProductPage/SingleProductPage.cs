using System.Collections.Generic;

namespace DancingGoat.Models
{
    /// <summary>
    /// Custom code for page of type <see cref="SingleProductPage"/>.
    /// </summary>
    public partial class SingleProductPage : IProductPage
    {
        /// <inheritdoc />
        IEnumerable<IProductFields> IProductPage.RelatedItem { get => RelatedItem; }
    }
}
