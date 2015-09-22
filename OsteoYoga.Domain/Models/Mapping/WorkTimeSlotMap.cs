using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class WorkTimeSlotMap : ClassMap<WorkTimeSlot>
    {
        public WorkTimeSlotMap()
        {
            Id(x => x.Id);

            Map(x => x.BeginTime);
            Map(x => x.EndTime);

            Table("WorkTimeSlot");
        }
    }
}
