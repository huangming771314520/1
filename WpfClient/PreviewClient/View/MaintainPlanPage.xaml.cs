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
using Model;

namespace PreviewClient.View
{
    /// <summary>
    /// MaintainPlanPage.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainPlanPage : Page
    {
        private int ItemsCount = 0;//计划数量
        private int FocusIndex = 0;//默认焦点位置


        public MaintainPlanPage()
        {
            InitializeComponent();
        }

        private void PageMaintainPlan_Loaded(object sender, RoutedEventArgs e)
        {
            List<object> listData = new List<object>();

            listData.Add(new
            {
                EquipmentName = "返回",
                MaintainInfo = "",
                PlanedContent = "",
                PlanDate = "",
                BackGround = "#FF498CF1"
            });

            //#498CF1-蓝色
            //#FA5C3C-橙色

            foreach (var item in StoreInfoModel.EquipmentInfos)
            {
                listData.Add(new
                {
                    EquipmentName = item.GetEquipment.EquipmentName,
                    MaintainInfo = item.GetEquipmentMaintenancePlan.MaintenanceName + "(" + (item.GetEquipmentMaintenancePlan.ActualStartTime == null ? "未开始" : "已开始") + ")",
                    PlanedContent = item.GetEquipmentMaintenancePlan.PlanedContent,
                    PlanDate = item.GetEquipmentMaintenancePlan.PlanedStartTime == null ? "" : Convert.ToDateTime(item.GetEquipmentMaintenancePlan.PlanedStartTime).ToString("yyyy-MM-dd"),
                    Tag = item,
                    BackGround = item.GetEquipmentMaintenancePlan.ActualStartTime == null ? "#498CF1" : "#FA5C3C"
                });
            }

            ItemsCount = listData.Count;
            PlanCardItems.ItemsSource = listData;

            MainPage.Focus();

            var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】进入了设备保养计划页面！", Model.StoreInfoModel.UserName));
        }

        private void MainPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    var positionA = PlanCardScrollViewer.VerticalOffset;
                    PlanCardScrollViewer.ScrollToVerticalOffset(positionA - 60);
                    break;
                case Key.Down:
                    var positionB = PlanCardScrollViewer.VerticalOffset;
                    PlanCardScrollViewer.ScrollToVerticalOffset(positionB + 60);
                    break;
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
                case Key.Enter:
                    if (FocusIndex.Equals(0))
                    {
                        var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】在设备保养计划页面选择了“返回”！", Model.StoreInfoModel.UserName));
                        NavigationService.GetNavigationService(this).Navigate(new Uri(@"View/ProducePlanPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        //保存当前所选计划
                        var obj = PlanCardItems.Items[FocusIndex];
                        var attr = obj.GetType().GetProperty("Tag");
                        StoreInfoModel.EquipmentInfo = attr.GetValue(obj, null) as EquipmentInfoModel.DataModel;

                        //是否正在保养，是将结束
                        if (StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.ActualStartTime != null)
                        {
                            //if (MessageBox.Show("是否立即结束？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                            //{
                            ResultModel result = new BLL.ExtraBLL.EquipmentInfoBLL().EQPMaintenancePlan(1);
                            //结束保养成功
                            if (result.Result)
                            {
                                var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<string>()
                                {
                                    string.Format(@"用户【{0}】选择了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName),
                                    string.Format(@"用户【{0}】结束了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName)
                                });

                                StoreInfoModel.EquipmentInfos.RemoveAt(FocusIndex - 1);
                                //planGridIndex = 0;
                                //planInfoContainer.Children.Clear();

                            }
                            else
                            {
                                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);

                                var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<string>()
                                {
                                    string.Format(@"用户【{0}】选择了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName),
                                    string.Format(@"用户【{0}】想要结束设备【{1}】的保养计划，但是失败了！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName)
                                });
                            }
                            //}
                        }
                        //是否正在保养，否将开始
                        else
                        {
                            //if (MessageBox.Show("是否立即开始？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                            //{
                            ResultModel result = new BLL.ExtraBLL.EquipmentInfoBLL().EQPMaintenancePlan(0);
                            //开始保养成功
                            if (result.Result)
                            {
                                ContentPresenter cp = PlanCardItems.ItemContainerGenerator.ContainerFromIndex(FocusIndex) as ContentPresenter;
                                List<Border> gds = Helpers.WpfHelper.FindVisualChildren<Border>(cp);
                                List<TextBlock> tbs = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cp);

                                gds[0].Background = new SolidColorBrush(Color.FromRgb(250, 92, 60));
                                tbs[1].Text = StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.MaintenanceName + "(已开始)";

                                var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<string>()
                                {
                                    string.Format(@"用户【{0}】选择了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName),
                                    string.Format(@"用户【{0}】开始了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName)
                                });

                            }
                            else
                            {
                                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);

                                var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<string>()
                                {
                                    string.Format(@"用户【{0}】选择了设备【{1}】的保养计划！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName),
                                    string.Format(@"用户【{0}】想要开始设备【{1}】的保养计划，但是失败了！", StoreInfoModel.UserName,StoreInfoModel.EquipmentInfo.GetEquipment.EquipmentName)
                                });
                            }
                            //}
                        }

                        FocusIndex = 0;
                        new BLL.ExtraBLL.EquipmentInfoBLL().GetEquipmentInfo();
                        PageMaintainPlan_Loaded(null, null);
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
            gds[0].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }


    }
}
