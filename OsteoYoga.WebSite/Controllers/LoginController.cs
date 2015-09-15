using System;
using System.Web.Mvc;
using System.Web.Routing;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.Contact;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.WebSite.Controllers
{
    public class LoginController : Controller
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

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Index")]
        public PartialViewResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }
            return PartialView("Index");    
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Login")]
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

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "SignIn")]
        [HttpPost]
        public PartialViewResult SignIn(Contact contact)
        {
            if (!ContactRepository.EmailAlreadyExists(contact.Mail))
            {
                contact.Profile = ProfileRepository.GetByName(Constants.GetInstance().PatientProfile);
                ContactRepository.Save(contact);
                SessionHelper.GetInstance().CurrentUser = contact;
                return PartialView("~/Views/RendezVous/Index.cshtml", OfficeRepository.GetAll());
            }
            ViewBag.SignInError = SignInResource.EmailAlreadyExists;
            return PartialView("SignIn", contact);
        }


        [ExceptionHandler(ExceptionType = typeof(Exception), View = "LoginWithFacebook")]
        [HttpPost]
        public ActionResult LoginWithFacebook(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().FacebookNetwork);
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "LoginWithGoogle")]
        [HttpPost]
        public ActionResult LoginWithGoogle(string id, string mail, string name)
        {
            return SocialNetworkLogin(id, mail, name, Constants.GetInstance().GoogleNetwork);
        }

        public PartialViewResult SocialNetworkLogin(string id, string mail, string name, string networkType)
        {
            if (ContactRepository.SocialNetworkEmailAlreadyExists(mail, id, networkType))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetBySocialNetworkEmail(mail, id, networkType);
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                Date date = new Date()
                {
                    Contact = SessionHelper.GetInstance().CurrentUser
                };
                
                //return new RedirectToRouteResult();  RedirectToAction("Index", "RendezVous",  new { date = date });
                return PartialView("~/Views/RendezVous/Index.cshtml", date);
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

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "PhoneSubscription")]
        [HttpPost]
        public ActionResult PhoneSubscription(Contact contact)
        {
            contact.Profile = ProfileRepository.GetByName(Constants.GetInstance().PatientProfile);
            ContactRepository.Save(contact);
            SessionHelper.GetInstance().CurrentUser = contact;
            Date date = new Date()
            {
                Contact = contact
            };
            return RedirectToAction("Index", "RendezVous", new {date = date});
        }
    }
}
