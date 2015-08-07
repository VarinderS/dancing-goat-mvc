using System;

using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.Protection;
using CMS.SiteProvider;

namespace MvcDemo.Web.Services
{
    /// <summary>
    /// Provides methods for managing subscriptions to newsletters for one site.
    /// </summary>
    public class NewsletterSubscriptionService
    {
        #region "Constants & Variables"

        private readonly string mSiteName;
        private int mSiteId;

        #endregion


        #region "Properties"

        private int SiteId
        {
            get
            {
                if (mSiteId == 0)
                {
                    mSiteId = SiteInfoProvider.GetSiteID(mSiteName);
                }

                return mSiteId;
            }
        }

        #endregion


        #region "Public Methods"

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterSubscriptionService"/> class.
        /// </summary>
        /// <param name="siteName">The code name of a site.</param>
        public NewsletterSubscriptionService(string siteName)
        {
            mSiteName = siteName;
        }


        /// <summary>
        /// Subscribes e-mail address to specific newsletter. 
        /// If no subscriber with the given e-mail is found, the new one is created.
        /// Action fails when:
        /// <list type="bullet">
        /// <item>
        /// <description>Request is performed from banned IP address</description>
        /// </item>
        /// <item>
        /// <description>Email is already subscribed to the newsletter</description>
        /// </item>
        /// <item>
        /// <description>Newsletter with the given name does not exist</description>
        /// </item>
        /// <item>
        /// <description>Current licence does not allow creating new subscribers</description>
        /// </item>
        /// </list>
        /// errorMessage output parameter is filled with localized error description in all these cases.
        /// No exception can be thrown.
        /// </summary>
        /// <param name="email">Subscriber's e-mail address</param>
        /// <param name="newsletterName">Name of the newsletter</param>
        /// <param name="errorMessage">Error message used when subscription failed</param>
        /// <returns>Returns true if subscription was successful</returns>
        public virtual bool Subscribe(string email, string newsletterName, out string errorMessage)
        {
            bool result = false;

            // Creates new transaction for saving subscriber's information
            using (var tr = new CMSTransactionScope())
            {
                // Saves subscriber into the database
                SubscriberInfo subscriber = SaveSubscriber(email, out errorMessage);
                if ((subscriber != null) && string.IsNullOrEmpty(errorMessage))
                {
                    // Assignes subscriber into the newsletter
                    bool newsletterSubscribed = SaveNewsletter(newsletterName, subscriber, out errorMessage);
                    if (newsletterSubscribed && string.IsNullOrEmpty(errorMessage))
                    {
                        result = true;
                    }
                }

                // Saves changes
                tr.Commit();
            }

            return result;
        }


        /// <summary>
        /// Unsubscribes subscriber from specific newsletter.
        /// If subscriber is already unsubscribed, nothing happens (except filling output parameter with confirmation message)
        /// No exception can be thrown.
        /// </summary>
        /// <param name="newsletterGuid">Newsletter unique identifier</param>
        /// <param name="subscriberGuid">Subscriber unique identifier</param>
        /// <param name="issueGuid">Issue unique identifier</param>
        /// <param name="additionalInformation">Additional information (error/status message etc.)</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        public virtual bool Unsubscribe(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, out string additionalInformation)
        {
            return UnsubscribeInternal(newsletterGuid, subscriberGuid, issueGuid, false, out additionalInformation);
        }


