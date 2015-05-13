using System;
using MvcDemo.Web.Services;

namespace MvcDemo.Web.Models.Pager
{
    public class PagerModel
    {
        public IPagedDataSource DataSource
        {
            get;
            set;
        }


        public Func<int, string> CreateUrlForPageIndex
        {
            get; 
            set;
        }
    }
}