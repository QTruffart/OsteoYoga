using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Helper.Helpers.Interfaces
{
    public interface IFreeSlotHelper
    {
        IList<FreeSlot> CalculateFreeSlotBetweenTwoDays(IList<Event> events, PratictionerPreference preference );
        IList<FreeSlot> CalculateFreeSlotOnADay(IList<Event> events, DateTime begin, DateTime end );
    }
}
