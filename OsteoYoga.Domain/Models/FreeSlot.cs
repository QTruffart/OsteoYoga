using System;

namespace OsteoYoga.Domain.Models
{
    public class FreeSlot : Entity
    {
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }
        public virtual PratictionerOffice Office { get; set; }

        public virtual double Duration => (End - Begin).TotalMinutes;
    }
}
