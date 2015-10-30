using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;

namespace OsteoYoga.Tests.Helper
{
    [TestClass]
    public class CryptographyHelperHaveTo
    {
        [TestMethod]
        public void Encrypt_And_Decrypt_Correctly_With_Key_In_Resource_File()
        {
            //arrange
            ICryptographyHelper cryptographyHelper = new CryptographyHelper();
            string password = "password";

            //act
            string encryptPassword = cryptographyHelper.Encrypt(password);
            string decryptPassword = cryptographyHelper.Decrypt(encryptPassword);

            //assert
            Assert.AreEqual(password, decryptPassword);
        }
    }
}
