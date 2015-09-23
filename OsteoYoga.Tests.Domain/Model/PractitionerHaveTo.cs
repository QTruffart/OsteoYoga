using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class PractitionerHaveTo
    {
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            IList<PratictionerOffice> preferences = new List<PratictionerOffice>();
            IList<Office> offices = new List<Office>();

            Pratictioner pratictioner = new Pratictioner()
            {
                OfficePreferences = preferences,
                Offices = offices
            };

            Assert.AreEqual(preferences, pratictioner.OfficePreferences);
        }
    }
}
