using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.RendezVous;
using OsteoYoga.WebSite.Controllers;
using OsteoYoga.WebSite.Helpers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class RendezVousControllerHaveTo
    {
        private const string PatientHours = "14:00-18:00";
        private const string ServerAddress = "http://osteoyoga.fr";
        private const string IndexPath = "Index";
        private const string LoginPath = "/Views/Login/Index.cshtml";
        private const string ProposeDatePath = "ProposeDate";
        private const string CreateDatePath = "CreateDate";
        private const int TimeSlotId = 1;
        private const string ValidatePath = "Validate";
        private readonly Contact contact = new Contact();
        private readonly Mock<Repository<Contact>> contactSlotRepoMock = new Mock<Repository<Contact>>();
        private readonly DateTime dateTime = new DateTime(2013, 07, 11);
        private readonly Mock<DateRepository> dateRepoMock = new Mock<DateRepository>();
        private readonly TimeSlot expectedTimeSlot = new TimeSlot();
        private readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private readonly Mock<TimeSlotRepository> timeSlotRepoMock = new Mock<TimeSlotRepository>();
        private readonly Mock<HolidayRepository> holidayRepoMock = new Mock<HolidayRepository>();
        private readonly Mock<Email> emailMock = new Mock<Email>();
        private readonly Mock<Constants> constantsMock = new Mock<Constants>();
        private RendezVousController Controller { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Controller = new RendezVousController
                {
                    SlotRepository = timeSlotRepoMock.Object,
                    ContactRepository = contactSlotRepoMock.Object,
                    DateRepository = dateRepoMock.Object,
                    HolidayRepository = holidayRepoMock.Object,
                };
            SessionHelper.Instance = sessionHelperMock.Object;
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(contact);
            Email.Instance = emailMock.Object;
            Constants.Instance = constantsMock.Object;
            constantsMock.Setup(cm => cm.ServerAddress(It.IsAny<HttpRequestBase>())).Returns(ServerAddress);
        }
        


        //[TestMethod]
        //public void GoToLoginPageIfThereIsNoContactConnectedOnIndexAction()
        //{
        //    sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);

        //    Assert.AreEqual(LoginPath, Controller.Index().ViewName);
        //}

        //[TestMethod]
        //public void GoToIndexIfThereIsContactConnectedOnIndexAction()
        //{
        //    IList<Holiday> holidays = new List<Holiday>();
        //    holidayRepoMock.Setup(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>())).Returns(holidays);

        //    PartialViewResult view = Controller.Index();

        //    holidayRepoMock.Verify(hrm => hrm.GetFutureHoliday(It.IsAny<DateTime>()), Times.Once());
        //    Assert.AreEqual(IndexPath, view.ViewName);
        //    Assert.AreEqual(holidays, view.Model);
        //}

        //[TestMethod]
        //public void GoToLoginPageIfThereIsNoContactConnectedOnProposeDateAction()
        //{
        //    sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);

        //    Assert.AreEqual(LoginPath, Controller.ProposeDate(DateTime.Now, string.Empty).ViewName);
        //}

        //[TestMethod]
        //public void GoToLoginPageIfThereIsNoContactConnectedOnCreateDateAction()
        //{
        //    sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);

        //    Assert.AreEqual(LoginPath, Controller.CreateDate(DateTime.Now, 0).ViewName);
        //}

        //[TestMethod]
        //public void TimeSlotInPatientHours()
        //{
        //    TimeSlot expectedTimeSlot = InitTimeSlot(dateTime, 14, 15);
        //    IList<TimeSlot> timeSlots = new List<TimeSlot>
        //        {
        //            expectedTimeSlot
        //        };
        //    timeSlotRepoMock.Setup(tsr => tsr.GetFreeTimeSlots(dateTime)).Returns(timeSlots);

        //    PartialViewResult view = Controller.ProposeDate(dateTime, PatientHours);

        //    timeSlotRepoMock.Verify(tsr => tsr.GetFreeTimeSlots(dateTime), Times.Once());
        //    Assert.AreEqual(ProposeDatePath, view.ViewName);
        //    Assert.AreEqual(expectedTimeSlot, view.Model);
        //}

        //[TestMethod]
        //public void PatientHoursInTimeSlot()
        //{
        //    TimeSlot timeSlot1 = InitTimeSlot(dateTime, 13, 18);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot1, Times.Once());
        //    TimeSlot timeSlot2 = InitTimeSlot(dateTime, 12, 13);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot2, Times.Exactly(2));
        //    TimeSlot timeSlot3 = InitTimeSlot(dateTime, 12, 15);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot3, Times.Exactly(3));
        //}

        //private void ProposeDateHaveToReturnAnErrorMessage(TimeSlot timeSlot, Times times)
        //{
        //    IList<TimeSlot> timeSlots = new List<TimeSlot>
        //        {
        //            timeSlot
        //        };
        //    timeSlotRepoMock.Setup(tsr => tsr.GetFreeTimeSlots(dateTime)).Returns(timeSlots);

        //    PartialViewResult view = Controller.ProposeDate(dateTime, PatientHours);

        //    timeSlotRepoMock.Verify(tsr => tsr.GetFreeTimeSlots(dateTime), times);
        //    Assert.AreEqual(DateResource.NoFreeTimeSlot, view.ViewBag.ProposeError);
        //    Assert.AreEqual(ProposeDatePath, view.ViewName);
        //    Assert.AreEqual(null, view.Model);
        //}

        //[TestMethod]
        //public void ProposeMinFreeTimeSlotCorrespondingWithPatientHours()
        //{
        //    TimeSlot timeSlot1 = InitTimeSlot(dateTime, 15, 16);
        //    TimeSlot timeSlot2 = InitTimeSlot(dateTime, 16, 17);
        //    TimeSlot timeSlotMin = InitTimeSlot(dateTime, 14, 15);
        //    IList<TimeSlot> timeSlots = new List<TimeSlot>
        //        {
        //            timeSlot1,
        //            timeSlot2,
        //            timeSlotMin,
        //        };
        //    timeSlotRepoMock.Setup(tsr => tsr.GetFreeTimeSlots(dateTime)).Returns(timeSlots);

        //    PartialViewResult view = Controller.ProposeDate(dateTime, PatientHours);

        //    timeSlotRepoMock.Verify(tsr => tsr.GetFreeTimeSlots(dateTime), Times.Once());
        //    Assert.AreEqual(ProposeDatePath, view.ViewName);
        //    Assert.AreEqual(timeSlotMin, view.Model);
        //}

        //[TestMethod]
        //public void ReturnAnErrorMessageIfThereAreNoTimeSlotFreeCorrespondingWithPatientDateTime()
        //{
        //    timeSlotRepoMock.Setup(tsr => tsr.GetFreeTimeSlots(dateTime)).Returns(new List<TimeSlot>());

        //    PartialViewResult view = Controller.ProposeDate(dateTime, PatientHours);

        //    timeSlotRepoMock.Verify(tsr => tsr.GetFreeTimeSlots(dateTime), Times.Once());
        //    Assert.AreEqual(DateResource.NoFreeTimeSlot, view.ViewBag.ProposeError);
        //    Assert.AreEqual(ProposeDatePath, view.ViewName);
        //    Assert.IsNull(view.Model);
        //}

        //[TestMethod]
        //public void CreateDateAndSendEmails()
        //{
        //    timeSlotRepoMock.Setup(tsrm => tsrm.GetById(TimeSlotId)).Returns(expectedTimeSlot);
            
        //    PartialViewResult view =  Controller.CreateDate(dateTime, TimeSlotId);

        //    timeSlotRepoMock.Verify(tsrm => tsrm.GetById(TimeSlotId), Times.Once());
        //    dateRepoMock.Verify(drm => drm.Save(It.Is<Date>(d => d.Day == dateTime &&
        //                                                         d.Contact == contact &&
        //                                                         d.IsConfirmed == false &&
        //                                                         d.TimeSlot == expectedTimeSlot)));
        //    emailMock.Verify(em => em.SendForAdminPropose(It.IsAny<Date>()));
        //    emailMock.Verify(em => em.SendForPatientPropose(It.Is<Date>(d => d.Day == dateTime && 
        //                                                                     d.Contact == contact && 
        //                                                                     d.IsConfirmed == false && 
        //                                                                     d.TimeSlot == expectedTimeSlot),
        //                                                    It.IsAny<string>()));
        //    Assert.AreEqual(CreateDatePath, view.ViewName);
        //}

        //[TestMethod]
        //public void ReturnAnErrorMessageIfThereAreAnExceptionThrownDuringEmailSending()
        //{
        //    timeSlotRepoMock.Setup(tsrm => tsrm.GetById(TimeSlotId)).Returns(expectedTimeSlot);
        //    emailMock.Setup(em => em.SendForAdminPropose(It.IsAny<Date>())).Throws<Exception>();

        //    PartialViewResult view = Controller.CreateDate(dateTime, TimeSlotId);

        //    Assert.AreEqual(DateResource.ErrorOccured, view.ViewBag.ResultMessage);
        //    Assert.AreEqual(CreateDatePath, view.ViewName);
        //}

        //[TestMethod]
        //public void ValidateADate()
        //{
        //    const string id = "id";
        //    Date date = new Date();
        //    emailMock.Setup(em => em.SendForAdminPropose(It.IsAny<Date>())).Throws<Exception>();
        //    dateRepoMock.Setup(drm => drm.Validate(id)).Returns(date);

        //    PartialViewResult view = Controller.Validate(id);
            
        //    dateRepoMock.Verify(drm => drm.Validate(id), Times.Once());
        //    emailMock.Verify(em => em.SendForPatientValidation(date), Times.Once());
        //    emailMock.Verify(em => em.SendForAdminValidation(date), Times.Once());
        //    Assert.AreEqual(ValidatePath, view.ViewName);
        //}

        //private static TimeSlot InitTimeSlot(DateTime date, int beginHour, int endHour)
        //{
        //    return new TimeSlot
        //        {
        //            BeginHour = new TimeSpan(beginHour, 00, 00),
        //            DayOfWeek = date.DayOfWeek,
        //            EndHour = new TimeSpan(endHour, 00, 00)
        //        };
        //}
    }
}