using CMS.DocumentEngine;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Models.Cafes
{
    public class CafeModel
    {
        public Attachment Photo
        {
            get; 
            set; 
        }


        public string Note
        {
            get;
            set;
        }


        public ContactModel Contact
        {
            get; 
            set;
        }
    }
}