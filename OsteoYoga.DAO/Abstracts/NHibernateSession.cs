using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Abstracts
{
    public static class NHibernateSession
    {
        private static ISessionFactory SessionFactory { get; set; }
        private static ISession session;

        //TODO A mettre dans un Singleton
        public static ISession Session
        {
            get{
                if (session == null)
                {
                    //Todo : A exporter vers le web.config
                    Configuration config = new Configuration().Configure();
                    //setup the fluent map configuration
                    SessionFactory = Fluently.Configure(config)
                        .CurrentSessionContext("web")
                        .Mappings(m => m.FluentMappings.Conventions.Add(DefaultLazy.Always()).AddFromAssemblyOf<Entity>())
                        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
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
    }
}
