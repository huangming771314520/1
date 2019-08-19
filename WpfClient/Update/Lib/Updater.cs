using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Update.Model;

namespace Update.Lib
{
    public class Updater
    {
        public static bool CheckUpdate()
        {
            XmlDocument docA = new XmlDocument();
            docA.Load("LocalVersion.xml");

            RootLocal.CurrentVersion = new Version(docA.SelectSingleNode("RootLocal/Version").InnerText.Trim());
            RootLocal.Workshop = docA.SelectSingleNode("RootLocal/Workshop").InnerText.Trim();
            RootLocal.ServerUrl = docA.SelectSingleNode("RootLocal/ServerUrl").InnerText.Trim();
            RootLocal.AppName = Path.GetFileNameWithoutExtension(docA.SelectSingleNode("RootLocal/AppName").InnerText.Trim());
            RootLocal.XmlName = docA.SelectSingleNode("RootLocal/XmlName").InnerText.Trim();
            RootLocal.ZipName = docA.SelectSingleNode("RootLocal/ZipName").InnerText.Trim();

            string url_NewVersionXml = string.Format(@"{0}/{1}/{2}", RootLocal.ServerUrl, RootLocal.Workshop, "NewVersion.xml");
            Stream stream_NewVersionXml = WebRequest.Create(url_NewVersionXml).GetResponse().GetResponseStream();

            XmlDocument docB = new XmlDocument();
            docB.Load(stream_NewVersionXml);

            RootServer.LatestVersion = new Version(docB.SelectSingleNode("RootServer/Version").InnerText.Trim());

            if (RootServer.LatestVersion > RootLocal.CurrentVersion)
            {
                string url_UpdateXml = string.Format(@"{0}/{1}/v{2}/{3}", RootLocal.ServerUrl, RootLocal.Workshop, RootServer.LatestVersion, RootLocal.XmlName);
                Stream stream_UpdateXml = WebRequest.Create(url_UpdateXml).GetResponse().GetResponseStream();

                XmlDocument docC = new XmlDocument();
                docC.Load(stream_UpdateXml);

                UpdateInfo.AppName = docC.SelectSingleNode("UpdateInfo/AppName").InnerText.Trim();
                UpdateInfo.AppVersion = new Version(docC.SelectSingleNode("UpdateInfo/AppVersion").InnerText.Trim());
                UpdateInfo.UpdateTime = Convert.ToDateTime(docC.SelectSingleNode("UpdateInfo/UpdateTime").InnerText.Trim());
                UpdateInfo.Describe = docC.SelectSingleNode("UpdateInfo/Describe").InnerText.Trim();
                UpdateInfo.UpdateType = docC.SelectSingleNode("UpdateInfo/UpdateType").InnerText.Trim();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void StartUpdate()
        {
            string appDirPath = Application.StartupPath;
            string tempDirName = Guid.NewGuid().ToString();
            string updateDirPath = Path.Combine(appDirPath, "Update Pack", tempDirName);

            Directory.CreateDirectory(updateDirPath);

            string xmlPath = Path.Combine(updateDirPath, "LocalVersion.xml");
            string exePath = Path.Combine(updateDirPath, "Update.exe");
            File.Copy(Path.Combine(appDirPath, "LocalVersion.xml"), xmlPath, true);
            File.Copy(Path.Combine(appDirPath, "Update.exe"), exePath, true);

            List<string> arguments = new List<string>();
            arguments.Add("\"" + appDirPath + "\"");
            arguments.Add("\"" + tempDirName + "\"");

            var info = new ProcessStartInfo(exePath);
            info.UseShellExecute = true;
            info.WorkingDirectory = exePath.Substring(0, exePath.LastIndexOf(Path.DirectorySeparatorChar));
            info.Arguments = string.Join(" ", arguments);
            Process.Start(info);
        }

    }
}
