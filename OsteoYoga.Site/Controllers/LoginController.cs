using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.Contact;
using OsteoYoga.Site.Controllers.BaseController;

namespace OsteoYoga.WebSite.Controllers
{
    public class LoginController : BaseController
    {
        public ContactRepository ContactRepository { get; set; }
        public OfficeRepository OfficeRepository { get; set; }
        public ProfileRepository ProfileRepository { get; set; }

        public LoginController()
        {
            ContactRepository = new ContactRepository();
            OfficeRepository = new OfficeRepository();
            ProfileRepository = new ProfileRepository();
        }
        
        public PartialViewResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }
            return PartialView("Index");    
        }

        [HttpPost]
        public PartialViewResult Login(string email)
        {
            if (ContactRepository.EmailAlreadyExists(email))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetByEmail(email);
                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }

            ViewBag.Errors = LoginResource.UnknownEmail;
            return PartialView("Index", email);
        }

        [HttpPost]
        public PartialViewResult SignIn(Contact contact)
        {
            if (!ContactRepository.EmailAlreadyExists(contact.Mail))
            {
                contact.Profiles = new List<Profile>() { ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
                ContactRepository.Save(contact);
                SessionHelper.GetInstance().CurrentUser = contact;
                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }
            ViewBag.SignInError = SignInResource.EmailAlreadyExists;
            return PartialView("SignIn", contact);
        }


        [HttpPost]
        public PartialViewResult LoginWithFacebook(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().FacebookNetwork);
        }
        
        [HttpPost]
        public PartialViewResult LoginWithGoogle(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().GoogleNetwork);
        }

        private PartialViewResult SocialNetworkLogin(string id, string mail, string name, string networkType)
        {
            if (ContactRepository.SocialNetworkEmailAlreadyExists(mail, id, networkType))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetBySocialNetworkEmail(mail, id, networkType);

                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }
            Contact contact = new Contact()
            {
                Mail = mail,
                FullName = name,
                NetworkType = networkType,
                NetworkId = id
            };
            return PartialView("PhoneSubscription", contact);
        }

        public PartialViewResult PhoneSubscription(Contact contact)
        {
            contact.Profiles = new List<Profile>(){ ProfileRepository.GetByName(Constants.GetInstance().PatientProfile) };
            ContactRepository.Save(contact);
            SessionHelper.GetInstance().CurrentUser = contact;
            return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
        }
    }
}
