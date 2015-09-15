using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.WebSite.Controllers
{
    public class AdministrationController : BaseController.BaseController
    {
        public ContactRepository ContactRepository { get; set; }


        public AdministrationController()
        {
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