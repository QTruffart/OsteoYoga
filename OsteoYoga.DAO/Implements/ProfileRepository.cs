using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Implements
{
    public class ProfileRepository : NHibernateRepository<Profile>, IProfileRepository
    {
        public virtual Profile GetByName(string name)
        {
            return Session.QueryOver<Profile>().Where(p => p.Name == name).List<Profile>().FirstOrDefault();
        }
    }
}
