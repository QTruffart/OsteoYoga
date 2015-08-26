using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.Contact;
using OsteoYoga.WebSite.Helpers;

namespace OsteoYoga.WebSite.Controllers
{
    public class LoginController : Controller
    {
        public ContactRepository ContactRepository { get; set; }
        public HolidayRepository HolidayRepository { get; set; }
        public LoginController()
        {
            ContactRepository = new ContactRepository();
            HolidayRepository = new HolidayRepository();
        }
        
        public PartialViewResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
            }
            return PartialView("Index");    
        }

        [HttpPost]
        public PartialViewResult Login(string email)
        {
            if (ContactRepository.EmailAlreadyExists(email))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetByEmail(email);
                return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
            }
            ViewBag.Errors = LoginResource.UnknownEmail;
            return PartialView("Index");
        }

        [HttpGet]
        public PartialViewResult SignIn()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
            }
            return PartialView("SignIn");    
        }

        [HttpPost]
        public PartialViewResult SignIn(Contact contact)
        {
            if (contact.IsValid())
            {
                if (!ContactRepository.EmailAlreadyExists(contact.Mail.Trim()))
                {
                    contact.ConfirmNumber = Guid.NewGuid();
                    contact.IsConfirmed = false;
                    ContactRepository.Save(contact);
                    //ViewBag.SigInNotification = "Un email vient de vous être envoyé. Vous devez suivre le lien présent dans ce mail afin de valider votre inscription";
                    SessionHelper.GetInstance().CurrentUser = contact;
                    return PartialView("/Views/RendezVous/Index.cshtml", GetHolidays());
                }
                ViewBag.SignInError = SignInResource.EmailAlreadyExists;
            }
            return PartialView("SignIn", contact);
        }


        [HttpPost]
        public PartialViewResult SignInWithFacebook(string id, string mail, string name)
        {
            if (ContactRepository.EmailAlreadyExists(mail))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetByEmail(mail);
                return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
            }
            Contact contact = new Contact()
            {
                Mail = mail,
                FullName = name,
                IsConfirmed = true
            };
            ContactRepository.Save(contact);
            SessionHelper.GetInstance().CurrentUser = contact;
            return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
        }


        [HttpPost]
        public PartialViewResult SignInWithGoogle(string id, string mail, string name)
        {
            if (ContactRepository.EmailAlreadyExists(mail))
            {
                SessionHelper.GetInstance().CurrentUser = ContactRepository.GetByEmail(mail);
                return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
            }
            Contact contact = new Contact()
            {
                Mail = mail,
                FullName = name,
                IsConfirmed = true
            };
            ContactRepository.Save(contact);
            SessionHelper.GetInstance().CurrentUser = contact;
            return PartialView("~/Views/RendezVous/Index.cshtml", GetHolidays());
        }

        private IList<Holiday> GetHolidays()
        {
            return HolidayRepository.GetFutureHoliday(
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            );
        }

    }
}
