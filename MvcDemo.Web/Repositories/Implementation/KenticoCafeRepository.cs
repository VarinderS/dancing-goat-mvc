using System.Collections.Generic;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of cafes.
    /// </summary>
    public class KenticoCafeRepository : CafeRepository
    {
        private readonly string mSiteName;
        private readonly string mCultureName;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoCafeRepository"/> class that returns cafes from the specified site using the specified language.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        /// <param name="cultureName">The name of a culture.</param>
        public KenticoCafeRepository(string siteName, string cultureName)
        {
            mSiteName = siteName;
            mCultureName = cultureName;
        }


        /// <summary>
        /// Returns an enumerable collection of company cafes ordered by a position in the content tree.
        /// </summary>
        /// <param name="count">The number of cafes to return.</param>
        /// <returns>An enumerable collection that contains the specified number of cafes ordered by a position in the content tree.</returns>
        public override IEnumerable<Cafe> GetCompanyCafes(int count = 0)
        {
            return CafeProvider.GetCafes()
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
        public override IEnumerable<Cafe> GetPartnerCafes()
        {
            return CafeProvider.GetCafes()
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .WhereFalse("CafeIsCompanyCafe")
                .OrderBy("NodeOrder");
        }
    }
}