using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Site.Controllers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class LoginControllerHaveTo
    {
        const string Email = "toto@toto.com";
        const string Name = "fullName";
        const string Id = "id";
        readonly string googleNetwork = Constants.GetInstance().GoogleNetwork;
        readonly string faceBookNetwork = Constants.GetInstance().FacebookNetwork;
        readonly Contact currentContact = new Contact();
        readonly Profile profile = new Profile();
        readonly IList<Office> offices = new List<Office>();
        readonly Mock<ContactRepository> contactRepositoryMock = new Mock<ContactRepository>();
        readonly Mock<ProfileRepository> profileRepositoryMock = new Mock<ProfileRepository>();
        readonly Mock<OfficeRepository> officeRepositoryMock = new Mock<OfficeRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private LoginController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new LoginController { 
                ContactRepository = contactRepositoryMock.Object,
                OfficeRepository = officeRepositoryMock.Object,
                ProfileRepository = profileRepositoryMock.Object
            };
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);
            officeRepositoryMock.Setup(hrm => hrm.GetAll()).Returns(offices);
            SessionHelper.Instance = sessionHelperMock.Object;
        }

        [TestMethod]
        public void Initialize_Correctly()
        {
            LoginController controller= new LoginController();

            Assert.IsInstanceOfType(controller.ContactRepository, typeof(ContactRepository));
            Assert.IsInstanceOfType(controller.OfficeRepository, typeof(OfficeRepository));
            Assert.IsInstanceOfType(controller.ProfileRepository, typeof(ProfileRepository));
        }

        #region Utilisateur déjà connecté ?
        [TestMethod]
        public void Load_RendezVous_Index_Page_If_The_Contact_Is_Connected()
        {
            sessionHelperMock.SetupGet(shm => shm.CurrentUser).Returns(currentContact);

            PartialViewResult view = Controller.Index();

            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", view.ViewName);
            Assert.AreEqual(offices, view.Model);
        }

        [TestMethod]
        public void Load_Login_Page_If_The_Contact_Is_Not_Connected()
        {
            sessionHelperMock.SetupGet(shm => shm.CurrentUser).Returns((Contact) null);

            PartialViewResult view = Controller.Index();

            Assert.AreEqual("Index", view.ViewName);
        }
        #endregion
        
        #region Connexion avec compte classique

        [TestMethod]
        public void Login_Get_Existing_Contact()
        {
            Contact contact = new Contact
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(contact.Mail)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetByEmail(contact.Mail)).Returns(contact);

            PartialViewResult viewResult = Controller.Login(Email);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(contact.Mail), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", viewResult.ViewName);
            Assert.AreEqual(offices, viewResult.Model);
        }

        [TestMethod]
        public void Login_Throw_To_The_Same_Page()
        {
            Contact contact = new Contact
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(contact.Mail)).Returns(false);
            
            PartialViewResult viewResult = Controller.Login(Email);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(contact.Mail), Times.Once());
            Assert.AreEqual("Index", viewResult.ViewName);
        }
        #endregion

        #region Inscription classique
            [TestMethod]
            public void SignIn_Error_If_Email_Already_Exists()
            {
                Contact contact = new Contact
                {
                    Mail = Email
                };
                contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(true);

                PartialViewResult viewResult = Controller.SignIn(contact);

                contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
                Assert.AreEqual("SignIn", viewResult.ViewName);
            }

            [TestMethod]
            public void SignIn_Create_A_New_Contact()
            {
                Contact contact = new Contact
                {
                    Mail = Email
                };
                contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(false);
                profileRepositoryMock.Setup(prm => prm.GetByName(Constants.GetInstance().PatientProfile)).Returns(profile);

                PartialViewResult viewResult = Controller.SignIn(contact);

                contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
                contactRepositoryMock.Verify(crm => crm.Save(contact));
                sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
                Assert.AreEqual("~/Views/RendezVous/Index.cshtml", viewResult.ViewName);
                CollectionAssert.Contains(contact.Profiles.ToList(), profile);
                Assert.AreEqual(offices, viewResult.Model);
            }
        #endregion
        
        #region Connexion avec les réseaux sociaux
        [TestMethod]
        public void Login_With_Facebook_Get_Existing_Contact()
        {
            Contact contact = new Contact();
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetBySocialNetworkEmail(Email, Id, faceBookNetwork)).Returns(contact);

            PartialViewResult viewResult = Controller.LoginWithFacebook(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml",  viewResult.ViewName);
        }


        [TestMethod]
        public void Login_With_Facebook_Go_To_Phone_Subscription()
        {
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork)).Returns(false);

            PartialViewResult viewResult = Controller.LoginWithFacebook(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork), Times.Once());
            Assert.AreEqual("PhoneSubscription", viewResult.ViewName);
            Assert.AreEqual(Id, ((Contact)viewResult.Model).NetworkId);
            Assert.AreEqual(faceBookNetwork, ((Contact)viewResult.Model).NetworkType);
            Assert.AreEqual(Email, ((Contact)viewResult.Model).Mail);
            Assert.AreEqual(Name, ((Contact)viewResult.Model).FullName);
        }


        [TestMethod]
        public void Login_With_Google_Get_Existing_Contact()
        {
            Contact contact = new Contact();
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetBySocialNetworkEmail(Email, Id, googleNetwork)).Returns(contact);

            PartialViewResult viewResult = Controller.LoginWithGoogle(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", viewResult.ViewName);
            Assert.AreEqual(offices, viewResult.Model);
        }

        [TestMethod]
        public void Login_With_Google_Go_To_Phone_Subscription()
        {
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork)).Returns(false);

            PartialViewResult viewResult = Controller.LoginWithGoogle(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork), Times.Once());
            Assert.AreEqual("PhoneSubscription", viewResult.ViewName);
            Assert.AreEqual(Id, ((Contact)viewResult.Model).NetworkId);
            Assert.AreEqual(googleNetwork, ((Contact)viewResult.Model).NetworkType);
            Assert.AreEqual(Email, ((Contact)viewResult.Model).Mail);
            Assert.AreEqual(Name, ((Contact)viewResult.Model).FullName);
        }
        #endregion
        
        #region Phone Subscription

        [TestMethod]
        public void PhoneSubscription_Save_Contact()
        {
            Contact contact = new Contact();
            profileRepositoryMock.Setup(prm => prm.GetByName(Constants.GetInstance().PatientProfile)).Returns(profile);

            PartialViewResult viewResult = Controller.PhoneSubscription(contact);

            contactRepositoryMock.Verify(crm => crm.Save(contact));
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("~/Views/RendezVous/Index.cshtml", viewResult.ViewName);
            Assert.AreEqual(offices, viewResult.Model);
            CollectionAssert.Contains(contact.Profiles.ToList(), profile);
        }

        #endregion
    }
}
