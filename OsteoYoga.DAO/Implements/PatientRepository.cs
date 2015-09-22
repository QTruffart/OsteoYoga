using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Implements
{
    public class PatientRepository : NHibernateRepository<Patient>, IContactRepository
    {
        public virtual bool EmailAlreadyExists(string mail)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail).RowCount() > 0;
        }

        public virtual Contact GetByEmail(string mail)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail).List<Contact>().FirstOrDefault();
        }
        
        public virtual bool SocialNetworkEmailAlreadyExists(string mail, string socialId, string networkType)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail && ts.NetworkId == socialId && ts.NetworkType == networkType).RowCount() > 0;
        }

        public virtual Contact GetBySocialNetworkEmail(string mail, string socialId, string networkType)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail && ts.NetworkId == socialId && ts.NetworkType == networkType).List<Contact>().FirstOrDefault();
        }
    }
}
