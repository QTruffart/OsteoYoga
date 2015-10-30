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
            IList<Event> events = GoogleRepository.GetAllForInterval(pratictionerOffices.MinDateInterval, pratictionerOffices.MaxDateInterval);
            IList<Event> eventsWithoutAllDay = events.Where(e => string.IsNullOrEmpty(e.Start.Date) && string.IsNullOrEmpty(e.End.Date)).ToList();

            IList<DateTime> result = new List<DateTime>();
            foreach (DateTime dateTime in defaultDaysOnPeriod)
            {
                if (!HourSlotHelper.IsDuringAnAllDayEvent(events, dateTime))
                {
                    IList<Event> eventsOnTheDay = eventsWithoutAllDay.Where(e => e.Start.DateTime.Value.Date == dateTime.Date).ToList();
                    if (HourSlotHelper.CalculateFreeHours(dateTime, expectedDuration, eventsOnTheDay).Any())
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

        public IList<FreeSlot> GetAllFreeSlotOnADay(PratictionerOffice pratictionerOffices, Duration duration, DateTime date)
        {
            IList<Event> events = GoogleRepository.GetAllForInterval(
                new DateTime(date.Year,date.Month,date.Day, 0, 0, 0), 
                new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
            );

            IList<FreeSlot> toSplit = HourSlotHelper.CalculateFreeHours(date, duration, events);
            IList<FreeSlot> toReturn = new List<FreeSlot>();
            foreach (FreeSlot freeSlot in toSplit)
            {
                int count = 0;
                while (count + duration.Value <= freeSlot.Duration)
                {
                    toReturn.Add(new FreeSlot()
                    {
                        Begin = new DateTime(date.Year, date.Month, date.Day, freeSlot.Begin.Hour, freeSlot.Begin.Minute, 0).AddMinutes(count),
                        End = new DateTime(date.Year, date.Month, date.Day, freeSlot.Begin.Hour, freeSlot.Begin.Minute, 0).AddMinutes(count + duration.Value),
                    });
                    count += duration.Value;
                }
            }
            return toReturn;
        }
    }
}