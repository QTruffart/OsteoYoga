using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class FreeSlotHelper: IFreeSlotHelper
    {
        public IList<FreeSlot> CalculateFreeSlotBetweenTwoDays(IList<Event> events, PratictionerPreference preference)
        {
            throw new NotImplementedException();
        }

        public IList<FreeSlot> CalculateFreeSlotOnADay(IList<Event> events, DateTime begin, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}
