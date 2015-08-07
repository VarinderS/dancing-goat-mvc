using System.Web.Mvc;

using CMS.DocumentEngine.Types;

using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Repositories;

namespace MvcDemo.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository mArticleRepository;
        private readonly IOutputCacheDependencies mOutputCacheDependencies;


        public ArticlesController(IArticleRepository repository, IOutputCacheDependencies outputCacheDependencies)
        {
            mArticleRepository = repository;
            mOutputCacheDependencies = outputCacheDependencies;
        }


        // GET: Articles
        [OutputCache(CacheProfile="Default", VaryByParam="none")]
        public ActionResult Index()
        {
            var articles = mArticleRepository.GetArticles();

            mOutputCacheDependencies.AddDependencyOnPages<Article>();

            return View(articles);
        }


        // GET: Articles/Show/{id}
        [OutputCache(CacheProfile = "Default", VaryByParam = "id")]
        public ActionResult Show(int id = 0)
        {
            var article = mArticleRepository.GetArticle(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            mOutputCacheDependencies.AddDependencyOnPages<Article>();

            return View(article);
        }
    }
}