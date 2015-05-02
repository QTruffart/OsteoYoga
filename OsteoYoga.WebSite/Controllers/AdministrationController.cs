using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Repository.DAO;
using OsteoYoga.WebSite.Helpers;

namespace OsteoYoga.WebSite.Controllers
{
    public class AdministrationController : Controller
    {
        public TimeSlotRepository TimeSlotRepository { get; set; }
        public ContactRepository ContactRepository { get; set; }


        public AdministrationController()
        {
            TimeSlotRepository = new TimeSlotRepository();
            ContactRepository = new ContactRepository();
        }

        public PartialViewResult Index()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("PassAdmin");
            }
            return PartialView();
        }

        public ActionResult InitializeTimeSlot()
        {
            TimeSlotRepository.DeleteAll();
            TimeSlotInitializer timeSlotInitializer = new TimeSlotInitializer(TimeSlotRepository);
            timeSlotInitializer.InitializeTimeSlots();
            return null;
        }

        

        public PartialViewResult UserList()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("PassAdmin");
            }
            IList<Contact> contacts = ContactRepository.GetAll();
            return PartialView("UserList", contacts);
        }

        public ViewResult PassAdmin(string password)
        {
            if (password != null)
            {

                if (Constants.GetInstance().PassAdmin == password.Trim())
                {
                    SessionHelper.GetInstance().AdminConnected = true;
                    return View("Index");
                }
                @ViewBag.Errors = "Le mot de passe est incorrect";
                return View("PassAdmin");   
            }
            return View("Index");
        }
    }
}