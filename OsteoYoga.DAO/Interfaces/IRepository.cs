using System.Collections.Generic;
using NHibernate;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IRepository<T>
    {
        ISession Session { get; set; }
        void Save(T toAdd);
        void Delete(T toDelete);
        T GetById(int id);
        IList<T> GetAll();
        void DeleteAll();
    }
}
