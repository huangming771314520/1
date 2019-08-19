using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreviewClient.View
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void TxtUserCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string userCode = txtUserCode.Text.Trim();

            if (e.Key.Equals(Key.Enter))
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    return;
                }

                try
                {
                    var loginResult = new BLL.ExtraBLL.WorkGroupInfoBLL().GetWorkGroupInfo(userCode);
                    if (loginResult.Result)
                    {
                        string targetUri = string.Empty;

                        //查班组负责所有设备的保养计划
                        var obj = new BLL.ExtraBLL.EquipmentInfoBLL().GetEquipmentInfo();

                        //如若当天有保养计划,则进入保养计划页面
                        if (obj.Data.Count > 0)
                        {
                            targetUri = @"View/MaintainPlanPage.xaml";
                        }
                        //否则进入计划页面
                        else
                        {
                            targetUri = @"View/ProducePlanPage.xaml";
                        }

                        var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】成功登录客户端！", Model.StoreInfoModel.UserName));

                        Thread thread = new Thread(new ThreadStart(delegate
                        {
                            //CheckForIllegalCrossThreadCalls = false;

                            Model.ResultModel loginStatus = new Model.ResultModel();

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

                        lblMsg.Text = @"登录成功！";
                        NavigationService.GetNavigationService(this).Navigate(new Uri(targetUri, UriKind.Relative));
                    }
                    else
                    {
                        lblMsg.Text = loginResult.Msg;
                        lblMsg.Foreground = Brushes.Red;
                        var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<Model.Models.ClientOperateLog>()
                        {
                            new Model.Models.ClientOperateLog()
                            {
                                Content =string.Format(@"用户【{0}】登录客户端失败！", userCode),
                                CreatePersonCode = userCode,
                                CreatePersonName = userCode
                            }
                        });
                    }
                }
                catch (Exception ex)
                {

                    lblMsg.Text = ex.Message;
                MessageBox.Show(ex.Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        private void PageLogin_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserCode.Focus();
            //LogoImage.Source= new BitmapImage(new Uri(@"../Resources/Img/logo.png", UriKind.RelativeOrAbsolute));
        }
    }
}
