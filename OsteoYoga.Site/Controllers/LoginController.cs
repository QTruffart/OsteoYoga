using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Repository.DAO.Interfaces;
using OsteoYoga.Resource.Contact;
using OsteoYoga.Site.ViewResults;

namespace OsteoYoga.Site.Controllers
{
    public class LoginController : BaseController.BaseController
    {
        public ContactRepository ContactRepository { get; set; }
        public ProfileRepository ProfileRepository { get; set; }

        public LoginController()
        {
            ContactRepository = new ContactRepository();
            ProfileRepository = new ProfileRepository();
        }
        
        public ActionResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return RedirectToAction("Index", "RendezVous");
            }
            return PartialView("Index");    
        }
        

        [HttpPost]
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
        public ActionResult SignIn(Contact contact)
        {
            if (!ContactRepository.EmailAlreadyExists(contact.Mail))
            {
                contact.Profiles = new List<Profile>() { ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
                ContactRepository.Save(contact);
                SessionHelper.GetInstance().CurrentUser = contact;
                return RedirectToAction("Index", "RendezVous");
            }
            ViewBag.SignInError = SignInResource.EmailAlreadyExists;
            return PartialView("SignIn", contact);
        }


        [HttpPost]
        public ActionResult LoginWithFacebook(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().FacebookNetwork);
        }
        
        [HttpPost]
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
            Contact contact = new Contact
            {
                Mail = mail,
                FullName = name,
                NetworkType = networkType,
                NetworkId = id
            };
            return PartialView("PhoneSubscription", contact);
        }

        public ActionResult PhoneSubscription(Contact contact)
        {
            contact.Profiles = new List<Profile>(){ ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
            ContactRepository.Save(contact);
            SessionHelper.GetInstance().CurrentUser = contact;
            return RedirectToAction("Index", "RendezVous");
        }
    }
}
