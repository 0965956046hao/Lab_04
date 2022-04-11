using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab04_VoPhamNhutHao.Startup))]
namespace Lab04_VoPhamNhutHao
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
