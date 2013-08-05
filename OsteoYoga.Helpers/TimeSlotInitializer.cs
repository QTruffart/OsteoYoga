using System;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace OsteoYoga.Helper
{
    public class TimeSlotInitializer
    {
        public TimeSlotInitializer(TimeSlotRepository repository)
        {
            Repository = repository;
        }

        private TimeSlotRepository Repository { get; set; }

        public void InitializeTimeSlots()
        {
            InitializeTimeSlotsForSaturday();
            InitializeTimeSlotsFor(DayOfWeek.Monday);
            InitializeTimeSlotsFor(DayOfWeek.Tuesday);    
            InitializeTimeSlotsFor(DayOfWeek.Thursday);    
            InitializeTimeSlotsFor(DayOfWeek.Friday);    
        }

        private void InitializeTimeSlotsFor(DayOfWeek day)
        {
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 09, 30, 0), EndHour = new TimeSpan(0, 10, 15, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 10, 15, 0), EndHour = new TimeSpan(0, 11, 00, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 11, 00, 0), EndHour = new TimeSpan(0, 11, 45, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 11, 45, 0), EndHour = new TimeSpan(0, 12, 30, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 13, 45, 0), EndHour = new TimeSpan(0, 14, 30, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 14, 30, 0), EndHour = new TimeSpan(0, 15, 15, 0), DayOfWeek = day });
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 15, 15, 0), EndHour = new TimeSpan(0, 16, 00, 0), DayOfWeek = day });
        }

        private void InitializeTimeSlotsForSaturday()
        {
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 09, 00, 0), EndHour = new TimeSpan(0, 09, 45, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 09, 45, 0), EndHour = new TimeSpan(0, 10, 30, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 10, 30, 0), EndHour = new TimeSpan(0, 11, 15, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 11, 15, 0), EndHour = new TimeSpan(0, 12, 00, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 12, 00, 0), EndHour = new TimeSpan(0, 12, 45, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 13, 45, 0), EndHour = new TimeSpan(0, 14, 30, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 14, 30, 0), EndHour = new TimeSpan(0, 15, 15, 0), DayOfWeek = DayOfWeek.Saturday});
            Repository.Save(new TimeSlot { BeginHour = new TimeSpan(0, 15, 15, 0), EndHour = new TimeSpan(0, 16, 00, 0), DayOfWeek = DayOfWeek.Saturday});
        }
    }
}
