using System.Web.Mvc;

using MvcDemo.Web.Models.Pager;

namespace MvcDemo.Web.Controllers
{
    public class PagerController : BaseController
    {
        // GET: Pager
        public ActionResult Index(PagerModel model)
        {
            return PartialView("_Pager", model);
        }
    }
}