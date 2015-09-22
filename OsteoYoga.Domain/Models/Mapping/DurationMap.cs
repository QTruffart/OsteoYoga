using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DurationMap : ClassMap<Duration>
    {
        public DurationMap()
        {
            Id(x => x.Id);

            Map(x => x.Value);
            References(x => x.PratictionerPreference).Cascade.SaveUpdate();

            Table("Duration");
        }
    }
}
