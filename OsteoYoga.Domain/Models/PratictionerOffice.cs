using System;
using System.Collections.Generic;
using System.Linq;

namespace OsteoYoga.Domain.Models
{
    public class PratictionerOffice : Entity
    {
        public virtual Pratictioner Pratictioner { get; set; }
        public virtual Office Office { get; set; }
        public virtual int Reminder { get; set; }
        public virtual int DateWaiting { get; set; }
        public virtual int MinInterval { get; set; }
        public virtual int MaxInterval { get; set; }
        public virtual IList<Duration> Durations { get; set; }
        public virtual IList<WorkTimeSlot> TimeSlots { get; set; }
        public virtual IList<DefaultWorkDaysPO> DefaultWorkDaysPO { get; set; }
        
        public virtual IList<DateTime> GetWorkDaysBetweenIntervals(DateTime reference)
        {

            DateTime begin = new DateTime(reference.Year, reference.Month, reference.Day, 0, 0, 0).AddDays(MinInterval);
            DateTime end = new DateTime(reference.Year, reference.Month, reference.Day, 23, 59, 59).AddDays(MaxInterval);

            IList<DateTime> toReturn = new List<DateTime>();
            for (DateTime date = begin; date.Date <= end.Date; date = date.AddDays(1))
            {
                if (DefaultWorkDaysPO.Select(w => w.DefaultWorkDay.DayOfWeek()).Any(dayOfWeek => dayOfWeek == date.DayOfWeek))
                {
                    toReturn.Add(date);
                }
            }
            return toReturn;
        }
    }
}
