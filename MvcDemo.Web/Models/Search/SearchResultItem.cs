using System;

using CMS.DocumentEngine;

namespace MvcDemo.Web.Models.Search
{
    public class SearchResultItem
    {
        public Attachment ImageAttachment
        {
            get;
            set;
        }


        public string Title
        {
            get;
            set;
        }


        public string Content
        {
            get;
            set;
        }


        public string PageTypeDispayName
        {
            get;
            set;
        }

        
        public string PageTypeCodeName
        {
            get;
            set;
        }


        public int DocumentId
        {
            get;
            set;
        }


        public string Date
        {
            get;
            set;
        }
    }
}