using System;

namespace OsteoYoga.Domain.Models
{
    public class DefaultWorkDaysPO : Entity
    {
        public virtual DefaultWorkDay DefaultWorkDay { get; set; }
        public virtual PratictionerOffice PratictionerOffice { get; set; }
        public virtual DateTime BeginTime { get; set; }
        public virtual DateTime EndTime { get; set; }
    }
}
