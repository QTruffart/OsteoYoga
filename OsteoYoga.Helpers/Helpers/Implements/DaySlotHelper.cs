﻿using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Calendar.v3.Data;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Interfaces;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class DaySlotHelper: IDaySlotHelper
    {
        public IHourSlotHelper HourSlotHelper { get; set; }
        public IGoogleRepository<Event> GoogleRepository { get; set; }

        public DaySlotHelper()
        {
            HourSlotHelper = new HourSlotHelper();
            GoogleRepository = new GoogleRepository();
        }

        public IList<DateTime> CalculateFreeDays(PratictionerOffice pratictionerOffices, Duration expectedDuration)
        {
            DateTime begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(pratictionerOffices.MinInterval);
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(pratictionerOffices.MaxInterval);
            IList<Event> events = GoogleRepository.GetAllForPractionerInterval(pratictionerOffices);
            IList<DateTime> result = new List<DateTime>();
            for (DateTime date = begin.Date; date.Date <= end; date = date.AddDays(1).Date)
            {
                if (HourSlotHelper.CalculateFreeHours(date, expectedDuration, events).Any())
                {
                    result.Add(date);
                }
            }
            return result;
        }
    }
}
