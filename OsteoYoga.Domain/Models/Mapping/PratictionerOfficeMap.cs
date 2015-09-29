using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PratictionerOfficeMap : ClassMap<PratictionerOffice>
    {
        public PratictionerOfficeMap()
        {
            Id(x => x.Id);
            Map(x => x.Reminder);
            Map(x => x.DateWaiting);
            Map(x => x.MinInterval);
            Map(x => x.MaxInterval);

            References(x => x.Office).Cascade.SaveUpdate();
            References(x => x.Pratictioner).Cascade.SaveUpdate();

            HasMany(x => x.Durations).Cascade.SaveUpdate();

            HasManyToMany(x => x.DefaultWorkDaysPO).Table("DefaultWorkDaysPO");

            Table("PratictionerOffice");
        }
    }
}
