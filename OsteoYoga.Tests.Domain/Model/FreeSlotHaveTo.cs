using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Tests.Domain.Model
{
    [TestClass]
    public class FreeSlotHaveTo
    {

        [TestMethod]
        public void InitializeCorrectlyInitialize()
        {
            DateTime expectedBegin = new DateTime();
            DateTime expectedEnd = new DateTime();
            PratictionerPreference preference = new PratictionerPreference();

            FreeSlot freeSlot = new FreeSlot()
                                    {
                                        Begin = expectedBegin,
                                        End = expectedEnd,
                                        Preference = preference
                                    };

            Assert.AreEqual(expectedBegin, freeSlot.Begin);
            Assert.AreEqual(expectedEnd, freeSlot.End);
            Assert.AreEqual(preference, freeSlot.Preference);
            Assert.AreEqual(freeSlot.Duration, (freeSlot.End - freeSlot.Begin).TotalMinutes);
        }
    }
}
