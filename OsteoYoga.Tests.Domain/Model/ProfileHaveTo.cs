using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class ProfileHaveTo
    {

        const string Name = "name";
        readonly IList<Contact> contacts = new List<Contact>();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            
            Profile profile = new Profile
                                    {
                                        Name = Name,
                                        Contacts = contacts
                                    };
            Assert.AreEqual(Name, profile.Name);
            Assert.AreEqual(contacts, profile.Contacts);
        }
    }
}
