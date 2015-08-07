using System;
using System.Collections.Generic;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of cafes.
    /// </summary>
    public class KenticoCafeRepository : ICafeRepository
    {
        private readonly string mSiteName;
        private readonly string mCultureName;
        private readonly bool mLatestVersionEnabled;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoCafeRepository"/> class that returns cafes from the specified site using the specified language.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        /// <param name="cultureName">The name of a culture.</param>
        /// <param name="latestVersionEnabled">Indicates whether the repository will provide the most recent version of pages.</param>
        public KenticoCafeRepository(string siteName, string cultureName, bool latestVersionEnabled)
        {
            mSiteName = siteName;
            mCultureName = cultureName;
            mLatestVersionEnabled = latestVersionEnabled;
        }


        /// <summary>
        /// Returns an enumerable collection of company cafes ordered by a position in the content tree.
        /// </summary>
        /// <param name="count">The number of cafes to return.</param>
        /// <returns>An enumerable collection that contains the specified number of cafes ordered by a position in the content tree.</returns>
        public IEnumerable<Cafe> GetCompanyCafes(int count = 0)
        {
            return CafeProvider.GetCafes()
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .WhereTrue("CafeIsCompanyCafe")
                .OrderBy("NodeOrder")
                .TopN(count);
        }


        /// <summary>
        /// Returns an enumerable collection of partner cafes ordered by a position in the content tree.
        /// </summary>
        /// <returns>An enumerable collection of partner cafes ordered by a position in the content tree.</returns>
        public IEnumerable<Cafe> GetPartnerCafes()
        {
            return CafeProvider.GetCafes()
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .WhereFalse("CafeIsCompanyCafe")
                .OrderBy("NodeOrder");
        }
    }
}