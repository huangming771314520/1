using MesWpfClient.AppStart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MesWpfClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                if (CheckUpdate())
                {
                    string exePath = AppDomain.CurrentDomain.BaseDirectory + "/Update.exe";
                    var info = new ProcessStartInfo(exePath);
                    info.UseShellExecute = true;
                    Process.Start(info);
                }
                else
                {
                    new SerialPortScanCode().Start();
                    App app = new App();
                    app.InitializeComponent();
                    app.Run();
                }
            }
            else
            {
                new SerialPortScanCode().Start();
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
        }

        private static bool CheckUpdate()
        {
            XmlDocument docA = new XmlDocument();
            docA.Load("LocalVersion.xml");

            var CurrentVersion = new Version(docA.SelectSingleNode("RootLocal/Version").InnerText.Trim());

            var Workshop = docA.SelectSingleNode("RootLocal/Workshop").InnerText.Trim();
            var ServerUrl = docA.SelectSingleNode("RootLocal/ServerUrl").InnerText.Trim();

            string url_NewVersionXml = string.Format(@"{0}/{1}/{2}", ServerUrl, Workshop, "NewVersion.xml");
            Stream stream_NewVersionXml = WebRequest.Create(url_NewVersionXml).GetResponse().GetResponseStream();

            XmlDocument docB = new XmlDocument();
            docB.Load(stream_NewVersionXml);

            var LatestVersion = new Version(docB.SelectSingleNode("RootServer/Version").InnerText.Trim());

            return LatestVersion > CurrentVersion;
        }



    }
}
