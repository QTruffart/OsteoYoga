using System;
using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface ITimeSlotRepository
    {
        IList<TimeSlot> GetFreeTimeSlots(DateTime day);
        HolidayRepository HolidayRepository { get; set; }
    }
}