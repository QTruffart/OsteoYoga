using System.Web.Mvc;
using log4net.Config;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.WebSite.Controllers.BaseController
{
    public abstract class BaseController : Controller
    {
        protected Logger Logger{get; set; }

        protected BaseController()
        {
            Logger = new Logger(GetType());
            XmlConfigurator.Configure();
        }
    }
}