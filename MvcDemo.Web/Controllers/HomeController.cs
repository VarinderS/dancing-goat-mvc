using System.Web.Mvc;
using System.Globalization;

using CMS.DocumentEngine.Types;

using MvcDemo.Web.Models.Home;

namespace MvcDemo.Web.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                LatestArticles = ArticleProvider.GetArticles()
                                                .OnSite("TestMvcDemo")
                                                .Culture(CultureInfo.CurrentUICulture.Name)
                                                .OrderByDescending("DocumentPublishFrom")
                                                .TopN(5)
            };

            var ourStory = AboutUsProvider.GetAboutUs()
                                          .OnSite("TestMvcDemo")
                                          .Culture(CultureInfo.CurrentUICulture.Name)
                                          .FirstObject;

            if (ourStory != null)
            {
                viewModel.OurStory = ourStory.AboutUsTeaser;
            }

            viewModel.CompanyCafes = CafeProvider.GetCafes()
                                                 .OnSite("TestMvcDemo")
                                                 .Culture(CultureInfo.CurrentUICulture.Name)
                                                 .Columns("CafeName", "CafePhoto")
                                                 .OrderBy("NodeLevel", "NodeOrder", "NodeName")
                                                 .WhereTrue("CafeIsCompanyCafe")
                                                 .TopN(4);
            
            return View(viewModel);
        }
    }
}
