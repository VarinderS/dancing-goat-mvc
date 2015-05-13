using System;

using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.Protection;
using CMS.SiteProvider;

namespace MvcDemo.Web.Services
{
    public class NewsletterSubscriptionService
    {
        #region "Constants & Variables"

        private const string CURRENT_SITE_NAME = "TestMvcDemo";
        private static int mCurrentSiteId;

        #endregion


        #region "Properties"

        private int CurrentSiteId
        {
            get
            {
                if (mCurrentSiteId == 0)
                {
                    mCurrentSiteId = SiteInfoProvider.GetSiteID(CURRENT_SITE_NAME);
                }

                return mCurrentSiteId;
            }
        }

        #endregion


        #region "Public Methods"

        /// <summary>
        /// Subscribes e-mail address to specific newsletter.
        /// </summary>
        /// <param name="email">E-mail address</param>
        /// <param name="newsletterName">Newsletter name</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Returns true if subscription was successful</returns>
        public virtual bool Subscribe(string email, string newsletterName, out string errorMessage)
        {
            var result = false;

            using (var tr = new CMSTransactionScope())
            {
                var subscriber = SaveSubscriber(email, out errorMessage);
                if ((subscriber != null) && string.IsNullOrEmpty(errorMessage))
                {
                    var newsletterSubscribed = SaveNewsletter(newsletterName, subscriber, out errorMessage);
                    if (newsletterSubscribed && string.IsNullOrEmpty(errorMessage))
                    {
                        result = true;
                    }
                }

                tr.Commit();
            }

            return result;
        }


        /// <summary>
        /// Unsubscribes subscriber.
        /// </summary>
        /// <param name="newsletterGuid">Newsletter Guid</param>
        /// <param name="subscriberGuid">Subscriber Guid</param>
        /// <param name="issueGuid">Issue Guid</param>
        /// <param name="additionalInformation">Additional information</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        public virtual bool Unsubscribe(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, out string additionalInformation)
        {
            return UnsubscribeInternal(newsletterGuid, subscriberGuid, issueGuid, false, out additionalInformation);
        }


