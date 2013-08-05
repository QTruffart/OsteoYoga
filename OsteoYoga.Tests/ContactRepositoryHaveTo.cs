using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
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
                ConfirmNumber = Guid.NewGuid(),
                IsConfirmed = false
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
        public void ReturnTrueIfEmailAlreadyExist()
        {
            contactRepository.Save(contact);
            Assert.IsTrue(contactRepository.EmailAlreadyExists(contact.Mail));
        }

        [TestMethod]
        public void ReturnFalseIfEmailDoesntExist()
        {
            Assert.IsFalse(contactRepository.EmailAlreadyExists("toto@toto.com"));
        }

        [TestMethod]
        public void ReturnNullIfContactDoesNotExist()
        {
            Assert.IsNull(contactRepository.GetByEmail("toto@toto.com"));
        }

        [TestMethod]
        public void ReturnContactByEmail()
        {
            contactRepository.Save(contact);

            Assert.AreEqual(contact, contactRepository.GetByEmail(contact.Mail));
        }
    }
}