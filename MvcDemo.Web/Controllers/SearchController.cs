using System.Web.Mvc;

using MvcDemo.Web.Services;

namespace MvcDemo.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService mService;
        private const int PAGE_SIZE = 5;


        public SearchController(SearchService searchService)
        {
            mService = searchService;
        }


        // GET: Search
        [ValidateInput(false)]
        public ActionResult Index(string searchText, int? page)
        {
            var model = mService.Search(searchText, page, PAGE_SIZE);

            return View(model);
        }
    }
}