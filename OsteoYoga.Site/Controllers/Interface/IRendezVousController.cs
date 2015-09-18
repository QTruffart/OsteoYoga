using System.Web.Mvc;
using OsteoYoga.Site.ViewResults;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.Site.Controllers.Interface
{
    public interface IRendezVousController
    {
        PartialViewResult Index(DateViewResult dateViewResult);
    }
}