using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class DefaultWorkDayPOHaveTo
    {
        readonly PratictionerOffice pratictionerOffice = new PratictionerOffice();
        readonly DefaultWorkDay defaultWorkDay = new DefaultWorkDay();

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            DateTime begin = new DateTime();
            DateTime end = new DateTime();


            DefaultWorkDaysPO defaultWorkDaysPO = new DefaultWorkDaysPO()
            {
                DefaultWorkDay = defaultWorkDay,
                PratictionerOffice = pratictionerOffice,
                BeginTime = begin,
                EndTime = end
            };
            
            Assert.AreEqual(defaultWorkDay, defaultWorkDaysPO.DefaultWorkDay);
            Assert.AreEqual(pratictionerOffice, defaultWorkDaysPO.PratictionerOffice);
            Assert.AreEqual(begin, defaultWorkDaysPO.BeginTime);
            Assert.AreEqual(end, defaultWorkDaysPO.EndTime);
        }
    }
}
