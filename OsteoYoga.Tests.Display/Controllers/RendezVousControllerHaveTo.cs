using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Repository.DAO.Interfaces;
using OsteoYoga.Site.Controllers;
using OsteoYoga.Site.ViewResults;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class RendezVousControllerHaveTo
    {
        private const string ServerAddress = "http://osteoyoga.fr";
        private readonly Patient patient = new Patient();
        private readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private readonly Mock<OfficeRepository> officeRepositoryMock = new Mock<OfficeRepository>();
        private readonly Mock<DurationRepository> durationRepositoryMock = new Mock<DurationRepository>();
        private readonly Mock<IGoogleRepository<Event>> googleRepositoryMock = new Mock<IGoogleRepository<Event>>();
        private readonly Mock<IFreeSlotHelper> freeSlotHelperMock = new Mock<IFreeSlotHelper>();

        private readonly PratictionerOffice office = new PratictionerOffice();
        
        private readonly IList<Duration> durations = new List<Duration>();
        private readonly IList<Office> offices = new List<Office>();
        private readonly IList<Event> events = new List<Event>();
        private readonly IList<FreeSlot> freeSlots = new List<FreeSlot>();

        private readonly Mock<Email> emailMock = new Mock<Email>();
        private readonly Mock<Constants> constantsMock = new Mock<Constants>();
        private RendezVousController Controller { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Controller = new RendezVousController
            {
                    OfficeRepository = officeRepositoryMock.Object,
                    GoogleRepository = googleRepositoryMock.Object,
                    DurationRepository = durationRepositoryMock.Object,
                    FreeSlotHelper = freeSlotHelperMock.Object
            };
            SessionHelper.Instance = sessionHelperMock.Object;
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(patient);
            Email.Instance = emailMock.Object;
            Constants.Instance = constantsMock.Object;
            constantsMock.Setup(cm => cm.ServerAddress(It.IsAny<HttpRequestBase>())).Returns(ServerAddress);
        }


        [TestMethod]
        public void CorrectlyInitialize()
        {
            RendezVousController controller = new RendezVousController();

            Assert.IsInstanceOfType(controller.DurationRepository, typeof(DurationRepository));
            Assert.IsInstanceOfType(controller.GoogleRepository, typeof(GoogleRepository));
            Assert.IsInstanceOfType(controller.OfficeRepository, typeof(OfficeRepository));
            Assert.IsInstanceOfType(controller.FreeSlotHelper, typeof(FreeSlotHelper));

        }

        [TestMethod]
        public void GoToIndexPageWithParameter()
        {
            //arrange
            durationRepositoryMock.Setup(drm => drm.GetAll()).Returns(durations);
            officeRepositoryMock.Setup(orm => orm.GetAll()).Returns(offices);
            googleRepositoryMock.Setup(grm => grm.GetAllForPractionerInterval(It.IsAny<PratictionerOffice>())).Returns(events);
            freeSlotHelperMock.Setup(fsm => fsm.CalculateFreeSlotBetweenTwoDays(events, It.IsAny<PratictionerOffice>())).Returns(freeSlots);
            
            //act
            PartialViewResult result = Controller.Index();

            //assert
            Assert.AreEqual(offices, ((DateViewResult)result.Model).Offices );
            Assert.AreEqual("Index", result.ViewName);
        }



        [TestMethod]
        public void Propose_Durations_With_Office()
        {


            //Office office = new Office();
            //officeRepositoryMock.Setup(o => o.GetAll()).Returns(offices);
            //Dates date = new Dates
            //{
            //    Patient = contact
            //};

            //PartialViewResult viewResult = Controller.FillDate(office);

            //Assert.AreEqual(date, viewResult.Model);
            //Assert.AreEqual(IndexPath, viewResult.ViewName);
        }



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
        //    sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Patient);

        //    Assert.AreEqual(LoginPath, Controller.ProposeDate(DateTime.Now, string.Empty).ViewName);
        //}

        //[TestMethod]
        //public void GoToLoginPageIfThereIsNoContactConnectedOnCreateDateAction()
        //{
        //    sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Patient);

        //    Assert.AreEqual(LoginPath, Controller.CreateDate(DateTime.Now, 0).ViewName);
        //}

        //[TestMethod]
        //public void TimeSlotInPatientHours()
        //{
        //    WorkTimeSlot expectedTimeSlot = InitTimeSlot(dateTime, 14, 15);
        //    IList<WorkTimeSlot> timeSlots = new List<WorkTimeSlot>
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
        //    WorkTimeSlot timeSlot1 = InitTimeSlot(dateTime, 13, 18);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot1, Times.Once());
        //    WorkTimeSlot timeSlot2 = InitTimeSlot(dateTime, 12, 13);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot2, Times.Exactly(2));
        //    WorkTimeSlot timeSlot3 = InitTimeSlot(dateTime, 12, 15);
        //    ProposeDateHaveToReturnAnErrorMessage(timeSlot3, Times.Exactly(3));
        //}

        //private void ProposeDateHaveToReturnAnErrorMessage(WorkTimeSlot timeSlot, Times times)
        //{
        //    IList<WorkTimeSlot> timeSlots = new List<WorkTimeSlot>
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
        //    WorkTimeSlot timeSlot1 = InitTimeSlot(dateTime, 15, 16);
        //    WorkTimeSlot timeSlot2 = InitTimeSlot(dateTime, 16, 17);
        //    WorkTimeSlot timeSlotMin = InitTimeSlot(dateTime, 14, 15);
        //    IList<WorkTimeSlot> timeSlots = new List<WorkTimeSlot>
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
        //    timeSlotRepoMock.Setup(tsr => tsr.GetFreeTimeSlots(dateTime)).Returns(new List<WorkTimeSlot>());

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
        //    dateRepoMock.Verify(drm => drm.Save(It.Is<Dates>(d => d.Day == dateTime &&
        //                                                         d.Patient == contact &&
        //                                                         d.IsConfirmed == false &&
        //                                                         d.WorkTimeSlot == expectedTimeSlot)));
        //    emailMock.Verify(em => em.SendForAdminPropose(It.IsAny<Dates>()));
        //    emailMock.Verify(em => em.SendForPatientPropose(It.Is<Dates>(d => d.Day == dateTime && 
        //                                                                     d.Patient == contact && 
        //                                                                     d.IsConfirmed == false && 
        //                                                                     d.WorkTimeSlot == expectedTimeSlot),
        //                                                    It.IsAny<string>()));
        //    Assert.AreEqual(CreateDatePath, view.ViewName);
        //}

        //[TestMethod]
        //public void ReturnAnErrorMessageIfThereAreAnExceptionThrownDuringEmailSending()
        //{
        //    timeSlotRepoMock.Setup(tsrm => tsrm.GetById(TimeSlotId)).Returns(expectedTimeSlot);
        //    emailMock.Setup(em => em.SendForAdminPropose(It.IsAny<Dates>())).Throws<Exception>();

        //    PartialViewResult view = Controller.CreateDate(dateTime, TimeSlotId);

        //    Assert.AreEqual(DateResource.ErrorOccured, view.ViewBag.ResultMessage);
        //    Assert.AreEqual(CreateDatePath, view.ViewName);
        //}

        //[TestMethod]
        //public void ValidateADate()
        //{
        //    const string id = "id";
        //    Dates date = new Dates();
        //    emailMock.Setup(em => em.SendForAdminPropose(It.IsAny<Dates>())).Throws<Exception>();
        //    dateRepoMock.Setup(drm => drm.Validate(id)).Returns(date);

        //    PartialViewResult view = Controller.Validate(id);

        //    dateRepoMock.Verify(drm => drm.Validate(id), Times.Once());
        //    emailMock.Verify(em => em.SendForPatientValidation(date), Times.Once());
        //    emailMock.Verify(em => em.SendForAdminValidation(date), Times.Once());
        //    Assert.AreEqual(ValidatePath, view.ViewName);
        //}

        //private static WorkTimeSlot InitTimeSlot(DateTime date, int beginHour, int endHour)
        //{
        //    return new WorkTimeSlot
        //        {
        //            BeginHour = new TimeSpan(beginHour, 00, 00),
        //            DayOfWeek = date.DayOfWeek,
        //            EndHour = new TimeSpan(endHour, 00, 00)
        //        };
        //}
    }
}