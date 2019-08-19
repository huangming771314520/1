using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            string conStr = ConfigurationManager.AppSettings["API"].ToString();
            Model.ConfigInfoModel.API = conStr;

            string docDirName = ConfigurationManager.AppSettings["DocDirName"].ToString();
            if (!string.IsNullOrEmpty(docDirName))
            {
                Model.ConfigInfoModel.DocDirName = docDirName;
            }

            InitializeComponent();
        }
    }
}