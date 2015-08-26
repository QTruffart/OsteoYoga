using System;
using System.Web.Mvc;
using _5.OsteoYoga.Exception;

namespace OsteoYoga.WebSite.Controllers
{
    public class HomeController : BaseController.BaseController
    {
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }
        public ActionResult About()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult MeContacterABayonne()
        {
            try
            {
                return PartialView("MeContacterABayonne");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
            
        }
        public PartialViewResult MeContacterARionDesLandes()
        {
            try
            {
                return PartialView("MeContacterARionDesLandes");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult Yogathérapie()
        {
            try
            {
                return PartialView("Yogathérapie");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult MaPratique()
        {
            try
            {
                return PartialView("MaPratique");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult Osthéopathie()
        {
            try
            {
                return PartialView("Osthéopathie");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }
        public PartialViewResult Partenaires()
        {
            try
            {
                return PartialView("Partenaires");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }
        public PartialViewResult Tarifs()
        {
            try
            {
                return PartialView("Tarifs");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }
        public PartialViewResult QuiSuisJe()
        {
            try
            {
                return PartialView("QuiSuisJe");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult MotifsCourants()
        {
            try
            {
                return PartialView("MotifsCourants");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult QuandConsulterBebe()
        {
            try
            {
                return PartialView("QuandConsulterBebe");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult MotifsMeconnus()
        {
            try
            {
                return PartialView("MotifsMeconnus");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult QuandConsulterGrossesse()
        {
            try
            {
                return PartialView("QuandConsulterGrossesse");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult QuestionsFrequentes()
        {
            try
            {
                return PartialView("QuestionsFrequentes");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult Certifications()
        {
            try
            {
                return PartialView("Certifications");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult CoursYoga()
        {
            try
            {
                return PartialView("CoursYoga");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult LiensDivers()
        {
            try
            {
                return PartialView("LiensDivers");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }

        public PartialViewResult Association()
        {
            try
            {
                return PartialView("Association");
            }
            catch (Exception ex)
            {
                Logger.Error(ExceptionRes.ExceptionView, ex);
            }
            return PartialView("Error");
        }
    }
}
