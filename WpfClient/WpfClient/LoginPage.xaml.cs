using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfClient
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TxtUserCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TxtUserCode_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            string userCode = txtUserCode.Text.Trim();

            //Enter-回车
            if (key.Equals(6))
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    return;
                }

                //模拟登陆
                var GetWorkGroupInfoModel = new BLL.ExtraBLL.WorkGroupInfoBLL().GetWorkGroupInfo(userCode);
                if (GetWorkGroupInfoModel.Result)
                {
                    string targetUri = string.Empty;

                    //查班组负责所有设备的保养计划
                    var obj = new BLL.ExtraBLL.EquipmentInfoBLL().GetEquipmentInfo();

                    //如若当天有保养计划,则进入保养计划页面
                    if (obj.Data.Count > 0)
                    {
                        targetUri = @"EQPMaintainPlanPage.xaml";
                    }
                    //否则进入计划页面
                    else
                    {
                        targetUri = @"ProductPlanPage.xaml";
                    }

                    lblMsg.Foreground = new SolidColorBrush(Color.FromRgb(93, 229, 32));
                    lblMsg.Content = @"登录成功！";
                    NavigationService.GetNavigationService(this).Navigate(new Uri(targetUri, UriKind.Relative));
                }
                else
                {
                    //MessageBox.Show(GetWorkGroupInfoModel.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    lblMsg.Foreground = new SolidColorBrush(Color.FromRgb(247, 125, 44));
                    lblMsg.Content = GetWorkGroupInfoModel.Msg;
                    return;
                }
            }
        }
    }
}
