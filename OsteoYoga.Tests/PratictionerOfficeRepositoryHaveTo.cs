using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Implements;

namespace OsteoYoga.Tests.DAO
{
    [TestClass]
    public class PratictionerOfficeRepositoryHaveTo : BaseTestsNHibernate
    {

        private readonly Pratictioner pratictioner = new Pratictioner() ;
        private readonly Office office = new Office() ;
      

        private readonly PratictionerOfficeRepository pratictionerOfficeRepository = new PratictionerOfficeRepository();
        private readonly PratictionerRepository pratictionerRepository = new PratictionerRepository();
        private readonly OfficeRepository officeRepository = new OfficeRepository();


        [TestMethod]
        public void Get_By_OfficeId_And_PratictionerId()
        {
            //arrange
            officeRepository.Save(office);
            pratictionerRepository.Save(pratictioner);
            PratictionerOffice pratictionerOffice = new PratictionerOffice()
            {
                Pratictioner = pratictioner,
                Office = office
            };

            pratictionerOfficeRepository.Save(pratictionerOffice);

            //act
            PratictionerOffice result = pratictionerOfficeRepository.GetByOfficeIdAndPratictionerId(office.Id, pratictioner.Id);

            //assert
            Assert.AreEqual(pratictionerOffice, result);
        }


        [TestCleanup]
        public override void CleanUp()
        {
            pratictionerOfficeRepository.DeleteAll();
            pratictionerRepository.DeleteAll();
            officeRepository.DeleteAll();
        }

       
    }
}