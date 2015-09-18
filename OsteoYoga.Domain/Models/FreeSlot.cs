using System;

namespace OsteoYoga.Domain.Models
{
    public class FreeSlot : Entity
    {
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }
    }
}
