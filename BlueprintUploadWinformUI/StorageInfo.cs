using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueprintUploadWinformUI
{
    public class StorageInfo
    {

        public static HubConnection hubConnection { get; set; }

        public static IHubProxy hubProxy { get; set; }

    }
}
