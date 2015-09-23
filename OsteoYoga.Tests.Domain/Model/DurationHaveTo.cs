using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DurationHaveTo
    {

        const int Value = 45;
        PratictionerOffice office = new PratictionerOffice();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            
            Duration duration = new Duration()
            {
                Value = Value,
                PratictionerOffice = office
                //Dates = dates,
            };
            Assert.AreEqual(Value, duration.Value);
            //Assert.AreEqual(dates, duration.Dates);
            Assert.AreEqual(office, duration.PratictionerOffice);
        }
    }
}
