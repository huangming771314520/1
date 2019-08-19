using MesClient.API;
using MesClient.Commands;
using MesClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MesClient.ViewModels
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

            WindowHelper.MainViewModel = this;
            WindowHelper.ShowPageLogin();

            //ApiManager.GetUserDetailInfoByBarCode("USER00000227");//下料
            //ApiManager.GetUserDetailInfoByBarCode("USER00000141");//非下料
            //ApiManager.GetUserDetailInfoByBarCode("USER00000142");//非下料
            //ApiManager.GetWorkingTicketData();

            ////WindowHelper.ShowPagePlanCard();

            //StoreInfo.CurrentWorkingTicket = StoreInfo.WorkingTickets[0];
            //ApiManager.GetMaterialInfo();

            //StoreInfo.CurrentPlanFiles.Add(new Entitys.FileInfoEntity()
            //{
            //    DocName = "2Z10ZDMFEN0002_4_1_1_洪城装配图文档.DWG",
            //    FileName = "aaa.DWG",
            //    WebAddressType = EnumManager.WebAddressTypeEnum.FTP
            //});
            //StoreInfo.CurrentPlanFiles.Add(new Entitys.FileInfoEntity()
            //{
            //    DocName = "4Z10ZDMFEN00020001_1_1_1_洪城零组件图纸文档.DWG",
            //    FileName = "bbb.DWG",
            //    WebAddressType = EnumManager.WebAddressTypeEnum.FTP
            //});
            //StoreInfo.CurrentPlanFiles.Add(new Entitys.FileInfoEntity()
            //{
            //    DocName = "4Z10ZDMFEN00020002_2_1_1_洪城零组件图纸文档.DWG",
            //    FileName = "ccc.DWG",
            //    WebAddressType = EnumManager.WebAddressTypeEnum.FTP
            //});

            ////记录开始生产日志
            //var logResult = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.Start);
            ////记录计划执行员工
            //var operatorResult = ApiManager.SetProduceOperator(new List<string>() { "USER00000001", "USER00000002" }, Convert.ToInt32(logResult.Data));
            ////录入计划实际开始时间
            //var actualDateResult = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.Start);

            //WindowHelper.ShowPagePlanExecute();

            var result = ApiManager.GetConnectionStatus();
            if (!result.Result)
            {
                MessageBox.Show("网络异常！");
            }

            HideTask(false);
            FullOrMin(window);
        }

        private Page currentPage;

        /// <summary>
        /// 当前显示的页面
        /// </summary>
        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                window.Activate();
                currentPage = value;
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

                window.MaxWidth = ConfigInfo.WinScreenWidth;
                window.MaxHeight = ConfigInfo.WinScreenHeight;
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
