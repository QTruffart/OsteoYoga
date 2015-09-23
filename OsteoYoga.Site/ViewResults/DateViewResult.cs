using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Site.ViewResults
{
    public class DateViewResult
    {
        public IEnumerable<Office> Offices { get; set; }

        public string SelectedOfficeId { get; set; }
        public string SelectedPratictionerId { get; set; }
        //public string SelectedSuburbId { get; set; }
        //public IEnumerable<Province> Provinces { get; set; }

        //public IList<Duration> Durations { get; set; }
        //public IList<FreeSlot> FreeSlots { get; set; }
    }
}