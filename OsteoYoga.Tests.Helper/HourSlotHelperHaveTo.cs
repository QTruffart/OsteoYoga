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
    public class HourSlotHelperHaveTo
    {
        //readonly Mock<IGoogleRepository<Event>>  googleRepositoryMock = new Mock<IGoogleRepository<Event>>();

        IHourSlotHelper HourSlotHelper { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            HourSlotHelper = new HourSlotHelper()
            {
                //GoogleRepository = googleRepositoryMock.Object
            };
        }

        //[TestMethod]
        //public void Correctly_Initialize()
        //{
        //    HourSlotHelper hourSlotHelper = new HourSlotHelper();

        //}


        [TestMethod]
        public void Return_Free_Time_Slots_On_A_Day()
        {
            // 08:00-Empty1-09:30 -- Doit retourner 1 TimeSpan de 08:00 à 09:30
            // 09:30-Event1-10:15 
            // 10:15-Empty2-11:14 -- Ne doit rien retourner
            // 11:14-Event2-11:44 
            // 11:44-Empty3-12:44 -- Doit retourner 1 TimeSpan de 11:44 à 14:00
            // 12:44-Event3-14:00 
            // 14:00-Empty4-16:15 -- Doit retourner 1 TimeSpan de 14:00 à 16:15
            // 16:15-Event4-16:45
            // 16:45-Event5-18:00
            // 18:00-Empty5-20:00 -- Doit retourner 1 TimeSpan de 18:00 à 20:00

            DateTime now0800 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8,0,0);
            DateTime now0930 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9,30,0);
            DateTime now1015 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10,15,0);
            DateTime now1114 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11,14,0);
            DateTime now1144 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11,44,0);
            DateTime now1244 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12,44,0);
            DateTime now1400 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14,0,0);
            DateTime now1615 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16,15,0);
            DateTime now1645 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16,45,0);
            DateTime now1800 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18,0,0);
            DateTime now2000 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20,0,0);

            Event event1 = new Event {Start = new EventDateTime() {DateTime = now0930 }, End = new EventDateTime {DateTime = now1015 } };
            Event event2 = new Event {Start = new EventDateTime() {DateTime = now1114 }, End = new EventDateTime {DateTime = now1144 } };
            Event event3 = new Event {Start = new EventDateTime() {DateTime = now1244 }, End = new EventDateTime {DateTime = now1400 } };
            Event event4 = new Event {Start = new EventDateTime() {DateTime = now1615 }, End = new EventDateTime {DateTime = now1645 } };
            Event event5 = new Event {Start = new EventDateTime() {DateTime = now1645 }, End = new EventDateTime {DateTime = now1800 } };

            IList<Event> eventsParmeter = new List<Event>() { event1, event2, event3, event4, event5 };
            
            DateTime dayToInspect = DateTime.Now;
            Duration duration = new Duration() { Value = 60 };
            
            IList<FreeSlot> results = HourSlotHelper.CalculateFreeHours(dayToInspect, duration, eventsParmeter);

            Assert.AreEqual(4, results.Count);

            Assert.AreEqual(now0800.Ticks, results[0].Begin.Ticks);
            Assert.AreEqual(now0930.Ticks, results[0].End.Ticks);

            Assert.AreEqual(now1144.Ticks, results[1].Begin.Ticks);
            Assert.AreEqual(now1244.Ticks, results[1].End.Ticks);

            Assert.AreEqual(now1400.Ticks, results[2].Begin.Ticks);
            Assert.AreEqual(now1615.Ticks, results[2].End.Ticks);
            
            Assert.AreEqual(now1800.Ticks, results[3].Begin.Ticks);
            Assert.AreEqual(now2000.Ticks, results[3].End.Ticks);

        }


        [TestMethod]
        public void Return_Free_Time_Slots_On_A_Day_If_There_Are_No_Event()
        {
           
            DateTime now0800 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            DateTime now2000 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);


            IList<Event> eventsParmeter = new List<Event>() {  };

            DateTime dayToInspect = DateTime.Now;
            Duration duration = new Duration() { Value = 60 };

            IList<FreeSlot> results = HourSlotHelper.CalculateFreeHours(dayToInspect, duration, eventsParmeter);

            Assert.AreEqual(1, results.Count);

            Assert.AreEqual(now0800.Ticks, results[0].Begin.Ticks);
            Assert.AreEqual(now2000.Ticks, results[0].End.Ticks);

        }
    }
}
