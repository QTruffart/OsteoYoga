using System;

namespace OsteoYoga.Domain.Models
{
    public class Date : Entity
    {
        public virtual DateTime BeginTime { get; set; }
        public virtual Duration Duration { get; set; }
        public virtual Office Office { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Pratictioner Pratictioner { get; set; }
    }
}
