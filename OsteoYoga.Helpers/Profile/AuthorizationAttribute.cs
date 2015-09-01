using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Repository.DAO;

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

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Contact contact = SessionHelper.GetInstance().CurrentUser;
            return contact != null && contact.Profile == ProfileRepository.GetByName(ProfileType);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }
    }
}