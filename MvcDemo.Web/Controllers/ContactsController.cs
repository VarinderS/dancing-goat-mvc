using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CMS.OnlineForms.Types;

using MvcDemo.Web.Models.Contacts;

namespace MvcDemo.Web.Controllers
{
    public class ContactsController : BaseController
    {
        #region "Actions"

        // GET: Contacts
        public ActionResult Index()
        {
            var model = GetIndexViewModel();
            model.Message = new MessageModel();

            return View(model);
        }


        // GET: Contacts/ThankYou
        public ActionResult ThankYou()
        {
            var model = GetIndexViewModel();
            model.MessageSent = true;

            return View("Index", model);
        }


        // POST: Contacts/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SendMessage(MessageModel message)
        {
            if (!ModelState.IsValid)
            {
                var model = GetIndexViewModel();
                model.Message = message;

                return View("Index", model);
            }

            try
            {
                // Store the message in the Forms application
                var formItem = new TestMvcDemoContactUsItem
                {
                    UserFirstName = message.FirstName,
                    UserLastName = message.LastName,
                    UserEmail = message.Email,
                    UserMessage = message.MessageText,
                };

                formItem.Insert();
            }
            catch
            {
                return View("Error");
            }

            return RedirectToAction("ThankYou");
        }


        [ChildActionOnly]
        public ActionResult CompanyAddress()
        {
            var address = GetCompanyContactModel();

            return PartialView("_Address", address);
        }


        [ChildActionOnly]
        public ActionResult CompanySocialLinks()
        {
            var socialLinks = SocialLinkProvider.GetSocialLinks()
                                                .OnSite("TestMvcDemo")
                                                .OrderByAscending("NodeOrder");

            return PartialView("_SocialLinks", socialLinks);
        }

        #endregion


        #region "Model methods"

        private IndexViewModel GetIndexViewModel()
        {
            var cafes = CafeProvider.GetCafes()
                                       .OnSite("TestMvcDemo")
                                       .Columns("CafeCity", "CafeStreet", "CafeName", "CafeZipCode", "CafeCountry", "CafePhone")
                                       .WhereTrue("CafeIsCompanyCafe")
                                       .TopN(4);

            return new IndexViewModel
            {
                CompanyContact = GetCompanyContactModel(),
                CompanyCafes = GetCompanyCafesModel(cafes)
            };
        }


        private ContactModel GetCompanyContactModel()
        {
            return new ContactModel(ContactProvider.GetContacts().OnSite("TestMvcDemo").FirstObject);
        }


        private List<ContactModel> GetCompanyCafesModel(IEnumerable<Cafe> cafes)
        {
            return cafes.Select(cafe => new ContactModel(cafe)).ToList();
        }

        #endregion
    }
}
