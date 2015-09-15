using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class OfficeHaveTo
    {

        const string Name = "name";
        readonly IList<Date> dates = new List<Date>();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Office office = new Office
            {
                Name = Name,
                Dates = dates
            };
            Assert.AreEqual(Name, office.Name);
        }
    }
}
