using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Adminn.Startup))]
namespace Adminn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
