using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class PatientHaveTo
    {
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
           string history = "history";

            Patient patient = new Patient()
            {
                History = history
            };

            Assert.AreEqual(history, patient.History);
        }
    }
}
