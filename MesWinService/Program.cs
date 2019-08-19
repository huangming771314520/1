using log4net.Config;
using MesWinService.Services;
using System;
using System.IO;
using Topshelf;

namespace MesWinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            HostFactory.Run(x =>
            {
                x.Service<GeneratePartSimhashService>(s =>
                {
                    s.ConstructUsing(name => new GeneratePartSimhashService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("洪城Mes系统数据同步服务，如果停止该服务，相似度数据将无法自动计算。");
                x.SetDisplayName("MesWinService");
                x.SetServiceName("MesWinService");
            });
        }
    }
}
