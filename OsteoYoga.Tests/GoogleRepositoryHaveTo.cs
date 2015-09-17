using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class GoogleRepositoryHaveTo
    {
        private readonly IGoogleRepository<Event> googleRepository = new GoogleRepository();

        private readonly PratictionerPreference pratictionerPreference = new PratictionerPreference()
        {
            Contact = new Contact(),
            DateWaiting = 30,
            Reminder = 45,
            MinInterval = 3,
            MaxInterval = 15
        };

        private readonly PratictionerPreference pratictionerPreference2 = new PratictionerPreference()
        {
            Contact = new Contact(),
            DateWaiting = 40,
            Reminder = 30
        };

        private readonly Date date1 = new Date()
        {
            Contact = new Contact() {FullName = "fullName", Mail = "padbox@gmail.com"},
            Duration = new Duration() {Value = 45},
            Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
            Begin = DateTime.Now.Add(new TimeSpan(1, 1, 0))
        };

        private readonly Date date2 = new Date()
        {
            Contact = new Contact() {FullName = "fullName2", Mail = "yopex24@hotmail.fr"},
            Duration = new Duration() {Value = 30},
            Office = new Office {Adress = "14 rue Richard Wagner 33700 Mérignac"},
            Begin = DateTime.Now.Add(new TimeSpan(2, 1, 0)),
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

            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerPreference);


            Assert.AreEqual(date1.Office.Adress, entity.Location);
            Assert.AreEqual(date1.Begin.ToString("ddMMyyyyHHmm"),
                ((DateTime) entity.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date1.Begin.AddMinutes(date1.Duration.Value).ToString("ddMMyyyyHHmm"),
                ((DateTime) entity.End.DateTime).ToString("ddMMyyyyHHmm"));
            CollectionAssert.Contains(entity.Reminders.Overrides.Select(o => o.Minutes).ToList(),
                pratictionerPreference.Reminder);
            Assert.AreEqual(Summary, entity.Summary);
            Assert.AreEqual(Description, entity.Description);
            CollectionAssert.Contains(entity.Attendees.Select(a => a.Email).ToList(), date1.Contact.Mail);
            CollectionAssert.Contains(entity.Attendees.Select(a => a.DisplayName).ToList(), date1.Contact.FullName);


            Assert.IsNotNull(entity.Id);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }

        [TestMethod]
        public void Update()
        {
            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerPreference);

            Event eventToUpdate = googleRepository.GetById(entity.Id);

            googleRepository.Update(eventToUpdate.Id, date2, "summary updated", "description updated",
                pratictionerPreference2);
            Event eventToCompare = googleRepository.GetById(eventToUpdate.Id);

            Assert.AreEqual(date2.Office.Adress, eventToCompare.Location);
            Assert.AreEqual(date2.Begin.ToString("ddMMyyyyHHmm"),
                ((DateTime) eventToCompare.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date2.Begin.AddMinutes(date2.Duration.Value).ToString("ddMMyyyyHHmm"),
                ((DateTime) eventToCompare.End.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual("summary updated", eventToCompare.Summary);
            Assert.AreEqual("description updated", eventToCompare.Description);
            CollectionAssert.Contains(eventToCompare.Attendees.Select(a => a.Email).ToList(), date2.Contact.Mail);
            CollectionAssert.Contains(eventToCompare.Attendees.Select(a => a.DisplayName).ToList(),
                date2.Contact.FullName);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }

        [TestMethod]
        public void GetById()
        {

            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerPreference);


            Event toCompare = googleRepository.GetById(entity.Id);

            Assert.AreEqual(date1.Office.Adress, toCompare.Location);
            Assert.AreEqual(date1.Begin.ToString("ddMMyyyyHHmm"),
                ((DateTime) toCompare.Start.DateTime).ToString("ddMMyyyyHHmm"));
            Assert.AreEqual(date1.Begin.AddMinutes(date1.Duration.Value).ToString("ddMMyyyyHHmm"),
                ((DateTime) toCompare.End.DateTime).ToString("ddMMyyyyHHmm"));
            CollectionAssert.Contains(toCompare.Reminders.Overrides.Select(o => o.Minutes).ToList(),
                pratictionerPreference.Reminder);
            Assert.AreEqual(Summary, toCompare.Summary);
            Assert.AreEqual(Description, entity.Description);
            CollectionAssert.Contains(toCompare.Attendees.Select(a => a.Email).ToList(), date1.Contact.Mail);
            CollectionAssert.Contains(toCompare.Attendees.Select(a => a.DisplayName).ToList(), date1.Contact.FullName);

            //Test CleanUp
            googleRepository.Delete(entity.Id);
        }

        [TestMethod]
        public void GetAll()
        {
            Event entity1 = googleRepository.Save(date1, "summary1", "description1", pratictionerPreference);
            Event entity2 = googleRepository.Save(date2, "summary2", "description2", pratictionerPreference);

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

            Event entity = googleRepository.Save(date1, Summary, Description, pratictionerPreference);
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
                Contact = new Contact() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                Begin = DateTime.Now.AddDays(14)
            };

            Date dateIntoInterval2 = new Date()
            {
                Contact = new Contact() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                Begin = DateTime.Now.AddDays(3)
            };

            Date dateBeforeInterval = new Date()
            {
                Contact = new Contact() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                Begin = DateTime.Now.AddDays(1)
            };

            Date dateAfterInterval = new Date()
            {
                Contact = new Contact() {FullName = "fullName", Mail = "padbox@gmail.com"},
                Duration = new Duration() {Value = 45},
                Office = new Office {Adress = "461 avenue de Verdun Mérignac 33700"},
                Begin = DateTime.Now.AddDays(15)
            };

            Event entityIntoInterval1 = googleRepository.Save(dateIntoInterval1, Summary, Description,pratictionerPreference);
            Event entityIntoInterval2 = googleRepository.Save(dateIntoInterval2, Summary, Description,pratictionerPreference);
            Event entityBeforeInterval = googleRepository.Save(dateBeforeInterval, Summary, Description,pratictionerPreference);
            Event entityAfterInterval = googleRepository.Save(dateAfterInterval, Summary, Description,pratictionerPreference);

            //act

            IList<Event> events = googleRepository.GetAllForPractionerInterval(pratictionerPreference);

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
    