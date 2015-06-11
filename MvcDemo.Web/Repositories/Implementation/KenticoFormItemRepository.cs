using CMS.OnlineForms.Types;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of form records.
    /// </summary>
    public class KenticoFormItemRepository : FormItemRepository
    {
        /// <summary>
        /// Creates a new form record from the specified "Contact us" form data, and returns it.
        /// </summary>
        /// <param name="message">The "Contact us" form data.</param>
        /// <returns>The form record created from the specified "Contact us" form data.</returns>
        public override TestMvcDemoContactUsItem CreateContactUsFormItem(MessageModel message)
        {
            var item = new TestMvcDemoContactUsItem
            {
                UserFirstName = message.FirstName,
                UserLastName = message.LastName,
                UserEmail = message.Email,
                UserMessage = message.MessageText,
            };

            item.Insert();

            return item;
        }
    }
}