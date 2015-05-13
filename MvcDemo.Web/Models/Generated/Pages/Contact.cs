using MvcDemo.Web.Models.Contacts;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Specification of Contact members and IContact interface relationship.
    /// </summary>
    public partial class Contact : IContact
    {
        public new string Name
        {
            get
            {
                return ContactName;
            }
        }


        public string Phone
        {
            get
            {
                return ContactPhone;
            }
        }


        public string Email
        {
            get
            {
                return ContactEmail;
            }
        }


        public string ZIP
        {
            get
            {
                return ContactZipCode;
            }
        }


        public string Street
        {
            get
            {
                return ContactStreet;
            }
        }


        public string City
        {
            get
            {
                return ContactCity;
            }
        }


        public string Country
        {
            get
            {
                return ContactCountry;
            }
        }
    }
}