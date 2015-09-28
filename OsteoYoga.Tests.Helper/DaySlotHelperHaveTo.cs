using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
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

        PratictionerOffice pratictionerOffice = new PratictionerOffice()
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
        public void Return_Days_Where_There_Are_Free_Hours()
        {
            //Arrange
            IList<Event> events = new List<Event>();
            googleRepositoryMock.Setup(g => g.GetAllForPractionerInterval(pratictionerOffice)).Returns(events);

            DateTime unexpectedDateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1); // Verify Never
            DateTime expectedDateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(2); //Verify Once
            DateTime unexpectedDateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(3); //Verify Once
            DateTime expectedDateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(4); //Verify Once
            DateTime unexpectedDateTime3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(5); //Verify Once
            DateTime expectedDateTime3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(6); //Verify Once
            DateTime unexpectedDateTime4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(7); //Verify Never

            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime1, duration,events)).Returns(new List<FreeSlot>() { new FreeSlot() });
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime2, duration, events)).Returns(new List<FreeSlot>() { new FreeSlot() });
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(expectedDateTime3, duration, events)).Returns(new List<FreeSlot>() { new FreeSlot() });
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(unexpectedDateTime2, duration, events)).Returns(new List<FreeSlot>() );
            hourSlotHelperMock.Setup(h => h.CalculateFreeHours(unexpectedDateTime3, duration, events)).Returns(new List<FreeSlot>() );

            //Act
            IList<DateTime> freeDays = DaySlotHelper.CalculateFreeDays(pratictionerOffice, duration);

            //Assert
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime1.Day)), duration, events), Times.Never);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (expectedDateTime1.Day)), duration, events), Times.Once);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime2.Day)), duration, events), Times.Once);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (expectedDateTime2.Day)), duration, events), Times.Once);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime3.Day)), duration, events), Times.Once);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (expectedDateTime3.Day)), duration, events), Times.Once);
            hourSlotHelperMock.Verify(h => h.CalculateFreeHours(It.Is<DateTime>(d => d.Day == (unexpectedDateTime4.Day)), duration, events), Times.Never);

            Assert.AreEqual(3, freeDays.Count);
            CollectionAssert.Contains(freeDays.ToList(), expectedDateTime1);
            CollectionAssert.Contains(freeDays.ToList(), expectedDateTime2);
            CollectionAssert.Contains(freeDays.ToList(), expectedDateTime3);
        }
    }
}
