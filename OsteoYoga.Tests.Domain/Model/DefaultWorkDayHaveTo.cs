using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DefaultWorkDayHaveTo
    {

        private string DayOfTheWeek = "Monday";

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {

            DefaultWorkDay defaultWorkDay = new DefaultWorkDay()
            {
                DayOfTheWeek = DayOfTheWeek
            };
            
            Assert.AreEqual(DayOfTheWeek, defaultWorkDay.DayOfTheWeek);
        }
    }
}
