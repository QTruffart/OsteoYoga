using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Helper.Helpers.Interfaces
{
    public interface IDaySlotHelper
    {
        IList<DateTime> CalculateFreeDays(PratictionerOffice pratictionerOffices, Duration expectedDuration);
        IGoogleRepository<Event> GoogleRepository { get; set; }

    }
}
