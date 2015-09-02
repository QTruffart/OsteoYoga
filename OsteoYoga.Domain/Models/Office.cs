using System.ComponentModel.DataAnnotations;

namespace OsteoYoga.Domain.Models
{
    public class Office : Entity
    {
        [Required]
        public virtual string Name { get; set; }
    }
}
