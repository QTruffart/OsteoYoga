using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class OfficeMap : ClassMap<Office>
    {
        public OfficeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            
            HasMany(x => x.Dates).Inverse();

            Table("Office");
        }
    }
}
