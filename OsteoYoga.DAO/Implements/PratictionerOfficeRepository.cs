using System.Collections.Generic;
using System.Linq;
using OsteoYoga.Domain.Models;
using OsteoYoga.Repository.DAO.Abstracts;
using OsteoYoga.Repository.DAO.Interfaces;

namespace OsteoYoga.Repository.DAO.Implements
{
    public class PratictionerOfficeRepository : NHibernateRepository<PratictionerOffice>, IPratictionerOfficeRepository
    {
        public PratictionerOffice GetByOfficeIdAndPratictionerId(int officeId, int pratictionerId)
        {
            return Session.QueryOver<PratictionerOffice>().Where(ts => ts.Office.Id == officeId && ts.Pratictioner.Id == pratictionerId).List<PratictionerOffice>().FirstOrDefault();
        }
    }
}
