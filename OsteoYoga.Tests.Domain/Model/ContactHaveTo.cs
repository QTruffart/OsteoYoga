﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Resource;

namespace OsteoYoga.Tests.Domain.Model
{

    public class StubContact : Contact
    {}

    [TestClass]
    public class ContactHaveTo
    {
        readonly IList<Date> dates = new List<Date>();
        readonly IList<Profile> profiles = new List<Profile>();
        const string NetworkType = "networkType";
        const string NetworkId = "netWorkId";
        const string FullName = "name";
        const string Mail = "mail@domaine.com";
        const string Phone = "+33(0)612345678";

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            Contact contact = new StubContact
            {
                                        FullName = FullName,
                                        Mail = Mail,
                                        Phone = Phone,
                                        //Dates = dates,
                                        NetworkId = NetworkId,
                                        NetworkType = NetworkType,
                                        Profiles = profiles
                                    };

            Assert.AreEqual(FullName, contact.FullName);
            Assert.AreEqual(Mail, contact.Mail);
            Assert.AreEqual(Phone, contact.Phone);
            //Assert.AreEqual(dates, contact.Dates);
            Assert.AreEqual(NetworkId, contact.NetworkId);
            Assert.AreEqual(NetworkType, contact.NetworkType);
            Assert.AreEqual(profiles, contact.Profiles);
        }

        [TestMethod]
        public void RenderToString()
        {
            Contact contact = new StubContact
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

            Contact contact = new StubContact()
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

            Contact contact = new StubContact { FullName = FullName, Phone = Phone, Mail = Mail };
            Assert.IsTrue(contact.IsValid());

            contact = new StubContact { FullName = FullName, Phone = Phone };
            Assert.IsFalse(contact.IsValid());

            contact = new StubContact { FullName = FullName, Mail = Mail };
            Assert.IsFalse(contact.IsValid());

            contact = new StubContact { Phone = Phone, Mail = Mail };
            Assert.IsFalse(contact.IsValid());
        }

    }
}
