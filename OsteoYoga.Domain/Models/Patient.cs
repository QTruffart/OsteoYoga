using System.Collections.Generic;

namespace OsteoYoga.Domain.Models
{
    public class Patient : Contact
    {
        public virtual string History { get; set; }
    }
}
