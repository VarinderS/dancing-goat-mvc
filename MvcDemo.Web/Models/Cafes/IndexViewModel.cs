using System.Collections.Generic;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Models.Cafes
{
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets the company cafes.
        /// </summary>
        public IEnumerable<CafeModel> CompanyCafes
        {
            get;
            set;
        }


        /// <summary>
        /// Dictionary with pairs city as a key and list of cafes as a value.
        /// </summary>
        public Dictionary<string, List<ContactModel>> PartnerCafes
        {
            get;
            set;
        }
    }
}