using System;

namespace OsteoYoga.Domain.Models
{
    public class PratictionerPreference : Entity
    {
        public virtual Contact Contact { get; set; }
        public virtual int Reminder { get; set; }
        public virtual int DateWaiting { get; set; }
        public virtual int MinInterval { get; set; }
        public virtual int MaxInterval { get; set; }

        public DateTime MinDateInterval
        {
            get
            {
                DateTime now = DateTime.Now.AddDays(MinInterval);
                return  new DateTime(now.Year, now.Month, now.Day);
            } 
        }

        public DateTime MaxDateInterval
        {
            get
            {
                DateTime now = DateTime.Now.AddDays(MaxInterval);
                return  new DateTime(now.Year, now.Month, now.Day);
            } 
        }
    }
}
