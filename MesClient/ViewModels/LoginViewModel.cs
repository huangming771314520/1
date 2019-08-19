using MesClient.API;
using MesClient.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MesClient.ViewModels
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

        private string barCode = string.Empty;

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode
        {
            get
            {
                return barCode;
            }
            set
            {
                barCode = value;
                OnPropertyChanged(nameof(BarCode));
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
            BarCode = "";

            if (string.IsNullOrEmpty(barCode))
            {
                LoginMsg = @"请扫用户条码！";
                return;
            }
            else
            {
                try
                {
                    var result = ApiManager.GetUserDetailInfoByBarCode(barCode);
                    if (result.Result)
                    {
                        WindowHelper.ShowPagePlanCard();

                        Task.Run(() =>
                        {
                            while (true)
                            {
                                var temp = ApiManager.GetUsetLoginStatus();
                                if (!temp.Result)
                                {
                                    MessageBox.Show(temp.Msg, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    Process.Start(Assembly.GetExecutingAssembly().Location);
                                    Environment.Exit(0);
                                }
                            }
                        });
                    }
                    else
                    {
                        LoginMsg = result.Msg;
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
