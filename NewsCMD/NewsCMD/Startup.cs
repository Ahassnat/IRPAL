using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsCMD.Startup))]
namespace NewsCMD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
