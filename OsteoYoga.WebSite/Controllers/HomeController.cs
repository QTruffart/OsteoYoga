using System;
using System.Web.Mvc;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.WebSite.Controllers
{
    public class HomeController : BaseController.BaseController
    {
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "About")]
        public ActionResult About()
        {
            return View();
        }

        [ExceptionHandler(ExceptionType = typeof (Exception), View = "MeContacterABayonne")]
        public PartialViewResult MeContacterABayonne()
        {
            return PartialView("MeContacterABayonne");
        }

        [ExceptionHandler(ExceptionType = typeof (Exception), View = "MeContacterARionDesLandes")]
        public PartialViewResult MeContacterARionDesLandes()
        {
            return PartialView("MeContacterARionDesLandes");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Yogathérapie")]
        public PartialViewResult Yogathérapie()
        {
            return PartialView("Yogathérapie");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "MaPratique")]
        public PartialViewResult MaPratique()
        {
            return PartialView("MaPratique");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Osthéopathie")]
        public PartialViewResult Osthéopathie()
        {
            return PartialView("Osthéopathie");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Partenaires")]
        public PartialViewResult Partenaires()
        {
            return PartialView("Partenaires");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Tarifs")]
        public PartialViewResult Tarifs()
        {
            return PartialView("Tarifs");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "QuiSuisJe")]
        public PartialViewResult QuiSuisJe()
        {
            return PartialView("QuiSuisJe");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "MotifsCourants")]
        public PartialViewResult MotifsCourants()
        {
            return PartialView("MotifsCourants");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "QuandConsulterBebe")]
        public PartialViewResult QuandConsulterBebe()
        {
            return PartialView("QuandConsulterBebe");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "MotifsMeconnus")]
        public PartialViewResult MotifsMeconnus()
        {
            return PartialView("MotifsMeconnus");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "QuandConsulterGrossesse")]
        public PartialViewResult QuandConsulterGrossesse()
        {
            return PartialView("QuandConsulterGrossesse");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "QuestionsFrequentes")]
        public PartialViewResult QuestionsFrequentes()
        {
            return PartialView("QuestionsFrequentes");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Certifications")]
        public PartialViewResult Certifications()
        {
            return PartialView("Certifications");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "CoursYoga")]
        public PartialViewResult CoursYoga()
        {
            return PartialView("CoursYoga");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "LiensDivers")]
        public PartialViewResult LiensDivers()
        {
            return PartialView("LiensDivers");
        }

        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Association")]
        public PartialViewResult Association()
        {
            return PartialView("Association");
        }
    }
}