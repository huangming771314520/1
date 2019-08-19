using MesClient.AppStart;
using MesClient.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace MesClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (!File.Exists(@"AppSetting.ini"))
            {
                File.AppendAllText("AppSetting.ini", "[setting]\r\n;串口名\r\nSerialPortName=COM1\r\n;图纸移动偏移量\r\nMoveOffset=70\r\n;放大\r\nZoomIn=150\r\n;缩小\r\nZoomOut=-120\r\n;打码机驱动名(已被弃用)\r\nPrintMachineName=Honeywell PC42t (203 dpi) - DP");
            }

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
            Stream stream_NewVersionXml = HttpHelper.GetStreamByUrl(url_NewVersionXml, 1000 * 60);

            XmlDocument docB = new XmlDocument();
            docB.Load(stream_NewVersionXml);

            var LatestVersion = new Version(docB.SelectSingleNode("RootServer/Version").InnerText.Trim());

            return LatestVersion > CurrentVersion;
        }

    }
}
