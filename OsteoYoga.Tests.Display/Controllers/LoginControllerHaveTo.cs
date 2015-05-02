using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

using Moq;
using OsteoYoga.Resource.Contact;
using OsteoYoga.WebSite.Controllers;
using OsteoYoga.WebSite.Helpers;


namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class LoginControllerHaveTo
    {
        const string Email = "toto@toto.com";
        Contact currentContact = new Contact();
        IList<Holiday> holidays = new List<Holiday>();
        readonly Mock<ContactRepository> contactRepositoryMock = new Mock<ContactRepository>();
        readonly Mock<HolidayRepository> holidayRepositoryMock = new Mock<HolidayRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private LoginController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new LoginController { 
                ContactRepository = contactRepositoryMock.Object,
                HolidayRepository = holidayRepositoryMock.Object
            };
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);
            holidayRepositoryMock.Setup(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>())).Returns(holidays);
            SessionHelper.Instance = sessionHelperMock.Object;
        }

        [TestMethod]
        public void LoadLoginPageIfThereIsNoContactConnected()
        {
            PartialViewResult view = Controller.Index();

            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void LoadRendezVousIndexPageIfThereIsContactConnected()
        {
            sessionHelperMock.SetupGet(shm => shm.CurrentUser).Returns(currentContact);

            PartialViewResult view = Controller.Index();

            holidayRepositoryMock.Verify(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>()), Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", view.ViewName);
            Assert.AreEqual(holidays, view.Model);
        }

        [TestMethod]
        public void ReLoadLoginPageIfTheEmailIsWrong()
        {
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(false);

            PartialViewResult view = Controller.Login(Email);

            Assert.AreEqual(LoginResource.UnknownEmail, view.ViewBag.Errors);
            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void LoadRendezVousIndexPageIfTheEmailIsCorrect()
        {
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetByEmail(Email)).Returns(currentContact);

            PartialViewResult view = Controller.Login(Email);

            holidayRepositoryMock.Verify(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>()), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = currentContact, Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", view.ViewName);
            Assert.AreEqual(holidays, view.Model);
        }

        [TestMethod]
        public void LoadSignInPageIfThereIsNoContactConnected()
        {
            PartialViewResult view = Controller.SignIn();

            Assert.AreEqual("SignIn", view.ViewName);
        }

        [TestMethod]
        public void ReLoadSignInPageIfTheEmailIsEmptyOrNull()
        {
            Mock<Contact> newContactMock = new Mock<Contact>();
            newContactMock.Setup(ncm => ncm.IsValid()).Returns(false);

            PartialViewResult view = Controller.SignIn(newContactMock.Object);

            Assert.AreEqual(newContactMock.Object, view.Model);
            Assert.AreEqual("SignIn", view.ViewName);
        }

        [TestMethod]
        public void ReLoadSignInPageIfTheEmailAlreadyExists()
        {
            Mock<Contact> newContactMock = new Mock<Contact>();
            newContactMock.Setup(ncm => ncm.IsValid()).Returns(true);
            newContactMock.SetupGet(ncm => ncm.Mail).Returns(Email);
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(true);
            
            PartialViewResult view = Controller.SignIn(newContactMock.Object);

            Assert.AreEqual(SignInResource.EmailAlreadyExists, view.ViewBag.SignInError);
            Assert.AreEqual(newContactMock.Object, view.Model);
            Assert.AreEqual("SignIn", view.ViewName);
        }

        [TestMethod]
        public void LoadRendezVousIndexPageIfTheContactIsCreated()
        {
            Contact newContact = new Contact(){FullName = "fullName", Phone = "0556579545", Mail = Email};
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(false);

            PartialViewResult view = Controller.SignIn(newContact);
            
            Assert.IsNotNull(newContact.ConfirmNumber);
            Assert.IsFalse(newContact.IsConfirmed);
            contactRepositoryMock.Verify(crm => crm.Save(newContact), Times.Once());
            holidayRepositoryMock.Verify(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>()), Times.Once());
            sessionHelperMock.VerifySet(shm=> shm.CurrentUser = newContact);
            Assert.AreEqual("/Views/RendezVous/Index.cshtml", view.ViewName);
            Assert.AreEqual(holidays, view.Model);
        }
    }
}
