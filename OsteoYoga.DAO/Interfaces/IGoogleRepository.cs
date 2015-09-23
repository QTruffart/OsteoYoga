using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IGoogleRepository<T>
    {
        Event Save(Date date, string summary, string description, PratictionerOffice office);
        Event Update(string eventId, Date date, string summary, string description, PratictionerOffice office);
        void Delete(string toDelete);
        Event GetById(string id);
        IList<Event> GetAll();
        IList<Event> GetAllForPractionerInterval(PratictionerOffice pratictionerOffice);
    }
}
