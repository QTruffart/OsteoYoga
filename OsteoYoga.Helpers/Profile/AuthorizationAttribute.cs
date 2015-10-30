using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Repository.DAO.Implements;

namespace OsteoYoga.Helper.Profile
{

    //TODO : A tester
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ProfileAttribute : AuthorizeAttribute
    {
        public ProfileRepository ProfileRepository { get; set; }

        protected ProfileAttribute()
        {
            ProfileRepository = new ProfileRepository();
        }
        public abstract string ProfileType { get; }
        public string ActionResult{ get; set; }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Contact contact = SessionHelper.GetInstance().CurrentUser;
            if (contact != null)
            {
                if (!contact.Profiles.Contains(ProfileRepository.GetByName(ProfileType)))
                {
                    ActionResult = "Index";
                    return false;
                }
                if (!contact.IsConfirmed)
                {
                    ActionResult = "ConfirmAccount";
                    return false;
                }
                return true;
            }
            ActionResult = "Index";
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = ActionResult }));
        }
    }
}