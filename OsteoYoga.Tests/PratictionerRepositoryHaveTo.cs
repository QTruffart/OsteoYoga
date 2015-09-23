using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class PratictionerRepositoryHaveTo : BaseTestsNHibernate
    {

        private readonly PratictionerOffice pratictionerOffice = new PratictionerOffice()
        {
            Pratictioner = new Pratictioner(),
            Office = new Office(),
            DateWaiting = 30,
            Reminder = 45,
            MinInterval = 3,
            MaxInterval = 15
        };

        private readonly Pratictioner pratictioner = new Pratictioner
        {
            FullName = "test",
            Mail = "test@test.com",
            Phone = "+33(0)556578996",
            NetworkId = "NetworkId",
            NetworkType = Constants.GetInstance().FacebookNetwork,
            
        };

        private readonly PratictionerRepository pratictionerRepository = new PratictionerRepository();

        [TestInitialize]
        public override void Initialize()
        {
            WorkTimeSlot slot = new WorkTimeSlot()
            {
                BeginTime = DateTime.Now, EndTime = DateTime.Now
            };
            IRepository<WorkTimeSlot> tRepository = new NHibernateRepository<WorkTimeSlot>();
            tRepository.Save(slot);
        }

        [TestCleanup]
        public override void CleanUp()
        {
            pratictionerRepository.DeleteAll();
        }

        [TestMethod]
        public void Return_True_If_Email_Already_Exist()
        {
            pratictionerRepository.Save(pratictioner);
            Assert.IsTrue(pratictionerRepository.EmailAlreadyExists(pratictioner.Mail));
        }

        [TestMethod]
        public void Return_False_If_Email_Doesnt_Exist()
        {
            Assert.IsFalse(pratictionerRepository.EmailAlreadyExists("toto@toto.com"));
        }

        [TestMethod]
        public void Return_Null_If_Contact_Does_Not_Exist()
        {
            Assert.IsNull(pratictionerRepository.GetByEmail("toto@toto.com"));
        }

        [TestMethod]
        public void Return_Contact_By_Email()
        {
            pratictionerRepository.Save(pratictioner);

            Assert.AreEqual(pratictioner, pratictionerRepository.GetByEmail(pratictioner.Mail));
        }

        [TestMethod]
        public void Return_True_If_Email_Already_Exist_For_Social_Network()
        {
            pratictionerRepository.Save(pratictioner);
            Assert.IsTrue(pratictionerRepository.SocialNetworkEmailAlreadyExists(pratictioner.Mail,pratictioner.NetworkId, pratictioner.NetworkType));
        }

        [TestMethod]
        public void Return_False_If_Email_Does_Not_Exist_For_Social_Network()
        {
            Assert.IsFalse(pratictionerRepository.SocialNetworkEmailAlreadyExists(pratictioner.Mail, pratictioner.NetworkId, pratictioner.NetworkType));
        }

        [TestMethod]
        public void ReturnNullIfContactDoesNotExist()
        {
            Assert.IsNull(pratictionerRepository.GetBySocialNetworkEmail(pratictioner.Mail, pratictioner.NetworkId, pratictioner.NetworkType));
        }

        [TestMethod]
        public void ReturnContactByEmail()
        {
            pratictionerRepository.Save(pratictioner);

            Assert.AreEqual(pratictioner, pratictionerRepository.GetBySocialNetworkEmail(pratictioner.Mail, pratictioner.NetworkId, pratictioner.NetworkType));
        }

    }
}