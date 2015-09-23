using System.Collections.Generic;
using System.Linq;
using NHibernate;
using OsteoYoga.Domain.Models.Interface;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Abstracts
{
    public class NHibernateRepository<T> : IRepository<T> where T : class, IEntity
    { 
        protected static ISession Session => NHibernateSession.Session;

        public virtual void Save(T toAdd)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(toAdd);
                transaction.Commit();
            }
        }

        public virtual void Delete(T toDelete)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Delete(toDelete);
                transaction.Commit();
            }
        }

        public virtual T GetById(int id)
        {
            return Session.QueryOver<T>().Where(e => e.Id == id).List<T>().FirstOrDefault();
        }

        public virtual IList<T> GetAll() 
        {            
            return Session.QueryOver<T>().List<T>();
        }

        //TODO Bug clés étrangères sur Dates
        public virtual void DeleteAll()
        {
            foreach (T entity in GetAll())
            {
                Session.Delete(entity);
            }
            Session.Flush();
        }
    }
}
