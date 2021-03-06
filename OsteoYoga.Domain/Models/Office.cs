﻿#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace OsteoYoga.Domain.Models
{
    public class Office : Entity
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Adress { get; set; }
        public virtual IList<PratictionerOffice> PratictionerPreference { get; set; }
        public virtual IList<Pratictioner> Pratictioners { get; set; }
    }
}
