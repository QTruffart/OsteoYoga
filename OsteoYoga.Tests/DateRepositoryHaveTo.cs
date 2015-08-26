using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class DateRepositoryHaveTo : BaseTestsNHibernate
    {
        private const string ConfirmationId = "id";
        readonly TimeSlotRepository timeSlotRepository = new TimeSlotRepository();
        readonly Repository<Contact> contactRepository = new Repository<Contact>();
        readonly DateRepository dateRepository = new DateRepository();
        readonly Contact contact = new Contact {FullName = "test", Mail = "test@test.com", Phone = "+33(0)556578996"};
       
        [TestInitialize]
        public override void Initialize()
        {
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
        public void GetByDay()
        {
            DateTime dateTime1 = new DateTime(2013, 07, 12);
            DateTime dateTime2 = new DateTime(2013, 07, 13);
            TimeSlot timeSlot1 = CreateTimeSlot(dateTime1.DayOfWeek, 12);
            TimeSlot timeSlot2 = CreateTimeSlot(dateTime2.DayOfWeek, 12);
            TimeSlot timeSlot3 = CreateTimeSlot(dateTime1.DayOfWeek, 13);
            timeSlotRepository.Save(timeSlot1);
            timeSlotRepository.Save(timeSlot2);
            timeSlotRepository.Save(timeSlot3);
            Date date1 = CreateDate(dateTime1, timeSlot1);
            CreateDate(dateTime2, timeSlot2);
            Date date3 = CreateDate(dateTime1, timeSlot3);

            IList<Date> dates = dateRepository.GetByDay(dateTime1);

            Assert.AreEqual(2, dates.Count);
            CollectionAssert.Contains(dates.ToList(), date1);
            CollectionAssert.Contains(dates.ToList(), date3);
        }

        [TestMethod]
        public void Validate()
        {
            DateTime dateTime = new DateTime(2013, 07, 12);
            TimeSlot timeSlot = CreateTimeSlot(dateTime.DayOfWeek, 12);
            timeSlotRepository.Save(timeSlot);
            CreateDate(dateTime, timeSlot);

            Assert.IsTrue(dateRepository.Validate(ConfirmationId).IsConfirmed);
        }

        [TestMethod]
        public void DoNotValidate()
        {
            DateTime dateTime = new DateTime(2013, 07, 12);
            TimeSlot timeSlot = CreateTimeSlot(dateTime.DayOfWeek, 12);
            timeSlotRepository.Save(timeSlot);
            CreateDate(dateTime, timeSlot);
            
            Assert.IsNull(dateRepository.Validate("incorrect"));
        }


        [TestMethod]
        public void GetFutureDate()
        {
            DateTime dateTime1 = new DateTime(2013, 07, 11);
            DateTime dateTime2 = new DateTime(2013, 07, 12);
            DateTime dateTime3 = new DateTime(2013, 07, 13);
            TimeSlot timeSlot1 = CreateTimeSlot(dateTime1.DayOfWeek);
            TimeSlot timeSlot2 = CreateTimeSlot(dateTime2.DayOfWeek);
            TimeSlot timeSlot3 = CreateTimeSlot(dateTime3.DayOfWeek);
            timeSlotRepository.Save(timeSlot1);
            timeSlotRepository.Save(timeSlot2);
            timeSlotRepository.Save(timeSlot3);
            CreateDate(dateTime1, timeSlot1);
            Date date1 = CreateDate(dateTime2, timeSlot2);
            Date date2 = CreateDate(dateTime3, timeSlot3);

            IList<Date> dates = dateRepository.GetFutureDates(new DateTime(2013, 07, 12));

            Assert.AreEqual(2, dates.Count);
            CollectionAssert.Contains(dates.ToList(), date1);
            CollectionAssert.Contains(dates.ToList(), date2);
        }

        private Date CreateDate(DateTime dateTime, TimeSlot timeSlot)
        {
            Date date = new Date
                {
                    ConfirmationId = ConfirmationId,
                    Contact = contact,
                    IsConfirmed = false,
                    Day = dateTime,
                    TimeSlot = timeSlot
                };
            dateRepository.Save(date);
            return date;
        }

        private TimeSlot CreateTimeSlot(DayOfWeek dayOfWeek, int hour = 9)
        {
            return new TimeSlot()
            {
                BeginHour = new TimeSpan(0, hour, 30, 0),
                EndHour = new TimeSpan(0, hour + 1, 30, 0),
                DayOfWeek = dayOfWeek
            };
        }
    }
}
