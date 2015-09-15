using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class Duration : Entity
    {
        public virtual int Value { get; set; }
        public virtual IList<Office> Offices { get; set; }
        public virtual IList<Date> Dates { get; set; }
    }
}
