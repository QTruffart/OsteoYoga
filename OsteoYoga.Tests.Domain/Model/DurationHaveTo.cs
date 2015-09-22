using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DurationHaveTo
    {

        const int Value = 45;
        PratictionerPreference preference = new PratictionerPreference();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            
            Duration duration = new Duration()
            {
                Value = Value,
                PratictionerPreference = preference
                //Dates = dates,
            };
            Assert.AreEqual(Value, duration.Value);
            //Assert.AreEqual(dates, duration.Dates);
            Assert.AreEqual(preference, duration.PratictionerPreference);
        }
    }
}
