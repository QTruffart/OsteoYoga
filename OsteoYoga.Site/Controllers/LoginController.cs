using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Resource.Contact;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.Site.Controllers
{
    public class LoginController : BaseController.BaseController
    {
        public PatientRepository ContactRepository { get; set; }
        public ProfileRepository ProfileRepository { get; set; }

        public LoginController()
        {
            ContactRepository = new PatientRepository();
            ProfileRepository = new ProfileRepository();
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Index")]
        public ActionResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return RedirectToAction("Index", "RendezVous");
            }
            return PartialView("Index");    
        }
        

        [HttpPost]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Login")]
        public ActionResult Login(string email)
        {
            if (ContactRepository.EmailAlreadyExists(email))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetByEmail(email);
                return RedirectToAction("Index", "RendezVous");
            }

            ViewBag.Errors = LoginResource.UnknownEmail;
            return PartialView("Index", email);
        }

        [HttpPost]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "SignIn")]
        public ActionResult SignIn(Patient patient)
        {
            if (!ContactRepository.EmailAlreadyExists(patient.Mail))
            {
                patient.Profiles = new List<Profile>() { ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
                ContactRepository.Save(patient);
                SessionHelper.GetInstance().CurrentUser = patient;
                return RedirectToAction("Index", "RendezVous");
            }
            ViewBag.SignInError = SignInResource.EmailAlreadyExists;
            return PartialView("SignIn", patient);
        }


        [HttpPost]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "LoginWithFacebook")]
        public ActionResult LoginWithFacebook(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().FacebookNetwork);
        }
        
        [HttpPost]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "LoginWithGoogle")]
        public ActionResult LoginWithGoogle(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().GoogleNetwork);
        }

        private ActionResult SocialNetworkLogin(string id, string mail, string name, string networkType)
        {
            if (ContactRepository.SocialNetworkEmailAlreadyExists(mail, id, networkType))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetBySocialNetworkEmail(mail, id, networkType);
                return RedirectToAction("Index", "RendezVous");
            }
            Patient contact = new Patient()
            {
                Mail = mail,
                FullName = name,
                NetworkType = networkType,
                NetworkId = id
            };
            return PartialView("PhoneSubscription", contact);
        }

        public ActionResult PhoneSubscription(Patient patient)
        {
            patient.Profiles = new List<Profile>(){ ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
            ContactRepository.Save(patient);
            SessionHelper.GetInstance().CurrentUser = patient;
            return RedirectToAction("Index", "RendezVous");
        }
    }
}
