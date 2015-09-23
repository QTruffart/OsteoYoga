using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class GoogleRepositoryHaveTo
    {
        private readonly IGoogleRepository<Event> googleRepository = new GoogleRepository();

        private readonly PratictionerOffice pratictionerOffice = new PratictionerOffice()
        {
            Pratictioner = new Pratictioner(),
            Office = new Office(),
            TimeSlots = new List<WorkTimeSlot>(),
            DateWaiting = 30,
            Reminder = 45,
            MinInterval = 3,
            MaxInterval = 15
        };

        private readonly PratictionerOffice pratictionerPreference2 = new PratictionerOffice()
        {
            Pratictioner = new Pratictioner(),
            TimeSlots = new List<WorkTimeSlot>(),
            Office = new Office(),
            DateWaiting = 40,
            Reminder = 30
        };

        private readonly Date date1 = new Date()
        {
            Patient = new Patient {FullName = "fullName", Mail = "padbox@gmail.com"},
            Duration = new Duration {Value = 45},
            Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
            BeginTime = DateTime.Now.Add(new TimeSpan(1, 1, 0))
        };

        private readonly Date date2 = new Date()
        {
            Patient = new Patient {FullName = "fullName2", Mail = "yopex24@hotmail.fr"},
            Duration = new Duration {Value = 30},
            Office = new Office {Adress = "14 rue Richard Wagner 33700 Mérignac"},
            BeginTime = DateTime.Now.Add(new TimeSpan(2, 1, 0)),
        };

        private const string Description = "description";
        private const string Summary = "summary";


        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void Save()
        {

            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerOffice);


            Assert.AreEqual(date1.Office.Adress, entity.Location);
            Assert.AreEqual(date1.BeginTime.ToString("ddMMyyyyHHmm"), ((DateTime) entity.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date1.BeginTime.AddMinutes(date1.Duration.Value).ToString("ddMMyyyyHHmm"), ((DateTime) entity.End.DateTime).ToString("ddMMyyyyHHmm"));
            CollectionAssert.Contains(entity.Reminders.Overrides.Select(o => o.Minutes).ToList(), pratictionerOffice.Reminder);
            Assert.AreEqual(Summary, entity.Summary);
            Assert.AreEqual(Description, entity.Description);
            CollectionAssert.Contains(entity.Attendees.Select(a => a.Email).ToList(), date1.Patient.Mail);
            CollectionAssert.Contains(entity.Attendees.Select(a => a.DisplayName).ToList(), date1.Patient.FullName);


            Assert.IsNotNull(entity.Id);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }

        [TestMethod]
        public void Update()
        {
            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerOffice);

            Event eventToUpdate = googleRepository.GetById(entity.Id);

            googleRepository.Update(eventToUpdate.Id, date2, "summary updated", "description updated", pratictionerPreference2);
            Event eventToCompare = googleRepository.GetById(eventToUpdate.Id);

            Assert.AreEqual(date2.Office.Adress, eventToCompare.Location);
            Assert.AreEqual(date2.BeginTime.ToString("ddMMyyyyHHmm"), ((DateTime) eventToCompare.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date2.BeginTime.AddMinutes(date2.Duration.Value).ToString("ddMMyyyyHHmm"), ((DateTime) eventToCompare.End.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual("summary updated", eventToCompare.Summary);
            Assert.AreEqual("description updated", eventToCompare.Description);
            CollectionAssert.Contains(eventToCompare.Attendees.Select(a => a.Email).ToList(), date2.Patient.Mail);
            CollectionAssert.Contains(eventToCompare.Attendees.Select(a => a.DisplayName).ToList(), date2.Patient.FullName);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }

        [TestMethod]
        public void GetById()
        {

            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerOffice);


            Event toCompare = googleRepository.GetById(entity.Id);

            Assert.AreEqual(date1.Office.Adress, toCompare.Location);
            Assert.AreEqual(date1.BeginTime.ToString("ddMMyyyyHHmm"),
                ((DateTime) toCompare.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date1.BeginTime.AddMinutes(date1.Duration.Value).ToString("ddMMyyyyHHmm"),
                ((DateTime) toCompare.End.DateTime).ToString("ddMMyyyyHHmm"));
            CollectionAssert.Contains(toCompare.Reminders.Overrides.Select(o => o.Minutes).ToList(),
                pratictionerOffice.Reminder);
            Assert.AreEqual(Summary, toCompare.Summary);
            Assert.AreEqual(Description, entity.Description);
            CollectionAssert.Contains(toCompare.Attendees.Select(a => a.Email).ToList(), date1.Patient.Mail);
            CollectionAssert.Contains(toCompare.Attendees.Select(a => a.DisplayName).ToList(), date1.Patient.FullName);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }



        [TestMethod]
        [Ignore] // Rouge étant donné que google limite le nombre résultat
        public void GetAll()
        {
            Event entity1 = googleRepository.Save(date1, "summary1", "description1", pratictionerOffice);
            Event entity2 = googleRepository.Save(date2, "summary2", "description2", pratictionerOffice);

            IList<Event> events = googleRepository.GetAll();

            CollectionAssert.Contains(events.Select(e => e.Id).ToList(), entity1.Id);
            CollectionAssert.Contains(events.Select(e => e.Id).ToList(), entity2.Id);

            //Test CleanUp
            googleRepository.Delete(entity1.Id);
            googleRepository.Delete(entity2.Id);
        }

        [TestMethod]
        public void Delete()
        {
            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerOffice);
            googleRepository.Delete(entity.Id);

            IList<Event> events = googleRepository.GetAll();

            CollectionAssert.DoesNotContain(events.Select(e => e.Id).ToList(), entity.Id);
        }

        [TestMethod]
        public void GetAllForPractionerInterval()
        {
            //arrange
            Date dateIntoInterval1 = new Date()
            {
                Patient = new Patient() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                BeginTime = DateTime.Now.AddDays(14)
            };

            Date dateIntoInterval2 = new Date()
            {
                Patient = new Patient() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                BeginTime = DateTime.Now.AddDays(3)
            };

            Date dateBeforeInterval = new Date()
            {
                Patient = new Patient() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                BeginTime = DateTime.Now.AddDays(1)
            };

            Date dateAfterInterval = new Date()
            {
                Patient = new Patient() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                BeginTime = DateTime.Now.AddDays(15)
            };

            Event entityIntoInterval1 = googleRepository.Save(dateIntoInterval1, Summary, Description,pratictionerOffice);
            Event entityIntoInterval2 = googleRepository.Save(dateIntoInterval2, Summary, Description,pratictionerOffice);
            Event entityBeforeInterval = googleRepository.Save(dateBeforeInterval, Summary, Description,pratictionerOffice);
            Event entityAfterInterval = googleRepository.Save(dateAfterInterval, Summary, Description,pratictionerOffice);

            //act

            IList<Event> events = googleRepository.GetAllForPractionerInterval(pratictionerOffice);

            //assert
            CollectionAssert.Contains(events.Select( e => e.Id).ToList(), entityIntoInterval1.Id);
            CollectionAssert.Contains(events.Select( e => e.Id).ToList(), entityIntoInterval2.Id);
            CollectionAssert.DoesNotContain(events.Select( e => e.Id).ToList(), entityBeforeInterval.Id);
            CollectionAssert.DoesNotContain(events.Select( e => e.Id).ToList(), entityAfterInterval.Id);

            googleRepository.Delete(entityIntoInterval1.Id);
            googleRepository.Delete(entityIntoInterval2.Id);
            googleRepository.Delete(entityBeforeInterval.Id);
            googleRepository.Delete(entityAfterInterval.Id);

        }
    }
}
    