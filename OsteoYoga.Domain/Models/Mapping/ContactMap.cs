using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class ContactMap : ClassMap<Contact>
    {
       //Constructor
        public ContactMap()
        {
            Id(x => x.Id);
            Map(x => x.FullName);
            Map(x => x.Mail);
            Map(x => x.NetworkId);
            Map(x => x.NetworkType);
            Map(x => x.Phone);

            HasMany(x => x.Dates).Inverse();
            HasManyToMany(x => x.Profiles).Cascade.All().Table("ContactProfile");

            Table("Contact");
        }
    }
}
