using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CostsWeb.Startup))]
namespace CostsWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
