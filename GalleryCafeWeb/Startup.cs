using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GalleryCafeWeb.Startup))]
namespace GalleryCafeWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
