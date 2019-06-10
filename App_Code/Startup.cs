using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cs.Startup))]
namespace cs
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
