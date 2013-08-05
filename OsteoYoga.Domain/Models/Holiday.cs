using System;
using OsteoYoga.Resource.Contact;
using OsteoYoga.Resource.Holiday;

namespace OsteoYoga.Domain.Models
{
    public class Holiday : Entity
    {
        public virtual DateTime Day { get; set; }
        public virtual TimeSpan BeginHour { get; set; }
        public virtual TimeSpan EndHour { get; set; }

        public override string ToString()
        {
            return Day.ToString("dd/MM/yyyy") + 
                   LoginResource.From +
                   new DateTime(BeginHour.Ticks).ToString("HH:mm") +
                   HolidayResource.To +
                   new DateTime(EndHour.Ticks).ToString("HH:mm");
        }
    }
}
