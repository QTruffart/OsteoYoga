using System;
using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IContactRepository
    {
        bool EmailAlreadyExists(string mail);
        Contact GetByEmail(string mail);
        bool SocialNetworkEmailAlreadyExists(string mail, string socialId, string networkType);
        Contact GetBySocialNetworkEmail(string mail, string socialId, string networkType);
    }
}