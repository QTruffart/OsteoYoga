using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using OsteoYoga.Display.Helpers;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.Holiday;

namespace OsteoYoga.Display.Controllers
{
    public class HolidayController : Controller
    {
        public Repository<Holiday> HolidayRepository { get; set; }

        public DateRepository DateRepository { get; set; }

        public HolidayController()
        {
            HolidayRepository = new Repository<Holiday>();
            DateRepository = new DateRepository();
        }

        public PartialViewResult ListOfHolidays()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            IList<Holiday> holidays = HolidayRepository.GetAll().OrderBy(h => h.Day).ToList();
            return PartialView("ListOfHolidays", holidays);
        }

        public PartialViewResult AddAnHoliday(DateTime dateTime, string hours)
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            IList<Date> datesOfTheDay = DateRepository.GetByDay(dateTime);
            TimeSpan beginHour = ToTimeSpan(hours.Split('-')[0]);
            TimeSpan endHour = ToTimeSpan(hours.Split('-')[1]);

            if (datesOfTheDay.Any(dod => IsContainedBy(beginHour,endHour,dod.TimeSlot.BeginHour, dod.TimeSlot.EndHour)))
            {
                ViewBag.Result = HolidayResource.DateTakenAtThisDate;
            }
            else
            {
                Holiday holiday = new Holiday { Day = dateTime, BeginHour = beginHour, EndHour = endHour};
                HolidayRepository.Save(holiday);
                ViewBag.Result = HolidayResource.HolidayTaken;
            }
            
            return PartialView("HolidayCreated");
        }

        private static TimeSpan ToTimeSpan(string begin)
        {
            return new TimeSpan(int.Parse(begin.Split(':')[0]), int.Parse(begin.Split(':')[1]), 0);
        }

        public PartialViewResult Index()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            return PartialView("Index");
        }

        public PartialViewResult CancelAnHoliday(int id)
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            Holiday holiday = HolidayRepository.GetById(id);
            HolidayRepository.Delete(holiday);
            ViewBag.Result = HolidayResource.HolidayCancelled;
            return PartialView("HolidayCancelled", holiday);
        }

        private bool IsContainedBy(TimeSpan timeSpanBegin, TimeSpan timeSpanEnd, TimeSpan toCompareBegin,
                                 TimeSpan toCompareEnd)
        {
            if (timeSpanBegin >= toCompareBegin && timeSpanBegin <= toCompareEnd)
            {
                return true;
            }
            if (timeSpanEnd >= toCompareBegin && timeSpanEnd <= toCompareEnd)
            {
                return true;
            }
            if (toCompareBegin >= timeSpanBegin && toCompareBegin <= timeSpanEnd)
            {
                return true;
            }
            if (toCompareEnd >= timeSpanBegin && toCompareEnd <= timeSpanEnd)
            {
                return true;
            }
            return false;
        }
    }
}