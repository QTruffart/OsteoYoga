using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OsteoYoga.Site.Startup))]
namespace OsteoYoga.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
