using System;

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