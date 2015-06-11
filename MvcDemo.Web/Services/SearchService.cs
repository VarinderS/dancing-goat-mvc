using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Search;
using CMS.SiteProvider;

using MvcDemo.Web.Models.Search;

using SearchResults = MvcDemo.Web.Models.Search.SearchResults;

namespace MvcDemo.Web.Services
{
    public class SearchService
    {
        #region "Variables"

        private DataSet mRawResults;
        private readonly string mCultureName;
        private readonly string mSearchIndexName;
        private readonly string mDefaultCulture;
        private readonly string mSiteName;

        #endregion


        #region "Public Methods"

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchIndexName">Search index</param>
        /// <param name="cultureName">Search culture name</param>
        /// <param name="siteName">Site name</param>
        public SearchService(string searchIndexName, string cultureName, string siteName)
        {
            mCultureName = cultureName;
            mSearchIndexName = searchIndexName;
            mDefaultCulture = SiteInfoProvider.GetSiteInfo(siteName).DefaultVisitorCulture;
            mSiteName = siteName;
        }


        /// <summary>
        /// Provides fulltext search for MVC application.
        /// </summary>
        /// <param name="query">Text to search</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public virtual SearchResults Search(string query, int? pageIndex, int pageSize)
        {
            var page = (pageIndex ?? 1) - 1;

            var searchResults = new SearchResults
            {
                PageIndex = page,
                PageSize = pageSize,
                Query = query
            };

            if (String.IsNullOrWhiteSpace(query))
            {
                searchResults.TotalItemCount = 0;
                return searchResults;
            }

            int numberOfResults;
            SearchInternal(query, page, pageSize, out numberOfResults);

            searchResults.TotalItemCount = numberOfResults;
            searchResults.Items = GetSearchItems();

            return searchResults;
        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// Using CMS search API fills dataset with raw data.  
        /// </summary>
        /// <param name="query">Text to search</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="numberOfResults">Total number of search results</param>
        private void SearchInternal(string query, int pageIndex, int pageSize, out int numberOfResults)
        {
            var documentCondition = new DocumentSearchCondition(null, mCultureName, mDefaultCulture, combineWithDefaultCulture: false);
            var condition = new SearchCondition(documentCondition: documentCondition);
            var searchExpression = SearchSyntaxHelper.CombineSearchCondition(query, condition);

            var parameters = new SearchParameters
            {
                SearchFor = searchExpression,
                Path = "/%",
                ClassNames = null,
                CurrentCulture = mCultureName,
                DefaultCulture = mDefaultCulture,
                CombineWithDefaultCulture = false,
                CheckPermissions = false,
                SearchInAttachments = false,
                User = MembershipContext.AuthenticatedUser,
                SearchIndexes = mSearchIndexName,
                StartingPosition = pageIndex * pageSize,
                NumberOfResults = 0,
                NumberOfProcessedResults = 100,
                DisplayResults = pageSize
            };

            // Search and save results
            mRawResults = SearchHelper.Search(parameters);
            numberOfResults = parameters.NumberOfResults;
        }


        /// <summary>
        /// Gets search items filled with all necessary properties from SearchContext.CurrentSearchResults and mRawResults collection.
        /// </summary>
        /// <returns>Search items collection</returns>
        private IEnumerable<SearchResultItem> GetSearchItems()
        {
            var searchItems = new List<SearchResultItem>();

            if (DataHelper.DataSourceIsEmpty(SearchContext.CurrentSearchResults) || (mRawResults == null) || (SearchContext.CurrentSearchResults == null))
            {
                return null;
            }

            var attachmentIdentifiers = new List<Guid>();
            foreach (DataRow row in mRawResults.Tables[0].Rows)
            {
                string date, pageTypeDisplayName, pageTypeCodeName;
                int documentId;
                var documentNodeId = GetDocumentNodeId(row["type"], row["id"]);
                var guid = ((row["image"] as string) == null) ? Guid.Empty : new Guid(row["image"].ToString());
                attachmentIdentifiers.Add(guid);

                GetAdditionalData(row["type"], documentNodeId, out date, out documentId, out pageTypeDisplayName, out pageTypeCodeName);

                var searchItem = new SearchResultItem
                {
                    DocumentId = documentId,
                    Title = row["title"].ToString(),
                    Content = row["content"].ToString(),
                    Date = date,
                    PageTypeDispayName = pageTypeDisplayName,
                    PageTypeCodeName = pageTypeCodeName
                };

                searchItems.Add(searchItem);
            }

            var attachments = AttachmentInfoProvider.GetAttachments().OnSite(mSiteName).BinaryData(false).WhereIn("AttachmentGUID", attachmentIdentifiers).ToDictionary(x => x.AttachmentGUID);
            for (int i = 0; i < searchItems.Count; i++)
            {
                AttachmentInfo attachment = null;
                if (attachments.TryGetValue(attachmentIdentifiers[i], out attachment))
                {
                    searchItems[i].ImageAttachment = new Attachment(attachment);
                }
            }

            return searchItems;
        }


        /// <summary>
        /// Gets additional data from SearchContext.CurrentSearchResults collection.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="documentNodeId">DocumentNodeID</param>
        /// <param name="date">Last modified date</param>
        /// <param name="documentId">DocumentID</param>
        /// <param name="pageTypeDisplayName">Page type display name</param>
        /// <param name="pageTypeCodeName">Page type code name</param>
        /// <returns>True if some additional data were found</returns>
        private static bool GetAdditionalData(object type, int documentNodeId, out string date, out int documentId, out string pageTypeDisplayName, out string pageTypeCodeName)
        {
            foreach (var key in SearchContext.CurrentSearchResults.Keys)
            {
                if (GetDocumentNodeId(type, key) == documentNodeId)
                {
                    var row = (DataRow)SearchContext.CurrentSearchResults[key];

                    date = row["DocumentModifiedWhen"].ToString();
                    documentId = ValidationHelper.GetInteger(row["DocumentID"], 0);
                    pageTypeDisplayName = row["ClassDisplayName"].ToString();
                    pageTypeCodeName = row["ClassName"].ToString();

                    return true;
                }
            }

            date = pageTypeDisplayName = pageTypeCodeName = "";
            documentId = 0;
            return false;
        }


        /// <summary>
        /// Parses the key and extracts DocumentNodeID depending on Type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <returns>Returns DocumentNodeID</returns>
        private static int GetDocumentNodeId(object type, object key)
        {
            var stringType = type.ToString();
            var stringKey = key.ToString();

            var i = stringKey.IndexOf(';') + 1;
            var j = stringKey.IndexOf(stringType, StringComparison.Ordinal) - 1;

            return ValidationHelper.GetInteger(stringKey.Substring(i, j - i), 0);
        }

        #endregion
    }
}