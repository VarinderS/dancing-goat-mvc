using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.Base;
using CMS.DocumentEngine.Types;

using MvcDemo.Web.Models.Cafes;
using MvcDemo.Web.Models.Contacts;
using MvcDemo.Web.Repositories;
using MvcDemo.Web.Services;

namespace MvcDemo.Web.Controllers
{
    public class CafesController : Controller
    {
        private readonly CafeRepository mCafeRepository;
        private readonly CountryRepository mCountryRepository;
        private readonly LocalizationService mLocalizationService;


        public CafesController(CafeRepository cafeRepository, CountryRepository countryRepository, LocalizationService localizationService)
        {
            mLocalizationService = localizationService;
            mCountryRepository = countryRepository;
            mCafeRepository = cafeRepository;
        }


        // GET: Cafes
        public ActionResult Index()
        {
            var companyCafes = mCafeRepository.GetCompanyCafes(4);

            var viewModel = new Models.Cafes.IndexViewModel
            {
                CompanyCafes = GetCompanyCafesModel(companyCafes)
            };

            var partnerCafes = mCafeRepository.GetPartnerCafes();

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
                var contact = CreateContactModel(cafe);

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
                Contact = CreateContactModel(cafe)
            });
        }


        private ContactModel CreateContactModel(IContact contact)
        {
            var countryStateName = CountryStateName.Parse(contact.Country);
            var country = mCountryRepository.GetCountry(countryStateName.CountryName);
            var state = mCountryRepository.GetState(countryStateName.StateName);

            var model = new ContactModel(contact)
            {
                CountryCode = country.CountryTwoLetterCode,
                Country = mLocalizationService.LocalizeString(country.CountryDisplayName)
            };

            if (state != null)
            {
                model.StateCode = state.StateName;
                model.State = mLocalizationService.LocalizeString(state.StateDisplayName);
            }

            return model;
        }
    }
}