using System;

namespace OsteoYoga.Domain.Models
{
    public class WorkTimeSlot : Entity
    {
        public virtual DateTime BeginTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        //public virtual PratictionerPreference PratictionerPreference { get; set; }
    }
}
