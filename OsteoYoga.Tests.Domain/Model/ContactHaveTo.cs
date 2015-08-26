using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class ContactHaveTo
    {

        const string FullName = "name";
        const string Mail = "mail@domaine.com";
        const string Phone = "+33(0)612345678";

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Contact contact = new Contact
                                    {
                                        FullName = FullName,
                                        Mail = Mail,
                                        Phone = Phone,
                                    };
            Assert.AreEqual(FullName, contact.FullName);
            Assert.AreEqual(Mail, contact.Mail);
            Assert.AreEqual(Phone, contact.Phone);
        }

        [TestMethod]
        public void RenderToString()
        {
            Contact contact = new Contact
                {
                    FullName = FullName,
                    Mail = Mail,
                    Phone = Phone,
            };

            Assert.AreEqual(FullName + " ( " + ModelResource.Mail + ": " + Mail + " ; " + ModelResource.Phone + ": " + Phone + " )", contact.ToString());
        }

        [TestMethod]
        public void KnowIfEmailIsCorrectlyFormatted()
        {
            const string incorrectMail1 = "maildomaine.com";
            const string incorrectMail2 = "mail@domainecom";
            const string incorrectMail3 = "maildomainecom";
            const string incorrectMail4 = "m\\ail@dom8/aine.com";
            const string correctMail = "mail@domaine.com";

            Contact contact = new Contact()
            {
                FullName = FullName,
                Phone = Phone,
            };

            contact.Mail = incorrectMail1;
            Assert.IsFalse(contact.IsValid());

            contact.Mail = incorrectMail2;
            Assert.IsFalse(contact.IsValid());
            
            contact.Mail = incorrectMail3;
            Assert.IsFalse(contact.IsValid());

            contact.Mail = incorrectMail4;
            Assert.IsFalse(contact.IsValid());

            contact.Mail = correctMail;
            Assert.IsTrue(contact.IsValid());
        }

        [TestMethod]
        public void KnowIfContactIsValid()
        {

            Contact contact = new Contact { FullName = FullName, Phone = Phone, Mail = Mail };
            Assert.IsTrue(contact.IsValid());

            contact = new Contact { FullName = FullName, Phone = Phone };
            Assert.IsFalse(contact.IsValid());

            contact = new Contact { FullName = FullName, Mail = Mail };
            Assert.IsFalse(contact.IsValid());

            contact = new Contact { Phone = Phone, Mail = Mail };
            Assert.IsFalse(contact.IsValid());
        }

    }
}
