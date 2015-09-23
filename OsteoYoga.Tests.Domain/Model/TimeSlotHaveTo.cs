using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class TimeSlotHaveTo
    {
        
        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {

            DateTime begin = new DateTime();
            DateTime end = new DateTime();
            PratictionerOffice office = new PratictionerOffice();
            WorkTimeSlot workTimeSlot = new WorkTimeSlot()
                                    {
                                        BeginTime = begin,
                                        EndTime = end,
                                        //PratictionerOffice = office
                                    };
            Assert.AreEqual(begin, workTimeSlot.BeginTime);
            Assert.AreEqual(end, workTimeSlot.EndTime);
            //Assert.AreEqual(office, WorkTimeSlot.PratictionerOffice);
        }
    }
}
