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

        public virtual DateTime MinDateInterval => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(MinInterval);
        public virtual DateTime MaxDateInterval => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(MaxInterval);
    }
}
