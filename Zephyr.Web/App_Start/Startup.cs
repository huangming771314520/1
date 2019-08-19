using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Zephyr.Areas.App_Start.Startup))]

namespace Zephyr.Areas.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Map("/messageHub", map =>
            //{
            //    map.RunSignalR(new Microsoft.AspNet.SignalR.HubConfiguration { EnableJavaScriptProxies = true });
            //});
            //app.Map("/NoticeHub", map =>
            //{
            //    map.RunSignalR(new Microsoft.AspNet.SignalR.HubConfiguration { EnableJavaScriptProxies = true });
            //});

            app.Map("/signalr", map =>
            {
                map.RunSignalR(new Microsoft.AspNet.SignalR.HubConfiguration { EnableJavaScriptProxies = true });
            });

        }
    }
}
