using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Site.ViewResults
{
    public class DateViewResult
    {
        public IList<Office> Offices { get; set; } 
        public IList<Duration> Durations { get; set; } 
    }
}