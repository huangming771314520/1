using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Model;

namespace PreviewClient.View
{
    /// <summary>
    /// ProducePlanPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProducePlanPage : Page
    {
        private int ItemsCount = 0;//计划数量
        private int FocusIndex = 0;//默认焦点位置


        public ProducePlanPage()
        {
            InitializeComponent();
        }

        private void PageProducePlan_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载数据

            lblUserInfo.Content = string.Format(@"姓名：{0}    工号：{1}", StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName, StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode);

            ProducePlanInfoModel GetPPlanInfo = new BLL.ExtraBLL.ProducePlanInfoBLL().GetProducePlanInfo();
            if (!GetPPlanInfo.Result)
            {
                MessageBox.Show(GetPPlanInfo.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ItemsCount = StoreInfoModel.ProducePlanInfos.Count;
            List<object> PlanList = new List<object>();
            foreach (var item in StoreInfoModel.ProducePlanInfos)
            {
                PlanList.Add(new
                {
                    ContractCode = item.GetProjectProduceDetial.ContractCode,//合同编号
                    ProjectName = item.GetProject.ProjectName,//工程项目
                    ProcessName = item.GetProductProcessRoute.ProcessName + "(" + item.GetProjectProduceDetial.Quantity + ")",//工序名称
                    PlanDate = item.GetProjectProduceDetial.PlanedStartTime == null ? (item.GetProjectProduceDetial.PlanedFinishTime == null ? "--------" :
                        Convert.ToDateTime(item.GetProjectProduceDetial.PlanedFinishTime).ToString("---- - MM:dd HH:mm:ss")) : (item.GetProjectProduceDetial.PlanedFinishTime == null ?
                        Convert.ToDateTime(item.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ----") : (Convert.ToDateTime(item.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ") +
                        Convert.ToDateTime(item.GetProjectProduceDetial.PlanedFinishTime).ToString("HH:mm:ss"))),
                    Tag = item,
                    BackGround = Helpers.WpfHelper.GetRandomColorStr(),
                    PartNameAndFigure = item.GetProcessBOM.PartName + "【" + item.GetProcessBOM.PartFigureCode + "】"
                });
            }
            PlanCardItems.ItemsSource = PlanList;

            #endregion

            MainPage.Focus();


            var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】进入了生产计划页面！", StoreInfoModel.UserName));
            if (PlanList.Count > 0)
                //RemoveBorderThickness(FocusIndex);
                AddBorderThickness(0);
        }

        private void MainPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Back)
            {
                if (MessageBox.Show("是否关机", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.FileName = "shutdown.exe";
                    ps.Arguments = "-s -t 600";
                    Process.Start(ps);
                }
            }


            switch (e.Key)
            {
                case Key.Left:
                    RemoveBorderThickness(FocusIndex);
                    FocusIndex = (FocusIndex - 1).Equals(-1) ? (ItemsCount - 1) : (FocusIndex - 1);
                    AddBorderThickness(FocusIndex);
                    break;
                case Key.Right:
                    RemoveBorderThickness(FocusIndex);
                    FocusIndex = (FocusIndex + 1).Equals(ItemsCount) ? 0 : (FocusIndex + 1);
                    AddBorderThickness(FocusIndex);
                    break;
                case Key.Up:
                    var positionA = PlanCardScrollViewer.VerticalOffset;
                    PlanCardScrollViewer.ScrollToVerticalOffset(positionA - 60);
                    break;
                case Key.Down:
                    var positionB = PlanCardScrollViewer.VerticalOffset;
                    PlanCardScrollViewer.ScrollToVerticalOffset(positionB + 60);
                    break;
                case Key.Enter:
                    //保存当前所选计划
                    var obj = PlanCardItems.Items[FocusIndex];
                    var attr = obj.GetType().GetProperty("Tag");
                    StoreInfoModel.ProducePlanInfo = attr.GetValue(obj, null) as ProducePlanInfoModel.DataModel;

                    //下料
                    if (StoreInfoModel.WorkGroupInfo.GetWorkGroup.DepartID.Equals("0218"/*"0209"*/))
                    {

                        NavigationService.GetNavigationService(this).Navigate(new Uri(@"View/BlankingRecordPage.xaml", UriKind.Relative));
                    }
                    //非下料车间 需要 看图纸
                    else
                    {
                        var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】选择了生产计划ID为【{1}】、项目名称为【{2}】的生产计划！",
                            StoreInfoModel.UserName, StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ID, StoreInfoModel.ProducePlanInfo.GetProject.ProjectName));

                        //初始化-加载该生产计划所需物料详情
                        var getMaterialInfo = new BLL.ExtraBLL.MaterialInfoBLL().GetMaterialDetail();
                        if (!getMaterialInfo.Result)
                        {
                            MessageBox.Show(getMaterialInfo.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        //下载图纸文件
                        var drawResult = new BLL.ExtraBLL.ProducePlanInfoBLL().DownLoadDrawing();
                        if (!drawResult.Result)
                        {
                            MessageBox.Show(drawResult.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        NavigationService.GetNavigationService(this).Navigate(new Uri(@"View/ProduceProcePage.xaml", UriKind.Relative));
                    }
                    break;
                default: break;
            }

            e.Handled = true;
        }

        public void RemoveBorderThickness(int n)
        {
            ContentPresenter cp = PlanCardItems.ItemContainerGenerator.ContainerFromIndex(n) as ContentPresenter;
            List<Border> gds = Helpers.WpfHelper.FindVisualChildren<Border>(cp);
            gds[0].BorderBrush = new SolidColorBrush(Color.FromRgb(235, 243, 143));
        }

        public void AddBorderThickness(int n)
        {
            ContentPresenter cp = PlanCardItems.ItemContainerGenerator.ContainerFromIndex(n) as ContentPresenter;
            List<Border> gds = Helpers.WpfHelper.FindVisualChildren<Border>(cp);
            if (gds.Count > 0)

                gds[0].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }


    }
}
