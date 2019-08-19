using MesWpfClient.AppStart;
using MesWpfClient.ViewModels;
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
using System.Windows.Shapes;

namespace MesWpfClient.Views.WinViews
{
    /// <summary>
    /// WindowMaterialScan.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMaterialScan : Window
    {
        /// <summary>
        /// 1 - 继续/下一步    0 - 取消/返回
        /// </summary>
        public int OperateType { get; set; }

        public WindowMaterialScan(string barCode = "")
        {
            InitializeComponent();

            new SerialPortScanCode().Start();
            DataContext = new MaterialScanViewModel(this, barCode);
        }

        private void BarCode_Loaded(object sender, RoutedEventArgs e)
        {
            barCode.Focus();
        }
    }
}
