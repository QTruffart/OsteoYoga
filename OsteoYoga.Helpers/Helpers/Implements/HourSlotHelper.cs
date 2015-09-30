﻿using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class HourSlotHelper : IHourSlotHelper
    {
        public HourSlotHelper()
        {
        }

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

        private bool InclusiveDays(long s1, long e1, long s2, long e2)
        {
            if (e1 < e2 && s1 > s2) return true;
            if (e1 < e2 && s1 > e2) return true;
            if (e1 < s2 && s1 > s2) return true;
            if (e1 > e2 && s1 < s2) return true;
            return false;
        }
    }
}
