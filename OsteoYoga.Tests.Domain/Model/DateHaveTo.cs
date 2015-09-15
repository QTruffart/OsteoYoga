using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DateHaveTo
    {


        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Contact contact = new Contact();
            Office office = new Office();
            Duration duration = new Duration();

            Date date = new Date()
            {
                Contact = contact,
                Office = office,
                Duration = duration
            };

            Assert.AreEqual(contact, date.Contact);
            Assert.AreEqual(office, date.Office);
            Assert.AreEqual(duration, date.Duration);
        }
    }
}
