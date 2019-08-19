using MesWpfClient.API;
using MesWpfClient.Commands;
using Model;
using System;
using System.Threading;
using System.Windows;

namespace MesWpfClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginViewModel()
        {
            //绑定登录扫码文本框事件
            LoginCommand = new DelegateCommand(uCode => Login(uCode));
        }

        //********************************************************************************************************//

        private string loginMsg = string.Empty;

        /// <summary>
        /// 登录结果
        /// </summary>
        public string LoginMsg
        {
            get
            {
                return loginMsg;
            }
            set
            {
                loginMsg = value;
                OnPropertyChanged(nameof(LoginMsg));
            }
        }

        //********************************************************************************************************//

        public DelegateCommand LoginCommand { get; set; }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="uCode"></param>
        public void Login(object uCode)
        {
            string barCode = uCode.ToString();
            if (string.IsNullOrEmpty(barCode))
            {
                LoginMsg = @"请扫用户条码！";
                return;
            }
            else
            {
                try
                {
                    var loginResult = new BLL.ExtraBLL.WorkGroupInfoBLL().GetWorkGroupInfo(barCode);
                    LoginMsg = loginResult.Msg;

                    if (loginResult.Result)
                    {
                        WindowHelper.ShowPageProducePlan();

                        Thread thread = new Thread(new ThreadStart(delegate
                        {
                            ResultModel loginStatus = new ResultModel();

                            while (true)
                            {
                                loginStatus = BLL.Helpers.ClientHelper.GetUsetLoginStatus();
                                if (!loginStatus.Result)
                                {
                                    MessageBox.Show(loginStatus.Msg, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                    Environment.Exit(0);
                                }
                            }

                        }));
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    LoginMsg = ex.Message;
                }
            }
        }

    }
}
