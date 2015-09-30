using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DefaultWorkDaysPOMap : ClassMap<DefaultWorkDaysPO>
    {
        public DefaultWorkDaysPOMap()
        {
            Id(x => x.Id);
            References(x => x.PratictionerOffice);
            References(x => x.DefaultWorkDay);

            Map(x => x.BeginTime);
            Map(x => x.EndTime);

            Table("DefaultWorkDaysPO");
        }
    }
}
