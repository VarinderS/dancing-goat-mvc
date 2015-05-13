using System;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Models.Cafes
{
    public class CafeModel
    {
        public Guid Photo
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