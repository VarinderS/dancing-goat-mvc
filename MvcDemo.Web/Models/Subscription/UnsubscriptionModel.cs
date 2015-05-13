using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MvcDemo.Web.Models.Subscription
{
    public class UnsubscriptionModel
    {
        [Required]
        public Guid SubscriberGuid 
        { 
            get; 
            set; 
        }


        [Required]
        public Guid NewsletterGuid
        {
            get;
            set;
        }


        [Required]
        public Guid IssueGuid
        {
            get;
            set;
        }


        public bool UnsubscribeFromAll
        {
            get;
            set;
        }


        [Bindable(false)]
        public bool IsError
        {
            get;
            set;
        }


        [Bindable(false)]
        public string UnsubscriptionResult
        {
            get;
            set;
        }
    }
}