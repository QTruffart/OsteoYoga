using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Site.Controllers.BaseController;

namespace OsteoYoga.WebSite.Controllers
{
    public class ContactController : BaseController
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
