using System.Web.Mvc;

using MvcDemo.Web.Models.Pager;

namespace MvcDemo.Web.Controllers
{
    public class PagerController : Controller
    {
        // GET: Pager
        [ValidateInput(false)]
        public ActionResult Index(PagerModel model)
        {
            return PartialView("_Pager", model);
        }
    }
}