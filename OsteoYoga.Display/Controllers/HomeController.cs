using System.Web.Mvc;

namespace OsteoYoga.Display.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public PartialViewResult MeContacter()
        {
            return PartialView("MeContacter");
        }

        public PartialViewResult Yogathérapie()
        {
            return PartialView("Yogathérapie");
        }

        public PartialViewResult MaPratique()
        {
            return PartialView("MaPratique");
        }

        public PartialViewResult Osthéopathie()
        {
            return PartialView("Osthéopathie");
        }
        public PartialViewResult Partenaires()
        {
            return PartialView("Partenaires");
        }
        public PartialViewResult Tarifs()
        {
            return PartialView("Tarifs");
        }
        public PartialViewResult QuiSuisJe()
        {
            return PartialView("QuiSuisJe");
        }

        public PartialViewResult MotifsCourants(){
            return PartialView("MotifsCourants");
        }

        public PartialViewResult QuandConsulterBebe()
        {
            return PartialView("QuandConsulterBebe");
        }

        public PartialViewResult MotifsMeconnus()
        {
            return PartialView("MotifsMeconnus");
        }

        public PartialViewResult QuandConsulterGrossesse()
        {
            return PartialView("QuandConsulterGrossesse");
        }
    }
}
