using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class HolidayRepositoryHaveTo : BaseTestsNHibernate
    {
        readonly HolidayRepository holidayRepository = new HolidayRepository();
       
        [TestInitialize]
        public override void Initialize(){}

        [TestCleanup]
        public override void CleanUp()
        {
            holidayRepository.DeleteAll();
        }

        [TestMethod]
        public void ReturnsFutureHolidays()
        {
            DateTime dateTime = new DateTime(2013,07,12);
            Holiday holiday1 = new Holiday {Day = dateTime.AddDays(-1)};
            Holiday holiday2 = new Holiday {Day = dateTime};
            Holiday holiday3 = new Holiday {Day = dateTime.AddDays(1)};
            holidayRepository.Save(holiday1);
            holidayRepository.Save(holiday2);
            holidayRepository.Save(holiday3);
            
            IList<Holiday> slots = holidayRepository.GetFutureHoliday(dateTime);

            Assert.AreEqual(2, slots.Count);
            CollectionAssert.Contains(slots.ToList(), holiday2);
            CollectionAssert.Contains(slots.ToList(), holiday3);
        }

        [TestMethod]
        public void ReturnsTrueIfThereIsAnHolidayOnHours()
        {
            TimeSlot slot1 = new TimeSlot { BeginHour = new TimeSpan(11,0,0), EndHour = new TimeSpan(13,0,0) };
            TimeSlot slot2 = new TimeSlot { BeginHour = new TimeSpan(13,0,0), EndHour = new TimeSpan(15,0,0) };
            TimeSlot slot3 = new TimeSlot { BeginHour = new TimeSpan(12,30,0), EndHour = new TimeSpan(13,30,0) };
            DateTime dateTime = new DateTime(2013, 07, 12);
            Holiday holiday = new Holiday { Day = dateTime, BeginHour = new TimeSpan(12,0,0), EndHour = new TimeSpan(14,0,0)};
            holidayRepository.Save(holiday);

            Assert.IsTrue(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot1.BeginHour, slot1.EndHour));
            Assert.IsTrue(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot2.BeginHour, slot2.EndHour));
            Assert.IsTrue(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot3.BeginHour, slot3.EndHour));
        }

        [TestMethod]
        public void ReturnsFalseIfThereIsNotAnHolidayOnHours()
        {
            TimeSlot slot1 = new TimeSlot { BeginHour = new TimeSpan(11, 0, 0), EndHour = new TimeSpan(12, 0, 0) };
            TimeSlot slot2 = new TimeSlot { BeginHour = new TimeSpan(14, 0, 0), EndHour = new TimeSpan(15, 0, 0) };
            TimeSlot slot3 = new TimeSlot { BeginHour = new TimeSpan(10, 30, 0), EndHour = new TimeSpan(11, 30, 0) };
            DateTime dateTime = new DateTime(2013, 07, 12);
            Holiday holiday = new Holiday { Day = dateTime, BeginHour = new TimeSpan(12, 0, 0), EndHour = new TimeSpan(14, 0, 0) };
            holidayRepository.Save(holiday);

            Assert.IsFalse(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot1.BeginHour, slot1.EndHour));
            Assert.IsFalse(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot2.BeginHour, slot2.EndHour));
            Assert.IsFalse(holidayRepository.ThereIsAnHolidayAtTheseHours(dateTime, slot3.BeginHour, slot3.EndHour));
        }
    }
}
