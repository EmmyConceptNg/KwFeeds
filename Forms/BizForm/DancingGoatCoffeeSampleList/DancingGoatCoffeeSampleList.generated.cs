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
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.OnlineForms;
using CMS.OnlineForms.Types;

[assembly: RegisterBizForm(DancingGoatCoffeeSampleListItem.CLASS_NAME, typeof(DancingGoatCoffeeSampleListItem))]

namespace CMS.OnlineForms.Types
{
	/// <summary>
	/// Represents a content item of type DancingGoatCoffeeSampleListItem.
	/// </summary>
	public partial class DancingGoatCoffeeSampleListItem : BizFormItem
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "BizForm.DancingGoatCoffeeSampleList";


		/// <summary>
		/// The instance of the class that provides extended API for working with DancingGoatCoffeeSampleListItem fields.
		/// </summary>
		private readonly DancingGoatCoffeeSampleListItemFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// FirstName.
		/// </summary>
		[DatabaseField]
		public string FirstName
		{
			get => ValidationHelper.GetString(GetValue(nameof(FirstName)), @"");
			set => SetValue(nameof(FirstName), value);
		}


		/// <summary>
		/// LastName.
		/// </summary>
		[DatabaseField]
		public string LastName
		{
			get => ValidationHelper.GetString(GetValue(nameof(LastName)), @"");
			set => SetValue(nameof(LastName), value);
		}


		/// <summary>
		/// Email.
		/// </summary>
		[DatabaseField]
		public string Email
		{
			get => ValidationHelper.GetString(GetValue(nameof(Email)), @"");
			set => SetValue(nameof(Email), value);
		}


		/// <summary>
		/// Address.
		/// </summary>
		[DatabaseField]
		public string Address
		{
			get => ValidationHelper.GetString(GetValue(nameof(Address)), @"");
			set => SetValue(nameof(Address), value);
		}


		/// <summary>
		/// City.
		/// </summary>
		[DatabaseField]
		public string City
		{
			get => ValidationHelper.GetString(GetValue(nameof(City)), @"");
			set => SetValue(nameof(City), value);
		}


		/// <summary>
		/// ZIPCode.
		/// </summary>
		[DatabaseField]
		public string ZIPCode
		{
			get => ValidationHelper.GetString(GetValue(nameof(ZIPCode)), @"");
			set => SetValue(nameof(ZIPCode), value);
		}


		/// <summary>
		/// State.
		/// </summary>
		[DatabaseField]
		public string State
		{
			get => ValidationHelper.GetString(GetValue(nameof(State)), @"");
			set => SetValue(nameof(State), value);
		}


		/// <summary>
		/// Country.
		/// </summary>
		[DatabaseField]
		public string Country
		{
			get => ValidationHelper.GetString(GetValue(nameof(Country)), @"");
			set => SetValue(nameof(Country), value);
		}


		/// <summary>
		/// Consent.
		/// </summary>
		[DatabaseField]
		public Guid Consent
		{
			get => ValidationHelper.GetGuid(GetValue(nameof(Consent)), Guid.Empty);
			set => SetValue(nameof(Consent), value);
		}


		/// <summary>
		/// Gets an object that provides extended API for working with DancingGoatCoffeeSampleListItem fields.
		/// </summary>
		[RegisterProperty]
		public DancingGoatCoffeeSampleListItemFields Fields
		{
			get => mFields;
		}


		/// <summary>
		/// Provides extended API for working with DancingGoatCoffeeSampleListItem fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class DancingGoatCoffeeSampleListItemFields : AbstractHierarchicalObject<DancingGoatCoffeeSampleListItemFields>
		{
			/// <summary>
			/// The content item of type DancingGoatCoffeeSampleListItem that is a target of the extended API.
			/// </summary>
			private readonly DancingGoatCoffeeSampleListItem mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="DancingGoatCoffeeSampleListItemFields" /> class with the specified content item of type DancingGoatCoffeeSampleListItem.
			/// </summary>
			/// <param name="instance">The content item of type DancingGoatCoffeeSampleListItem that is a target of the extended API.</param>
			public DancingGoatCoffeeSampleListItemFields(DancingGoatCoffeeSampleListItem instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// FirstName.
			/// </summary>
			public string FirstName
			{
				get => mInstance.FirstName;
				set => mInstance.FirstName = value;
			}


			/// <summary>
			/// LastName.
			/// </summary>
			public string LastName
			{
				get => mInstance.LastName;
				set => mInstance.LastName = value;
			}


			/// <summary>
			/// Email.
			/// </summary>
			public string Email
			{
				get => mInstance.Email;
				set => mInstance.Email = value;
			}


			/// <summary>
			/// Address.
			/// </summary>
			public string Address
			{
				get => mInstance.Address;
				set => mInstance.Address = value;
			}


			/// <summary>
			/// City.
			/// </summary>
			public string City
			{
				get => mInstance.City;
				set => mInstance.City = value;
			}


			/// <summary>
			/// ZIPCode.
			/// </summary>
			public string ZIPCode
			{
				get => mInstance.ZIPCode;
				set => mInstance.ZIPCode = value;
			}


			/// <summary>
			/// State.
			/// </summary>
			public string State
			{
				get => mInstance.State;
				set => mInstance.State = value;
			}


			/// <summary>
			/// Country.
			/// </summary>
			public string Country
			{
				get => mInstance.Country;
				set => mInstance.Country = value;
			}


			/// <summary>
			/// Consent.
			/// </summary>
			public Guid Consent
			{
				get => mInstance.Consent;
				set => mInstance.Consent = value;
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="DancingGoatCoffeeSampleListItem" /> class.
		/// </summary>
		public DancingGoatCoffeeSampleListItem() : base(CLASS_NAME)
		{
			mFields = new DancingGoatCoffeeSampleListItemFields(this);
		}

		#endregion
	}
}