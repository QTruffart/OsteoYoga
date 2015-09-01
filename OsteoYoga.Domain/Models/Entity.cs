namespace OsteoYoga.Domain.Models
{
    public class Entity : IEntity
    {
       

        public virtual int Id { get; set; }


        // Let is now override the equality operator
        // == and != comes in pair, if we define one we need to define other too.
        public static bool operator ==(Entity rational1, Entity rational2)
        {
            return rational2 != null && (rational1 != null && rational1.Id == rational2.Id);
        }

        public static bool operator !=(Entity rational1, Entity rational2)
        {
            return rational2 != null && (rational1 != null && rational1.Id != rational2.Id);
        }

        public override bool Equals(object obj)
        {
            Entity r = obj as Entity;
            if (r != null)
            {
                return r == this;
            }
            return false;
        }

        protected bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
