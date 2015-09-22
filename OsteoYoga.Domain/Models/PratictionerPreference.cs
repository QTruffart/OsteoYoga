using System;
using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class PratictionerPreference : Entity
    {
        public virtual Pratictioner Pratictioner { get; set; }
        public virtual Office Office { get; set; }
        public virtual int Reminder { get; set; }
        public virtual int DateWaiting { get; set; }
        public virtual int MinInterval { get; set; }
        public virtual int MaxInterval { get; set; }
        public virtual IList<Duration> Durations { get; set; }
        public virtual IList<WorkTimeSlot> TimeSlots { get; set; }


        public virtual DateTime MinDateInterval
        {
            get
            {
                DateTime now = DateTime.Now.AddDays(MinInterval);
                return  new DateTime(now.Year, now.Month, now.Day);
            } 
        }

        public virtual DateTime MaxDateInterval
        {
            get
            {
                DateTime now = DateTime.Now.AddDays(MaxInterval);
                return  new DateTime(now.Year, now.Month, now.Day);
            } 
        }

        
    }
}
