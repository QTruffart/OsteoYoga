using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DefaultWorkDayMap : ClassMap<DefaultWorkDay>
    {
        public DefaultWorkDayMap()
        {
            Id(x => x.Id);
            Map(x => x.DayOfTheWeek);

            Table("DefaultWorkDay");
        }
    }
}
