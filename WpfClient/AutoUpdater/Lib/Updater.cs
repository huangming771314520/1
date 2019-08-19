using System;
using System.IO;
using System.Windows;


using System.Xml.Linq;
using System.Xml;
using System.Threading;

namespace Ezhu.AutoUpdater
{
    public class Updater
    {
        private static Updater _instance;
        private static string localVersion;
        private static string fileXmlUrl;
        private static string fileUrl;

        public static Updater Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Updater();
                }
                return _instance;
            }
        }

        public static void CheckUpdateStatus(ref bool isStartUpdate)
        {
            bool temp = false;

            Stream stream = System.Net.WebRequest.Create(fileXmlUrl).GetResponse().GetResponseStream();

            XDocument xDoc = XDocument.Load(stream);
            UpdateInfo updateInfo = new UpdateInfo();
            XElement root = xDoc.Element("UpdateInfo");
            updateInfo.AppName = root.Element("AppName").Value;
            updateInfo.AppVersion = root.Element("AppVersion") == null || string.IsNullOrEmpty(root.Element("AppVersion").Value) ? null : new Version(root.Element("AppVersion").Value);
            updateInfo.RequiredMinVersion = root.Element("RequiredMinVersion") == null || string.IsNullOrEmpty(root.Element("RequiredMinVersion").Value) ? null : new Version(root.Element("RequiredMinVersion").Value);
            updateInfo.Desc = root.Element("Desc").Value;
            updateInfo.MD5 = Guid.NewGuid();

            stream.Close();
            temp = Updater.Instance.IsStartUpdate(updateInfo);

            isStartUpdate = temp;
        }

        //获取本地版本信息
        public static void GetLocalVersion()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "LocalVersion.xml";
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(Path, settings);
            doc.Load(reader);

            // 得到根节点bookstore
            XmlNode xn = doc.SelectSingleNode("RootLocal");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)item;

                if (xe.Name == "Version")
                {
                    // 得到版本
                    localVersion = xe.GetAttribute("no").ToString();

                    //string No = xe.GetAttribute("No").ToString();
                }
                if (xe.Name == "ServerDownUrl")
                {
                    // 得到下载地址
                    fileUrl = xe.GetAttribute("url").ToString();

                    Constants.RemoteUrl = fileUrl;
                }
                if (xe.Name == "XmlDownUrl")
                {
                    // 得到下载地址                   
                    fileXmlUrl = xe.GetAttribute("url").ToString();
                }

            }

            reader.Close();

        }



        public bool IsStartUpdate(UpdateInfo updateInfo)
        {
            if (updateInfo.RequiredMinVersion != null && Updater.Instance.CurrentVersion < updateInfo.RequiredMinVersion)
            {
                //当前版本比需要的版本小，不更新
                return false;
            }

            if (Updater.Instance.CurrentVersion >= updateInfo.AppVersion)
            {
                //当前版本是最新的，不更新
                return false;
            }

            //更新程序复制到缓存文件夹
            string appDir = System.IO.Path.Combine(System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0, System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf(System.IO.Path.DirectorySeparatorChar)));
            string updateFileDir = System.IO.Path.Combine(System.IO.Path.Combine(appDir.Substring(0, appDir.LastIndexOf(System.IO.Path.DirectorySeparatorChar))), "Update");
            if (!Directory.Exists(updateFileDir))
            {
                Directory.CreateDirectory(updateFileDir);
            }
            updateFileDir = System.IO.Path.Combine(updateFileDir, updateInfo.MD5.ToString());
            if (!Directory.Exists(updateFileDir))
            {
                Directory.CreateDirectory(updateFileDir);
            }

            string xmlPath = System.IO.Path.Combine(updateFileDir, "LocalVersion.xml");
            string exePath = System.IO.Path.Combine(updateFileDir, "AutoUpdater.exe");
            File.Copy(System.IO.Path.Combine(appDir, "LocalVersion.xml"), xmlPath, true);
            File.Copy(System.IO.Path.Combine(appDir, "AutoUpdater.exe"), exePath, true);

            var info = new System.Diagnostics.ProcessStartInfo(exePath);
            info.UseShellExecute = true;
            info.WorkingDirectory = exePath.Substring(0, exePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar));
            updateInfo.Desc = updateInfo.Desc;
            info.Arguments = "update " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(CallExeName)) + " " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(updateFileDir)) + " " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(appDir)) + " " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(updateInfo.AppName)) + " " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(updateInfo.AppVersion.ToString())) + " " + (string.IsNullOrEmpty(updateInfo.Desc) ? "" : Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(updateInfo.Desc)));
            System.Diagnostics.Process.Start(info);

            return true;
        }

        public bool UpdateFinished = false;

        private string _callExeName;
        public string CallExeName
        {
            get
            {
                if (string.IsNullOrEmpty(_callExeName))
                {
                    _callExeName = System.Reflection.Assembly.GetEntryAssembly().Location.Substring(System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1).Replace(".exe", "");
                }
                return _callExeName;
            }
        }

        /// <summary>
        /// 获得当前应用软件的版本
        /// </summary>
        public virtual Version CurrentVersion
        {
            get { return new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion); }
        }

        /// <summary>
        /// 获得当前应用程序的根目录
        /// </summary>
        public virtual string CurrentApplicationDirectory
        {
            get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); }
        }
    }
}
