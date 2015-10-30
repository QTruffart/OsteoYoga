using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Implements;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class PatientRepositoryHaveTo : BaseTestsNHibernate
    {
        private readonly Patient contact = new Patient
        {
            FullName = "test",
            Password = "password",
            Mail = "test@test.com",
            Phone = "+33(0)556578996",
            NetworkId = "NetworkId",
            NetworkType = Constants.GetInstance().FacebookNetwork,
        };

        private readonly PatientRepository contactRepository = new PatientRepository();

        [TestInitialize]
        public override void Initialize()
        {
        }

        [TestCleanup]
        public override void CleanUp()
        {
            contactRepository.DeleteAll();
        }

        [TestMethod]
        public void Return_True_If_Email_Already_Exist()
        {
            contactRepository.Save(contact);
            Assert.IsTrue(contactRepository.EmailAlreadyExists(contact.Mail));
        }

        [TestMethod]
        public void Return_False_If_Email_Doesnt_Exist()
        {
            Assert.IsFalse(contactRepository.EmailAlreadyExists("toto@toto.com"));
        }

        [TestMethod]
        public void Return_Null_If_Contact_Does_Not_Exist()
        {
            Assert.IsNull(contactRepository.GetByEmailAndPassword("toto@toto.com", "password"));
        }

        [TestMethod]
        public void Return_Null_If_Password_Is_Incorrect()
        {
            contactRepository.Save(contact);

            Assert.IsNull(contactRepository.GetByEmailAndPassword(contact.Mail, "wrongPassword"));
        }

        [TestMethod]
        public void Return_Contact_By_Email()
        {
            contactRepository.Save(contact);

            Assert.AreEqual(contact, contactRepository.GetByEmailAndPassword(contact.Mail, "password"));
        }

        [TestMethod]
        public void Return_True_If_Email_Already_Exist_For_Social_Network()
        {
            contactRepository.Save(contact);
            Assert.IsTrue(contactRepository.SocialNetworkEmailAlreadyExists(contact.Mail,contact.NetworkId, contact.NetworkType));
        }

        [TestMethod]
        public void Return_False_If_Email_Does_Not_Exist_For_Social_Network()
        {
            Assert.IsFalse(contactRepository.SocialNetworkEmailAlreadyExists(contact.Mail, contact.NetworkId, contact.NetworkType));
        }

        [TestMethod]
        public void ReturnNullIfContactDoesNotExist()
        {
            Assert.IsNull(contactRepository.GetBySocialNetworkEmail(contact.Mail, contact.NetworkId, contact.NetworkType));
        }

        [TestMethod]
        public void ReturnContactByEmail()
        {
            contactRepository.Save(contact);

            Assert.AreEqual(contact, contactRepository.GetBySocialNetworkEmail(contact.Mail, contact.NetworkId, contact.NetworkType));
        }

    }
}