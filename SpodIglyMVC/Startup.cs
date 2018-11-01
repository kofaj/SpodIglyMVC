using Owin;

namespace SpodIglyMVC
{
    public partial class Startup
    {        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}