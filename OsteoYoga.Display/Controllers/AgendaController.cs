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
        public TimeSlotRepository TimeSlotRepository { get; set; }
        public ContactRepository ContactRepository { get; set; }

        public AgendaController()
        {
            DateRepository = new DateRepository();
            HolidayRepository = new HolidayRepository();
            TimeSlotRepository = new TimeSlotRepository();
            ContactRepository = new ContactRepository();
        }

        public PartialViewResult Index()
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            IList<Date> dates = /*new List<Date>();*/DateRepository.GetFutureDates(date.AddDays(-3));
            ViewBag.Holidays = /* new List<Holiday>();*/ HolidayRepository.GetFutureHoliday(date);
            return PartialView("Index", dates);
        }

        [HttpGet]
        public PartialViewResult GetDetailDate(int id)
        {
            if (!SessionHelper.GetInstance().AdminConnected)
            {
                return PartialView("/Views/Administration/PassAdmin.cshtml");
            }
            Date date = DateRepository.GetById(id);
            return PartialView("GetDetailDate", date);
        }

        [HttpPost]
        public bool DeleteDate(int id)
        {
            Date date = DateRepository.GetById(id);
            DateRepository.Delete(date);
            return false;
        }

        [HttpGet]
        public PartialViewResult CreateDate()
        {
            return PartialView("CreateDate", ContactRepository.GetAll());
        }


        [HttpPost]
        public Date CreateDate(int timeSlotId, int contactId, DateTime datetime)
        {
            TimeSlot timeSlot = TimeSlotRepository.GetById(timeSlotId);
            Contact contact = ContactRepository.GetById(contactId);

            Date date = new Date{
                                      ConfirmationId = Guid.NewGuid().ToString(),
                                      Contact = contact,
                                      TimeSlot = timeSlot,
                                      Day = datetime,
                                      IsConfirmed = true
                                  };
            DateRepository.Save(date);
            return date;
        }
        
        public PartialViewResult GetTimeSlotsForADay(DateTime dateTime){
            IList<TimeSlot> slots = TimeSlotRepository.GetFreeTimeSlots(dateTime);
            return PartialView("GetTimeSlotsForADay", slots);
        }
    }
}
