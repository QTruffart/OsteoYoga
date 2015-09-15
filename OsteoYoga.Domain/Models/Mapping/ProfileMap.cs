using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class ProfileMap : ClassMap<Profile>
    {
        public ProfileMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);

            HasManyToMany(x => x.Contacts).Cascade.All().Table("ContactProfile");

            Table("Profile");
        }
    }
}
