using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantWebApp.Startup))]
namespace RestaurantWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
