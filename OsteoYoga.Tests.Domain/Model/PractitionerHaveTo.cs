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
            //IList<PratictionerOffice> preferences = new List<PratictionerOffice>();
            IList<Profile> profiles = new List<Profile>();
            string profession = "profession";

            Pratictioner pratictioner = new Pratictioner()
            {
                Profession = profession,
                Profiles = profiles
            };

            Assert.AreEqual(profession, pratictioner.Profession);
            Assert.AreEqual(profiles, pratictioner.Profiles);
        }
    }
}
