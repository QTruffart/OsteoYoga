using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PratictionerMap : SubclassMap<Pratictioner>
    {
        public PratictionerMap()
        {
            HasMany(x => x.OfficePreferences).Cascade.SaveUpdate();
            Abstract();
            Table("Pratictioner");
        }
    }
}
