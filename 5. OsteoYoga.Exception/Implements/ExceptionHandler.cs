using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net.Config;
using _5.OsteoYoga.Exception.Interfaces;

namespace _5.OsteoYoga.Exception.Implements
{
    public class ExceptionHandler : HandleErrorAttribute, IExceptionHandler
    {
        public Logger Logger { get; set; }
        
        public override void OnException(ExceptionContext filterContext)
        {
            
            Logger = new Logger(filterContext.Controller.GetType(), false);
            System.Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            Logger.Error("Erreur non gérée sur l'action", ex, View);

            var model = new HandleErrorInfo(filterContext.Exception, filterContext.Controller.GetType().Name, "Error");
            
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}
