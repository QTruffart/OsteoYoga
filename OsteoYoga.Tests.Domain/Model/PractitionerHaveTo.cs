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
            IList<PratictionerPreference> preferences = new List<PratictionerPreference>();

            Pratictioner pratictioner = new Pratictioner()
            {
                OfficePreferences = preferences
            };

            Assert.AreEqual(preferences, pratictioner.OfficePreferences);
        }
    }
}
