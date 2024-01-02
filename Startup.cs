using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Finance_Tracker.Startup))]
namespace Finance_Tracker
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
