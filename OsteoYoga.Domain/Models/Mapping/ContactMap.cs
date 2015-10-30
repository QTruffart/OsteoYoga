using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class ContactMap : ClassMap<Contact>
    {
       //Constructor
        public ContactMap()
        {

            Id(x => x.Id)
                .Column("Id")
                .CustomType("Int32")
                .Access.Property()
                .CustomSqlType("int")
                .Not.Nullable()
                .Precision(10)
                .GeneratedBy.Identity();

            DiscriminateSubClassesOnColumn("ClassType").Not.Nullable();

            Map(x => x.FullName);
            Map(x => x.Mail);
            Map(x => x.Password);
            Map(x => x.NetworkId);
            Map(x => x.NetworkType);
            Map(x => x.Phone);
            Map(x => x.IsConfirmed);
            Map(x => x.ConfirmedCode);

            HasManyToMany(x => x.Profiles).Cascade.All().Table("ContactProfile");

            Table("Contact");
        }
    }
}
