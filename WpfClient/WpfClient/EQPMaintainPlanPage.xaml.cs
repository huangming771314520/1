using System;
using System.Collections;
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
using System.Windows.Threading;
using Model;

namespace WpfClient
{
    /// <summary>
    /// EQPMaintainPlanPage.xaml 的交互逻辑
    /// </summary>
    public partial class EQPMaintainPlanPage : Page
    {
        private Grid[] planGridContainers;
        private int planGridIndex = 0;
        //private bool isEQPMaintain = false;

        public EQPMaintainPlanPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载数据

            new BLL.ExtraBLL.EquipmentInfoBLL().GetEquipmentInfo();

            planGridContainers = new Grid[StoreInfoModel.EquipmentInfos.Count + 1];

            #region 加载返回工单磁贴

            planGridContainers[0] = new Grid
            {
                Margin = new Thickness(5),
                Cursor = Cursors.Hand,
                Width = 250,
                Height = 160,
                Background = new SolidColorBrush(Color.FromRgb(66, 166, 247)),
                Focusable = true
            };

            planGridContainers[0].Children.Add(new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 28,
                Foreground = new SolidColorBrush(ConfigInfoModel.ColorSetting.TxtColorA),
                Content = "<< 返回工单"
            });

            planInfoContainer.Children.Add(planGridContainers[0]);

            #endregion

            #region 加载保养计划任务磁贴

