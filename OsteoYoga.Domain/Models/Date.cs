using System;

namespace OsteoYoga.Domain.Models
{
    public class Date : Entity
    {
        public virtual TimeSlot TimeSlot { get; set; }
        public virtual DateTime Day { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual string ConfirmationId { get; set; }
        public override string ToString()
        {
            return Day.ToString("dd/MM/yyyy");

        }
    }
}
