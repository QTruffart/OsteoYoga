using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Resource.Contact;
using OsteoYoga.Site.Controllers;

namespace OsteoYoga.Tests.Display.Controllers
{
    [TestClass]
    public class LoginControllerHaveTo
    {
        const string Email = "toto@toto.com";
        const string Password = "password";
        const string EncryptedPassword = "encryptedPassword";
        const string Name = "fullName";
        const string Id = "id";
        readonly string googleNetwork = Constants.GetInstance().GoogleNetwork;
        readonly string faceBookNetwork = Constants.GetInstance().FacebookNetwork;
        readonly Patient currentPatient = new Patient();
        readonly Profile profile = new Profile();
        readonly Mock<PatientRepository> contactRepositoryMock = new Mock<PatientRepository>();
        readonly Mock<ProfileRepository> profileRepositoryMock = new Mock<ProfileRepository>();
        readonly Mock<CryptographyHelper> cryptographyHelperMock = new Mock<CryptographyHelper>();
        readonly Mock<IMailHelper> mailHelperMock = new Mock<IMailHelper>();

        readonly Mock<SessionHelper> sessionHelperMock = new Mock<SessionHelper>();
        private LoginController Controller { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Controller = new LoginController { 
                ContactRepository = contactRepositoryMock.Object,
                ProfileRepository = profileRepositoryMock.Object,
                CryptographyHelper = cryptographyHelperMock.Object,
                MailHelper = mailHelperMock.Object
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
            Assert.IsInstanceOfType(controller.CryptographyHelper, typeof(CryptographyHelper));
            Assert.IsInstanceOfType(controller.MailHelper, typeof(ConfirmMailHelper));
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
                Mail = Email,
                Password = Password
            };


            cryptographyHelperMock.Setup(chm => chm.Encrypt(contact.Password)).Returns(EncryptedPassword);
            contactRepositoryMock.Setup(crm => crm.GetByEmailAndPassword(contact.Mail, EncryptedPassword)).Returns(contact);

            ActionResult viewResult = Controller.Login(Email, Password);

            contactRepositoryMock.Verify(crm => crm.GetByEmailAndPassword(contact.Mail, EncryptedPassword), Times.Once());
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Login_Throw_To_The_Same_Page()
        {
            Contact contact = new Patient
            {
                Mail = Email,
                Password = Password
            };
            cryptographyHelperMock.Setup(chm => chm.Encrypt(contact.Password)).Returns(EncryptedPassword);
            contactRepositoryMock.Setup(crm => crm.GetByEmailAndPassword(contact.Mail, EncryptedPassword)).Returns((Contact) null);

            ActionResult viewResult = (PartialViewResult) Controller.Login(Email, Password);

            contactRepositoryMock.Verify(crm => crm.GetByEmailAndPassword(contact.Mail, EncryptedPassword), Times.Once());
            Assert.AreEqual(LoginResource.UnknownEmail, ((PartialViewResult)viewResult).ViewBag.Errors);
            Assert.AreEqual("Index", ((PartialViewResult)viewResult).ViewName);
        }
        #endregion

        #region Inscription classique
        [TestMethod]
        public void SignIn_Error_If_Email_Already_Exists()
        {
            Patient patient = new Patient
            {
                Mail = Email,
                Password = Password

            };
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(true);

            ActionResult viewResult = Controller.SignIn(patient);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
            Assert.AreEqual("SignIn", ((PartialViewResult)viewResult).ViewName);
            Assert.AreEqual(SignInResource.EmailAlreadyExists, ((PartialViewResult)viewResult).ViewBag.SignInError);
        }

        [TestMethod]
        public void SignIn_Create_A_New_Contact()
        {
            Patient contact = new Patient
            {
                Mail = Email,
                Password = Password
            };
            cryptographyHelperMock.Setup(chm => chm.Encrypt(contact.Password)).Returns(EncryptedPassword);
            contactRepositoryMock.Setup(crm => crm.EmailAlreadyExists(Email)).Returns(false);
            profileRepositoryMock.Setup(prm => prm.GetByName(Constants.GetInstance().PatientProfile)).Returns(profile);

            ActionResult viewResult = Controller.SignIn(contact);

            contactRepositoryMock.Verify(crm => crm.EmailAlreadyExists(Email), Times.Once());
            contactRepositoryMock.Verify(crm => crm.Save(contact));
            sessionHelperMock.VerifySet(shm => shm.CurrentUser = contact, Times.Once());
            mailHelperMock.Verify(mhm => mhm.SendMail(contact), Times.Once());

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
            mailHelperMock.Verify(mhm => mhm.SendMail(patient),Times.Once());
            CollectionAssert.Contains(patient.Profiles.ToList(), profile);
            Assert.AreEqual("RendezVous", ((RedirectToRouteResult)viewResult).RouteValues["controller"]);
            Assert.AreEqual("Index", ((RedirectToRouteResult)viewResult).RouteValues["action"]);
            
        }

        #endregion

        #region Confirm Mail


        [TestMethod]
        public void Send_A_Confirm_Link_To_The_Contact_To_Confirm_Account()
        {
            //arrange
            Contact  contact = new Patient();
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(contact);

            //act
            PartialViewResult result = Controller.ConfirmAccount();

            //assert
            Assert.AreEqual(contact.Id, result.Model);
            Assert.AreEqual("ConfirmAccount", result.ViewName);
        }
        
        [TestMethod]
        public void Go_To_Login_Page_If_There_Are_Not_Logged_User()
        {
            //arrange
            sessionHelperMock.Setup(shm => shm.CurrentUser).Returns(null as Contact);
            
            //act
            PartialViewResult result = Controller.ConfirmAccount();

            //assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Validate_Code_Go_To_RDV_Page()
        {
            //arrange
            int validCode = 54123;
            int idContact= 1;

            Patient contact= new Patient() { ConfirmedCode = validCode, IsConfirmed = false, Id = idContact};
            contactRepositoryMock.Setup(crm => crm.GetById(idContact)).Returns(contact);

            //act
            RedirectToRouteResult result = ((RedirectToRouteResult)Controller.ValidateAccountCode(validCode, idContact));

            //assert
            Assert.AreEqual("RendezVous", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            contactRepositoryMock.Verify(crm => crm.Save(It.Is<Patient>(p => p.IsConfirmed && p.Id == idContact && p.ConfirmedCode == validCode)));
        }

        #endregion
    }
}
