using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.WebSite.Controllers;
using OsteoYoga.WebSite.Helpers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class AdministrationControllerHaveTo
    {
        private const string PassPagePath = "PassAdmin";
        readonly Mock<TimeSlotRepository> timeSlotRepositoryMock = new Mock<TimeSlotRepository>();
        readonly Mock<ContactRepository> contactRepositoryMock = new Mock<ContactRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        readonly Mock<Constants> constantsMock = new Mock<Constants>();
        private AdministrationController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new AdministrationController()
            {
                TimeSlotRepository = timeSlotRepositoryMock.Object,
                ContactRepository = contactRepositoryMock.Object
            };
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(true);
            SessionHelper.Instance = sessionHelperMock.Object;
            Constants.Instance = constantsMock.Object;
        }

        [TestMethod]
        public void InitializeTimeSlotCallTimeSlotRepository()
        {
            Controller.InitializeTimeSlot();

            timeSlotRepositoryMock.Verify(tsr =>tsr.DeleteAll(), Times.Once());
            VerifySaveCallForTimeSlotFor(DayOfWeek.Monday);
            VerifySaveCallForTimeSlotFor(DayOfWeek.Tuesday);
            VerifySaveCallForTimeSlotFor(DayOfWeek.Thursday);
            VerifySaveCallForTimeSlotFor(DayOfWeek.Friday);
            VerifySaveCallForTimeSlotForSaturday();
        }

        private void VerifySaveCallForTimeSlotFor(DayOfWeek day)
        {
            VerifySaveCallForTimeSlot(day, 09, 30, 10, 15);
            VerifySaveCallForTimeSlot(day, 10, 15, 11, 00);
            VerifySaveCallForTimeSlot(day, 11, 00, 11, 45);
            VerifySaveCallForTimeSlot(day, 11, 45, 12, 30);
            VerifySaveCallForTimeSlot(day, 13, 45, 14, 30);
            VerifySaveCallForTimeSlot(day, 14, 30, 15, 15);
            VerifySaveCallForTimeSlot(day, 15, 15, 16, 00);
        }

        private void VerifySaveCallForTimeSlotForSaturday()
        {
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 09, 00, 09, 45);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 09, 45, 10, 30);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 10, 30, 11, 15);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 11, 15, 12, 00);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 12, 00, 12, 45);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 13, 45, 14, 30);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 14, 30, 15, 15);
            VerifySaveCallForTimeSlot(DayOfWeek.Saturday, 15, 15, 16, 00);
        }

        private void VerifySaveCallForTimeSlot(DayOfWeek dayOfWeek, int beginHour, int beginMinutes, int endHour, int endMinutes)
        {
            timeSlotRepositoryMock.Verify(tsr =>tsr.Save(It.Is<TimeSlot>( ts => ts.DayOfWeek == dayOfWeek 
                                                                       && ts.BeginHour.Hours == beginHour
                                                                       && ts.BeginHour.Minutes == beginMinutes
                                                                       && ts.EndHour.Hours == endHour
                                                                       && ts.EndHour.Minutes == endMinutes)), Times.Once());
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
