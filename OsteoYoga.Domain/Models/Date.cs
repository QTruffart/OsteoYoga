using System;

namespace OsteoYoga.Domain.Models
{
    public class Date : Entity
    {
        public virtual DateTime Begin { get; set; }
        public virtual Office Office { get; set; }
        public virtual Duration Duration { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
