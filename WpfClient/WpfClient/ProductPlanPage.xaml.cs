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
using BLL;
using Model;

namespace WpfClient
{
    /// <summary>
    /// ProductPlanPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductPlanPage : Page
    {
        private FocusAreaEnum focusArea = FocusAreaEnum.LeftFocus;//默认焦点区域
        private Grid[] planGridContainers;//所有生产计划磁贴 Grid
        private int planGridIndex = 0;//当前磁贴焦点索引

        /// <summary>
        /// 页面焦点区域枚举
        /// </summary>
        private enum FocusAreaEnum
        {
            /// <summary>
            /// 左区域
            /// </summary>
            LeftFocus = 0,
            /// <summary>
            /// 右区域
            /// </summary>
            RightFocus = 1
        }

        public ProductPlanPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载数据

            lblUserName.Content = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName;
            lblUserCode.Content = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode;

            //获取计划任务
            ProducePlanInfoModel GetPPlanInfo = new BLL.ExtraBLL.ProducePlanInfoBLL().GetProducePlanInfo();
            if (!GetPPlanInfo.Result)
            {
                MessageBox.Show(GetPPlanInfo.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //获取班组所有生产计划
            planGridContainers = new Grid[GetPPlanInfo.Data.Count];

            #endregion

            #region 加载任务计划列表数据

            long Num = 0;
            lvwPlanInfo.ItemsSource = GetPPlanInfo.Data.Select(item =>
            {
                Num++;
                return new
                {
                    Num = Num,
                    //PartFigureCode = item.GetProductProcessRoute.FigureCode,
                    PartFigureCode = item.GetProcessBOM.PartFigureCode,
                    ProductName = item.GetProductProcessRoute.ProcessName
                };
            }).ToList();

            #endregion

            #region 加载任务计划磁贴

            for (int i = 0; i < GetPPlanInfo.Data.Count; i++)
            {
                ProducePlanInfoModel.DataModel model = GetPPlanInfo.Data[i];

                planGridContainers[i] = new Grid
                {
                    Margin = new Thickness(5),
                    Cursor = Cursors.Hand,
                    MinWidth = 250,
                    Background = new SolidColorBrush(GetRandomColor()),
                    Focusable = true,
                    Tag = model
                };

                Grid grid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                Binding bdingGridWidth = new Binding
                {
                    ElementName = "pInfoColWidth",
                    Path = new PropertyPath("Width")
                };
                grid.SetBinding(WidthProperty, bdingGridWidth);

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
                    model.GetProjectProduceDetial.ContractCode,//合同编号
                    model.GetProject.ProjectName,//工程项目
                    model.GetProductProcessRoute.ProcessName+"("+model.GetProjectProduceDetial.Quantity+")",//工序名称
                    model.GetProjectProduceDetial.PlanedStartTime==null?(model.GetProjectProduceDetial.PlanedFinishTime==null?"--------":
                    Convert.ToDateTime(model.GetProjectProduceDetial.PlanedFinishTime).ToString("---- - MM:dd HH:mm:ss")):(model.GetProjectProduceDetial.PlanedFinishTime==null?
                    Convert.ToDateTime(model.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ----"):(Convert.ToDateTime(model.GetProjectProduceDetial.PlanedStartTime).ToString("MM-dd HH:mm:ss - ")+
                    Convert.ToDateTime(model.GetProjectProduceDetial.PlanedFinishTime).ToString("HH:mm:ss")))
                };
                for (int j = 0; j < 4; j++)
                {
                    Label lblContent = new Label()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = j.Equals(3) ? 16 : 20,
                        Foreground = new SolidColorBrush(ConfigInfoModel.ColorSetting.TxtColorA),
                        Content = contentArr[j]
                    };
                    grid.Children.Add(lblContent);

                    Grid.SetColumn(lblContent, 1);
                    Grid.SetRow(lblContent, j + 1);
                }

                planGridContainers[i].MouseDown += new MouseButtonEventHandler(Grid_MouseDown);
                planGridContainers[i].MouseUp += new MouseButtonEventHandler(Grid_MouseUp);

                planGridContainers[i].Children.Add(grid);
                planInfoContainer.Children.Add(planGridContainers[i]);
            }

            #endregion

            #region 页面区域焦点

            //默认选择第一个生产计划
            if (planGridContainers.Length > 0)
            {
                Border borderGrid = new Border();
                borderGrid.BorderBrush = new SolidColorBrush(Color.FromRgb(158, 183, 241));
                borderGrid.BorderThickness = new Thickness(2);
                planGridContainers[0].Children.Add(borderGrid);
            }

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255));
            border.BorderThickness = new Thickness(2);
            if (focusArea.Equals(FocusAreaEnum.LeftFocus))
            {
                tbPlanInfoList.Children.Add(border);
            }
            else if (focusArea.Equals(FocusAreaEnum.RightFocus))
            {
                tbPlanInfoContainer.Children.Add(border);
            }
            else
            {

            }

            //任务计划列表默认选中第一条
            lvwPlanInfo.Focus();
            if (lvwPlanInfo.Items.Count > 0)
            {
                lvwPlanInfo.SelectedIndex = 0;
            }

            #endregion

        }

        #region 获取随机颜色

        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <returns></returns>
        public Color GetRandomColor()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            switch (random.Next(1, 5))
            {
                case 1: return ConfigInfoModel.ColorSetting.Ban;
                case 2: return ConfigInfoModel.ColorSetting.Notice;
                case 3: return ConfigInfoModel.ColorSetting.Instruct;
                case 4: return ConfigInfoModel.ColorSetting.Safety;
                default: return Color.FromRgb(255, 255, 255);
            }
        }

        #endregion

        #region 鼠标点击事件 - 点击生产计划

        private Grid GridContainer = null;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridContainer = (Grid)sender;
            e.Handled = true;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            if (GridContainer != null && GridContainer.Equals(grid))
            {
                StoreInfoModel.ProducePlanInfo = grid.Tag as ProducePlanInfoModel.DataModel;
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

                NavigationService.GetNavigationService(this).Navigate(new Uri("ProductProcessPage.xaml", UriKind.Relative));
            }
            GridContainer = null;
        }

        #endregion

        #region 按tab键切换焦点区域，页面按键操作

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
            //获取焦点后的边框
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(158, 183, 241));
            border.BorderThickness = new Thickness(2);

            switch ((int)e.Key)
            {
                //tab键-切换焦点区域
                case 3:
                    #region
                    focusArea = ((int)focusArea + 1).Equals(2) ? 0 : (FocusAreaEnum)((int)focusArea + 1);
                    Border focusBorder = new Border();
                    focusBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                    focusBorder.BorderThickness = new Thickness(2);
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            for (int i = 0; i < tbPlanInfoContainer.Children.Count; i++)
                            {
                                if (tbPlanInfoContainer.Children[i] is Border)
                                {
                                    tbPlanInfoContainer.Children.Remove(tbPlanInfoContainer.Children[i]);
                                }
                            }
                            tbPlanInfoList.Children.Add(focusBorder);
                            break;
                        case FocusAreaEnum.RightFocus:
                            for (int i = 0; i < tbPlanInfoList.Children.Count; i++)
                            {
                                if (tbPlanInfoList.Children[i] is Border)
                                {
                                    tbPlanInfoList.Children.Remove(tbPlanInfoList.Children[i]);
                                }
                            }
                            tbPlanInfoContainer.Children.Add(focusBorder);
                            break;
                        default: break;
                    }
                    #endregion
                    break;
                //enter键-回车
                case 6:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.RightFocus:
                            StoreInfoModel.ProducePlanInfo = planGridContainers[planGridIndex].Tag as ProducePlanInfoModel.DataModel;
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

                            NavigationService.GetNavigationService(this).Navigate(new Uri("ProductProcessPage.xaml", UriKind.Relative));
                            break;
                        default: break;
                    }
                    #endregion
                    break;
                //左
                case 23:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.RightFocus:
                            if (planGridContainers.Length > 0)
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
                        default: break;
                    }
                    #endregion
                    break;
                //上
                case 24:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            int n = lvwPlanInfo.SelectedIndex;
                            n = (n - 1) <= -1 ? (lvwPlanInfo.Items.Count - 1) : (n - 1);
                            lvwPlanInfo.SelectedIndex = n;
                            break;
                        case FocusAreaEnum.RightFocus:
                            double ah = svPInfoContainer.VerticalOffset;
                            double num = svPInfoContainer.ScrollableHeight / 10;
                            ah = (ah - num) < 0 ? 0 : (ah - num);
                            svPInfoContainer.ScrollToVerticalOffset(ah);
                            break;
                        default: break;
                    }
                    #endregion
                    break;
                //右
                case 25:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.RightFocus:
                            if (planGridContainers.Length > 0)
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
                        default: break;
                    }
                    #endregion
                    break;
                //下
                case 26:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            int n = lvwPlanInfo.SelectedIndex;
                            n = (n + 1).Equals(lvwPlanInfo.Items.Count) ? 0 : (n + 1);
                            lvwPlanInfo.SelectedIndex = n;
                            break;
                        case FocusAreaEnum.RightFocus:
                            double ah = svPInfoContainer.VerticalOffset;
                            double num = svPInfoContainer.ScrollableHeight / 10;
                            ah = (ah + num) > svPInfoContainer.ScrollableHeight ? svPInfoContainer.ScrollableHeight : (ah + num);
                            svPInfoContainer.ScrollToVerticalOffset(ah);
                            break;
                        default: break;
                    }
                    #endregion
                    break;
                default: break;
            }
        }

        #endregion

    }

}
