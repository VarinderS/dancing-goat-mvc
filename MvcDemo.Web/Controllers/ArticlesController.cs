using System.Web.Mvc;

using MvcDemo.Web.Repositories;
using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ArticleRepository mArticleRepository;


        public ArticlesController(ArticleRepository repository)
        {
            mArticleRepository = repository;
        }


        // GET: Articles
        public ActionResult Index()
        {
            return View(mArticleRepository.GetLatestArticles());
        }


        // GET: Articles/Show/{id}
        public ActionResult Show(int id = 0)
        {
            var article = mArticleRepository.GetArticle(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }
    }
}