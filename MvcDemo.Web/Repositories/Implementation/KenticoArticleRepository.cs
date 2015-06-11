using System.Collections.Generic;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of articles.
    /// </summary>
    public sealed class KenticoArticleRepository : ArticleRepository
    {
        private readonly string mSiteName;
        private readonly string mCultureName;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoArticleRepository"/> class that returns articles from the specified site in the specified language.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        /// <param name="cultureName">The name of a culture.</param>
        public KenticoArticleRepository(string siteName, string cultureName)
        {
            mSiteName = siteName;
            mCultureName = cultureName;
        }


        /// <summary>
        /// Returns an enumerable collection of articles ordered by the date of publication. The most recent articles come first.
        /// </summary>
        /// <param name="count">The number of articles to return.</param>
        /// <returns>An enumerable collection that contains the specified number of articles ordered by the date of publication.</returns>
        public override IEnumerable<Article> GetLatestArticles(int count = 0)
        {
            return ArticleProvider.GetArticles()
                .OnSite(mSiteName)
                .Culture(mCultureName)
                .TopN(count)
                .OrderByDescending("DocumentPublishFrom");
        }


        /// <summary>
        /// Returns the article with the specified identifier.
        /// </summary>
        /// <param name="articleID">The article identifier.</param>
        /// <returns>The article with the specified identifier, if found; otherwise, null.</returns>
        public override Article GetArticle(int articleID)
        {
            return ArticleProvider.GetArticles()
                .OnSite(mSiteName)
                .WithID(articleID)
                .Culture(mCultureName)
                .FirstObject;
        }
    }
}