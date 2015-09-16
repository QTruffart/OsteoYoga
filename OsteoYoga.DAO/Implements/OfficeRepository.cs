using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Implements
{
    public class OfficeRepository : NHibernateRepository<Office>, IOfficeRepository
    {
    }
}
