//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com. 
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;

using CMS;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine.Types;
using CMS.DocumentEngine;

[assembly: RegisterDocumentType(Cafe.CLASS_NAME, typeof(Cafe))]

namespace CMS.DocumentEngine.Types 
{
    /// <summary>
    /// Sample item class.
    /// </summary>
    public partial class Cafe : TreeNode
    {
        #region "Constants"

        /// <summary>
        /// Class name of the item.
        /// </summary>
        public const string CLASS_NAME = "TestMvcDemo.Cafe";

        #endregion


        #region "Properties"

        /// <summary>
        /// CafeID.
        /// </summary>
        [DatabaseField]
        public int CafeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CafeID"), 0);
            }
            set
            {
                SetValue("CafeID", value);
            }
        }


        /// <summary>
        /// Name.
        /// </summary>
        [DatabaseField]
        public string CafeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeName"), "");
            }
            set
            {
                SetValue("CafeName", value);
            }
        }


        /// <summary>
        /// If true, cafe is company cafe. Otherwise, it is partner cafe.
        /// </summary>
        [DatabaseField]
        public bool CafeIsCompanyCafe
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("CafeIsCompanyCafe"), false);
            }
            set
            {
                SetValue("CafeIsCompanyCafe", value);
            }
        }


        /// <summary>
        /// Street.
        /// </summary>
        [DatabaseField]
        public string CafeStreet
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeStreet"), "");
            }
            set
            {
                SetValue("CafeStreet", value);
            }
        }


        /// <summary>
        /// City.
        /// </summary>
        [DatabaseField]
        public string CafeCity
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeCity"), "");
            }
            set
            {
                SetValue("CafeCity", value);
            }
        }


        /// <summary>
        /// Country.
        /// </summary>
        [DatabaseField]
        public string CafeCountry
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeCountry"), "USA");
            }
            set
            {
                SetValue("CafeCountry", value);
            }
        }


        /// <summary>
        /// Zip code.
        /// </summary>
        [DatabaseField]
        public string CafeZipCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeZipCode"), "");
            }
            set
            {
                SetValue("CafeZipCode", value);
            }
        }


        /// <summary>
        /// Phone.
        /// </summary>
        [DatabaseField]
        public string CafePhone
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafePhone"), "");
            }
            set
            {
                SetValue("CafePhone", value);
            }
        }


        /// <summary>
        /// Photo.
        /// </summary>
        [DatabaseField]
        public Guid CafePhoto
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("CafePhoto"), Guid.Empty);
            }
            set
            {
                SetValue("CafePhoto", value);
            }
        }


        /// <summary>
        /// Additional notes.
        /// </summary>
        [DatabaseField]
        public string CafeAdditionalNotes
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CafeAdditionalNotes"), "");
            }
            set
            {
                SetValue("CafeAdditionalNotes", value);
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor.
        /// </summary>
        public Cafe()
            : base(CLASS_NAME)
        {
        }

        #endregion
    }
}