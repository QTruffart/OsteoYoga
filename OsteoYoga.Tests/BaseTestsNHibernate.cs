using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Tests.DAO
{

    [TestClass]
    public abstract class BaseTestsNHibernate
    {
        [TestInitialize]
        public virtual void Initialize()
        {
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
        }
    }
}