using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Display.Controllers;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

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
        public void GoToMeContacter()
        {
            Assert.AreEqual("MeContacter", Controller.MeContacter().ViewName);
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
    }
}
