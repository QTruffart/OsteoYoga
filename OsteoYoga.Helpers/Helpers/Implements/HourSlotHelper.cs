using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class HourSlotHelper : IHourSlotHelper
    {
        public IList<FreeSlot> CalculateFreeHours(DateTime day, Duration duration, IList<Event> eventsOnTheDay)
        {
            DateTime begin = new DateTime(day.Year,day.Month,day.Day, 8,0,0);
            DateTime end = new DateTime(day.Year,day.Month,day.Day, 20,0,0);

            IList<FreeSlot> freeSlots = new List<FreeSlot>();
            eventsOnTheDay.Add(new Event() {Start = new EventDateTime() {DateTime = begin }, End = new EventDateTime() { DateTime = begin } });
            eventsOnTheDay.Add(new Event() {Start = new EventDateTime() {DateTime = end }, End = new EventDateTime() { DateTime = end } });
            IList<Event> eventsOrderded = eventsOnTheDay.OrderBy(e => e.Start.DateTime).ToList();


            if (!eventsOrderded.Any())
            {
                freeSlots.Add(new FreeSlot()
                {
                    Begin = begin,
                    End = end
                });
            }
            foreach (Event eventTime in eventsOrderded)
            {
                int indexCurrentElement = eventsOrderded.IndexOf(eventTime);
                if (eventTime != eventsOrderded.Last())
                {
                    Event nextEvent = eventsOrderded[indexCurrentElement + 1];

                    if ((nextEvent.Start.DateTime - eventTime.End.DateTime).Value.TotalMinutes >= duration.Value)
                    {
                        freeSlots.Add(new FreeSlot()
                        {
                            Begin = eventTime.End.DateTime.Value,
                            End = nextEvent.Start.DateTime.Value
                        });
                    }
                }
            }
            return freeSlots;
        }

        public bool IsDuringAnAllDayEvent(IList<Event> events, DateTime dateToInspect)
        {
            foreach (Event @event in events)
            {
                if (!string.IsNullOrEmpty(@event.Start.Date) && !string.IsNullOrEmpty(@event.End.Date))
                {
                    DateTime begin;
                    if (DateTime.TryParseExact(@event.Start.Date, "yyyy-MM-dd", new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal, out begin))
                    {
                        DateTime end;
                        if (DateTime.TryParseExact(@event.End.Date, "yyyy-MM-dd", new DateTimeFormatInfo(), DateTimeStyles.AdjustToUniversal, out end))
                        {
                            if (dateToInspect >= begin && dateToInspect < end) return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
