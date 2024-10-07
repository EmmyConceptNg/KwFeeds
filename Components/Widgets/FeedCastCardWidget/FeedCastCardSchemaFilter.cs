using System.Collections.Generic;

using DancingGoat.Models;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// FeedCast card widget filter for content item selector.
    /// </summary>
    public class FeedCastCardSchemaFilter : IReusableFieldSchemasFilter
    {
        /// <inheritdoc/>
        IEnumerable<string> IReusableFieldSchemasFilter.AllowedSchemaNames => new List<string> { IFeedCastFields.REUSABLE_FIELD_SCHEMA_NAME };
    }
}
