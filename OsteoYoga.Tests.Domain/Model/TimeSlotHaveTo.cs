using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class TimeSlotHaveTo
    {
        
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {

            TimeSpan beginHour = new TimeSpan(0, 9, 30, 0);
            TimeSpan endHour = new TimeSpan(0, 10, 30, 0);
            TimeSlot timeSlot = new TimeSlot()
                                    {
                                        BeginHour = beginHour,
                                        EndHour = endHour,
                                        DayOfWeek = DayOfWeek.Monday,
                                    };
            Assert.AreEqual(beginHour, timeSlot.BeginHour);
            Assert.AreEqual(endHour, timeSlot.EndHour);
            Assert.AreEqual(DayOfWeek.Monday, timeSlot.DayOfWeek);
        }
    }
}
