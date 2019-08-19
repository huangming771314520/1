using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PreviewClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //MessageBox.Show("测试代码控制启动");

            //Version version = new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion);
            //Ezhu.AutoUpdater.Updater.GetLocalVersion();
            //Ezhu.AutoUpdater.Updater.CheckUpdateStatus();
            MainWindow mw = new MainWindow();

            mw.Show();

        }
    }
}
