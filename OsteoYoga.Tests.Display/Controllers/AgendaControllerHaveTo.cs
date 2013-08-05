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
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private AgendaController Controller { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Controller = new AgendaController
            {
                DateRepository = dateRepositoryMock.Object,
                HolidayRepository = holidayRepositoryMock.Object,
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
    }
}
