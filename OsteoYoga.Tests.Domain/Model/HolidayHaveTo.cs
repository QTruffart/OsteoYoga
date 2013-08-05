using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource.Contact;
using OsteoYoga.Resource.Holiday;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class HolidayHaveTo
    {
        
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            DateTime day = new DateTime();
            TimeSpan begin = new TimeSpan();
            TimeSpan end = new TimeSpan();

            Holiday holiday = new Holiday {Day = day, BeginHour = begin, EndHour = end};

            Assert.AreEqual(day, holiday.Day);
            Assert.AreEqual(begin, holiday.BeginHour);
            Assert.AreEqual(end, holiday.EndHour);
        }

        [TestMethod]
        public void RenderToString()
        {
            DateTime day = new DateTime(2013, 07, 18); 
            TimeSpan begin = new TimeSpan(8, 0, 0);
            TimeSpan end = new TimeSpan(16, 0, 0);
            Holiday holiday = new Holiday
            {
                Day = day,
                BeginHour = begin,
                EndHour = end
            };
            Assert.AreEqual(day.ToString("dd/MM/yyyy") +
                            LoginResource.From + new DateTime(begin.Ticks).ToString("HH:mm") +
                            HolidayResource.To + new DateTime(end.Ticks).ToString("HH:mm"), holiday.ToString());
        }
    }
}
