using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CARTS.Startup))]
namespace CARTS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
