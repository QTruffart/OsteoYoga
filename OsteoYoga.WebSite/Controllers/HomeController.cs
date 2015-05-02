using System.Web.Mvc;
using log4net;
using log4net.Config;

namespace OsteoYoga.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public PartialViewResult MeContacterABayonne()
        {
            XmlConfigurator.Configure();
            log.Error("message");
            return PartialView("MeContacterABayonne");
        }
        public PartialViewResult MeContacterARionDesLandes()
        {

            return PartialView("MeContacterARionDesLandes");
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

        public PartialViewResult QuestionsFrequentes()
        {
            return PartialView("QuestionsFrequentes");
        }

        public ActionResult Certifications()
        {
            return PartialView("Certifications");
        }

        public ActionResult CoursYoga()
        {
            return PartialView("CoursYoga");
        }

        public ActionResult LiensDivers()
        {
            return PartialView("LiensDivers");
        }
    }
}
