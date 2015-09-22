using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Helper.Profile;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Implements;
using OsteoYoga.Repository.DAO.Interfaces;
using OsteoYoga.Site.Controllers.Interface;
using OsteoYoga.Site.ViewResults;
using _5.OsteoYoga.Exception.Implements;

namespace OsteoYoga.Site.Controllers
{
    public class RendezVousController : BaseController.BaseController , IRendezVousController
    {
        public IOfficeRepository OfficeRepository { get; set; }
        public IRepository<Duration> DurationRepository { get; set; }
        public IGoogleRepository<Event> GoogleRepository { get; set; }
        public IFreeSlotHelper FreeSlotHelper { get; set; }

        public RendezVousController()
        {
            OfficeRepository = new OfficeRepository();
            DurationRepository = new DurationRepository();
            GoogleRepository = new GoogleRepository();
            FreeSlotHelper = new FreeSlotHelper();
        }

        [HttpGet]
        [PatientProfile]
        [AdministratorProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "RendezVous")]
        public PartialViewResult Index()
        {
            //TODO : A gérer en base avec praticien courant
            PratictionerPreference preference = new PratictionerPreference()
            {
                DateWaiting = 5,
                MaxInterval = 30,
                MinInterval = 3,
                Reminder = 30
            };
            IList<Event> events = GoogleRepository.GetAllForPractionerInterval(preference);

            DateViewResult model = new DateViewResult()
            {
                Offices = OfficeRepository.GetAll(),
                Durations = DurationRepository.GetAll(),
                FreeSlots = FreeSlotHelper.CalculateFreeSlotBetweenTwoDays(events, preference)
            };

            return PartialView("Index", model);
        }

        [HttpGet]
        [PatientProfile]
        [AdministratorProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "RendezVous")]
        public PartialViewResult Index(DateViewResult dateViewResult)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [PatientProfile]
        [AdministratorProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "ProposeDate")]
        public PartialViewResult ProposeDate(Date date)
        {



            //if (SessionHelper.GetInstance().CurrentUser != null)
            //{
            //    string patientBegin = patientHours.Split('-')[0];
            //    string patientEnd = patientHours.Split('-')[1];

            //    PatientHours patient = new PatientHours
            //                             {
            //                                 BeginHour =  new TimeSpan(int.Parse(patientBegin.Split(':')[0]), int.Parse(patientBegin.Split(':')[1]) ,0),
            //                                 EndHour=  new TimeSpan(int.Parse(patientEnd.Split(':')[0]), int.Parse(patientEnd.Split(':')[1]) ,0),
            //                                 Dates = patientDate
            //                             };
            //    IList<WorkTimeSlot> timeSlots = SlotRepository.GetFreeTimeSlots(patient.Dates);
            //    timeSlots = timeSlots.Where(ts => ts.BeginHour >= patient.BeginHour && ts.EndHour <= patient.EndHour).ToList();
            //    if ( timeSlots.Count > 0)
            //    {
            //        return PartialView("ProposeDate", timeSlots.Min());
            //    }
            //    ViewBag.ProposeError = DateResource.NoFreeTimeSlot;
            //    return PartialView("ProposeDate");
            //}
            return PartialView("/Views/Login/Index.cshtml");
        }

        [PatientProfile]
        [AdministratorProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "CreateDate")]
        public PartialViewResult CreateDate(DateTime dateTime, int timeSlotId)
        {
            //Patient contact = SessionHelper.GetInstance().CurrentUser;
            //if (contact != null)
            //{
            //    WorkTimeSlot timeSlot = SlotRepository.GetById(timeSlotId);

            //    try
            //    {
            //        Guid confirmationId = Guid.NewGuid();
            //        Dates date = new Dates{
            //                             Patient = contact,
            //                             WorkTimeSlot = timeSlot,
            //                             Day = dateTime, 
            //                             IsConfirmed = false,
            //                             ConfirmationId = confirmationId.ToString()
            //                        };
            //        DateRepository.Save(date);
            //        string serverAddress = Constants.GetInstance().ServerAddress(Request);
            //        Email.GetInstance().SendForPatientPropose(date, serverAddress);
            //        Email.GetInstance().SendForAdminPropose(date);
            //        return PartialView("CreateDate", date);
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.ResultMessage = DateResource.ErrorOccured;
            //        return PartialView("CreateDate");
            //    }
            //}
            return PartialView("/Views/Login/Index.cshtml");
        }

        [HttpGet]
        public PartialViewResult Validate(string id)
        {
            //Dates date = DateRepository.Validate(id);
            //Email.GetInstance().SendForPatientValidation(date);
            //Email.GetInstance().SendForAdminValidation(date);
            return PartialView("Validate");
        }

    }
}
