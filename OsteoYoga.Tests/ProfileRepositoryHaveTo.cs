using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Implements;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class ProfileRepositoryHaveTo : BaseTestsNHibernate
    {
        private readonly Profile profile = new Profile()
            {
                Name = "name",
            };

        private readonly ProfileRepository profileRepository = new ProfileRepository();

        [TestInitialize]
        public override void Initialize()
        {
        }

        [TestCleanup]
        public override void CleanUp()
        {
            profileRepository.DeleteAll();
        }

        [TestMethod]
        public void Return_Null_If_Contact_Does_Not_Exist()
        {
            Assert.IsNull(profileRepository.GetByName("name"));
        }

        [TestMethod]
        public void Return_Profile_By_Name()
        {
            profileRepository.Save(profile);
            Assert.AreEqual(profile, profileRepository.GetByName("name"));
        }
    }
}