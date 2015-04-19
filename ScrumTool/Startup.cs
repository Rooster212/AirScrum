using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScrumTool.Startup))]
namespace ScrumTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
