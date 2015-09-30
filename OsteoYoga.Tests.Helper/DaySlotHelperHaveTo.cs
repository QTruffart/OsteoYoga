using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.Helper
{
    [TestClass]
    public class DaySlotHelperHaveTo
    {
        IDaySlotHelper DaySlotHelper { get; set; }

        readonly Mock<IHourSlotHelper> hourSlotHelperMock = new Mock<IHourSlotHelper>();
        readonly Mock<IGoogleRepository<Event>> googleRepositoryMock = new Mock<IGoogleRepository<Event>>();

        readonly PratictionerOffice pratictionerOffice = new PratictionerOffice()
        {
            MaxInterval = 6,
            MinInterval = 2,
        };
        Duration duration = new Duration() { Value = 45 };


        [TestInitialize]
        public void Initialize()
        {
            DaySlotHelper = new DaySlotHelper()
            {
                HourSlotHelper = hourSlotHelperMock.Object,
                GoogleRepository = googleRepositoryMock.Object
            };
        }

        [TestMethod]
        public void Correctly_Initialize()
        {
            DaySlotHelper daySlotHelper = new DaySlotHelper();

            Assert.IsInstanceOfType(daySlotHelper.HourSlotHelper, typeof(HourSlotHelper));
            Assert.IsInstanceOfType(daySlotHelper.GoogleRepository, typeof(GoogleRepository));
        }



        [TestMethod]
        public void Get_Work_Days_Between_Two_Days()
        {
            //arrange
            PratictionerOffice pratictionerOffice = new PratictionerOffice()
            {
                MinInterval = 1,
                MaxInterval = 4,
                DefaultWorkDaysPO = new List<DefaultWorkDaysPO>()
                {
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Monday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Tuesday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Thursday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Friday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Sunday" } }
                }
            };

            //act
            IList<DateTime> workDays = DaySlotHelper.GetAllWorkDaysOnPeriod(pratictionerOffice, new DateTime(2015, 09, 28));


            //assert
            Assert.AreEqual(3, workDays.Count);
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "29/09/2015");
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "01/10/2015");
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "02/10/2015");
        }





        //[TestMethod]
        //public void Return_Days_Where_There_Are_Free_Hours()
        //{
        //    //Arrange
        //    DateTime eventDate1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(5);
        //    DateTime eventDate2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(6);
        //    DateTime eventDate3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4);


        //    Event event1 = new Event() { Start = new EventDateTime() { Date = eventDate1.ToString(Constants.GetInstance().GoogleDateFormat) }};
        //    Event event2 = new Event() {Start = new EventDateTime() {DateTime = eventDate2}};
        //    Event event3 = new Event() {Start = new EventDateTime() {DateTime = eventDate3}};
        //    Event event4 = new Event() {Start = new EventDateTime() {DateTime = eventDate3}};
        //    IList<Event> events = new List<Event>() { event1, event2, event3, event4 };

        //    googleRepositoryMock.Setup(g => g.GetAllForPractionerInterval(pratictionerOffice)).Returns(events);

        //    DateTime unexpectedDateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1); // Verify Never -> Au delà des limites
        //    DateTime expectedDateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(2); //Verify Once -> Cas ok minimum
        //    DateTime unexpectedDateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(3); //Verify Once -> Cas ko sans freeSlot retourné
        //    DateTime expectedDateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4); //Verify Once Cas ok
        //    DateTime unexpectedDateTime3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(5); //Verify Never -> Cas avec un event toute la journée
        //    DateTime expectedDateTime3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(6); //Verify Once -> Cas ok avec event et un rdv dessus
        //    DateTime unexpectedDateTime4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(7); //Verify Never -> Cas au delà des limites

        //    hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime1, duration, It.IsAny<IList<Event>>())).Returns(new List<FreeSlot>() { new FreeSlot() });
        //    hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime2, duration, It.IsAny<IList<Event>>())).Returns(new List<FreeSlot>() { new FreeSlot() });
        //    hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime3, duration, It.IsAny<IList<Event>>())).Returns(new List<FreeSlot>() { new FreeSlot() });
        //    hourSlotHelperMock.Setup(h => h.CalculateFreeHours(unexpectedDateTime2, duration, It.IsAny<IList<Event>>())).Returns(new List<FreeSlot>() );

        //    //Act
        //    IList<DateTime> freeDays = DaySlotHelper.CalculateFreeDays(pratictionerOffice, duration);

        //    //Assert
        //    //Cas ko Au delà des limites
        //    hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime1.Day)), duration, It.IsAny<IList<Event>>()), Times.Never);

        //    //Cas ok minimum avec liste d'event vide
        //    hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (expectedDateTime1.Day)), duration, It.Is<IList<Event>>(e => e.Count == 0)), Times.Once);

        //    //Cas ko sans freeSlot retourné
        //    hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime2.Day)), duration, It.IsAny<IList<Event>>()), Times.Once);

        //    //Cas ok avec deux events envoyés (event3 et event4)
        //    hourSlotHelperMock.Verify(
        //        h => h.CalculateFreeHours(
        //            It.Is<DateTime>(d => d.Day == (expectedDateTime2.Day)), 
        //            duration, 
        //            It.Is<IList<Event>>(e => e.Count == 2 
        //                                  && e.Contains(event3) 
        //                                  && e.Contains(event4))), Times.Once);

        //    //Cas ko avec event toute la journée
        //    hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime3.Day)), duration, It.IsAny<IList<Event>>()), Times.Never);

        //    //Cas ok avec un event envoyé (event2)
        //    hourSlotHelperMock.Verify(
        //       h => h.CalculateFreeHours(
        //           It.Is<DateTime>(d => d.Day == (expectedDateTime3.Day)),
        //           duration,
        //           It.Is<IList<Event>>(e => e.Count == 1
        //                                 && e.Contains(event2))), Times.Once);

        //    //Cas ko Au delà des limites
        //    hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime4.Day)), duration, It.IsAny<IList<Event>>()), Times.Never);

        //    Assert.AreEqual(3, freeDays.Count);
        //    CollectionAssert.Contains(freeDays.ToList(), expectedDateTime1);
        //    CollectionAssert.Contains(freeDays.ToList(), expectedDateTime2);
        //    CollectionAssert.Contains(freeDays.ToList(), expectedDateTime3);
        //}
    }
}
