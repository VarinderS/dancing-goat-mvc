using System.Collections.Generic;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of stories about company's strategy, history and philosophy.
    /// </summary>
    public sealed class KenticoAboutUsRepository : AboutUsRepository
    {
        private readonly string mSiteName;
        private readonly string mCultureName;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoAboutUsRepository"/> class that returns stories from the specified site in the specified language.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        /// <param name="cultureName">The name of a culture.</param>
        public KenticoAboutUsRepository(string siteName, string cultureName)
        {
            mSiteName = siteName;
            mCultureName = cultureName;
        }

        
        /// <summary>
        /// Returns the story that describes company's strategy and history.
        /// </summary>
        /// <returns>The story that describes company's strategy and history, if found; otherwise, null.</returns>
        public override AboutUs GetOurStory()
        {
            return AboutUsProvider.GetAboutUs()
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .FirstObject;
        }


        /// <summary>
        /// Returns an enumerable collection of stories about company's philosophy ordered by a position in the content tree.
        /// </summary>
        /// <returns>An enumerable collection of stories about company's philosophy ordered by a position in the content tree.</returns>
        public override IEnumerable<AboutUsSection> GetSideStories()
        {
            return AboutUsSectionProvider.GetAboutUsSections()
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .OrderBy("NodeOrder");
        }
    }
}