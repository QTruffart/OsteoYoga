using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using OsteoYoga.Resource;
using OsteoYoga.Resource.Contact;

namespace OsteoYoga.Domain.Models
{
    public class Date : Entity
    {
        public virtual DateTime Begin { get; set; }
        public virtual Duration Duration { get; set; }
        public virtual Office Office { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Pratictioner Pratictioner { get; set; }
    }
}
