using System.Web.Mvc;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Controllers
{
    public class AboutController : BaseController
    {
        // GET: About
        public ActionResult Index()
        {
            var sections = AboutUsSectionProvider.GetAboutUsSections()
                                                 .OnSite("TestMvcDemo")
                                                 .OrderBy("NodeOrder");

            return View(sections);
        }
    }
}