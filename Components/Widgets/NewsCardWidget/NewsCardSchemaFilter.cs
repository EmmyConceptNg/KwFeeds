using System.Collections.Generic;

using DancingGoat.Models;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace DancingGoat.Widgets
{
    /// <summary>
    /// News card widget filter for content item selector.
    /// </summary>
    public class NewsCardSchemaFilter : IReusableFieldSchemasFilter
    {
        /// <inheritdoc/>
        IEnumerable<string> IReusableFieldSchemasFilter.AllowedSchemaNames => new List<string> { INewsFields.REUSABLE_FIELD_SCHEMA_NAME };
    }
}
