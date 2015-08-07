using System.Web.Mvc;

using CMS.DocumentEngine.Types;

using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Repositories;

namespace MvcDemo.Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutUsRepository mAboutUsRepository;
        private readonly IOutputCacheDependencies mOutputCacheDependencies;


        public AboutController(IAboutUsRepository aboutUsRepository, IOutputCacheDependencies outputCacheDependencies)
        {
            mAboutUsRepository = aboutUsRepository;
            mOutputCacheDependencies = outputCacheDependencies;
        }


        // GET: About
        [OutputCache(CacheProfile = "Default", VaryByParam = "none")]
        public ActionResult Index()
        {
            var sideStories = mAboutUsRepository.GetSideStories();

            mOutputCacheDependencies.AddDependencyOnPages<AboutUsSection>();

            return View(sideStories);
        }
    }
}