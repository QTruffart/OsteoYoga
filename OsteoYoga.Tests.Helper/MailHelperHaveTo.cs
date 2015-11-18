using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Implements;

namespace OsteoYoga.Tests.Helper
{
    [TestClass]
    public class MailHelperHaveTo
    {
        [TestMethod]
        //[Ignore]
        public void Email_Send_Test()
        {
            //arrange
            Contact contact = new Patient() {Mail = "quentin.truffart@gmail.com"};

            //act
            ConfirmMailHelper mailHelper = new ConfirmMailHelper();

            mailHelper.SendMail(contact);
        }
    }
}