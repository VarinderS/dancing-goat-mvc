using System;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Tests;
using CMS.DocumentEngine.Types;
using CMS.Globalization;
using CMS.Tests;

using MvcDemo.Web.Controllers;
using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Models.Contacts;
using MvcDemo.Web.Repositories;
using MvcDemo.Web.Services;

using NSubstitute;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace MvcDemo.Web.Tests.Unit
{
    [TestFixture]
    public class ContactsControllerTests : UnitTests
    {
        private ContactsController mController;
        private MessageModel mMessageModel;
        private IFormItemRepository mFormItemRepository;


        [SetUp]
        public void SetUp()
        {
            Fake<CountryInfo, CountryInfoProvider>();
            Fake<StateInfo, StateInfoProvider>();
            Fake().DocumentType<Contact>(Contact.CLASS_NAME);

            var country = new CountryInfo { CountryName = "USA", CountryTwoLetterCode = "US" };
            var state = new StateInfo { StateName = "Illinois", StateDisplayName = "Illinois" };
            var contact = TreeNode.New<Contact>().With(x => x.Fields.Country = "USA;Illinois");

            var socialLinkRepository = Substitute.For<ISocialLinkRepository>();
            var cafeRepository = Substitute.For<ICafeRepository>();
            var contactRepository = Substitute.For<IContactRepository>();
            var countryRepository = Substitute.For<ICountryRepository>();
            var localizationService = Substitute.For<LocalizationService>();
            var outputCacheDependencies = Substitute.For<IOutputCacheDependencies>();
            
            contactRepository.GetCompanyContact().Returns(contact);
            countryRepository.GetCountry(country.CountryName).Returns(country);
            countryRepository.GetState(state.StateName).Returns(state);

            mFormItemRepository = Substitute.For<IFormItemRepository>();
            mController = new ContactsController(cafeRepository, socialLinkRepository, contactRepository, mFormItemRepository, countryRepository, localizationService, outputCacheDependencies);
            mMessageModel = CreateMessageModel();
        }


        [Test]
        public void Index_RendersDefaultView()
        {
            mController.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView();
        }


        [Test]
        public void SendMessage_WithMessageModel_PassesCorrectDataToFormItemRepository()
        {
            mController.WithCallTo(c => c.SendMessage(mMessageModel));
            mFormItemRepository.Received().CreateContactUsFormItem(mMessageModel);
        }


        [Test]
        public void SendMessage_WithMessageModel_RedirectsToThankYouAction()
        {
            mController.WithCallTo(c => c.SendMessage(mMessageModel))
                .ShouldRedirectTo(c => c.ThankYou);
        }


        [Test]
        public void SendMessage_WithExceptionDuringInsertingFormItem_RendersErrorView()
        {
            mFormItemRepository.When(x => x.CreateContactUsFormItem(mMessageModel)).Throw(x => new Exception());
            mController.WithCallTo(c => c.SendMessage(mMessageModel)).ShouldRenderView("Error");
        }


        private MessageModel CreateMessageModel()
        {
            return new MessageModel
            {
                Email = "ezekiel.kemboi@startmail.com",
                FirstName = "Ezekiel",
                LastName = "Kemboi",
                MessageText = "This is a message from Ezekiel Kemboi"
            };
        }
    }
}