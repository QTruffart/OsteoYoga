using System;
using System.Configuration;
using System.Web;

namespace OsteoYoga.Helper
{
    public class Constants
    {
        private static Constants _instance;

        public static Constants Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public static Constants GetInstance()
        {
            return _instance ?? (_instance = new Constants());
        }

        public virtual string ServerAddress(HttpRequestBase request)
        {
            return  "http://" + request.ServerVariables["SERVER_NAME"] + ":" + request.ServerVariables["SERVER_PORT"]; 
        }

        

        public virtual TimeSpan AMBegin()
        {
            string amBegin = ConfigurationManager.AppSettings["AMBegin"];
            return new TimeSpan(int.Parse(amBegin.Split(':')[0]), int.Parse(amBegin.Split(':')[1]), 0);
        }
        public virtual TimeSpan AMEnd()
        {
            string amEnd = ConfigurationManager.AppSettings["AMEnd"];
            return new TimeSpan(int.Parse(amEnd.Split(':')[0]), int.Parse(amEnd.Split(':')[1]), 0);
        }
        public virtual TimeSpan PMBegin()
        {
            string pmBegin = ConfigurationManager.AppSettings["PMBegin"];
            return new TimeSpan(int.Parse(pmBegin.Split(':')[0]), int.Parse(pmBegin.Split(':')[1]), 0);
        }
        public virtual TimeSpan PMEnd()
        {
            string pmEnd = ConfigurationManager.AppSettings["PMEnd"];
            return new TimeSpan(int.Parse(pmEnd.Split(':')[0]), int.Parse(pmEnd.Split(':')[1]), 0);
        }
        public virtual string MailNico { get { return ConfigurationManager.AppSettings["MailNico"]; } }
        public virtual string NomNico { get { return ConfigurationManager.AppSettings["NomNico"]; } }
        public virtual string PassAdmin { get { return ConfigurationManager.AppSettings["PassAdmin"]; } }
        public virtual string PassMail { get { return ConfigurationManager.AppSettings["PassMail"]; } }
    }
}
