using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class Duration : Entity
    {
        public virtual int Value { get; set; }
        public virtual PratictionerPreference PratictionerPreference { get; set; }
    }
}
