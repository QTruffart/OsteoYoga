using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class DateMap : ClassMap<Date>
    {
       //Constructor
        public DateMap()
        {
            Id(x => x.Id);


            References(x => x.Office);
            References(x => x.Duration);
            References(x => x.Pratictioner);
            References(x => x.Patient);

            Map(x => x.Begin);
            
            Table("Date");
        }
    }
}
