using System;

namespace OsteoYoga.Helper.Profile
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PatientProfileAttribute : ProfileAttribute
    {
        public override string ProfileType
        {
            get { return Constants.GetInstance().PatientProfile; }
        }
    }
}