using System.Web.Mvc;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Site.Controllers
{
    public class ContactController : BaseController.BaseController
    {
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Create(Contact contact)
        {
            return View();
        }
    }
}
