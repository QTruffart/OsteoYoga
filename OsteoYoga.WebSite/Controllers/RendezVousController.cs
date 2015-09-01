using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Resource.RendezVous;

namespace OsteoYoga.WebSite.Controllers
{
    public class RendezVousController : Controller
    {
        public TimeSlotRepository SlotRepository { get; set; }
        public Repository<Contact> ContactRepository { get; set; }
        public DateRepository DateRepository { get; set; }
        public HolidayRepository HolidayRepository { get; set; }

        public RendezVousController()
        {
            SlotRepository = new TimeSlotRepository();
            ContactRepository = new Repository<Contact>();
            DateRepository = new DateRepository();
            HolidayRepository = new HolidayRepository();
        }

        [HttpGet]
        public PartialViewResult Index()
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                IList<Holiday> holidays = HolidayRepository.GetFutureHoliday(
                                                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                                        );
                return PartialView("Index", holidays);
            }
            return PartialView("/Views/Login/Index.cshtml");
        }

        [HttpPost]
        public PartialViewResult ProposeDate(DateTime patientDate, string patientHours)
        {
            if (SessionHelper.GetInstance().CurrentUser != null)
            {
                string patientBegin = patientHours.Split('-')[0];
                string patientEnd = patientHours.Split('-')[1];

                PatientHours patient = new PatientHours
                                         {
                                             BeginHour =  new TimeSpan(int.Parse(patientBegin.Split(':')[0]), int.Parse(patientBegin.Split(':')[1]) ,0),
                                             EndHour=  new TimeSpan(int.Parse(patientEnd.Split(':')[0]), int.Parse(patientEnd.Split(':')[1]) ,0),
                                             Date = patientDate
                                         };
                IList<TimeSlot> timeSlots = SlotRepository.GetFreeTimeSlots(patient.Date);
                timeSlots = timeSlots.Where(ts => ts.BeginHour >= patient.BeginHour && ts.EndHour <= patient.EndHour).ToList();
                if ( timeSlots.Count > 0)
                {
                    return PartialView("ProposeDate", timeSlots.Min());
                }
                ViewBag.ProposeError = DateResource.NoFreeTimeSlot;
                return PartialView("ProposeDate");
            }
            return PartialView("/Views/Login/Index.cshtml");
        }

        public PartialViewResult CreateDate(DateTime dateTime, int timeSlotId)
        {
            Contact contact = SessionHelper.GetInstance().CurrentUser;
            if (contact != null)
            {
                TimeSlot timeSlot = SlotRepository.GetById(timeSlotId);

                try
                {
                    Guid confirmationId = Guid.NewGuid();
                    Date date = new Date{
                                         Contact = contact,
                                         TimeSlot = timeSlot,
                                         Day = dateTime, 
                                         IsConfirmed = false,
                                         ConfirmationId = confirmationId.ToString()
                                    };
                    DateRepository.Save(date);
                    string serverAddress = Constants.GetInstance().ServerAddress(Request);
                    Email.GetInstance().SendForPatientPropose(date, serverAddress);
                    Email.GetInstance().SendForAdminPropose(date);
                    return PartialView("CreateDate", date);
                }
                catch (Exception ex)
                {
                    ViewBag.ResultMessage = DateResource.ErrorOccured;
                    return PartialView("CreateDate");
                }
            }
            return PartialView("/Views/Login/Index.cshtml");
        }

        [HttpGet]
        public PartialViewResult Validate(string id)
        {
            Date date = DateRepository.Validate(id);
            Email.GetInstance().SendForPatientValidation(date);
            Email.GetInstance().SendForAdminValidation(date);
            return PartialView("Validate", date);
        }

    }
}
