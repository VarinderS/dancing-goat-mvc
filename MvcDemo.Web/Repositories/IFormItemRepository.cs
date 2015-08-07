using CMS.OnlineForms.Types;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Repositories
{
    /// <summary>
    /// Represents a contract for a collection of form records.
    /// </summary>
    public interface IFormItemRepository
    {
        /// <summary>
        /// Creates a new form record from the specified "Contact us" form data, and returns it.
        /// </summary>
        /// <param name="message">The "Contact us" form data.</param>
        /// <returns>The form record created from the specified "Contact us" form data.</returns>
        TestMvcDemoContactUsItem CreateContactUsFormItem(MessageModel message);
    }
}