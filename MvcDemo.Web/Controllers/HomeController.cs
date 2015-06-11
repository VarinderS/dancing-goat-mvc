using System.Web.Mvc;

using MvcDemo.Web.Models.Home;
using MvcDemo.Web.Repositories;

namespace MvcDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArticleRepository mArticleRepository;
        private readonly AboutUsRepository mAboutUsRepository;
        private readonly CafeRepository mCafeRepository;


        public HomeController(ArticleRepository repository, AboutUsRepository aboutUsRepository, CafeRepository cafeRepository)
        {
            mArticleRepository = repository;
            mAboutUsRepository = aboutUsRepository;
            mCafeRepository = cafeRepository;
        }


        // GET: Home
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                LatestArticles = mArticleRepository.GetLatestArticles(5)
            };

            var ourStory = mAboutUsRepository.GetOurStory();

            if (ourStory != null)
            {
                viewModel.OurStory = ourStory.AboutUsTeaser;
            }

            viewModel.CompanyCafes = mCafeRepository.GetCompanyCafes(4);

            return View(viewModel);
        }
    }
}
