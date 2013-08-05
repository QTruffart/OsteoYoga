using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public virtual IList<Holiday> GetFutureHoliday(DateTime day)
        {
            return Session.QueryOver<Holiday>().Where(h => h.Day >= day).List<Holiday>();
        }

        public virtual bool ThereIsAnHolidayAtTheseHours(DateTime dateTime, TimeSpan beginHour, TimeSpan endHour)
        {
            IList<Holiday> holidaysOnDay = Session.QueryOver<Holiday>().Where(h => h.Day == dateTime).List<Holiday>();

            return holidaysOnDay.Any(h => (h.BeginHour > beginHour && h.BeginHour < endHour) ||
                                     (h.EndHour > beginHour && h.EndHour < endHour) ||
                                     (beginHour > h.BeginHour && beginHour < h.EndHour) ||
                                     (endHour > h.BeginHour && endHour < h.EndHour));

        }
    }
}
