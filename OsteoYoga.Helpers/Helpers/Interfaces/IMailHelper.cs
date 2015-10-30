using OsteoYoga.Domain.Models.Interface;

namespace OsteoYoga.Helper.Helpers.Interfaces
{
    public interface IMailHelper
    {
        string TemplatePath { get;  }
        void SendMail(IMailModel model);
    }
}