        /// <summary>
        /// Unsubscribes subscriber from all marketing materials.
        /// </summary>
        /// <param name="newsletterGuid">Newsletter Guid</param>
        /// <param name="subscriberGuid">Subscriber Guid</param>
        /// <param name="issueGuid">Issue Guid</param>
        /// <param name="additionalInformation">Additional information</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        public virtual bool UnsubscribeFromAll(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, out string additionalInformation)
        {
            return UnsubscribeInternal(newsletterGuid, subscriberGuid, issueGuid, true, out additionalInformation);
        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// Save subscriber.
        /// </summary>
        /// <param name="email">Subscriber's email</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Subscriber info object</returns>
        private SubscriberInfo SaveSubscriber(string email, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check banned IPs
            if (!BannedIPInfoProvider.IsAllowed(CURRENT_SITE_NAME, BanControlEnum.AllNonComplete))
            {
                errorMessage = ResHelper.GetString("General.BannedIP");
                return null;
            }

            var subscriber = SubscriberInfoProvider.GetSubscriberInfo(email, CurrentSiteId);
            if (subscriber == null)
            {
                // Create subscriber
                subscriber = new SubscriberInfo
                {
                    SubscriberEmail = email,
                    SubscriberSiteID = CurrentSiteId
                };

                // Check subscriber limits
                if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
                {
                    errorMessage = ResHelper.GetString("LicenseVersionCheck.Subscribers");
                    return null;
                }

                // Save subscriber info
                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            // Return subscriber info object
            return subscriber;
        }


        /// <summary>
        /// Saves the Newsletter.
        /// </summary>
        /// <param name="newsletterName">Newsletter name</param>
        /// <param name="subscriber">Subscriber object</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>True if save was successful</returns>
        private bool SaveNewsletter(string newsletterName, SubscriberInfo subscriber, out string errorMessage)
        {
            var subscriptionService = Service<ISubscriptionService>.Entry();
            errorMessage = string.Empty;

            // Check if subscriber info object exists
            if ((subscriber == null) || string.IsNullOrEmpty(newsletterName))
            {
                return false;
            }

            // Get newsletter info
            var newsletter = NewsletterInfoProvider.GetNewsletterInfo(newsletterName, CurrentSiteId);
            if (newsletter != null)
            {
                try
                {
                    // Check if subscriber is not already subscribed
                    if (!subscriptionService.IsSubscribed(subscriber.SubscriberID, newsletter.NewsletterID))
                    {
                        subscriptionService.Subscribe(subscriber.SubscriberID, newsletter.NewsletterID, new SubscribeSettings
                        {
                            RemoveAlsoUnsubscriptionFromAllNewsletters = true,
                        });

                        return true;
                    }

                    // Info message - subscriber is already in site
                    errorMessage = ResHelper.GetString("NewsletterSubscription.SubscriberIsAlreadySubscribed");
                }
                catch (Exception exception)
                {
                    errorMessage = exception.Message;
                }
            }
            else
            {
                errorMessage = ResHelper.GetString("NewsletterSubscription.NewsletterDoesNotExist");
            }

            return false;
        }


        /// <summary>
        /// Unsubscribes subscriber.
        /// </summary>
        /// <param name="newsletterGuid">Newsletter Guid</param>
        /// <param name="subscriberGuid">Subscriber Guid</param>
        /// <param name="issueGuid">Issue Guid</param>
        /// <param name="unsubscribeFromAll">If true, subscriber is unsubscribed from all marketing materials</param>
        /// <param name="additionalInformation">Additional information</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        private bool UnsubscribeInternal(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, bool unsubscribeFromAll, out string additionalInformation)
        {
            var subscriptionService = Service<ISubscriptionService>.Entry();
            var unSubscriptionProvider = Service<IUnsubscriptionProvider>.Entry();
            var localizationService = Service<ILocalizationService>.Entry();

            // Validate Subscriber and Newsletter GUIDs
            if ((subscriberGuid == Guid.Empty) || (newsletterGuid == Guid.Empty))
            {
                // Either SubscriberGUID or NewsletterGUID was not supplied, don't unsubscribe
                additionalInformation = ResHelper.GetString("TestMvcDemo.News.InvallidUnsubscriptionLink");
                return false;
            }

            var subscriber = SubscriberInfoProvider.GetSubscriberInfo(subscriberGuid, CurrentSiteId);
            var newsletter = NewsletterInfoProvider.GetNewsletterInfo(newsletterGuid, CurrentSiteId);
            var issue = IssueInfoProvider.GetIssueInfo(issueGuid, CurrentSiteId);

            if ((subscriber == null) || (newsletter == null) || (issue == null))
            {
                additionalInformation = ResHelper.GetString("TestMvcDemo.News.InvallidUnsubscriptionLink");
                return false;
            }

            int? issueId = issue.IssueID;

            using (var tr = new CMSTransactionScope())
            {
                try
                {
                    if (unsubscribeFromAll)
                    {
                        // Unsubscribe if not already unsubscribed
                        if (!unSubscriptionProvider.IsUnsubscribedFromAllNewsletters(subscriber.SubscriberEmail, newsletter.NewsletterSiteID))
                        {
                            subscriptionService.UnsubscribeFromAllNewsletters(subscriber.SubscriberEmail, CurrentSiteId, issueId);
                            tr.Commit();
                        }

                        additionalInformation = ResHelper.GetString("TestMvcDemo.News.UnsubscribedAll");
                        return true;
                    }

                    // Unsubscribe if not already unsubscribed
                    if (!unSubscriptionProvider.IsUnsubscribedFromSingleNewsletter(subscriber.SubscriberEmail, newsletter.NewsletterID, newsletter.NewsletterSiteID))
                    {
                        subscriptionService.UnsubscribeFromSingleNewsletter(subscriber.SubscriberEmail, newsletter.NewsletterID, issueId);
                        tr.Commit();
                    }

                    additionalInformation = ResHelper.GetString("TestMvcDemo.News.Unsubscribed");
                    return true;
                }
                catch (Exception exception)
                {
                    Service<IEventLogService>.Entry().LogException("Newsletters", "Unsubscribe", exception);
                    additionalInformation = localizationService.GetString("newsletter.unsubscribefailed");
                    return false;
                }
            }
        }

        #endregion
    }
}