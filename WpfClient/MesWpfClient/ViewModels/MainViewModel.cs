using MesWpfClient.API;
using MesWpfClient.Commands;
using Model;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace MesWpfClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        #endregion


        private MainWindow window;

        public MainViewModel(MainWindow _window)
        {
            window = _window;

            WindowHelper.CurrentWindow = this;

            WindowHelper.ShowPageLogin();
            //WindowHelper.ShowPageProduceProce();
            //WindowHelper.ShowPageBlankingRecord();
            //WindowHelper.ShowPageMaintainPlan();

            var result = BLL.Helpers.ClientHelper.GetConnectionStatus();
            if (!result.Result)
            {
                MessageBox.Show("网络异常！");
            }

            //HideTask(false);
            //FullOrMin(window);
        }

        //******************************************************************************************************//

        private Page currentPage;

        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                window.Activate();
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        //******************************************************************************************************//

        private void FullOrMin(Window window)
        {
            //如果是窗口,则全屏
            if (window.WindowState == WindowState.Normal)
            {
                //变成无边窗体
                window.WindowState = WindowState.Normal;//假如已经是Maximized，就不能进入全屏，所以这里先调整状态
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.Topmost = true;//最大化后总是在最上面

                window.MaxWidth = ConfigInfoModel.WinScreenWidth;
                window.MaxHeight = ConfigInfoModel.WinScreenHeight;
                window.WindowState = WindowState.Maximized;

                //解决切换应用程序的问题
                window.Activated += new EventHandler(window_Activated);
                window.Deactivated += new EventHandler(window_Deactivated);
            }

        }

        private void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = false;
        }

        private void window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = true;
        }

        public static void HideTask(bool isHide)
        {
            try
            {
                IntPtr trayHwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
                ShowWindow(trayHwnd, (uint)(isHide ? 0 : 1));
            }
            catch
            {

            }
        }

        //******************************************************************************************************//

    }
}
