using CMS.Helpers;
using CMS.Globalization;

namespace MvcDemo.Web.Models.Contacts
{
    public class ContactModel
    {
        public string Name
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }


        public string Email
        {
            get;
            set;
        }


        public string ZIP
        {
            get;
            set;
        }


        public string Street
        {
            get;
            set;
        }


        public string City
        {
            get;
            set;
        }


        public string Country
        {
            get;
            set;
        }


        public string CountryCode
        {
            get;
            set;
        }


        public string State
        {
            get;
            set;
        }


        public string StateCode
        {
            get;
            set;
        }


        public ContactModel()
        {

        }


        public ContactModel(IContact contact)
        {
            Name = contact.Name;
            Phone = contact.Phone;
            Email = contact.Email;
            ZIP = contact.ZIP;
            Street = contact.Street;
            City = contact.City;

            // Fills country and state related properties
            ParseCountryState(contact.Country);
        }


        private void ParseCountryState(string countryState)
        {
            if (string.IsNullOrEmpty(countryState))
            {
                return;
            }

            string[] parts = countryState.Split(';');
            if (parts.Length == 0)
            {
                return;
            }
         
            var ci = CountryInfoProvider.GetCountryInfo(parts[0]);
            if (ci != null)
            {
                Country = ResHelper.LocalizeString(ci.CountryDisplayName);
                CountryCode = ci.CountryTwoLetterCode;
                if (parts.Length > 1)
                {
                    var si = StateInfoProvider.GetStateInfo(parts[1]);
                    if (si != null)
                    {
                        State = ResHelper.LocalizeString(si.StateDisplayName);
                        StateCode = si.StateCode;
                    }
                }
            }
        }
    }
}