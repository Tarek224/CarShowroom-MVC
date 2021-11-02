using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarShowroom.Startup))]
namespace CarShowroom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
