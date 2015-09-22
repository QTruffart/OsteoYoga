using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class ContactMap : ClassMap<Contact>
    {
       //Constructor
        public ContactMap()
        {
            UseUnionSubclassForInheritanceMapping();

            Id(x => x.Id)
                .Column("Id")
                .CustomType("Int32")
                .Access.Property()
                .CustomSqlType("int")
                .Not.Nullable()
                .Precision(10)
                .GeneratedBy.Assigned();

            Map(x => x.FullName);
            Map(x => x.Mail);
            Map(x => x.NetworkId);
            Map(x => x.NetworkType);
            Map(x => x.Phone);

            HasManyToMany(x => x.Profiles).Cascade.All().Table("ContactProfile");

            Table("Patient");
        }
    }
}
