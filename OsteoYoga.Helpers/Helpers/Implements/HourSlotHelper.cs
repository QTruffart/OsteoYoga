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
        public IList<FreeSlot> CalculateFreeHours(DateTime day, Duration duration, IList<Event> events)
        {
            DateTime begin = new DateTime(day.Year,day.Month,day.Day, 8,0,0);
            DateTime end = new DateTime(day.Year,day.Month,day.Day, 20,0,0);

            IList<FreeSlot> freeSlots = new List<FreeSlot>();

            IList<Event> eventsOrderded = events.OrderBy(e => e.Start.DateTime).ToList();
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

                if (indexCurrentElement == 0)
                {
                    if ((eventTime.Start.DateTime - begin).Value.TotalMinutes >= duration.Value)
                    {
                        freeSlots.Add(new FreeSlot()
                        {
                            Begin = begin,
                            End = eventTime.Start.DateTime.Value
                        });
                    }
                }
                else if (eventTime == eventsOrderded.Last())
                {
                    if (( end - eventTime.End.DateTime).Value.TotalMinutes >= duration.Value)
                    {
                        freeSlots.Add(new FreeSlot()
                        {
                            Begin = eventTime.End.DateTime.Value,
                            End = end
                        });
                    }
                }
                else
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
                            if (dateToInspect >= begin && dateToInspect <= end) return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
