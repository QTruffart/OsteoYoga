using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class TimeSlotRepositoryHaveTo : BaseTestsNHibernate
    {
        private const string ConfirmationId = "id";
        DateTime dateTime = new DateTime(2013, 07, 12);
        Mock<HolidayRepository> holidayRepository = new Mock<HolidayRepository>();
        readonly TimeSlotRepository timeSlotRepository = new TimeSlotRepository();
        readonly Repository<Contact> contactRepository = new Repository<Contact>();
        readonly Repository<Date> dateRepository = new Repository<Date>();
        readonly Contact contact = new Contact {FullName = "test", Mail = "test@test.com", Phone = "+33(0)556578996"};

        [TestInitialize]
        public override void Initialize()
        {
            timeSlotRepository.HolidayRepository = holidayRepository.Object;
            contactRepository.Save(contact);
        }

        [TestCleanup]
        public override void CleanUp()
        {
            dateRepository.DeleteAll();
            timeSlotRepository.DeleteAll();
            contactRepository.DeleteAll();
        }

        [TestMethod]
        public void ReturnsFreeTimeSlot()
        {
            TimeSlot timeSlot1 = CreateTimeSlot(dateTime.DayOfWeek, 11);
            TimeSlot timeSlot2 = CreateTimeSlot(dateTime.DayOfWeek, 12);
            TimeSlot timeSlot3 = CreateTimeSlot(dateTime.DayOfWeek, 13);
            timeSlotRepository.Save(timeSlot1);
            timeSlotRepository.Save(timeSlot2);
            timeSlotRepository.Save(timeSlot3);
            dateRepository.Save(new Date { ConfirmationId = ConfirmationId, Contact = contact, IsConfirmed = false, Day = dateTime, TimeSlot = timeSlot2});
            holidayRepository.Setup(hr => hr.ThereIsAnHolidayAtTheseHours(It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                             .Returns(false);
            
            IList<TimeSlot> slots = timeSlotRepository.GetFreeTimeSlots(dateTime);

            Assert.AreEqual(2, slots.Count);
            CollectionAssert.Contains(slots.ToList(), timeSlot1);
            CollectionAssert.Contains(slots.ToList(), timeSlot3);
        }

        [TestMethod]
        public void ReturnsOnlyTimeSlotWhichAreNotInHolidaysHour()
        {
            TimeSlot timeSlot1 = CreateTimeSlot(dateTime.DayOfWeek, 11);
            TimeSlot timeSlot2 = CreateTimeSlot(dateTime.DayOfWeek, 12);
            timeSlotRepository.Save(timeSlot1);
            timeSlotRepository.Save(timeSlot2);

            holidayRepository.Setup(hr => hr.ThereIsAnHolidayAtTheseHours(dateTime, timeSlot1.BeginHour, timeSlot1.EndHour))
                             .Returns(true);
            holidayRepository.Setup(hr => hr.ThereIsAnHolidayAtTheseHours(dateTime, timeSlot2.BeginHour, timeSlot2.EndHour))
                             .Returns(false);

            IList<TimeSlot> slots = timeSlotRepository.GetFreeTimeSlots(dateTime);

            Assert.AreEqual(1, slots.Count);
            CollectionAssert.Contains(slots.ToList(), timeSlot2);
        }



        [TestMethod]
        public void ReturnsEmptyListIfThereAreNoFreeTimeSlot()
        {
            TimeSlot timeSlot = CreateTimeSlot(dateTime.DayOfWeek, 11);
            timeSlotRepository.Save(timeSlot);
            dateRepository.Save(new Date { ConfirmationId = ConfirmationId, Contact = contact, IsConfirmed = false, Day = dateTime, TimeSlot = timeSlot });

            IList<TimeSlot> slots = timeSlotRepository.GetFreeTimeSlots(dateTime);

            holidayRepository.Verify(hr => hr.ThereIsAnHolidayAtTheseHours(It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()), Times.Never());
            Assert.AreEqual(0, slots.Count);
        }
        //TODO : obsolète

        //[TestMethod]
        //public void ReturnsEmptyListIfThereIsAnHoliday()
        //{
        //    DateTime dateTime = new DateTime(2013, 07, 12);
        //    Holiday holiday = new Holiday {Day = dateTime};
        //    TimeSlot timeSlot1 = CreateTimeSlot(dateTime.DayOfWeek, 11);
        //    timeSlotRepository.Save(timeSlot1);
        //    holidayRepository.Save(holiday);

        //    IList<TimeSlot> slots = timeSlotRepository.GetFreeTimeSlots(dateTime);

        //    Assert.AreEqual(0, slots.Count);
        //}

        private TimeSlot CreateTimeSlot(DayOfWeek dayOfWeek, int hour = 9)
        {
            return new TimeSlot()
            {
                BeginHour = new TimeSpan(0, hour, 30, 0),
                EndHour = new TimeSpan(0, hour + 1 , 30, 0),
                DayOfWeek = dayOfWeek
            };
        }
    }
}
