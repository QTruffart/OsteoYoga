namespace OsteoYoga.Domain.Models
{
    public class PratictionerPreference : Entity
    {
        public virtual Contact Contact { get; set; }
        public virtual int Reminder { get; set; }
        public virtual int DateInterval { get; set; }
    }
}
