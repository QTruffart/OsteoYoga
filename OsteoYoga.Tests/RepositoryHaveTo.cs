using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class RepositoryHaveTo : BaseTestsNHibernate
    {
        IRepository<TimeSlot> repository = new Repository<TimeSlot>();

        OfficeRepository officeRepository = new OfficeRepository();

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            //repository.DeleteAll();
            officeRepository.DeleteAll();
        }

        [TestMethod]
        public void Save()
        {
            TimeSlot entity = new TimeSlot();

            repository.Save(entity);

            Assert.IsNotNull(entity.Id);
        }

        [TestMethod]
        public void Update()
        {
            TimeSlot entity = new TimeSlot();

            repository.Save(entity);

            entity.DayOfWeek = DayOfWeek.Saturday;

            repository.Save(entity);

            Assert.AreEqual(DayOfWeek.Saturday, entity.DayOfWeek);
        }

        [TestMethod]
        public void GetById()
        {
            TimeSlot expectedEntity = new TimeSlot();
            repository.Save(expectedEntity);

            Entity resultEntity = repository.GetById(expectedEntity.Id);

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

            TimeSlot expectedEntity1 = new TimeSlot();
            repository.Save(expectedEntity1);

            repository.Delete(expectedEntity1);

            IList<Entity> resultEntities = repository.GetAll().ToList<Entity>();
            Assert.AreEqual(0, resultEntities.Count);
        }

        [TestMethod]
        public void DeleteAll()
        {

            TimeSlot expectedEntity1 = new TimeSlot();
            TimeSlot expectedEntity2 = new TimeSlot();
            repository.Save(expectedEntity1);
            repository.Save(expectedEntity2);
            
            repository.DeleteAll();

            IList<Entity> resultEntities = repository.GetAll().ToList<Entity>();
            Assert.AreEqual(0, resultEntities.Count);
        }
    }
}
