using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class ContactRepositoryHaveTo : BaseTestsNHibernate
    {
        private readonly Contact contact = new Contact
            {
                FullName = "test",
                Mail = "test@test.com",
                Phone = "+33(0)556578996",
                NetworkId = "NetworkId",
                NetworkType = Constants.GetInstance().FacebookNetwork,
                
            };

        private readonly ContactRepository contactRepository = new ContactRepository();

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
            Assert.IsNull(contactRepository.GetByEmail("toto@toto.com"));
        }

        [TestMethod]
        public void Return_Contact_By_Email()
        {
            contactRepository.Save(contact);

            Assert.AreEqual(contact, contactRepository.GetByEmail(contact.Mail));
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