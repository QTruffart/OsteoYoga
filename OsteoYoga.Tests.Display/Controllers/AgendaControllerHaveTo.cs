using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Display.Controllers;
using OsteoYoga.Display.Helpers;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class AgendaControllerHaveTo
    {
        private const int ID = 1;
        private const string PassPagePath = "/Views/Administration/PassAdmin.cshtml";
        readonly Mock<DateRepository> dateRepositoryMock = new Mock<DateRepository>();
        readonly Mock<HolidayRepository> holidayRepositoryMock = new Mock<HolidayRepository>();
        readonly Mock<TimeSlotRepository> timeSlotRepositoryMock = new Mock<TimeSlotRepository>();
        readonly Mock<ContactRepository> contactRepositoryMock = new Mock<ContactRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private AgendaController Controller { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Controller = new AgendaController
            {
                DateRepository = dateRepositoryMock.Object,
                HolidayRepository = holidayRepositoryMock.Object,
                TimeSlotRepository = timeSlotRepositoryMock.Object,
                ContactRepository = contactRepositoryMock.Object
            };
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(true);
            SessionHelper.Instance = sessionHelperMock.Object;
        }

       
        [TestMethod]
        public void GetAllFutureDate()
        {
            IList<Date> dates = new List<Date>();
            IList<Holiday> holidays = new List<Holiday>();
            dateRepositoryMock.Setup(drm => drm.GetFutureDates(It.Is<DateTime>(dt => dt.Date == DateTime.Now.AddDays(-3).Date))).Returns(dates);
            holidayRepositoryMock.Setup(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>())).Returns(holidays);

            PartialViewResult view = Controller.Index();

            dateRepositoryMock.Verify(drm => drm.GetFutureDates(It.Is<DateTime>(dt => dt.Date == DateTime.Now.AddDays(-3).Date)));
            dateRepositoryMock.Verify(drm => drm.GetFutureDates(It.IsAny<DateTime>()));
            Assert.AreEqual("Index", view.ViewName);
            Assert.AreEqual(dates, view.Model);
            Assert.AreEqual(holidays, view.ViewBag.Holidays);
        }

        [TestMethod]
        public void GetDateDetails()
        {
            int id = 1;
            Date date = new Date();
            dateRepositoryMock.Setup(drm => drm.GetById(id)).Returns(date);

            PartialViewResult view = Controller.GetDetailDate(id);

            dateRepositoryMock.Verify(drm => drm.GetById(id));
            Assert.AreEqual("GetDetailDate", view.ViewName);
            Assert.AreEqual(date, view.Model);
        }

        [TestMethod]
        public void DeleteDate()
        {
            Date date = new Date();
            dateRepositoryMock.Setup(drm => drm.GetById(ID)).Returns(date);

            Controller.DeleteDate(ID);

            dateRepositoryMock.Verify(drm => drm.GetById(ID));
            dateRepositoryMock.Verify(drm => drm.Delete(date));
        }

        [TestMethod]
        public void ReturnOnPassPageIfNoAdminConnected()
        {
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(false);

            Assert.AreEqual(PassPagePath, Controller.GetDetailDate(ID).ViewName);
            Assert.AreEqual(PassPagePath, Controller.Index().ViewName);
        }

        [TestMethod]
        public void GetCreateDatePage()
        {
            List<Contact> contacts = new List<Contact>();
            contactRepositoryMock.Setup(crm => crm.GetAll()).Returns(contacts);

            PartialViewResult viewResult = Controller.CreateDate();

            contactRepositoryMock.Verify(crm => crm.GetAll(), Times.Once());
            Assert.AreEqual("CreateDate", viewResult.ViewName);
            Assert.AreEqual(contacts, viewResult.Model);
        }

        [TestMethod]
        public void CreateADate(){

            int timeSlotId = 1;
            int contactId = 2;
            Contact contact = new Contact(){Id = contactId,};
            TimeSlot timeSlot = new TimeSlot(){Id = timeSlotId};
            DateTime dateTime = DateTime.Now;
            contactRepositoryMock.Setup(crm => crm.GetById(contactId)).Returns(contact);
            timeSlotRepositoryMock.Setup(tsrm => tsrm.GetById(timeSlotId)).Returns(timeSlot);
            dateRepositoryMock.Setup(drm => drm.Save(It.Is<Date>(d => d.Contact == contact && d.TimeSlot == timeSlot && d.Day == dateTime)));
            
            Date date = Controller.CreateDate(timeSlotId, contactId, dateTime);

            contactRepositoryMock.Verify(crm => crm.GetById(contactId), Times.Once());
            timeSlotRepositoryMock.Verify(tsrm => tsrm.GetById(timeSlotId), Times.Once());
            dateRepositoryMock.Verify(drm => drm.Save(It.Is<Date>(d => d.Contact == contact && d.TimeSlot == timeSlot && d.Day == dateTime)), Times.Once());
            Assert.AreEqual(contact, date.Contact);
            Assert.AreEqual(timeSlot, date.TimeSlot);
            Assert.AreEqual(dateTime, date.Day);
            Assert.IsTrue(date.IsConfirmed);
            Assert.IsNotNull(date.ConfirmationId);
        }

        [TestMethod]
        public void GetFreeTimeSlots(){
            DateTime dateTime = DateTime.Now;
            IList<TimeSlot> slots = new List<TimeSlot>();
            timeSlotRepositoryMock.Setup(tsrm => tsrm.GetFreeTimeSlots(dateTime)).Returns(slots);

            PartialViewResult viewResult = Controller.GetTimeSlotsForADay(dateTime);

            timeSlotRepositoryMock.Verify(tsrm => tsrm.GetFreeTimeSlots(dateTime), Times.Once());
            Assert.AreEqual(slots, viewResult.Model);
            Assert.AreEqual("GetTimeSlotsForADay", viewResult.ViewName);
        }
    }
}
