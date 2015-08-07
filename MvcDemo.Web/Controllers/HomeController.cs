using System.Web.Mvc;

using CMS.DocumentEngine.Types;

using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Models.Home;
using MvcDemo.Web.Repositories;

namespace MvcDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleRepository mArticleRepository;
        private readonly IAboutUsRepository mAboutUsRepository;
        private readonly ICafeRepository mCafeRepository;
        private readonly IOutputCacheDependencies mOutputCacheDependencies;


        public HomeController(IArticleRepository repository, IAboutUsRepository aboutUsRepository, ICafeRepository cafeRepository, IOutputCacheDependencies outputCacheDependencies)
        {
            mArticleRepository = repository;
            mAboutUsRepository = aboutUsRepository;
            mCafeRepository = cafeRepository;
            mOutputCacheDependencies = outputCacheDependencies;
        }


        // GET: Home
        [OutputCache(CacheProfile = "Default", VaryByParam = "none")]
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                LatestArticles = mArticleRepository.GetArticles(5)
            };

            var ourStory = mAboutUsRepository.GetOurStory();

            if (ourStory != null)
            {
                viewModel.OurStory = ourStory.Fields.Teaser;
            }

            viewModel.CompanyCafes = mCafeRepository.GetCompanyCafes(4);

            mOutputCacheDependencies.AddDependencyOnPages<Article>();
            mOutputCacheDependencies.AddDependencyOnPages<AboutUs>();
            mOutputCacheDependencies.AddDependencyOnPages<Cafe>();
            
            return View(viewModel);
        }
    }
}
