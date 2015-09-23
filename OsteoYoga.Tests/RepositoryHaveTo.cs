using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class RepositoryHaveTo : BaseTestsNHibernate
    {
        IRepository<Contact> contactRepository = new NHibernateRepository<Contact>();
        IRepository<Profile> profileRepository = new NHibernateRepository<Profile>();
        IRepository<Office> officeRepository = new NHibernateRepository<Office>();

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            contactRepository.DeleteAll();
            profileRepository.DeleteAll();
            officeRepository.DeleteAll();
        }

        [TestMethod]
        public void Save()
        {
            Contact entity = new Patient();

            contactRepository.Save(entity);

            Assert.IsNotNull(entity.Id);
        }

        [TestMethod]
        public void Update()
        {
            Contact entity = new Patient();

            contactRepository.Save(entity);

            entity.FullName = "NouveauNom";

            contactRepository.Save(entity);

            Assert.AreEqual("NouveauNom", entity.FullName);
        }

        [TestMethod]
        public void GetById()
        {
            Contact expectedEntity = new Pratictioner();
            contactRepository.Save(expectedEntity);

            Entity resultEntity = contactRepository.GetById(expectedEntity.Id);

            Assert.AreEqual(expectedEntity, resultEntity);
        }

        [TestMethod]
        public void GetAll()
        {

            Office officeEntity1 = new Office(){Name = "Name1"};
            Office officeEntity2 = new Office() { Name = "Name2" };
            officeRepository.Save(officeEntity1);
            officeRepository.Save(officeEntity2);

            IList<Office> resultEntities = officeRepository.GetAll().ToList();

            Assert.AreEqual(2, resultEntities.Count);
            CollectionAssert.Contains(resultEntities.ToList(), officeEntity1);
            CollectionAssert.Contains(resultEntities.ToList(), officeEntity2);
        }

        [TestMethod]
        public void Delete()
        {

            Profile expectedEntity1 = new Profile();
            profileRepository.Save(expectedEntity1);

            profileRepository.Delete(expectedEntity1);

            IList<Entity> resultEntities = contactRepository.GetAll().ToList<Entity>();
            Assert.AreEqual(0, resultEntities.Count);
        }

        [TestMethod]
        public void DeleteAll()
        {

            Profile expectedEntity1 = new Profile();
            Profile expectedEntity2 = new Profile();
            profileRepository.Save(expectedEntity1);
            profileRepository.Save(expectedEntity2);

            profileRepository.DeleteAll();

            IList<Entity> resultEntities = profileRepository.GetAll().ToList<Entity>();
            Assert.AreEqual(0, resultEntities.Count);
        }
    }
}
