using System.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCApp.Startup))]
namespace MVCApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseFacebookAuthentication(
                appId: ConfigurationManager.AppSettings["fbAppId"],
                appSecret: ConfigurationManager.AppSettings["fbAppSecret"]);
            app.UseGoogleAuthentication(
                clientId: ConfigurationManager.AppSettings["googleClientId"],
                clientSecret: ConfigurationManager.AppSettings["googleSecret"]);
            
        }
    }
}
