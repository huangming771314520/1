using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace BlueprintUploadWinformUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigInfo.API = ConfigurationManager.AppSettings["API"].ToString();
            ConfigInfo.SignalRURI = ConfigurationManager.AppSettings["SignalRURI"].ToString();
            ConfigInfo.FtpSite = ConfigurationManager.AppSettings["FtpSite"].ToString();
            ConfigInfo.FtpUid = ConfigurationManager.AppSettings["FtpUid"].ToString();
            ConfigInfo.FtpPwd = ConfigurationManager.AppSettings["FtpPwd"].ToString();
            ConfigInfo.LocalKey = ConfigurationManager.AppSettings["LocalKey"].ToString();
            ConfigInfo.FtpSiteDicPath = ConfigurationManager.AppSettings["FtpSiteDicPath"].ToString();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resourceName = "BlueprintUploadWinformUI.Resources." + new AssemblyName(args.Name).Name + ".dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

    }
}
