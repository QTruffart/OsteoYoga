using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public virtual bool EmailAlreadyExists(string mail)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail).RowCount() > 0;
        }

        public virtual Contact GetByEmail(string mail)
        {
            return Session.QueryOver<Contact>().Where(ts => ts.Mail == mail).List<Contact>().FirstOrDefault();
        }
    }
}
