using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class Pratictioner : Contact
    {
        //public virtual IList<Office> Offices { get; set; }
        //public virtual IList<PratictionerOffice> OfficePreferences { get; set; }
        public virtual string Profession { get; set; }
    }
}
