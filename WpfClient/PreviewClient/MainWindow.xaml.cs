using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreviewClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public static Point GetCursorPos()
        {
            Point point = new Point();
            GetCursorPos(ref point);
            return point;
        }

        public MainWindow()
        {
            InitializeComponent();

            var result = BLL.Helpers.ClientHelper.GetConnectionStatus();
            if (!result.Result)
            {
                System.Windows.MessageBox.Show("网络异常！");
            }
            //HideTask(false);
            //Task.Delay(1000);
            //FullOrMin(this.WindowMain);
        }
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

                //获取窗口句柄 
                var handle = new WindowInteropHelper(window).Handle;

                //获取当前显示器屏幕
                Screen screen = Screen.FromHandle(handle);

                //调整窗口最大化,全屏的关键代码就是下面3句
                window.MaxWidth = screen.Bounds.Width;
                window.MaxHeight = screen.Bounds.Height;
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
            //var result = BLL.Helpers.ClientHelper.GetUsetLoginStatus();
            //if (!result.Result)
            //{
            //    System.Windows.MessageBox.Show("网络异常！");
            //}
        }

        public static void HideTask(bool isHide)
        {
            try
            {
                IntPtr trayHwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
                //IntPtr hStar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Button", null);

                if (isHide)
                {
                    ShowWindow(trayHwnd, 0);
                    //ShowWindow(hStar, 0);
                }
                else
                {
                    ShowWindow(trayHwnd, 1);
                    //ShowWindow(hStar, 1);
                }
            }
            catch { }
        }

    }
}
