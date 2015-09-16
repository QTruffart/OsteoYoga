using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IGoogleRepository<T>
    {
        Event Save(DateTime begin, DateTime end, string summary, string description);
        void Delete(string toDelete);
        Event GetById(string id);
        IList<Event> GetAll();
        void DeleteAll();
    }
}
