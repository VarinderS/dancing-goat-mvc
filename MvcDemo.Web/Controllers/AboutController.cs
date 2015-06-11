using System.Web.Mvc;

using MvcDemo.Web.Repositories;

namespace MvcDemo.Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly AboutUsRepository mAboutUsRepository;


        public AboutController(AboutUsRepository aboutUsRepository)
        {
            mAboutUsRepository = aboutUsRepository;
        }


        // GET: About
        public ActionResult Index()
        {
            return View(mAboutUsRepository.GetSideStories());
        }
    }
}