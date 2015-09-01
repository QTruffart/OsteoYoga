using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class ProfileHaveTo
    {

        const string Name = "name";
        
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Profile profile = new Profile
                                    {
                                        Name = Name
                                    };
            Assert.AreEqual(Name, profile.Name);
        }
    }
}
