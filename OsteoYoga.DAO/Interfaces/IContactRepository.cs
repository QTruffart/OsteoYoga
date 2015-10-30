using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IContactRepository
    {
        bool EmailAlreadyExists(string mail);
        Contact GetByEmailAndPassword(string mail, string password);
        bool SocialNetworkEmailAlreadyExists(string mail, string socialId, string networkType);
        Contact GetBySocialNetworkEmail(string mail, string socialId, string networkType);
    }
}