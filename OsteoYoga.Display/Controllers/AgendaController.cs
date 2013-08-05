using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Display.Helpers;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Display.Controllers
{
    public class AgendaController : Controller
    {
        //
        // GET: /Index/
        public DateRepository DateRepository { get; set; }

        public HolidayRepository HolidayRepository { get; set; }

        public AgendaController()
        {
            DateRepository = new DateRepository();
            HolidayRepository = new HolidayRepository();
        }

        public PartialViewResult Index()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            IList<Date> dates = DateRepository.GetFutureDates(date.AddDays(-3));
            ViewBag.Holidays = HolidayRepository.GetFutureHoliday(date);
            return PartialView("Index", dates);
        }

        public PartialViewResult GetDetailDate(int id)
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            Date date = DateRepository.GetById(id);
            return PartialView("GetDetailDate", date);
        }

        public ActionResult DeleteDate(int id)
        {
            Date date = DateRepository.GetById(id);
            DateRepository.Delete(date);
            return new EmptyResult();
        }

    }
}
