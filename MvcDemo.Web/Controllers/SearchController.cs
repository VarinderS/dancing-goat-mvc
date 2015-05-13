using System.Globalization;
using System.Web.Mvc;

using MvcDemo.Web.Models.Search;

namespace MvcDemo.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly SearchService mService;
        private const int PAGE_SIZE = 5;
        private const string INDEX_NAME = "TestMvcDemo.Index";
        private const string SITE_NAME = "TestMvcDemo";


        public SearchController()
        {
            mService = new SearchService(INDEX_NAME, CultureInfo.CurrentCulture.Name, SITE_NAME);    
        }


        [ValidateInput(false)]
        // GET: Search
        public ActionResult Index(string searchText, int? page)
        {
            var model = mService.Search(searchText, page, PAGE_SIZE);

            return View(model);
        }
    }
}