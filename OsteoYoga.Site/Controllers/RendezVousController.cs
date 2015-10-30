using System;
using System.Collections.Generic;
using System.Globalization;
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
        public IDaySlotHelper DaySlotHelper { get; set; }
        public IPratictionerOfficeRepository PratictionerOfficeRepository { get; set; }

        public RendezVousController()
        {
            OfficeRepository = new OfficeRepository();
            PratictionerOfficeRepository = new PratictionerOfficeRepository();
            DurationRepository = new DurationRepository();
            GoogleRepository = new GoogleRepository();
            DaySlotHelper = new DaySlotHelper();
        }

        [HttpGet]
        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "RendezVous")]
        public PartialViewResult Index()
        {
            DateViewResult model = new DateViewResult()
            {
                Offices = OfficeRepository.GetAll()
            };

            return PartialView("Index", model);
        }

        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Pratictioners")]
        public JsonResult Pratictioners(int officeId)
        {
            IList<Pratictioner> pratictioners = OfficeRepository.GetById(officeId).Pratictioners;
            IList<DropDowJsonViewResult> result = pratictioners.Select(p => new DropDowJsonViewResult() {Id = p.Id, Name = p.FullName}).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "Durations")]
        public JsonResult Durations(int officeId, int pratictionerId)
        {
            IList<Duration> durations = PratictionerOfficeRepository.GetByOfficeIdAndPratictionerId(officeId, pratictionerId).Durations;
            IList<DropDowJsonViewResult> result = durations.Select(p => new DropDowJsonViewResult() { Id = p.Id, Name = p.Value.ToString() }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "FreeDays")]
        public JsonResult FreeDays(int officeId, int pratictionerId, int durationId)
        {
            PratictionerOffice pratictionerOffice = PratictionerOfficeRepository.GetByOfficeIdAndPratictionerId(officeId, pratictionerId);
            Duration duration = pratictionerOffice.Durations.FirstOrDefault(d => d.Id == durationId);

            IList<DateTime> workDaysOnPeriod = DaySlotHelper.GetAllWorkDaysOnPeriod(pratictionerOffice, DateTime.Now);
            IList<DateTime> freeDays = DaySlotHelper.CalculateFreeDays(pratictionerOffice, duration, workDaysOnPeriod);

            IList<FreeDayJsonViewResult> result = freeDays.Select(d => new FreeDayJsonViewResult {FreeDay = d}).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PatientProfile]
        [ExceptionHandler(ExceptionType = typeof(Exception), View = "FreeSlots")]
        public JsonResult FreeSlots(string choosedDay, int officeId, int pratictionerId, int durationId)
        {
            DateTime choosedDayParsed;
            if(DateTime.TryParseExact(choosedDay, "dd/MM/yyyy", new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal, out choosedDayParsed))
            {
                PratictionerOffice pratictionerOffice = PratictionerOfficeRepository.GetByOfficeIdAndPratictionerId(officeId, pratictionerId);
                Duration duration = pratictionerOffice.Durations.FirstOrDefault(d => d.Id == durationId);
                IList<FreeSlot> freeSlots = DaySlotHelper.GetAllFreeSlotOnADay(pratictionerOffice, duration, choosedDayParsed);


                IList<FreeSlotJsonViewResult> freeSlotJsonViewResults = freeSlots.Select(
                    f => new FreeSlotJsonViewResult
                    {
                        FreeSlotBegin = f.Begin,
                        FreeSlotEnd = f.End
                    }).ToList();
                return Json(freeSlotJsonViewResults, JsonRequestBehavior.AllowGet);
            }
            throw new Exception("Mauvais format de date");
        }
    }
}
