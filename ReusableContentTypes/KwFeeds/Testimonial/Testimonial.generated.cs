//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS.ContentEngine;

namespace KwFeeds
{
	/// <summary>
	/// Represents a content item of type <see cref="Testimonial"/>.
	/// </summary>
	[RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
	public partial class Testimonial : IContentItemFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "KwFeeds.Testimonial";


		/// <summary>
		/// Represents system properties for a content item.
		/// </summary>
		[SystemField]
		public ContentItemFields SystemFields { get; set; }


		/// <summary>
		/// Name.
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Title.
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// Content.
		/// </summary>
		public string Content { get; set; }


		/// <summary>
		/// Image.
		/// </summary>
		public ContentItemAsset Image { get; set; }
	}
}