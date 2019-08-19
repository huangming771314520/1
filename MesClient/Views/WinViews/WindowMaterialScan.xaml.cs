using MesClient.AppStart;
using MesClient.ViewModels;
using System.Windows;

namespace MesClient.Views.WinViews
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
            //barCode.Focus();
        }
    }
}
