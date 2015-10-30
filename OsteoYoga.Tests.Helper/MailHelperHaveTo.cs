using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.Helper
{
    [TestClass]
    public class MailHelperHaveTo
    {
        [TestMethod]
        public void MethodName()
        {
            //arrange
            Contact contact = new Patient() {Mail = "quentin.truffart@gmail.com"};

            //act
            ConfirmMailHelper mailHelper = new ConfirmMailHelper();

            mailHelper.SendMail(contact);
        }

    }
}
