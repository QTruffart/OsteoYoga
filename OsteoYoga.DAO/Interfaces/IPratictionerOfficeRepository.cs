using System;
using System.Collections.Generic;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Repository.DAO.Interfaces
{
    public interface IPratictionerOfficeRepository : IRepository<PratictionerOffice>
    {
        PratictionerOffice GetByOfficeIdAndPratictionerId(int officeId, int pratictionerId);
    }
}