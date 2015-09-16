using System.Collections.Generic;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IRepository<T>
    {
        void Save(T toAdd);
        void Delete(T toDelete);
        T GetById(int id);
        IList<T> GetAll();
        void DeleteAll();
    }
}
