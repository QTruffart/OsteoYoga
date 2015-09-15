using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DurationHaveTo
    {

        const int Value = 45;
        IList<Date> dates = new List<Date>();
        IList<Office> offices = new List<Office>();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            
            Duration duration = new Duration()
            {
                Value = Value,
                Dates = dates,
                Offices = offices
            };
            Assert.AreEqual(Value, duration.Value);
            Assert.AreEqual(dates, duration.Dates);
            Assert.AreEqual(offices, duration.Offices);
        }
    }
}
