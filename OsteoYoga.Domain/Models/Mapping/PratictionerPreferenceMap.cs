using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PratictionerPreferenceMap : ClassMap<PratictionerPreference>
    {
        public PratictionerPreferenceMap()
        {
            Id(x => x.Id);
            Map(x => x.Reminder);
            Map(x => x.DateWaiting);
            Map(x => x.MaxInterval);
            Map(x => x.MaxInterval);

            References(x => x.Contact).Column("ContactId");

            Table("PratictionerPreference");
        }
    }
}
