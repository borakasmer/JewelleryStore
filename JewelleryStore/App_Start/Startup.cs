using Microsoft.Owin;
using Owin;

namespace JewelleryStore
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}