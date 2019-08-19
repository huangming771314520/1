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
using MxDrawXLib;
using BLL;
using Model;
using System.Runtime.InteropServices;

namespace WpfClient
{
    /// <summary>
    /// ProductProcessPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductProcessPage : Page
    {
        #region 模拟用户操作

        [DllImport("user32.dll")]
        private static extern int mouse_event(MouseEventFlag dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(int bVk, int bScan, uint dwFlags, uint dwExtraInfo);

        #region 操作枚举

        [Flags]
        private enum MouseEventFlag : uint
        {
            /// <summary>
            /// 移动鼠标
            /// </summary>
            Move = 0x0001,
            /// <summary>
            /// 模拟鼠标左键按下
            /// </summary>
            LeftDown = 0x0002,
            /// <summary>
            /// 模拟鼠标左键抬起
            /// </summary>
            LeftUp = 0x0004,
            /// <summary>
            /// 模拟鼠标右键按下
            /// </summary>
            RightDown = 0x0008,
            /// <summary>
            /// 模拟鼠标右键抬起
            /// </summary>
            RightUp = 0x0010,
            /// <summary>
            /// 模拟鼠标中键按下
            /// </summary>
            MiddleDown = 0x0020,
            /// <summary>
            /// 模拟鼠标中键抬起
            /// </summary>
            MiddleUp = 0x0040,
            /// <summary>
            /// 
            /// </summary>
            XDown = 0x0080,
            /// <summary>
            /// 
            /// </summary>
            XUp = 0x0100,
            /// <summary>
            /// 模拟鼠标滚轮滚动
            /// </summary>
            Wheel = 0x0800,
            /// <summary>
            /// 
            /// </summary>
            VirtualDesk = 0x4000,
            /// <summary>
            /// 标示是否采用绝对坐标
            /// </summary>
            Absolute = 0x8000
        }

        #endregion

        public static void MouseLefDownEvent(int pointX, int pointY, int value)
        {
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / 1920, pointY * 65536 / 1080, 0, 0);
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / 1920, pointY * 65536 / 1080, value, 0);
        }
        public static void MouseSimulateWheelEvent(int value)
        {
            int pointX = 1920 / 2;
            int pointY = 1080 / 2;
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / 1920, pointY * 65536 / 1080, 0, 0);
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / 1920, pointY * 65536 / 1080, value, 0);
        }

        #endregion

        private AxMxDrawXLib.AxMxDrawX axMxDrawX = new AxMxDrawXLib.AxMxDrawX();//MxDraw插件
        private double pLbx = 0, pLby = 0, pRtx = 0, pRty = 0;//图纸大小
        private double pointX = 0, pointY = 0;//图纸中心点
        private FocusAreaEnum focusArea = FocusAreaEnum.LeftFocus;//页面焦点区域
        private Grid[] gridDrawings = new Grid[StoreInfoModel.ProducePlanInfo.GetPartFiles.Count + 1];//生产计划所用图纸
        private int gridDrawingIndex = 0;//当前图纸索引

        /// <summary>
        /// 页面区域枚举
        /// </summary>
        private enum FocusAreaEnum
        {
            /// <summary>
            /// 左区域
            /// </summary>
            LeftFocus = 0,
            /// <summary>
            /// 中间区域
            /// </summary>
            CenterFocus = 1,
            /// <summary>
            /// 右区域
            /// </summary>
            RightFocus = 2
        }

        public ProductProcessPage()
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
            #region 加载基本数据

            //要加工的零件本体
            StoreInfoModel.MaterialInfo = StoreInfoModel.MaterialInfos.FirstOrDefault(item => item.GetPart.PartCode.Equals(StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode));
            //要加工的零件子集
            StoreInfoModel.MaterialChildInfos = StoreInfoModel.MaterialInfos.Where(item => !item.GetPart.PartCode.Equals(StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode)).ToList();

            #region 加载物料详情






            for (int i = 0; i < StoreInfoModel.MaterialChildInfos.Count; i++)
            {
                Viewbox viewbox = new Viewbox();

                Grid grid = new Grid();
                grid.Margin = new Thickness(5);
                grid.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
                grid.Width = 200;
                grid.Height = 100;

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(24) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(24) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(24) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(140) });

                Label lblA = new Label() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 255)), Content = "零件编码:" };
                Label lblB = new Label() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 255)), Content = StoreInfoModel.MaterialChildInfos[i].GetMaterialDetail.PartCode };
                Label lblC = new Label() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(140, 255, 26)), Content = "零件名称:" };
                Label lblD = new Label() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(140, 255, 26)), Content = StoreInfoModel.MaterialChildInfos[i].GetMaterialDetail.PartName };
                Label lblE = new Label() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(255, 128, 0)), Content = "已录数量:" };
                Label lblF = new Label() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(255, 128, 0)), Content = "0/0" };
                TextBox txtPartCode = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, Padding = new Thickness(6, 2, 6, 2), Margin = new Thickness(5, 0, 5, 0) };

                grid.Children.Add(lblA);
                grid.Children.Add(lblB);
                grid.Children.Add(lblC);
                grid.Children.Add(lblD);
                grid.Children.Add(lblE);
                grid.Children.Add(lblF);
                grid.Children.Add(txtPartCode);

                Grid.SetColumn(lblA, 0);
                Grid.SetColumn(lblB, 1);
                Grid.SetColumn(lblC, 0);
                Grid.SetColumn(lblD, 1);
                Grid.SetColumn(lblE, 0);
                Grid.SetColumn(lblF, 1);
                Grid.SetColumnSpan(txtPartCode, 2);
                Grid.SetColumn(txtPartCode, 0);

                Grid.SetRow(lblA, 0);
                Grid.SetRow(lblB, 0);
                Grid.SetRow(lblC, 1);
                Grid.SetRow(lblD, 1);
                Grid.SetRow(lblE, 2);
                Grid.SetRow(lblF, 2);
                Grid.SetRow(txtPartCode, 3);

                viewbox.Child = grid;

                MaterialInfoContainer.Children.Add(viewbox);
            }

            #endregion

            #region 加载图纸显示

            lblProjectName.Content = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName;

            ((System.ComponentModel.ISupportInitialize)axMxDrawX).BeginInit();
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost() { Child = axMxDrawX };
            ((System.ComponentModel.ISupportInitialize)axMxDrawX).EndInit();

            axMxDrawX.ShowCommandWindow = false;
            axMxDrawX.ShowModelBar = false;
            axMxDrawX.ShowStatusBar = false;
            axMxDrawX.ShowToolBars = false;
            axMxDrawX.Padding = new System.Windows.Forms.Padding(0);
            axMxDrawX.Margin = new System.Windows.Forms.Padding(0);
            axMxDrawX.IsFirstRunPan = true;
            axMxDrawX.IsDrawCoord = false;

            mxDraw.Children.Add(host);

            if (StoreInfoModel.ProducePlanInfo.GetPartFiles.Count > 0)
            {
                axMxDrawX.OpenDwgFile(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[0].FileName));
                axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
                pointX = (pRtx - pLbx) / 2;
                pointY = (pRty - pLby) / 2;
                axMxDrawX.ZoomCenter(pointX, pointY);
                axMxDrawX.ZoomScale(0.08);
            }

            #endregion

            #region 加载图纸列表

            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetPartFiles.Count + 1; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(i.Equals(0) ? 60 : 80);
                drawingListContainer.RowDefinitions.Add(row);

                gridDrawings[i] = new Grid();
                Grid.SetRow(gridDrawings[i], i);

                Binding binding = new Binding()
                {
                    RelativeSource = RelativeSource.Self,
                    Path = new PropertyPath("FontSize")
                };
                TextBlock textBlock = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = i.Equals(0) ? "详情" : ("图纸" + i)
                };
                textBlock.SetBinding(WidthProperty, binding);

                gridDrawings[i].Children.Add(textBlock);

                drawingListContainer.Children.Add(gridDrawings[i]);
            }

            //******************************************************************************//
            //默认显示详情
            Grid pInfoGrid = new Grid()
            {
                Margin = new Thickness(0, 1, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(65, 65, 65))
            };
            TextBlock pInfoTextBlock = new TextBlock()
            {
                Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                Margin = new Thickness(5),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Text = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProcessDesc
            };
            pInfoGrid.Children.Add(pInfoTextBlock);
            projectInfo.Children.Add(pInfoGrid);
            //******************************************************************************//

            #endregion

            #endregion

            #region 录入生产日志,录入计划开始时间

            #region 录入生产日志

            //记录生产日志
            ResultModel logResult = new BLL.ExtraBLL.ProducePlanInfoBLL().ProduceLog(BLL.ExtraBLL.ProducePlanInfoBLL.ProduceLogTypeEnum.Start);

            #endregion

            #region 录入计划开始时间

            //录入计划实际开始时间
            ResultModel actualDateResult = new BLL.ExtraBLL.ProducePlanInfoBLL().ActualProducePlanDate(BLL.ExtraBLL.ProducePlanInfoBLL.ProducePlanStateEnum.Start);

            #endregion

            #endregion

            #region 默认焦点在左边

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255));
            border.BorderThickness = new Thickness(2);
            tbMaterialInfo.Children.Add(border);

            #endregion
        }

        #region 打开图纸的方法

        /// <summary>
        /// 打开图纸
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool OpenDrawing(string path)
        {
            try
            {
                bool result = axMxDrawX.OpenDwgFile(path);
                axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
                pointX = (pRtx - pLbx) / 2;
                pointY = (pRty - pLby) / 2;
                axMxDrawX.AutoZoomAll = true;
                return result;
            }
            catch
            {
                return false;
            }
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
            //偏移量
            int num = 50;

            switch ((int)e.Key)
            {
                //tab键-切换焦点区域
                case 3:
                    #region
                    focusArea = ((int)focusArea + 1).Equals(3) ? 0 : (FocusAreaEnum)((int)focusArea + 1);
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                    border.BorderThickness = new Thickness(2);
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            for (int i = 0; i < tbDrawingList.Children.Count; i++)
                            {
                                if (tbDrawingList.Children[i] is Border)
                                {
                                    tbDrawingList.Children.Remove(tbDrawingList.Children[i]);
                                }
                            }
                            tbMaterialInfo.Children.Add(border);
                            break;
                        case FocusAreaEnum.CenterFocus:
                            for (int i = 0; i < tbMaterialInfo.Children.Count; i++)
                            {
                                if (tbMaterialInfo.Children[i] is Border)
                                {
                                    tbMaterialInfo.Children.Remove(tbMaterialInfo.Children[i]);
                                }
                            }
                            tbDrawingShow.Children.Add(border);
                            break;
                        case FocusAreaEnum.RightFocus:
                            for (int i = 0; i < tbDrawingShow.Children.Count; i++)
                            {
                                if (tbDrawingShow.Children[i] is Border)
                                {
                                    tbDrawingShow.Children.Remove(tbDrawingShow.Children[i]);
                                }
                            }
                            tbDrawingList.Children.Add(border);
                            gridDrawings[gridDrawingIndex].Background = new SolidColorBrush(Color.FromRgb(240, 0, 240));
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
                        case FocusAreaEnum.CenterFocus:
                            pointX += num;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus: break;
                        default: break;
                    }
                    #endregion
                    break;
                //上
                case 24:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.CenterFocus:
                            pointY -= num;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:
                            #region
                            gridDrawings[gridDrawingIndex].Background = new SolidColorBrush(Color.FromRgb(65, 65, 65));
                            gridDrawingIndex = (gridDrawingIndex - 1).Equals(-1) ? gridDrawings.Length - 1 : (gridDrawingIndex - 1);
                            gridDrawings[gridDrawingIndex].Background = new SolidColorBrush(Color.FromRgb(240, 0, 240));

                            //详情
                            if (gridDrawingIndex.Equals(0))
                            {
                                Grid pInfoGrid = new Grid()
                                {
                                    Margin = new Thickness(0, 1, 0, 0),
                                    Background = new SolidColorBrush(Color.FromRgb(65, 65, 65))
                                };
                                TextBlock pInfoTextBlock = new TextBlock()
                                {
                                    Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                                    Margin = new Thickness(5),
                                    FontSize = 14,
                                    TextWrapping = TextWrapping.Wrap,
                                    Text = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProcessDesc
                                };
                                pInfoGrid.Children.Add(pInfoTextBlock);
                                projectInfo.Children.Add(pInfoGrid);
                            }
                            //地图
                            else
                            {
                                if (projectInfo.Children.Count > 0)
                                {
                                    projectInfo.Children.Clear();
                                }

                                OpenDrawing(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[gridDrawingIndex - 1].FileName));
                            }
                            #endregion
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
                        case FocusAreaEnum.CenterFocus:
                            pointX -= num;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus: break;
                        default: break;
                    }
                    #endregion
                    break;
                //下
                case 26:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.CenterFocus:
                            pointY += num;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:
                            #region
                            gridDrawings[gridDrawingIndex].Background = new SolidColorBrush(Color.FromRgb(65, 65, 65));
                            gridDrawingIndex = (gridDrawingIndex + 1).Equals(gridDrawings.Length) ? 0 : (gridDrawingIndex + 1);
                            gridDrawings[gridDrawingIndex].Background = new SolidColorBrush(Color.FromRgb(240, 0, 240));

                            //详情
                            if (gridDrawingIndex.Equals(0))
                            {
                                Grid pInfoGrid = new Grid()
                                {
                                    Margin = new Thickness(0, 1, 0, 0),
                                    Background = new SolidColorBrush(Color.FromRgb(65, 65, 65))
                                };
                                TextBlock pInfoTextBlock = new TextBlock()
                                {
                                    Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                                    Margin = new Thickness(5),
                                    FontSize = 14,
                                    TextWrapping = TextWrapping.Wrap,
                                    Text = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProcessDesc
                                };
                                pInfoGrid.Children.Add(pInfoTextBlock);
                                projectInfo.Children.Add(pInfoGrid);
                            }
                            //地图
                            else
                            {
                                if (projectInfo.Children.Count > 0)
                                {
                                    projectInfo.Children.Clear();
                                }

                                OpenDrawing(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[gridDrawingIndex - 1].FileName));
                            }
                            #endregion
                            break;
                        default: break;
                    }
                    #endregion
                    break;
                //-
                case 143:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.CenterFocus:
                            MouseSimulateWheelEvent(-120);
                            break;
                        case FocusAreaEnum.RightFocus: break;
                        default: break;
                    }
                    #endregion
                    break;
                //+
                case 141:
                    #region
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus: break;
                        case FocusAreaEnum.CenterFocus:
                            MouseSimulateWheelEvent(120);
                            break;
                        case FocusAreaEnum.RightFocus: break;
                        default: break;
                    }
                    #endregion
                    break;
                //Ctrl键-在图纸界面焦点区域：弹出是否完工，暂停，取消对话框
                case 118:
                case 119:
                    #region
                    if (focusArea.Equals(FocusAreaEnum.CenterFocus))
                    {
                        //MessageBoxResult dr = MessageBox.Show("如果“完工”，请选择“是”；\n如果“暂停”，请选择“否”；\n如果误操作，请选择“取消”；", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                        ProduceStateWindow window = new ProduceStateWindow();
                        window.ShowDialog();
                        MessageBoxResult dr = window.Result;

                        //完工
                        if (dr == MessageBoxResult.Yes)
                        {
                            //物料出库
                            ResultModel OutPutResult = new BLL.ExtraBLL.MaterialInfoBLL().MaterialInventory(BLL.ExtraBLL.MaterialInfoBLL.MaterialStateEnum.OtherOutput);
                            if (!OutPutResult.Result)
                            {
                                MessageBox.Show(OutPutResult.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                            //记录生产转序入库数量
                            StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity = window.Num;

                            //录入计划实际结束时间
                            ResultModel actualDateResult = new BLL.ExtraBLL.ProducePlanInfoBLL().ActualProducePlanDate(BLL.ExtraBLL.ProducePlanInfoBLL.ProducePlanStateEnum.End);

                            //获取工艺路线，判断是否需要报检
                            var IsInspectionReport = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.IsInspectionReport ?? 0;

                            //不需要报检
                            if (IsInspectionReport.Equals(1))
                            {
                                if (string.IsNullOrEmpty(StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.WarehouseName))
                                {
                                    MessageBox.Show(@"你生产的那啥没人要啊！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    //转序入库
                                    ResultModel TurnInputResult = new BLL.ExtraBLL.MaterialInfoBLL().MaterialInventory(BLL.ExtraBLL.MaterialInfoBLL.MaterialStateEnum.TurnInput);

                                    if (!TurnInputResult.Result)
                                    {
                                        MessageBox.Show(TurnInputResult.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }
                            }
                            //需要报检
                            else
                            {
                                //生成报检单
                                ResultModel result = new BLL.ExtraBLL.MaterialInfoBLL().InspectionReport();
                            }

                            //记录生产日志
                            ResultModel logResult = new BLL.ExtraBLL.ProducePlanInfoBLL().ProduceLog(BLL.ExtraBLL.ProducePlanInfoBLL.ProduceLogTypeEnum.End);

                            //跳转至选择任务计划页面

                            NavigationService.GetNavigationService(this).Navigate(new Uri("ProductPlanPage.xaml", UriKind.Relative));
                        }
                        //暂停
                        else if (dr == MessageBoxResult.No)
                        {
                            //记录生产日志
                            ResultModel logResult = new BLL.ExtraBLL.ProducePlanInfoBLL().ProduceLog(BLL.ExtraBLL.ProducePlanInfoBLL.ProduceLogTypeEnum.Pause);

                            //跳转至选择任务计划页面

                            NavigationService.GetNavigationService(this).Navigate(new Uri("ProductPlanPage.xaml", UriKind.Relative));
                        }
                        //取消
                        else
                        {

                        }
                    }
                    #endregion
                    break;

                default: break;
            }
        }

        #endregion



    }
}
