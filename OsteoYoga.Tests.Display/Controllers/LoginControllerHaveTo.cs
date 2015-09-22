using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Repository.DAO.Implements;
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
        readonly Patient currentPatient = new Patient();
        readonly Profile profile = new Profile();
        readonly Mock<PatientRepository> contactRepositoryMock = new Mock<PatientRepository>();
        readonly Mock<ProfileRepository> profileRepositoryMock = new Mock<ProfileRepository>();
        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private LoginController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new LoginController { 
                ContactRepository = contactRepositoryMock.Object,
                ProfileRepository = profileRepositoryMock.Object
            };
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);
            SessionHelper.Instance = sessionHelperMock.Object;
        }

        [TestMethod]
        public void Initialize_Correctly()
        {
            LoginController controller= new LoginController();

            Assert.IsInstanceOfType(controller.ContactRepository, typeof(PatientRepository));
            Assert.IsInstanceOfType(controller.ProfileRepository, typeof(ProfileRepository));
        }

        #region Utilisateur déjà connecté ?
        [TestMethod]
        public void Load_RendezVous_Index_Page_If_The_Contact_Is_Connected()
        {
            sessionHelperMock.SetupGet(shm => shm.CurrentUser).Returns(currentPatient);

            ActionResult viewResult = Controller.Index();

            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Load_Login_Page_If_The_Contact_Is_Not_Connected()
        {
            sessionHelperMock.SetupGet(shm => shm.CurrentUser).Returns((Contact) null);

            ActionResult view = Controller.Index();

            Assert.AreEqual("Index", ((PartialViewResult)view).ViewName);
        }
        #endregion
        
        #region Connexion avec compte classique

        [TestMethod]
        public void Login_Get_Existing_Contact()
        {
            Contact contact = new Patient
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(contact.Mail)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetByEmail(contact.Mail)).Returns(contact);

            ActionResult viewResult = Controller.Login(Email);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(contact.Mail), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Login_Throw_To_The_Same_Page()
        {
            Contact contact = new Patient
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(contact.Mail)).Returns(false);
            
            ActionResult viewResult = Controller.Login(Email);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(contact.Mail), Times.Once());
            Assert.AreEqual("Index", ((PartialViewResult)viewResult).ViewName);
        }
        #endregion

        #region Inscription classique
        [TestMethod]
        public void SignIn_Error_If_Email_Already_Exists()
        {
            Patient patient = new Patient
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(true);

            ActionResult viewResult = Controller.SignIn(patient);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
            Assert.AreEqual("SignIn", ((PartialViewResult)viewResult).ViewName);
        }

        [TestMethod]
        public void SignIn_Create_A_New_Contact()
        {
            Patient contact = new Patient
            {
                Mail = Email
            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(false);
            profileRepositoryMock.Setup(prm => prm.GetByName(Constants.GetInstance().PatientProfile)).Returns(profile);

            ActionResult viewResult = Controller.SignIn(contact);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
            contactRepositoryMock.Verify(crm => crm.Save(contact));
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            
            CollectionAssert.Contains(contact.Profiles.ToList(), profile);

            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }
        #endregion
        
        #region Connexion avec les réseaux sociaux
        [TestMethod]
        public void Login_With_Facebook_Get_Existing_Contact()
        {
            Contact contact = new Patient();
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetBySocialNetworkEmail(Email, Id, faceBookNetwork)).Returns(contact);

            ActionResult viewResult = Controller.LoginWithFacebook(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }


        [TestMethod]
        public void Login_With_Facebook_Go_To_Phone_Subscription()
        {
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork)).Returns(false);

            ActionResult viewResult = Controller.LoginWithFacebook(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, faceBookNetwork), Times.Once());
            Assert.AreEqual(Id, ((Contact)((PartialViewResult)viewResult).Model).NetworkId);
            Assert.AreEqual(faceBookNetwork, ((Contact)((PartialViewResult)viewResult).Model).NetworkType);
            Assert.AreEqual(Email, ((Contact)((PartialViewResult)viewResult).Model).Mail);
            Assert.AreEqual(Name, ((Contact)((PartialViewResult)viewResult).Model).FullName);
            Assert.AreEqual("PhoneSubscription", ((PartialViewResult)viewResult).ViewName);
        }


        [TestMethod]
        public void Login_With_Google_Get_Existing_Contact()
        {
            Contact contact = new Patient();
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork)).Returns(true);
            contactRepositoryMock.Setup(crm => crm.GetBySocialNetworkEmail(Email, Id, googleNetwork)).Returns(contact);

            ActionResult viewResult = Controller.LoginWithGoogle(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Login_With_Google_Go_To_Phone_Subscription()
        {
            contactRepositoryMock.Setup(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork)).Returns(false);

            ActionResult viewResult = Controller.LoginWithGoogle(Id, Email, Name);

            contactRepositoryMock.Verify(crm => crm.SocialNetworkEmailAlreadyExists(Email, Id, googleNetwork), Times.Once());
            Assert.AreEqual(Id, ((Contact)((PartialViewResult)viewResult).Model).NetworkId);
            Assert.AreEqual(googleNetwork, ((Contact)((PartialViewResult)viewResult).Model).NetworkType);
            Assert.AreEqual(Email, ((Contact)((PartialViewResult)viewResult).Model).Mail);
            Assert.AreEqual(Name, ((Contact)((PartialViewResult)viewResult).Model).FullName);
            Assert.AreEqual("PhoneSubscription", ((PartialViewResult)viewResult).ViewName);
        }
        #endregion
        
        #region Phone Subscription

        [TestMethod]
        public void PhoneSubscription_Save_Contact()
        {
            Patient patient = new Patient();
            profileRepositoryMock.Setup(prm => prm.GetByName(Constants.GetInstance().PatientProfile)).Returns(profile);

            ActionResult viewResult = Controller.PhoneSubscription(patient);

            contactRepositoryMock.Verify(crm => crm.Save(patient));
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = patient, Times.Once());
            CollectionAssert.Contains(patient.Profiles.ToList(), profile);
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }

        #endregion
    }
}
