using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcDemo.Web.Models.Subscription
{
    public class SubscribeModel
    {
        [Required(ErrorMessage = "general.requireemail")]
        [EmailAddress(ErrorMessage = "general.correctemailformat")]
        [DisplayName("TestMvcDemo.News.SubscriberEmail")]
        [MaxLength(250, ErrorMessage = "TestMvcDemo.News.LongEmail")]
        public string Email
        {
            get;
            set;
        }


        [Bindable(BindableSupport.No)]
        public bool SubscriptionSaved
        {
            get;
            set;
        }
    }
}