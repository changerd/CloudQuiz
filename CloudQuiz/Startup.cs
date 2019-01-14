using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudQuiz.Startup))]
namespace CloudQuiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
