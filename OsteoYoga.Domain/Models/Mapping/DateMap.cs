using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DateMap : ClassMap<Date>
    {
        public DateMap()
        {
            Id(x => x.Id);

            Map(x => x.Begin);

            References(x => x.Duration).Column("DurationId");
            References(x => x.Office).Column("OfficeId");
            References(x => x.Contact).Column("ContactId");

            Table("Dates");
        }
    }
}
