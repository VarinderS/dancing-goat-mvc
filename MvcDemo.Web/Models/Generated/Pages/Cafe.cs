using MvcDemo.Web.Models.Contacts;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Specification of Cafe members and IContact interface relationship.
    /// </summary>
    public partial class Cafe : IContact
    {
        public new string Name
        {
            get
            {
                return CafeName;
            }
        }


        public string Phone
        {
            get
            {
                return CafePhone;
            }
        }


        public string Email
        {
            get
            {
                return "";
            }
        }


        public string ZIP
        {
            get
            {
                return CafeZipCode;
            }
        }


        public string Street
        {
            get
            {
                return CafeStreet;
            }
        }


        public string City
        {
            get
            {
                return CafeCity;
            }
        }


        public string Country
        {
            get
            {
                return CafeCountry;
            }
        }
    }
}