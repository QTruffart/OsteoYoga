using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class DaySlotHelper: IDaySlotHelper
    {
        public IHourSlotHelper HourSlotHelper { get; set; }
        public IGoogleRepository<Event> GoogleRepository { get; set; }


        
        public DaySlotHelper()
        {
            HourSlotHelper = new HourSlotHelper();
            GoogleRepository = new GoogleRepository();
        }

        public IList<DateTime> CalculateFreeDays(PratictionerOffice pratictionerOffices, Duration expectedDuration, IList<DateTime> defaultDaysOnPeriod)
        {
            IList<Event> events = GoogleRepository.GetAllForPractionerInterval(pratictionerOffices);
            IList<DateTime> result = new List<DateTime>();
            foreach (DateTime dateTime in defaultDaysOnPeriod)
            {
                if (!HourSlotHelper.IsDuringAnAllDayEvent(events, dateTime))
                {
                    if (HourSlotHelper.CalculateFreeHours(dateTime, expectedDuration, events).Any())
                    {
                        result.Add(dateTime);
                    }
                }
            }
            return result;
        }

        public IList<DateTime> GetAllWorkDaysOnPeriod(PratictionerOffice pratictionerOffice, DateTime reference)
        {
            IList<DateTime> toReturn = new List<DateTime>();

            DateTime begin = new DateTime(reference.Year, reference.Month, reference.Day, 0, 0, 0).AddDays(pratictionerOffice.MinInterval);
            DateTime end = new DateTime(reference.Year, reference.Month, reference.Day, 23, 59, 59).AddDays(pratictionerOffice.MaxInterval);

            for (DateTime date = begin; date.Date <= end.Date; date = date.AddDays(1))
            {
                if (pratictionerOffice.DefaultWorkDaysPO.Select(w => w.DefaultWorkDay.DayOfWeek()).Any(dayOfWeek => dayOfWeek == date.DayOfWeek))
                {
                    toReturn.Add(date);
                }
            }
            return toReturn;
        }


        private bool EventAllDayOnThisDate(DateTime date, string eventDate)
        {
            string format = Constants.GetInstance().GoogleDateFormat;
            DateTime toReturn;
            if (DateTime.TryParseExact(eventDate, format, new CultureInfo(CultureInfo.CurrentCulture.Name),
                DateTimeStyles.AdjustToUniversal, out toReturn))
            {
                return toReturn.Date == date.Date;
            }
            return false;
        }
    }
}
