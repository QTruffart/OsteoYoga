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
            Map(x => x.MinInterval);
            Map(x => x.MaxInterval);

            References(x => x.Office).Cascade.SaveUpdate();
            References(x => x.Pratictioner).Cascade.SaveUpdate();

            HasMany(x => x.Durations).Cascade.SaveUpdate();
            //HasMany(x => x.TimeSlots).Cascade.SaveUpdate();

            Table("PratictionerPreference");
        }
    }
}
