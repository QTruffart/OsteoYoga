using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class OfficeHaveTo
    {

        const string Name = "name";
        
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Office contact = new Office
                                    {
                                        Name = Name
                                    };
            Assert.AreEqual(Name, contact.Name);
        }
    }
}
