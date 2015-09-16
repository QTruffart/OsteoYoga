using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Site.Controllers;


namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class HomeControllerHaveTo
    {
        
        private HomeController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new HomeController();
        }

        [TestMethod]
        public void GoToMeContacterABayonne()
        {
            Assert.AreEqual("MeContacterABayonne", Controller.MeContacterABayonne().ViewName);
        }
        [TestMethod]
        public void GoToMeContacterRionDesLandes()
        {
            Assert.AreEqual("MeContacterARionDesLandes", Controller.MeContacterARionDesLandes().ViewName);
        }

        [TestMethod]
        public void GoToYogathérapie()
        {
            Assert.AreEqual("Yogathérapie", Controller.Yogathérapie().ViewName);
        }

        [TestMethod]
        public void GoToMaPratique()
        {
            Assert.AreEqual("MaPratique", Controller.MaPratique().ViewName);
        }

        [TestMethod]
        public void GoToOsthéopathie()
        {
            Assert.AreEqual("Osthéopathie", Controller.Osthéopathie().ViewName);
        }

        [TestMethod]
        public void GoToPartenaires()
        {
            Assert.AreEqual("Partenaires", Controller.Partenaires().ViewName);
        }

        [TestMethod]
        public void GoToTarifs()
        {
            Assert.AreEqual("Tarifs", Controller.Tarifs().ViewName);
        }

        [TestMethod]
        public void GoToQuiSuisJe()
        {
            Assert.AreEqual("QuiSuisJe", Controller.QuiSuisJe().ViewName);
        }

        [TestMethod]
        public void GoToQuandConsulterBebe()
        {
            Assert.AreEqual("QuandConsulterBebe", Controller.QuandConsulterBebe().ViewName);
        }

        [TestMethod]
        public void GoToQuandConsulterGrossesse()
        {
            Assert.AreEqual("QuandConsulterGrossesse", Controller.QuandConsulterGrossesse().ViewName);
        }

        [TestMethod]
        public void GoToMotifsCourants()
        {
            Assert.AreEqual("MotifsCourants", Controller.MotifsCourants().ViewName);
        }

        [TestMethod]
        public void GoToMotifsMeconnus()
        {
            Assert.AreEqual("MotifsMeconnus", Controller.MotifsMeconnus().ViewName);
        }

        [TestMethod]
        public void QuestionsFrequentes()
        {
            Assert.AreEqual("QuestionsFrequentes", Controller.QuestionsFrequentes().ViewName);
        }

        [TestMethod]
        public void Certifications()
        {
            Assert.AreEqual("Certifications", Controller.Certifications().ViewName);
        }

        [TestMethod]
        public void CoursYoga()
        {
            Assert.AreEqual("CoursYoga", Controller.CoursYoga().ViewName);
        }

        [TestMethod]
        public void LiensDivers()
        {
            Assert.AreEqual("LiensDivers", Controller.LiensDivers().ViewName);
        }

        [TestMethod]
        public void Association()
        {
            Assert.AreEqual("Association", Controller.Association().ViewName);
        }
    }
}
