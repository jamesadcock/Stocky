using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Stocky.Startup))]
namespace Stocky
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
