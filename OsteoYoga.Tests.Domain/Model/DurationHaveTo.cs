using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DurationHaveTo
    {

        const int Value = 45;

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Duration duration = new Duration()
            {
                Value = Value
            };
            Assert.AreEqual(Value, duration.Value);
        }
    }
}
