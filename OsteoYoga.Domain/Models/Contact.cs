using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using OsteoYoga.Resource;

namespace OsteoYoga.Domain.Models
{
    public class Contact : Entity
    {
        [Required]
        public virtual string FullName { get; set; }

        [Required]
        [RegularExpression("^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$", ErrorMessage = "L'email doit être du format : \"______@_____.___\"")]
        public virtual string Mail { get; set; }
        
        [Required]
        public virtual string Phone { get; set; }
        
        [Required]
        public virtual Guid ConfirmNumber { get; set; }
        
        [Required]
        public virtual bool IsConfirmed { get; set; }

        public override string ToString()
        {
            return FullName + " ( " + ModelResource.Mail + ": " + Mail + " ; " + ModelResource.Phone + ": " + Phone + " )";
        }

        public virtual bool IsValid()
        {
            Regex myRegex = new Regex("^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
            if (Mail != null && Phone != null && FullName != null && myRegex.IsMatch(Mail))
            {
                return true;
            }
            return false;
        }
    }
}
