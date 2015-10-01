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

            googleRepositoryMock.Setup(g => g.GetAllForPractionerInterval(pratictionerOffice)).Returns(events);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, unexpecteddate1)).Returns(true);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, unexpecteddate2)).Returns(false);
            hourSlotHelperMock.Setup(h => h.IsDuringAnAllDayEvent(events, expecteddate)).Returns(false);

            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(unexpecteddate2, duration, events)).Returns(new List<FreeSlot>() {});
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expecteddate, duration, events)).Returns(new List<FreeSlot>() {new FreeSlot()});

            //Act
            IList<DateTime> freeDays = DaySlotHelper.CalculateFreeDays(pratictionerOffice, duration, dateToInspect);

            //Assert
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(unexpecteddate1, duration, events), Times.Never);
            Assert.AreEqual(1, freeDays.Count);
            CollectionAssert.Contains(freeDays.ToList(), expecteddate);
        }
    }
}
