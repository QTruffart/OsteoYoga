using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IProfileRepository
    {
        Profile GetByName(string name);
    }
}