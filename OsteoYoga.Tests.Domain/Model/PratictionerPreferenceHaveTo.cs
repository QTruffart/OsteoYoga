using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class PratictionerPreferenceHaveTo
    {
        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {

            Pratictioner practitioner = new Pratictioner();
            Office office = new Office();
            IList<Duration> durations = new List<Duration>();
            IList<WorkTimeSlot> timeSlots = new List<WorkTimeSlot>();


            PratictionerOffice pratictionerOffice = new PratictionerOffice()
            {
                Pratictioner = practitioner,
                Office = office,
                Durations = durations,
                TimeSlots = timeSlots,
                Reminder = 30,
                DateWaiting = 50,
                MinInterval = 3,
                MaxInterval = 45,
            };

            Assert.AreEqual(practitioner, pratictionerOffice.Pratictioner);
            Assert.AreEqual(30, pratictionerOffice.Reminder);
            Assert.AreEqual(50, pratictionerOffice.DateWaiting);
            Assert.AreEqual(3,  pratictionerOffice.MinInterval);
            Assert.AreEqual(45, pratictionerOffice.MaxInterval);
        }

        [TestMethod]
        public void Get_Min_Date_Value_By_Min_Interval()
        {
            //arrange
            now = now.AddDays(3);

            //act
            PratictionerOffice pratictionerOffice = new PratictionerOffice(){ 
                MinInterval = 3,
            };
            
            //assert
            Assert.AreEqual(now.ToString("d"), pratictionerOffice.MinDateInterval.ToString("d"));
        }

        [TestMethod]
        public void Get_Max_Date_Value_By_Min_Interval()
        {
            //arrange
            now = now.AddDays(15);

            //act
            PratictionerOffice pratictionerOffice = new PratictionerOffice(){ 
                MaxInterval = 15,
            };
            
            //assert
            Assert.AreEqual(now.ToString("d"), pratictionerOffice.MaxDateInterval.ToString("d"));
        }

    }
}
