using System;
using System.Web;
using System.Web.Mvc;
using OsteoYoga.Domain.Models;
using OsteoYoga.Helper.Helpers;

namespace OsteoYoga.Helper.Profile
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PatientProfileAttribute : ProfileAttribute
    {
        public override string ProfileType
        {
            get { return Constants.GetInstance().PatientProfile; }
        }
    }
}