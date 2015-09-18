using System.Web;
using OsteoYoga.Domain.Models;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class SessionHelper
    {
        private static SessionHelper _instance;
        private const string CurrentContact = "CurrentContact";
        private const string adminConnected = "AdminConnected";

        public static SessionHelper Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public static SessionHelper GetInstance()
        {
            return _instance ?? (_instance = new SessionHelper());
        }

        public virtual Contact CurrentUser 
        { 
            get
            {
                return HttpContext.Current.Session[CurrentContact] as Contact;
            }
            set { HttpContext.Current.Session[CurrentContact] = value; }
        }

        public virtual bool AdminConnected
        {
            get
            {
                return (bool) (HttpContext.Current.Session[adminConnected] ?? false);
            }
            set { HttpContext.Current.Session[adminConnected] = value; }
        }
    }
}