using System.Web.Mvc;

using CMS.Helpers;

using MvcDemo.Web.Models.Subscription;
using MvcDemo.Web.Services;

namespace MvcDemo.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly NewsletterSubscriptionService mService;


        public SubscriptionController(NewsletterSubscriptionService subscriptionService)
        {
            mService = subscriptionService;
        }


        // POST: Subscription/Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Subscribe(SubscribeModel model)
        {
            if (ModelState.IsValid)
            {
                string errorMessage;

                if (mService.Subscribe(model.Email, "TestMvcDemoDancingGoatNewsletter", out errorMessage))
                {
                    model.SubscriptionSaved = true;
                }
                else
                {
                    ModelState.AddModelError("Email", string.IsNullOrEmpty(errorMessage) ? ResHelper.GetString("TestMvcDemo.News.SubscribeError") : errorMessage);
                }
            }

            return PartialView("_Subscribe", model);
        }


        // GET: Subscription/Unsubscribe
        [ValidateInput(false)]
        public ActionResult Unsubscribe(UnsubscriptionModel model)
        {
            if (ModelState.IsValid)
            {
                string result;
                if (model.UnsubscribeFromAll)
                {
                    model.IsError = !mService.UnsubscribeFromAll(model.NewsletterGuid, model.SubscriberGuid, model.IssueGuid, out result);
                }
                else
                {
                    model.IsError = !mService.Unsubscribe(model.NewsletterGuid, model.SubscriberGuid, model.IssueGuid, out result);
                }

                model.UnsubscriptionResult = result;
            }
            else
            {
                model.UnsubscriptionResult = ResHelper.GetString("newsletter.unsubscribefailed");
                model.IsError = true;
            }

            return View(model);
        }


        // GET: Subscription/Show
        public ActionResult Show()
        {
            var model = new SubscribeModel();
            return PartialView("_Subscribe", model);
        }
    }
}