using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Site.ViewResults
{
    public class DateViewResult
    {
        public IList<Office> Offices { get; set; }
        public IList<Duration> Durations { get; set; }
        public IList<FreeSlot> FreeSlots { get; set; }
    }
}