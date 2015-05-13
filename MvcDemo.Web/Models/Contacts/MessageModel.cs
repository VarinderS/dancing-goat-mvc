using System.ComponentModel.DataAnnotations;

namespace MvcDemo.Web.Models.Contacts
{
    /// <summary>
    /// Represents message with contact data.
    /// </summary>
    public class MessageModel
    {
        [Display(Name = "general.firstname")]
        [DataType(DataType.Text)]
        [MaxLength(200)]
        public string FirstName
        {
            get;
            set;
        }


        [Display(Name = "general.lastname")]
        [DataType(DataType.Text)]
        [MaxLength(200)]
        public string LastName
        {
            get;
            set;
        }


        [Required(ErrorMessage = "general.requireemail")]
        [Display(Name = "general.emailaddress")]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "TestMvcDemo.News.LongEmail")]
        public string Email
        {
            get;
            set;
        }


        [Required(ErrorMessage = "general.requiresmessage")]
        [Display(Name = "general.message")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500)]
        public string MessageText
        {
            get;
            set;
        }
    }
}