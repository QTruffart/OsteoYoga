using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.Holiday;
using OsteoYoga.WebSite.Controllers;
using OsteoYoga.WebSite.Helpers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class HolidayControllerHaveTo
    {

        private const string PassPagePath = "/Views/Administration/PassAdmin.cshtml";
        readonly Mock<Repository<Holiday>> holidayRepositoryMock = new Mock<Repository<Holiday>>();
        readonly Mock<DateRepository> dateRepositoryMock = new Mock<DateRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private HolidayController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new HolidayController
                {
                    HolidayRepository = holidayRepositoryMock.Object,
                    DateRepository = dateRepositoryMock.Object
                };
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(true);
            SessionHelper.Instance = sessionHelperMock.Object;
        }

        [TestMethod]
        public void LoadDefaultPage()
        {
            PartialViewResult view = Controller.Index();

            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void GetAllHolidaysAndOrderThem()
        {
            DateTime dateTime1 = new DateTime(2013, 07, 25);
            DateTime dateTime2 = new DateTime(2013, 07, 24);
            IList<Holiday> holidays = new List<Holiday>()
                {
                    new Holiday { Day = dateTime1 }, 
                    new Holiday { Day = dateTime2 }
                };
            holidayRepositoryMock.Setup(hrm => hrm.GetAll()).Returns(holidays);

            PartialViewResult view = Controller.ListOfHolidays();

            holidayRepositoryMock.Verify(drm => drm.GetAll());
            Assert.AreEqual(2, ((IList<Holiday>)view.Model).Count);
            Assert.AreEqual(dateTime2, ((IList<Holiday>)view.Model)[0].Day);
            Assert.AreEqual(dateTime1, ((IList<Holiday>)view.Model)[1].Day);
        }

        [TestMethod]
        public void AddAnHoliday()
        {
            DateTime dateTime = new DateTime(2013, 07, 15);
            const string hours = "08:00-12:45";
            TimeSpan begin = new TimeSpan(8,0,0);
            TimeSpan end = new TimeSpan(12,45,0);
            dateRepositoryMock.Setup(drm => drm.GetByDay(dateTime)).Returns(new List<Date>());

            PartialViewResult view = Controller.AddAnHoliday(dateTime, hours);

            dateRepositoryMock.Verify(drm => drm.GetByDay(dateTime), Times.Once());
            holidayRepositoryMock.Verify(hrm => hrm.Save(It.Is<Holiday>(h => h.Day.Date == dateTime.Date && 
                                                                             h.BeginHour == begin && 
                                                                             h.EndHour == end)), Times.Once());
            Assert.AreEqual(HolidayResource.HolidayTaken, view.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view.ViewName);
        }

        [TestMethod]
        public void AddAnHolidayIfThereAreDateTakenOutOfHours()
        {
            const string hours = "08:00-12:45";
            DateTime dateTime = new DateTime(2013, 07, 15);
            IList<Date> dates = new List<Date> {new Date { Day = dateTime, 
                                                           TimeSlot = new TimeSlot()
                                                           {
                                                               BeginHour = new TimeSpan(12,46,0),
                                                               EndHour = new TimeSpan(16,0,0)
                                                           }}};
            TimeSpan begin = new TimeSpan(8, 0, 0);
            TimeSpan end = new TimeSpan(12, 45, 0);
            dateRepositoryMock.Setup(drm => drm.GetByDay(dateTime)).Returns(dates);

            PartialViewResult view = Controller.AddAnHoliday(dateTime, hours);

            holidayRepositoryMock.Verify(hrm => hrm.Save(It.Is<Holiday>(h => h.Day.Date == dateTime.Date &&
                                                                             h.BeginHour == begin &&
                                                                             h.EndHour == end)), Times.Once());
            Assert.AreEqual(HolidayResource.HolidayTaken, view.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view.ViewName);
        }

        [TestMethod]
        public void DontAddAnHolidayIfThereAreDateTakenAtThisDateAndInThisHours()
        {
            const string hours1 = "08:00-12:46";
            const string hours2 = "15:59-17:00";
            const string hours3 = "14:00-15:00";
            const string hours4 = "07:00-19:00";
            DateTime dateTime = new DateTime(2013, 07, 15);
            IList<Date> dates = new List<Date> {new Date{ Day = dateTime, TimeSlot = new TimeSlot()
                                                                    {
                                                                        BeginHour = new TimeSpan(12,45,0),
                                                                        EndHour = new TimeSpan(16,0,0)
                                                                    }}};
            dateRepositoryMock.Setup(drm => drm.GetByDay(dateTime)).Returns(dates);

            PartialViewResult view = Controller.AddAnHoliday(dateTime, hours1);

            holidayRepositoryMock.Verify(hrm => hrm.Save(It.IsAny<Holiday>()), Times.Never());
            Assert.AreEqual(HolidayResource.DateTakenAtThisDate, view.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view.ViewName);

            PartialViewResult view2 = Controller.AddAnHoliday(dateTime, hours2);

            holidayRepositoryMock.Verify(hrm => hrm.Save(It.IsAny<Holiday>()), Times.Never());
            Assert.AreEqual(HolidayResource.DateTakenAtThisDate, view2.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view2.ViewName);

            PartialViewResult view3 = Controller.AddAnHoliday(dateTime, hours3);

            holidayRepositoryMock.Verify(hrm => hrm.Save(It.IsAny<Holiday>()), Times.Never());
            Assert.AreEqual(HolidayResource.DateTakenAtThisDate, view3.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view3.ViewName);

            PartialViewResult view4 = Controller.AddAnHoliday(dateTime, hours4);
            holidayRepositoryMock.Verify(hrm => hrm.Save(It.IsAny<Holiday>()), Times.Never());
            Assert.AreEqual(HolidayResource.DateTakenAtThisDate, view4.ViewBag.Result);
            Assert.AreEqual("HolidayCreated", view4.ViewName);
        }

        [TestMethod]
        public void CancelAnHoliday()
        {
            const int id = 1;
            Holiday holiday = new Holiday();
            holidayRepositoryMock.Setup(hrm => hrm.GetById(id)).Returns(holiday);

            PartialViewResult view = Controller.CancelAnHoliday(id);
            
            holidayRepositoryMock.Verify(hrm => hrm.Delete(holiday), Times.Once());
            Assert.AreEqual("HolidayCancelled", view.ViewName);
            Assert.AreEqual(HolidayResource.HolidayCancelled, view.ViewBag.Result);
            Assert.AreEqual(holiday, view.Model);
        }

        [TestMethod]
        public void ReturnOnPassPageIfNoAdminConnected()
        {
            sessionHelperMock.SetupGet(shm => shm.AdminConnected).Returns(false);

            Assert.AreEqual(PassPagePath, Controller.AddAnHoliday(new DateTime(), string.Empty ).ViewName);
            Assert.AreEqual(PassPagePath, Controller.CancelAnHoliday(1).ViewName);
            Assert.AreEqual(PassPagePath, Controller.ListOfHolidays().ViewName);
            Assert.AreEqual(PassPagePath, Controller.Index().ViewName);
        }

    }
}
