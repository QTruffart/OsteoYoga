using System;

namespace OsteoYoga.Domain.Models
{
    public class TimeSlot : Entity, IComparable
    {
        public virtual TimeSpan BeginHour { get; set; }
        
        public virtual TimeSpan EndHour { get; set; }

        public virtual DayOfWeek DayOfWeek { get; set; }

        //TODO tests
        public override string ToString()
        {
            string beginHour = BeginHour.Hours.ToString("D2") + "H" + BeginHour.Minutes.ToString("D2");
            string endHour = EndHour.Hours.ToString("D2") + "H" + EndHour.Minutes.ToString("D2");
            return string.Format("De {0} à {1}", beginHour, endHour);

        }

        public virtual int CompareTo(object obj)
        {
            TimeSlot toCompare = obj as TimeSlot;
            if (BeginHour <= toCompare.BeginHour)
            {
                return -1;
            }
            return 1;
        }
    }
}
