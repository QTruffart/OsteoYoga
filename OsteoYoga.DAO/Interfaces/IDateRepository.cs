using System;
using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IDateRepository
    {
        Date Validate(string id);
        IList<Date> GetFutureDates(DateTime day);
        IList<Date> GetByDay(DateTime dateTime);
    }
}