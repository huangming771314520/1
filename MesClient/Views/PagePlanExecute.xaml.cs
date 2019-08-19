using MesClient.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace MesClient.Views
{
    /// <summary>
    /// PagePlanExecute.xaml 的交互逻辑
    /// </summary>
    public partial class PagePlanExecute : Page
    {
        AxMxDrawXLib.AxMxDrawX axMxDrawX;
        private string firstDrawPath = string.Empty;

        public PagePlanExecute()
        {
            InitializeComponent();

            MxDrawXLib.MxDrawApplication app = new MxDrawXLib.MxDrawApplication();
            app.InitMxDrawOcx("", "湖北洪城通用机械股份有限公司", "湖北洪城通用机械股份有限公司MES系统", "18971438760", "010ADE5E0DA2A305784A00001C220CA8320E5B4BFBEAD6B3AC5926EDF3E53A46B314A88852C4DCDE3DECF60325710000070A796586FBEA03F9720000242A0EC1C00B65A20465E37C87F062B8CD76CF30A2C4C5C3553DA823D5795527F89D18F0BDC04AA73C190000262A91655C85AC1354C3859CDC2C0E84B81418DB4CE1E8E909AA2C2EDB1E6ECB86F3657BCE0345533AF70000232A0CA8320E5B4BFBEAD6B3AC5926EDF3E53A46B314A88852C43C26D984C0A4C1F5AB63AFDFB544A59200000B12CBAB9FF17B61A88DCCFB24112739F7860000080A102C07683817AE710000");

            axMxDrawX = new AxMxDrawXLib.AxMxDrawX();

            ((ISupportInitialize)axMxDrawX).BeginInit();
            WindowsFormsHost host = new WindowsFormsHost() { Child = axMxDrawX };
            ((ISupportInitialize)axMxDrawX).EndInit();

            axMxDrawX.ShowCommandWindow = false;
            axMxDrawX.ShowModelBar = false;
            axMxDrawX.ShowStatusBar = false;
            axMxDrawX.ShowToolBars = false;
            axMxDrawX.Padding = new Padding(0);
            axMxDrawX.Margin = new Padding(0);
            axMxDrawX.IsFirstRunPan = true;
            axMxDrawX.IsDrawCoord = false;
            axMxDrawX.AutoZoomAll = true;
            axMxDrawX.Iniset = "DisplayPrecision=999";

            //axMxDrawX.UserName = "湖北洪城通用机械股份有限公司";
            //axMxDrawX.UserPhone = "18971438760";
            //axMxDrawX.UserData = "010ADE5E0DA2A305784A00001C220CA8320E5B4BFBEAD6B3AC5926EDF3E53A46B314A88852C4DCDE3DECF60325710000070A796586FBEA03F9720000242A0EC1C00B65A20465E37C87F062B8CD76CF30A2C4C5C3553DA823D5795527F89D18F0BDC04AA73C190000262A91655C85AC1354C3859CDC2C0E84B81418DB4CE1E8E909AA2C2EDB1E6ECB86F3657BCE0345533AF70000232A0CA8320E5B4BFBEAD6B3AC5926EDF3E53A46B314A88852C43C26D984C0A4C1F5AB63AFDFB544A59200000B12CBAB9FF17B61A88DCCFB24112739F7860000080A102C07683817AE710000";
            //axMxDrawX.UserSoftwareName = "湖北洪城通用机械股份有限公司MES系统";

            mxDraw.Children.Add(host);
            DataContext = new PlanExecuteViewModel(axMxDrawX, ref firstDrawPath);
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(firstDrawPath))
            {
                axMxDrawX.OpenDwgFile(firstDrawPath);
            }
        }

        private void MxDraw_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            axMxDrawX.Focus();
        }

    }
}
