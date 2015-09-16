using _5.OsteoYoga.Exception.Implements;
using IController = OsteoYoga.Site.Controllers.Interface.IController;

namespace OsteoYoga.Site.Controllers.BaseController
{
    public abstract class BaseController : System.Web.Mvc.Controller, Interface.IController
    {
        public Logger Logger {get; set; }
    }
}