using System;
using System.Collections.Generic;
using NHibernate.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class TimeSlotRepository : Repository<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository()
        {
            HolidayRepository = new HolidayRepository();
        }

        public virtual IList<TimeSlot> GetFreeTimeSlots(DateTime day)
        {
            IList<TimeSlot> timeSlots = new List<TimeSlot>();
            IList<TimeSlot> occupedTimeSlotByDate = Session.QueryOver<Date>().Where(d => d.Day == day).Select(d => d.TimeSlot).List<TimeSlot>();
            timeSlots = Session.QueryOver<TimeSlot>().Where(ts => ts.DayOfWeek == day.DayOfWeek).List<TimeSlot>();
            occupedTimeSlotByDate.ForEach(ts => timeSlots.Remove(ts));
            if (timeSlots.Count > 0)
            {
                IList<TimeSlot> occupedTimeSlotByHoliday = new List<TimeSlot>();
                foreach (TimeSlot timeSlot in timeSlots)
                {
                    if (HolidayRepository.ThereIsAnHolidayAtTheseHours(day, timeSlot.BeginHour, timeSlot.EndHour))
                    {
                        occupedTimeSlotByHoliday.Add(timeSlot);
                    }
                }
                occupedTimeSlotByHoliday.ForEach(ts => timeSlots.Remove(ts));
                
            }
            return timeSlots;
        }

        public HolidayRepository HolidayRepository { get; set; }
    }
}