        /// <summary>
        /// Unsubscribes subscriber from all marketing materials.
        /// If subscriber is already unsubscribed, nothing happens (except filling output parameter with confirmation message)
        /// No exception can be thrown.
        /// </summary>
        /// <param name="newsletterGuid">Newsletter unique identifier</param>
        /// <param name="subscriberGuid">Subscriber unique identifier</param>
        /// <param name="issueGuid">Issue unique identifier</param>
        /// <param name="additionalInformation">Additional information (status message etc.)</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        public virtual bool UnsubscribeFromAll(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, out string additionalInformation)
        {
            return UnsubscribeInternal(newsletterGuid, subscriberGuid, issueGuid, true, out additionalInformation);
        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// Saves subscriber.
        /// </summary>
        /// <param name="email">Subscriber's e-mail address</param>
        /// <param name="errorMessage">Error message used when save failed</param>
        /// <returns>Subscriber info object</returns>
        private SubscriberInfo SaveSubscriber(string email, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Checks whether this IP address is banned or not
            if (!BannedIPInfoProvider.IsAllowed(mSiteName, BanControlEnum.AllNonComplete))
            {
                errorMessage = ResHelper.GetString("General.BannedIP");
                return null;
            }

            // Gets information about subscriber based on email address and current site
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(email, SiteId);
            if (subscriber == null)
            {
                // Creates new subscriber
                subscriber = new SubscriberInfo
                {
                    SubscriberEmail = email,
                    SubscriberSiteID = SiteId
                };

                // Checks subscriber limits
                if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
                {
                    errorMessage = ResHelper.GetString("LicenseVersionCheck.Subscribers");
                    return null;
                }

                // Saves subscriber info
                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            // Returns subscriber info object
            return subscriber;
        }


        /// <summary>
        /// Saves the Newsletter.
        /// </summary>
        /// <param name="newsletterName">Name of the newsletter</param>
        /// <param name="subscriber">Subscriber object</param>
        /// <param name="errorMessage">Error message used when save failed</param>
        /// <returns>True if save was successful</returns>
        private bool SaveNewsletter(string newsletterName, SubscriberInfo subscriber, out string errorMessage)
        {
            // Creates new Service for subscriptions
            var subscriptionService = Service<ISubscriptionService>.Entry();
            errorMessage = string.Empty;

            // Checks if subscriber info object exists
            if ((subscriber == null) || string.IsNullOrEmpty(newsletterName))
            {
                return false;
            }

            // Gets information about newsletter on current site
            NewsletterInfo newsletter = NewsletterInfoProvider.GetNewsletterInfo(newsletterName, SiteId);
            if (newsletter != null)
            {
                try
                {
                    // Checks if subscriber is not already subscribed
                    if (!subscriptionService.IsSubscribed(subscriber.SubscriberID, newsletter.NewsletterID))
                    {
                        subscriptionService.Subscribe(subscriber.SubscriberID, newsletter.NewsletterID, new SubscribeSettings
                        {
                            RemoveAlsoUnsubscriptionFromAllNewsletters = true,
                        });

                        return true;
                    }

                    // Sets info message - subscriber is already in site
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
        /// <param name="newsletterGuid">Newsletter unique identifier</param>
        /// <param name="subscriberGuid">Subscriber unique identifier</param>
        /// <param name="issueGuid">Issue unique identifier</param>
        /// <param name="unsubscribeFromAll">If true, subscriber is unsubscribed from all marketing materials</param>
        /// <param name="additionalInformation">Additional information (status message etc.)</param>
        /// <returns>Returns true if unsubscription was successful</returns>
        private bool UnsubscribeInternal(Guid newsletterGuid, Guid subscriberGuid, Guid issueGuid, bool unsubscribeFromAll, out string additionalInformation)
        {
            // Creates required Services
            var subscriptionService = Service<ISubscriptionService>.Entry();
            var unSubscriptionProvider = Service<IUnsubscriptionProvider>.Entry();

            // Validates Subscriber and Newsletter GUIDs
            if ((subscriberGuid == Guid.Empty) || (newsletterGuid == Guid.Empty))
            {
                // Either SubscriberGUID or NewsletterGUID was not supplied, don't unsubscribe
                additionalInformation = ResHelper.GetString("TestMvcDemo.News.InvallidUnsubscriptionLink");
                return false;
            }

            // Gets information about subscriber, newsletter and issue
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(subscriberGuid, SiteId);
            NewsletterInfo newsletter = NewsletterInfoProvider.GetNewsletterInfo(newsletterGuid, SiteId);
            IssueInfo issue = IssueInfoProvider.GetIssueInfo(issueGuid, SiteId);

            if ((subscriber == null) || (newsletter == null) || (issue == null))
            {
                additionalInformation = ResHelper.GetString("TestMvcDemo.News.InvallidUnsubscriptionLink");
                return false;
            }

            int? issueId = issue.IssueID;

            // Creates new transaction for saving subscriber's information
            using (var tr = new CMSTransactionScope())
            {
                try
                {
                    if (unsubscribeFromAll)
                    {
                        // Unsubscribes if not already unsubscribed
                        if (!unSubscriptionProvider.IsUnsubscribedFromAllNewsletters(subscriber.SubscriberEmail, newsletter.NewsletterSiteID))
                        {
                            subscriptionService.UnsubscribeFromAllNewsletters(subscriber.SubscriberEmail, SiteId, issueId);
                            tr.Commit();
                        }

                        additionalInformation = ResHelper.GetString("TestMvcDemo.News.UnsubscribedAll");
                        return true;
                    }

                    // Unsubscribes if not already unsubscribed
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
                    additionalInformation = ResHelper.GetString("newsletter.unsubscribefailed");
                    return false;
                }
            }
        }

        #endregion
    }
}