            for (int i = 0; i < StoreInfoModel.EquipmentInfos.Count; i++)
            {
                EquipmentInfoModel.DataModel model = StoreInfoModel.EquipmentInfos[i];

                planGridContainers[i + 1] = new Grid
                {
                    Margin = new Thickness(5),
                    Cursor = Cursors.Hand,
                    MinWidth = 250,
                    //Background = new SolidColorBrush(Color.FromRgb(73, 140, 241)),
                    Background = new SolidColorBrush(model.GetEquipmentMaintenancePlan.ActualStartTime == null ? Color.FromRgb(73, 140, 241) : Color.FromRgb(250, 92, 60)),
                    Focusable = true,
                    Tag = model
                };
                Binding bdingGridWidth = new Binding
                {
                    ElementName = "pInfoColWidth",
                    Path = new PropertyPath("Width")
                };
                planGridContainers[i + 1].SetBinding(WidthProperty, bdingGridWidth);

                Grid grid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5) });
                for (int j = 0; j < 4; j++)
                {
                    RowDefinition rowHeight = new RowDefinition();
                    Binding bdingRowHeight = new Binding
                    {
                        ElementName = "pInfoRowHeight",
                        Path = new PropertyPath("Height")
                    };
                    rowHeight.SetBinding(RowDefinition.HeightProperty, bdingRowHeight);

                    grid.RowDefinitions.Add(rowHeight);
                }
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5) });

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5) });
                ColumnDefinition colWidth = new ColumnDefinition();
                Binding bdingColWidth = new Binding
                {
                    ElementName = "pInfoColWidth",
                    Path = new PropertyPath("Width")
                };
                colWidth.SetBinding(ColumnDefinition.WidthProperty, bdingColWidth);
                grid.ColumnDefinitions.Add(colWidth);
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5) });

                string[] contentArr = new string[] {
                    model.GetEquipmentMaintenancePlan.EquipmentName,//设备名称
                    model.GetEquipmentMaintenancePlan.MaintenanceName+"("+(model.GetEquipmentMaintenancePlan.ActualStartTime==null?"未开始":"已开始")+")",//保养类型
                    model.GetEquipmentMaintenancePlan.PlanedContent,//维护内容
                    model.GetEquipmentMaintenancePlan.PlanedStartTime==null?"":Convert.ToDateTime(model.GetEquipmentMaintenancePlan.PlanedStartTime).ToString("yyyy-MM-dd")
                };
                for (int j = 0; j < 4; j++)
                {
                    Label lblContent = new Label()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = j.Equals(2) ? 16 : 20,
                        Foreground = new SolidColorBrush(ConfigInfoModel.ColorSetting.TxtColorA),
                        Content = contentArr[j]
                    };
                    grid.Children.Add(lblContent);

                    Grid.SetColumn(lblContent, 1);
                    Grid.SetRow(lblContent, j + 1);
                }

                planGridContainers[i + 1].Children.Add(grid);
                planInfoContainer.Children.Add(planGridContainers[i + 1]);
            }

            #endregion

            #region 默认选择返回工单

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            border.BorderThickness = new Thickness(2);
            planGridContainers[0].Children.Add(border);

            #endregion


            #endregion


            pageContainer.Focus();
        }

        /// <summary>
        /// 页面按键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageContainer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 页面按键抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageContainer_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            border.BorderThickness = new Thickness(2);

            switch ((int)e.Key)
            {
                //tab键-切换焦点区域
                case 3:

                    break;
                //enter键-回车
                case 6:
                    //if (isEQPMaintain)
                    //{
                    //    return;
                    //}

                    if (planGridIndex.Equals(0))
                    {
                        NavigationService.GetNavigationService(this).Navigate(new Uri(@"ProductPlanPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        //isEQPMaintain = true;

                        StoreInfoModel.EquipmentInfo = planGridContainers[planGridIndex].Tag as EquipmentInfoModel.DataModel;
                        //是否正在保养，是将结束
                        if (StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.ActualStartTime != null)
                        {
                            //if (MessageBox.Show("是否立即结束？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                            //{
                            ResultModel result = new BLL.ExtraBLL.EquipmentInfoBLL().EQPMaintenancePlan(1);
                            //结束保养成功
                            if (result.Result)
                            {
                                StoreInfoModel.EquipmentInfos.RemoveAt(planGridIndex - 1);
                                //planGridIndex = 0;
                                //planInfoContainer.Children.Clear();
                                //Page_Loaded(null, null);
                            }
                            else
                            {
                                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                                planGridContainers[planGridIndex].Background = new SolidColorBrush(Color.FromRgb(250, 92, 60));
                                ((planGridContainers[planGridIndex].Children[0] as Grid).Children[1] as Label).Content = StoreInfoModel.EquipmentInfo.GetEquipmentMaintenancePlan.MaintenanceName + "(已开始)";
                            }
                            else
                            {
                                MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            //}
                        }

                        planGridIndex = 0;
                        planInfoContainer.Children.Clear();
                        Page_Loaded(null, null);
                        //isEQPMaintain = false;
                    }

                    break;
                //左
                case 23:
                    if (planGridContainers.Length > 1)
                    {
                        for (int i = 0; i < planGridContainers[planGridIndex].Children.Count; i++)
                        {
                            if (planGridContainers[planGridIndex].Children[i] is Border)
                            {
                                planGridContainers[planGridIndex].Children.Remove(planGridContainers[planGridIndex].Children[i]);
                            }
                        }
                        planGridIndex = (planGridIndex - 1).Equals(-1) ? (planGridContainers.Length - 1) : (planGridIndex - 1);
                        planGridContainers[planGridIndex].Children.Add(border);
                    }
                    break;
                //上
                case 24:

                    break;
                //右
                case 25:
                    if (planGridContainers.Length > 1)
                    {
                        for (int i = 0; i < planGridContainers[planGridIndex].Children.Count; i++)
                        {
                            if (planGridContainers[planGridIndex].Children[i] is Border)
                            {
                                planGridContainers[planGridIndex].Children.Remove(planGridContainers[planGridIndex].Children[i]);
                            }
                        }
                        planGridIndex = (planGridIndex + 1).Equals(planGridContainers.Length) ? 0 : (planGridIndex + 1);
                        planGridContainers[planGridIndex].Children.Add(border);
                    }
                    break;
                //下
                case 26:

                    break;
                default: break;
            }
        }
    }
}
