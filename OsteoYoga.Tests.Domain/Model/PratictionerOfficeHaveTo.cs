using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class PratictionerOfficeHaveTo
    {
        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {

            Pratictioner practitioner = new Pratictioner();
            Office office = new Office();
            IList<Duration> durations = new List<Duration>();
            IList<WorkTimeSlot> timeSlots = new List<WorkTimeSlot>();
            IList<DefaultWorkDaysPO> defaultWorkDays = new List<DefaultWorkDaysPO>();


            PratictionerOffice pratictionerOffice = new PratictionerOffice()
            {
                Pratictioner = practitioner,
                Office = office,
                Durations = durations,
                TimeSlots = timeSlots,
                DefaultWorkDaysPO = defaultWorkDays,
                Reminder = 30,
                DateWaiting = 50,
                MinInterval = 3,
                MaxInterval = 45,
            };

            Assert.AreEqual(practitioner, pratictionerOffice.Pratictioner);
            Assert.AreEqual(office, pratictionerOffice.Office);
            Assert.AreEqual(durations, pratictionerOffice.Durations);
            Assert.AreEqual(defaultWorkDays, pratictionerOffice.DefaultWorkDaysPO);
            Assert.AreEqual(30, pratictionerOffice.Reminder);
            Assert.AreEqual(50, pratictionerOffice.DateWaiting);
            Assert.AreEqual(3,  pratictionerOffice.MinInterval);
            Assert.AreEqual(45, pratictionerOffice.MaxInterval);
        }

      

        [TestMethod]
        public void Get_Work_Days_Between_Two_Days()
        {
            //arrange
            PratictionerOffice pratictionerOffice = new PratictionerOffice()
            {
                MinInterval = 1,
                MaxInterval = 4,
                DefaultWorkDaysPO = new List<DefaultWorkDaysPO>()
                {
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Monday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Tuesday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Thursday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Friday" } },
                    new DefaultWorkDaysPO { DefaultWorkDay = new DefaultWorkDay() { DayOfTheWeek = "Sunday" } }
                }
            };

            //act
            IList<DateTime> workDays =  pratictionerOffice.GetWorkDaysBetweenIntervals(new DateTime(2015,09,28));


            //assert
            Assert.AreEqual(3, workDays.Count);
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "29/09/2015");
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "01/10/2015");
            CollectionAssert.Contains(workDays.Select(w => w.ToString("dd/MM/yyyy")).ToList(), "02/10/2015");
        }
    }
}
