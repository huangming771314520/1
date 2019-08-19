using PrintBarCodeService.WinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Topshelf;

namespace PrintBarCodeService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<PrintService>(s =>
                {
                    s.ConstructUsing(name => new PrintService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("该服务用来持续监听端口并且拿到数据后自动打码");
                x.SetDisplayName("Windows 条码打印服务");
                x.SetServiceName("PrintService");
            });
        }
    }
}
