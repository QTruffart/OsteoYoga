using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Helper
{
    [TestClass]
    public class FreeSlotHelperHaveTo
    {
        DateTime tomorrowMorning = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0,0,0);
        DateTime tomorrowNight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

        [TestMethod]
        public void Return_Empty_List_If_There_Are_No_Free_Slot_Between_Two_Dates()
        {
            PratictionerPreference preference = new PratictionerPreference()
            {
                DateWaiting = 5,
                MinInterval = 1,
                MaxInterval = 2
            };

            IList<Event> events = new List<Event>()
            {
                new Event()
                {
                    
                }
            };
        }
    }
}
