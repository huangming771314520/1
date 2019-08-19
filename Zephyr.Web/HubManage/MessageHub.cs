using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Zephyr.Areas.HubManage
{
    [HubName("MessageService")]
    public class MessageHub : Hub
    {
        public static Dictionary<string, string> UserList = new Dictionary<string, string>();
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BomID">发送BomID至客户端</param>
        /// <param name="FigureNumber">发送FigureNumber至客户端</param>
        /// <param name="UserCode">UserCode该用户接受信息</param>
        public void Touch(int BomID, string FigureNumber,string UserCode)
        {
            if (UserList.Keys.Contains(UserCode))
            {
                Clients.Client(UserList[UserCode]).touch(BomID, FigureNumber);
            }
            //Clients.Others.touch(BomID, FigureNumber);
        }

        public void SendLogin(string UserCode)
        {
            if (UserList.Keys.Contains(UserCode))
            {
                UserList[UserCode] = Context.ConnectionId;
            }
            else
            {
                UserList.Add(UserCode, Context.ConnectionId);
            }
        }

        public void FinishUpload(string UserCode)
        {
            Clients.Others.finishUpload(UserCode);
        }
    }
}