using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PratictionerMap : SubclassMap<Pratictioner>
    {
        public PratictionerMap()
        {
            DiscriminatorValue(@"Pratictioner");
        }
    }
}
