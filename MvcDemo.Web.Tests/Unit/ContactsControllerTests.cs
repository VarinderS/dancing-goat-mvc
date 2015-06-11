using System;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Tests;
using CMS.DocumentEngine.Types;
using CMS.Globalization;
using CMS.Tests;

using MvcDemo.Web.Controllers;
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
        private FormItemRepository mFormItemRepository;


        [SetUp]
        public void SetUp()
        {
            Fake<CountryInfo, CountryInfoProvider>();
            Fake<StateInfo, StateInfoProvider>();
            Fake().DocumentType<Contact>(Contact.CLASS_NAME);

            var country = new CountryInfo { CountryName = "USA", CountryTwoLetterCode = "US" };
            var state = new StateInfo { StateName = "Illinois", StateDisplayName = "Illinois" };
            var contact = TreeNode.New<Contact>().With(x => x.ContactCountry = "USA;Illinois");

            var socialLinkRepository = Substitute.For<SocialLinkRepository>();
            var cafeRepository = Substitute.For<CafeRepository>();
            var contactRepository = Substitute.For<ContactRepository>();
            var countryRepository = Substitute.For<CountryRepository>();
            var localizationService = Substitute.For<LocalizationService>();
            
            contactRepository.GetCompanyContact().Returns(contact);
            countryRepository.GetCountry(country.CountryName).Returns(country);
            countryRepository.GetState(state.StateName).Returns(state);

            mFormItemRepository = Substitute.For<FormItemRepository>();
            mController = new ContactsController(cafeRepository, socialLinkRepository, contactRepository, mFormItemRepository, countryRepository, localizationService);
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