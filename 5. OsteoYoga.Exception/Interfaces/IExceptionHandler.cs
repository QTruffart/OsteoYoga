using System.Web.Mvc;
using _5.OsteoYoga.Exception.Implements;

namespace _5.OsteoYoga.Exception.Interfaces
{
    public interface IExceptionHandler
    {
        Logger Logger { get; set; }
    }
}
