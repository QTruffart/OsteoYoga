using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Helper.Profile;
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
        public IPratictionerOfficeRepository PratictionerOfficeRepository { get; set; }

        public RendezVousController()
        {
            OfficeRepository = new OfficeRepository();
            PratictionerOfficeRepository = new PratictionerOfficeRepository();
            DurationRepository = new DurationRepository();
            GoogleRepository = new GoogleRepository();
            FreeSlotHelper = new FreeSlotHelper();
        }

        [HttpGet]
        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "RendezVous")]
        public PartialViewResult Index()
        {
            ////TODO : A gérer en base avec praticien courant

            //IList<Event> events = GoogleRepository.GetAllForPractionerInterval(office);

            DateViewResult model = new DateViewResult()
            {
                Offices = OfficeRepository.GetAll()
            };

            return PartialView("Index", model);
        }

        [PatientProfile]
        public JsonResult Pratictioners(int officeId)
        {
            IList<Pratictioner> pratictioners = OfficeRepository.GetById(officeId).Pratictioners;
            IList<DropDowJsonViewResult> result = pratictioners.Select(p => new DropDowJsonViewResult() {Id = p.Id, Name = p.FullName}).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PatientProfile]
        public JsonResult Durations(int officeId, int pratictionerId)
        {
            IList<Duration> durations = PratictionerOfficeRepository.GetByOfficeIdAndPratictionerId(officeId, pratictionerId).Durations;
            IList<DropDowJsonViewResult> result = durations.Select(p => new DropDowJsonViewResult() { Id = p.Id, Name = p.Value.ToString() }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[PatientProfile]
        //[AdministratorProfile]
        //[ExceptionHandler(ExceptionType = typeof(Exception), View = "ProposeDate")]
        //public PartialViewResult ProposeDate(Date date)
        //{



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
        //    return PartialView("/Views/Login/Index.cshtml");
        //}

        //[PatientProfile]
        //[AdministratorProfile]
        //[ExceptionHandler(ExceptionType = typeof(Exception), View = "CreateDate")]
        //public PartialViewResult CreateDate(DateTime dateTime, int timeSlotId)
        //{
        //    //Patient contact = SessionHelper.GetInstance().CurrentUser;
        //    //if (contact != null)
        //    //{
        //    //    WorkTimeSlot timeSlot = SlotRepository.GetById(timeSlotId);

        //    //    try
        //    //    {
        //    //        Guid confirmationId = Guid.NewGuid();
        //    //        Dates date = new Dates{
        //    //                             Patient = contact,
        //    //                             WorkTimeSlot = timeSlot,
        //    //                             Day = dateTime, 
        //    //                             IsConfirmed = false,
        //    //                             ConfirmationId = confirmationId.ToString()
        //    //                        };
        //    //        DateRepository.Save(date);
        //    //        string serverAddress = Constants.GetInstance().ServerAddress(Request);
        //    //        Email.GetInstance().SendForPatientPropose(date, serverAddress);
        //    //        Email.GetInstance().SendForAdminPropose(date);
        //    //        return PartialView("CreateDate", date);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        ViewBag.ResultMessage = DateResource.ErrorOccured;
        //    //        return PartialView("CreateDate");
        //    //    }
        //    //}
        //    return PartialView("/Views/Login/Index.cshtml");
        //}

        //[HttpGet]
        //public PartialViewResult Validate(string id)
        //{
        //    //Dates date = DateRepository.Validate(id);
        //    //Email.GetInstance().SendForPatientValidation(date);
        //    //Email.GetInstance().SendForAdminValidation(date);
        //    return PartialView("Validate");
        //}

    }
}
