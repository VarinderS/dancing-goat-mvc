using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories
{
    /// <summary>
    /// Represents a contract for a collection of contact information.
    /// </summary>
    public abstract class ContactRepository
    {
        /// <summary>
        /// Returns company's contact information.
        /// </summary>
        /// <returns>Company's contact information, if found; otherwise, null.</returns>
        public abstract Contact GetCompanyContact();
    }
}