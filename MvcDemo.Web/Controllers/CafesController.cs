using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CMS.Base;

using MvcDemo.Web.Models.Cafes;
using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Controllers
{
    public class CafesController : BaseController
    {
        // GET: Cafes
        public ActionResult Index()
        {
            var companyCafes = CafeProvider.GetCafes()
                .OnSite("TestMvcDemo")
                .WhereTrue("CafeIsCompanyCafe")
                .OrderByDescending("DocumentPublishFrom")
                .TopN(4);

            var viewModel = new Models.Cafes.IndexViewModel
            {
                CompanyCafes = GetCompanyCafesModel(companyCafes)
            };

            var partnerCafes = CafeProvider.GetCafes()
                .OnSite("TestMvcDemo")
                .WhereFalse("CafeIsCompanyCafe")
                .OrderByDescending("DocumentPublishFrom");

            viewModel.PartnerCafes = GetPartnerCafesModel(partnerCafes);

            return View(viewModel);
        }


        private Dictionary<string, List<ContactModel>> GetPartnerCafesModel(IEnumerable<Cafe> cafes)
        {
            var cityCafes = new Dictionary<string, List<ContactModel>>();

            // Group partner cafes by their location
            foreach (var cafe in cafes)
            {
                var city = cafe.City.ToLowerCSafe();
                var contact = new ContactModel(cafe);

                if (cityCafes.ContainsKey(city))
                {
                    cityCafes[city].Add(contact);
                }
                else
                {
                    cityCafes.Add(city, new List<ContactModel> {contact});
                }
            }

            return cityCafes;
        }


        private IEnumerable<CafeModel> GetCompanyCafesModel(IEnumerable<Cafe> cafes)
        {
            return cafes.Select(cafe => new CafeModel
            {
                Photo = cafe.CafePhoto,
                Note = cafe.CafeAdditionalNotes,
                Contact = new ContactModel(cafe)
            });
        }
    }
}