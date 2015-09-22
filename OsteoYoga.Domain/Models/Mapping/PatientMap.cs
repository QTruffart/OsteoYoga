using FluentNHibernate.Mapping;

namespace OsteoYoga.Domain.Models.Mapping
{
    public class PatientMap : SubclassMap<Patient>
    {
        public PatientMap()
        {
            Map(x => x.History);
            Abstract();
            Table("Patient");
        }
    }
}
