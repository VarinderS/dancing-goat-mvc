using System.Globalization;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;

namespace MvcDemo.Web.Controllers
{
    public class ArticlesController : BaseController
    {
        // GET: Articles
        public ActionResult Index()
        {
            var articles = ArticleProvider.GetArticles()
                                          .OnSite("TestMvcDemo")
                                          .Culture(CultureInfo.CurrentUICulture.Name)
                                          .OrderByDescending("DocumentPublishFrom");

            return View(articles);
        }


        // GET: Articles/Show/{id}
        public ActionResult Show(int id = 0)
        {
            var article = ArticleProvider.GetArticles()
                                         .WithID(id)
                                         .Culture(CultureInfo.CurrentUICulture.Name)
                                         .FirstObject;

            if (article == null)
            {
                return NotFoundResult();
            }

            return View(article);
        }
    }
}