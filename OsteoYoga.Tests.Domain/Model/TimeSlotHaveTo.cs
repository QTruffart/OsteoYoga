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
            PratictionerPreference preference = new PratictionerPreference();
            WorkTimeSlot workTimeSlot = new WorkTimeSlot()
                                    {
                                        BeginTime = begin,
                                        EndTime = end,
                                        //PratictionerPreference = preference
                                    };
            Assert.AreEqual(begin, workTimeSlot.BeginTime);
            Assert.AreEqual(end, workTimeSlot.EndTime);
            //Assert.AreEqual(preference, WorkTimeSlot.PratictionerPreference);
        }
    }
}
