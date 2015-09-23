using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PratictionerMap : SubclassMap<Pratictioner>
    {
        public PratictionerMap()
        {
            DiscriminatorValue(@"Pratictioner");
            //HasMany(x => x.OfficePreferences).Cascade.SaveUpdate();
            //HasManyToMany(x => x.Offices)
            //   .Cascade.All()
            //   .Table("PratictionerOffice");
        }
    }
}
