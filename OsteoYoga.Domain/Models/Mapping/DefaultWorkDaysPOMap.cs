using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DefaultWorkDaysPOMap : ClassMap<DefaultWorkDaysPO>
    {
        public DefaultWorkDaysPOMap()
        {
            CompositeId().KeyReference(x => x.PratictionerOffice, "PratictionerOfficeKey").KeyProperty(x => x.PratictionerOffice);
            CompositeId().KeyReference(x => x.DefaultWorkDay, "DefaultWorkDayKey").KeyProperty(x => x.DefaultWorkDay);

            //References(x => x.PratictionerOffice);
            //References(x => x.DefaultWorkDay);

            Map(x => x.BeginTime);
            Map(x => x.EndTime);

            Table("DefaultWorkDaysPO");
        }
    }
}
