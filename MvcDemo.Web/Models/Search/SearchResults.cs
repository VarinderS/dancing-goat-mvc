using System.Collections.Generic;
using MvcDemo.Web.Services;

namespace MvcDemo.Web.Models.Search
{
    public class SearchResults : IPagedDataSource
    {
        #region "Properties"

        public string Query
        {
            get;
            set;
        }

        public IEnumerable<SearchResultItem> Items
        {
            get;
            set;
        }

        #endregion


        #region "IPagedDataSource Implementation"

        public int PageIndex
        {
            get;
            set;
        }


        public int PageSize
        {
            get;
            set;
        }


        public int TotalItemCount
        {
            get;
            set;
        }

        #endregion
    }
}