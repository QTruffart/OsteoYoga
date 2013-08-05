using System;
using System.Collections.Generic;
using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class DateRepository : Repository<Date>, IDateRepository
    {
        public virtual Date Validate(string id)
        {
            Date date = Session.QueryOver<Date>().Where(d => d.ConfirmationId == id).List<Date>().FirstOrDefault();
            if (date != null)
            {
                date.IsConfirmed = true;
                Save(date);
            }
            return date;
        }

        public virtual IList<Date> GetFutureDates(DateTime day)
        {
            return Session.QueryOver<Date>().Where(d => d.Day >= day).List<Date>();
        }

        public virtual IList<Date> GetByDay(DateTime dateTime)
        {
            return Session.QueryOver<Date>().Where(d => d.Day == dateTime).List<Date>();
        }
    }
}
