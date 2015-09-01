using System.ComponentModel.DataAnnotations;

namespace OsteoYoga.Domain.Models
{
    public class Profile : Entity
    {
        [Required]
        public virtual string Name { get; set; }
    }
}
