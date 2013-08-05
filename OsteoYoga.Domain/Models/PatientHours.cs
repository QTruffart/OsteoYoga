using System;

namespace OsteoYoga.Domain.Models
{
    public class PatientHours
    {
        public DateTime Date { get; set; }

        public TimeSpan BeginHour { get; set; }

        public TimeSpan EndHour { get; set; }
    }
}