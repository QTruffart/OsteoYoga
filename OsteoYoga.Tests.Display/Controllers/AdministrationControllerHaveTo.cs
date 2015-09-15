using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;
using OsteoYoga.WebSite.Controllers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class AdministrationControllerHaveTo
    {
        private const string PassPagePath = "PassAdmin";
        readonly Mock<ContactRepository> contactRepositoryMock = new Mock<ContactRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        readonly Mock<Constants> constantsMock = new Mock<Constants>();
        private AdministrationController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new AdministrationController()
            {
                ContactRepository = contactRepositoryMock.Object
            };
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(true);
            SessionHelper.Instance = sessionHelperMock.Object;
            Constants.Instance = constantsMock.Object;
        }


        [TestMethod]
        public void GetAllContacts()
        {
            IList<Contact> contacts = new List<Contact>();
            contactRepositoryMock.Setup(crm => crm.GetAll()).Returns(contacts);

            PartialViewResult view = Controller.UserList();

            contactRepositoryMock.Verify(drm => drm.GetAll());
            Assert.AreEqual(contacts, view.Model);
            Assert.AreEqual("UserList", view.ViewName);
        }

        [TestMethod]
        public void ReturnOnPassPageIfNoAdminConnected()
        {
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(false);

            Assert.AreEqual(PassPagePath, Controller.UserList().ViewName);
            Assert.AreEqual(PassPagePath, Controller.Index().ViewName);
        }

        [TestMethod]
        public void ConnectAdmin()
        {
            const string passWord = "pass";
            constantsMock.Setup(cm => cm.PassAdmin).Returns(passWord);

            ViewResult view = Controller.PassAdmin(passWord);

            sessionHelperMock.VerifySet(shm => shm.AdminConnected = true);
            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void ErrorOnConnectAdmin()
        {
            const string passWord = "pass";
            constantsMock.Setup(cm => cm.PassAdmin).Returns("erreur");

            ViewResult view = Controller.PassAdmin(passWord);

            sessionHelperMock.VerifySet(shm => shm.AdminConnected = true, Times.Never());
            Assert.AreEqual("PassAdmin", view.ViewName);
            Assert.AreEqual("Le mot de passe est incorrect", view.ViewBag.Errors);
        }
    }
}
