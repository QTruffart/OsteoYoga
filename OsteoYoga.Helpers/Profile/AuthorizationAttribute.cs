using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers;
using OsteoYoga.Helper.Helpers.Implements;
using OsteoYoga.Repository.DAO;
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

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Contact contact = SessionHelper.GetInstance().CurrentUser;
            return contact != null && contact.Profiles.Contains(ProfileRepository.GetByName(ProfileType));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Login", action = "Index" }));
        }
    }
}