using Microsoft.VisualStudio.TestTools.UnitTesting;

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