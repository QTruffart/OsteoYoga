using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using OsteoYoga.Domain.Models;
using OsteoYoga.Domain.Models.Interface;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    { 
        private static ISessionFactory SessionFactory { get; set; }
        private ISession session;

        //TODO A mettre dans un Singleton
        public ISession Session
        {
            get{
                if (session == null)
                {
                    Configuration config = new Configuration().Configure();
                    //setup the fluent map configuration
                    SessionFactory = Fluently.Configure(config)
                        .Mappings(m => m.FluentMappings.Conventions.Add(DefaultLazy.Always()).AddFromAssemblyOf<T>())
                        .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                        .BuildSessionFactory();

                    session = SessionFactory.OpenSession();
                }
                return session;
            }
            set
            {
                if (session == null)
                {
                    session = value;
                }
            }
        }

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
                session.Delete(toDelete);
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
                session.Delete(entity);
            }
            Session.Flush();
        }
    }
}
