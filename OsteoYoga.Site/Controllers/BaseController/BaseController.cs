using System.Web.Mvc;

namespace OsteoYoga.Site.Controllers.BaseController
{
    public abstract class BaseController : Controller
    {
        protected _5.OsteoYoga.Exception.Implements.Logger Logger {get; set; }

        protected BaseController()
        {
        }
    }
}