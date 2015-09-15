using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;

namespace Test
{
    class Program
    {
        public class NHibernateHelper
        {
            private static ISessionFactory _sessionFactory;

            private static ISessionFactory SessionFactory
            {
                get
                {
                    if (_sessionFactory == null)

                        InitializeSessionFactory();
                    return _sessionFactory;
                }
            }

            private static void InitializeSessionFactory()
            {
                _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Data Source=yopex-pc\mssql2008;Initial Catalog=Osteyoga123;Integrated Security=True") // Modify your ConnectionString
                                  .ShowSql()
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Entity>()).ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                    .BuildSessionFactory();
            }

            public static ISession OpenSession()
            {
                return SessionFactory.OpenSession();
            }
        }

        static void Main(string[] args)
        {
            var profile = new Profile { Name = "TOTdO"};

            var contact = new Contact()
            {
                FullName = "FullNames",
                Profiles = new List<Profile>() {profile}
            };
            ContactRepository repository = new ContactRepository();
            repository.Save(contact);
            Console.WriteLine("Department Created: " + profile.Name);
            Console.ReadLine();
        }
    }
}
