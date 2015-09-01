using System;
using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public virtual Profile GetByName(string name)
        {
            return Session.QueryOver<Profile>().Where(p => p.Name == name).List<Profile>().FirstOrDefault();
        }
    }
}
