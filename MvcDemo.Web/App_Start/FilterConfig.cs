using System.Web.Mvc;

using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Display custom view when action returns NotFoundViewResult.
            filters.Add(new HandleNotFoundErrorAttribute());
        }
    }
}