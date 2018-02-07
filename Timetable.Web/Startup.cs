using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Timetable.Web.Startup))]
namespace Timetable.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
