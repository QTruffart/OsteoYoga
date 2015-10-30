using System;
using System.Collections;
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
        
        [TestMethod]
        public void Return_Days_Where_There_Are_Free_Hours()
        {
            //Arrange
            IList<Event> events = new List<Event>();

            DateTime expecteddate = new DateTime(2015,10,09);
            DateTime unexpecteddate1 = new DateTime(2015,10,12);
            DateTime unexpecteddate2 = new DateTime(2015,10,14);

            IList<DateTime> dateToInspect = new List<DateTime>() { expecteddate , unexpecteddate1 , unexpecteddate2 };

            googleRepositoryMock.Setup(g => g.GetAllForInterval(pratictionerOffice.MinDateInterval, pratictionerOffice.MaxDateInterval)).Returns(events);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, unexpecteddate1)).Returns(true);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, unexpecteddate2)).Returns(false);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, expecteddate)).Returns(false);

            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(unexpecteddate2, duration, events)).Returns(new List<FreeSlot>() {});
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expecteddate, duration, events)).Returns(new List<FreeSlot>() {new FreeSlot()});

            //Act
            IList<DateTime> freeDays = DaySlotHelper.CalculateFreeDays(pratictionerOffice, duration, dateToInspect);

            //Assert
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(unexpecteddate1, duration, It.IsAny<IList<Event>>()), Times.Never);
            Assert.AreEqual(1, freeDays.Count);
            CollectionAssert.Contains(freeDays.ToList(), expecteddate);
        }
        
        [TestMethod]
        public void Return_FreeSlots_By_Duration()
        {
            //Arrange
            PratictionerOffice pratictionerOffice = new PratictionerOffice();
            DateTime date = new DateTime(2015,12,13);
            IList<Event> events = new List<Event>();
            IList<FreeSlot> freeSlots = new List<FreeSlot>()
            {
                new FreeSlot() { Begin = new DateTime(2015,12,13,09,0,0), End = new DateTime(2015,12,13,10,30,0)}, // 2 FreeSlot
                new FreeSlot() { Begin = new DateTime(2015,12,13,12,0,0), End = new DateTime(2015,12,13,12,45,0)}, // 1 FreeSlot
                new FreeSlot() { Begin = new DateTime(2015,12,13,14,0,0), End = new DateTime(2015,12,13,15,29,0)}, // 1 FreeSlot
            };

            googleRepositoryMock.Setup(
                grm => grm.GetAllForInterval(It.Is<DateTime>(d => d.ToString("dd/MM/yyyy HH:mm") == "13/12/2015 00:00"),
                        It.Is<DateTime>(d => d.ToString("dd/MM/yyyy HH:mm") == "13/12/2015 23:59"))).Returns(events);
            hourSlotHelperMock.Setup(hsh => hsh.CalculateFreeHours(date, duration, events)).Returns(freeSlots);


            //Act
            IList<FreeSlot> slots = DaySlotHelper.GetAllFreeSlotOnADay(pratictionerOffice, duration, date);

            //Assert
            Assert.AreEqual(4, slots.Count);

            Assert.AreEqual(2015,slots[0].Begin.Year);
            Assert.AreEqual(12,slots[0].Begin.Month);
            Assert.AreEqual(13,slots[0].Begin.Day);
            Assert.AreEqual(9, slots[0].Begin.Hour);
            Assert.AreEqual(0, slots[0].Begin.Minute);

            Assert.AreEqual(2015, slots[0].End.Year);
            Assert.AreEqual(12, slots[0].End.Month);
            Assert.AreEqual(13, slots[0].End.Day);
            Assert.AreEqual(9, slots[0].End.Hour);
            Assert.AreEqual(45, slots[0].End.Minute);

            Assert.AreEqual(2015, slots[1].Begin.Year);
            Assert.AreEqual(12, slots[1].Begin.Month);
            Assert.AreEqual(13, slots[1].Begin.Day);
            Assert.AreEqual(9, slots[1].Begin.Hour);
            Assert.AreEqual(45, slots[1].Begin.Minute);

            Assert.AreEqual(2015, slots[1].End.Year);
            Assert.AreEqual(12, slots[1].End.Month);
            Assert.AreEqual(13, slots[1].End.Day);
            Assert.AreEqual(10, slots[1].End.Hour);
            Assert.AreEqual(30, slots[1].End.Minute);

            Assert.AreEqual(2015, slots[2].Begin.Year);
            Assert.AreEqual(12, slots[2].Begin.Month);
            Assert.AreEqual(13, slots[2].Begin.Day);
            Assert.AreEqual(12, slots[2].Begin.Hour);
            Assert.AreEqual(0, slots[2].Begin.Minute);

            Assert.AreEqual(2015, slots[2].End.Year);
            Assert.AreEqual(12, slots[2].End.Month);
            Assert.AreEqual(13, slots[2].End.Day);
            Assert.AreEqual(12, slots[2].End.Hour);
            Assert.AreEqual(45, slots[2].End.Minute);
            
            Assert.AreEqual(2015, slots[3].Begin.Year);
            Assert.AreEqual(12, slots[3].Begin.Month);
            Assert.AreEqual(13, slots[3].Begin.Day);
            Assert.AreEqual(14, slots[3].Begin.Hour);
            Assert.AreEqual(0, slots[3].Begin.Minute);

            Assert.AreEqual(2015, slots[3].End.Year);
            Assert.AreEqual(12, slots[3].End.Month);
            Assert.AreEqual(13, slots[3].End.Day);
            Assert.AreEqual(14, slots[3].End.Hour);
            Assert.AreEqual(45, slots[3].End.Minute);


        }
    }
}
