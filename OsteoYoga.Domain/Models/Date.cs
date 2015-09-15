namespace OsteoYoga.Domain.Models
{
    public class Date : Entity
    {
        public virtual Office Office { get; set; }
        public virtual Duration Duration { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
