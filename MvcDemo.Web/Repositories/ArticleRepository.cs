﻿using System.Collections.Generic;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Repositories
{
    /// <summary>
    /// Represents a contract for a collection of articles.
    /// </summary>
    public abstract class ArticleRepository
    {
        /// <summary>
        /// Returns an enumerable collection of articles ordered by the date of publication. The most recent articles come first.
        /// </summary>
        /// <param name="count">The number of articles to return.</param>
        /// <returns>An enumerable collection that contains the specified number of articles ordered by the date of publication.</returns>
        public abstract IEnumerable<Article> GetLatestArticles(int count = 0);


        /// <summary>
        /// Returns the article with the specified identifier.
        /// </summary>
        /// <param name="articleID">The article identifier.</param>
        /// <returns>The article with the specified identifier, if found; otherwise, null.</returns>
        public abstract Article GetArticle(int articleID);
    }
}