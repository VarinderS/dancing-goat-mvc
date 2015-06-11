using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of contact information.
    /// </summary>
    public class KenticoContactRepository : ContactRepository
    {
        private readonly string mSiteName;
        private readonly string mCultureName;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoContactRepository"/> class that returns contact information from the specified site using the specified language.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        /// <param name="cultureName">The name of a culture.</param>
        public KenticoContactRepository(string siteName, string cultureName)
        {
            mSiteName = siteName;
            mCultureName = cultureName;
        }


        /// <summary>
        /// Returns company's contact information.
        /// </summary>
        /// <returns>Company's contact information, if found; otherwise, null.</returns>
        public override Contact GetCompanyContact()
        {
            return ContactProvider.GetContacts()
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .FirstObject;
        }
    }
}