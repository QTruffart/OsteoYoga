using System;
using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IHolidayRepository
    {
        IList<Holiday> GetFutureHoliday(DateTime day);
        bool ThereIsAnHolidayAtTheseHours(DateTime dateTime, TimeSpan beginHour, TimeSpan endHour);
    }
}