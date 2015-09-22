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
            Patient contact = new Patient();
            Office office = new Office();
            Pratictioner pratictioner = new Pratictioner();
            Duration duration = new Duration();

            Date date = new Date()
            {
                Patient = contact,
                Office = office,
                Duration = duration,
                Pratictioner = pratictioner
            };

            Assert.AreEqual(contact, date.Patient);
            Assert.AreEqual(office, date.Office);
            Assert.AreEqual(duration, date.Duration);
            Assert.AreEqual(pratictioner, date.Pratictioner);
        }
    }
}
