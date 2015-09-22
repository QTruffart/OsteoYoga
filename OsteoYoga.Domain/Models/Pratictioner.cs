using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class Pratictioner : Contact
    {
        public virtual IList<PratictionerPreference> OfficePreferences { get; set; }
    }
}
