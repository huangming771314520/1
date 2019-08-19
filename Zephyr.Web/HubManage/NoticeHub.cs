using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zephyr.Areas.HubManage
{
    [HubName("NoticeService")]
    public class NoticeHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void OpenClient(string json)
        {
            Clients.Others.OpenClient(json);
        }

        public void FinishUpload(string json)
        {
            Clients.Others.FinishUpload(json);
        }

    }
}