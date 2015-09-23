using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class OfficeHaveTo
    {

        const string Name = "name";
        const string Adress = "adress";

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            
            IList<PratictionerOffice> pratictionerPreference = new List<PratictionerOffice>()
            {
                new PratictionerOffice() { Pratictioner = new Pratictioner()}
            };
            Office office = new Office
            {
                Name = Name,
                Adress = Adress,
                PratictionerPreference = pratictionerPreference
            };
            Assert.AreEqual(Name, office.Name);
            Assert.AreEqual(Adress, office.Adress);
            Assert.AreEqual(pratictionerPreference, office.PratictionerPreference);
        }
    }
}
