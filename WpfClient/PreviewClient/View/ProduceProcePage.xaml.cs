using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Runtime.InteropServices;
using System.IO;
using PreviewClient.ViewModels;
using BLL.ExtraBLL;
using Model.Models;
//using System.Drawing;

namespace PreviewClient.View
{
    /// <summary>
    /// ProduceProcePage.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceProcePage : Page
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

        #region 用户操作

        //public static void MouseLefDownEvent(int pointX, int pointY, int value)
        //{
        //    mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / 1920, pointY * 65536 / 1080, 0, 0);
        //    mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / 1920, pointY * 65536 / 1080, value, 0);
        //}

        public static void MouseSimulateWheelEvent(int value)
        {
            int pointX = ConfigInfoModel.WinScreenWidth / 2;
            int pointY = ConfigInfoModel.WinScreenHeight / 2;
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / ConfigInfoModel.WinScreenWidth, pointY * 65536 / ConfigInfoModel.WinScreenHeight, 0, 0);
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / ConfigInfoModel.WinScreenWidth, pointY * 65536 / ConfigInfoModel.WinScreenHeight, value, 0);
        }

        #endregion

        #endregion

        private AxMxDrawXLib.AxMxDrawX axMxDrawX = new AxMxDrawXLib.AxMxDrawX();//MxDraw插件
        private double pLbx = 0, pLby = 0, pRtx = 0, pRty = 0;//图纸大小
        private double pointX = 0, pointY = 0;//图纸中心点
        private FocusAreaEnum focusArea = FocusAreaEnum.LeftFocus;//页面焦点区域
        private int workStepCount = 0, workStepIndex = 0;//工步数量，当前工步索引
        private string strWorkStepsInfo = string.Empty;//工步详细描述
        private List<string> drawFilePaths = new List<string>();//图纸文件路径
        private int drawingCount = 0, drawingIndex = 0;//图纸数量，当前图纸索引
        private int drawMoveOffset = 50;//图纸移动偏移量
        public newProduceProceViewModel dataContext = new newProduceProceViewModel();

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
            RightFocus = 2,
            /// <summary>
            /// 下区域
            /// </summary>
            DownFocus = 3
        }


        public ProduceProcePage()
        {
            InitializeComponent();
        }

        private void PageProduceProce_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载基本数据

            //lblProjectName.Content = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName;

            #endregion

            #region 加载工步列表

            List<object> WorkStepList = new List<object>();
            workStepCount = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps.Count;
            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps.Count; i++)
            {
                WorkStepList.Add(new
                {
                    WorkStepsTitle = string.Format("工步{0}", ToUpper(i + 1)),
                    WorkStepsName = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsName,
                    WorkStepsInfo = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsContent
                });

                dataContext.WorkSteps.Add(new WorkStepModel()
                {
                    LineNum = (i + 1),
                    WorkStepsName = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsName,
                    WorkStepsInfo = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsContent
                });
            }

            dataContext.ProjectName = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName;
            dataContext.WorkStep = dataContext.WorkSteps.Count > 0 ? dataContext.WorkSteps[0] : new WorkStepModel();
            this.DataContext = dataContext;

            //var aaaaa = WorkStepsList.DataContext;
            //
            //ContentPresenter cp = WorkStepsList.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
            //List<Border> gds = Helpers.WpfHelper.FindVisualChildren<Border>(WorkStepsList);

            //ContentPresenter cp = PlanCardItems.ItemContainerGenerator.ContainerFromIndex(FocusIndex) as ContentPresenter;
            //List<Border> gds = Helpers.WpfHelper.FindVisualChildren<Border>(cp);
            //List<TextBlock> tbs = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cp);
            //WorkStepBar.ItemsSource = WorkStepList;

            #endregion

            #region 图纸列表

            List<object> DrawList = new List<object>();
            string[] fileTypeArr = new string[] { ".jpg", ".JPG", ".png", ".PNG", ".dwg", ".DWG", ".pdf", ".PDF" };

            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetQualityReportDocs.Count; i++)
            {
                QMS_QualityReportDoc qrd = StoreInfoModel.ProducePlanInfo.GetQualityReportDocs[i];
                if (fileTypeArr.Contains(System.IO.Path.GetExtension(qrd.FileName)))
                {
                    drawingCount++;
                    drawFilePaths.Add(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, qrd.FileName));
                    DrawList.Add(new { DrawName = qrd.FileName });
                }
            }

            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetProcessFigures.Count; i++)
            {
                PRS_ProcessFigure pf = StoreInfoModel.ProducePlanInfo.GetProcessFigures[i];
                if (fileTypeArr.Contains(System.IO.Path.GetExtension(pf.FileName)))
                {
                    drawingCount++;
                    drawFilePaths.Add(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, pf.FileName));
                    DrawList.Add(new { DrawName = pf.FileName });
                }
            }

            //drawingCount = StoreInfoModel.ProducePlanInfo.GetPartFiles.Count;
            //for (int i = 0; i < drawingCount; i++)
            //{
            //    drawFilePaths.Add(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[drawingIndex].FileName));
            //    //DrawList.Add(new { DrawName = string.Format("图{0}", ToUpper(i + 1)) });
            //    DrawList.Add(new { DrawName = StoreInfoModel.ProducePlanInfo.GetPartFiles[i].FileName });
            //}
            //for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetQualityReportDocs.Count; i++)
            //{
            //    if (new string[] { ".jpg", ".JPG", ".png", ".PNG", ".dwg", ".DWG", ".pdf", ".PDF" }.Contains(System.IO.Path.GetExtension(StoreInfoModel.ProducePlanInfo.GetQualityReportDocs[i].FileName)))
            //    {
            //        drawingCount++;
            //        drawFilePaths.Add(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetQualityReportDocs[drawingIndex].FileName));
            //        DrawList.Add(new { DrawName = StoreInfoModel.ProducePlanInfo.GetQualityReportDocs[i].FileName });
            //    }
            //}

            DrawCardItems.ItemsSource = DrawList;

            #endregion

            #region 图纸展示

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

            //if (StoreInfoModel.ProducePlanInfo.GetPartFiles.Count > 0)
            //{
            //    axMxDrawX.OpenDwgFile(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[0].FileName));
            //    axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
            //    pointX = (pRtx - pLbx) / 2;
            //    pointY = (pRty - pLby) / 2;
            //    axMxDrawX.ZoomCenter(pointX, pointY);
            //    axMxDrawX.ZoomScale(0.08);
            //}

            if (drawingCount > 0)
            {
                axMxDrawX.OpenDwgFile(drawFilePaths[0]);
                axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
                pointX = (pRtx - pLbx) / 2;
                pointY = (pRty - pLby) / 2;
                axMxDrawX.ZoomCenter(pointX, pointY);
                axMxDrawX.ZoomScale(0.08);
            }

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

            #region 加载物料信息

            materielItems.ItemsSource = GetMaterielItems();

            //List<object> materielData = new List<object>();
            //if (StoreInfoModel.MaterialChildInfos.Count <= 0)
            //{
            //    materielData.Add(new
            //    {
            //        PartCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartCode,
            //        PartName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName,
            //        PartQuantity = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity
            //    });
            //}
            //else
            //{
            //    foreach (var item in StoreInfoModel.MaterialChildInfos)
            //    {
            //        materielData.Add(new
            //        {
            //            PartCode = item.GetMaterialDetail.PartCode,
            //            PartName = item.GetMaterialDetail.PartName,
            //            PartQuantity = item.GetMaterialDetail.PartQuantity
            //        });
            //    }
            //}
            //materielItems.ItemsSource = materielData;

            #endregion

            #endregion

            #region 默认焦点区域、选择项

            switch (focusArea)
            {
                case FocusAreaEnum.LeftFocus:
                    RemoveTabBorder(tbDrawingList);
                    AddTabBorder(tbWorkStepsInfo);
                    break;
                case FocusAreaEnum.CenterFocus:
                    RemoveTabBorder(tbWorkStepsInfo);
                    AddTabBorder(mxDraw);
                    break;
                case FocusAreaEnum.RightFocus:
                    RemoveTabBorder(mxDraw);
                    AddTabBorder(materielList);
                    break;
                case FocusAreaEnum.DownFocus:
                    RemoveTabBorder(materielList);
                    AddTabBorder(tbDrawingList);
                    break;
                default: break;
            }

            #endregion

            InputMethod.SetIsInputMethodEnabled(MainPage, false);
            InputMethod.SetPreferredImeState(MainPage, InputMethodState.Off);

            ShowWorkStepsInfo();
            MainPage.Focus();

            var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】进入了开始生产、查看图纸的页面！", StoreInfoModel.UserName));
        }


        //private List<TextBlock> GetMaterielItems()
        //{
        //    materielItems.Height = materielList.Height;
        //    List<TextBlock> itemList = new List<TextBlock>();
        //    var list = StoreInfoModel.MaterialInfo;

        //    if (StoreInfoModel.MaterialChildInfos.Count > 0)
        //    {
        //        foreach (var item in StoreInfoModel.MaterialChildInfos)
        //        {
        //            string MaterielName = item.GetMaterialDetail.PartName;
        //            string MaterielCount = item.GetMaterialDetail.PartQuantity.ToString();
        //            string MaterielCode = item.GetMaterialDetail.PartCode;
        //            TextBlock tb = new TextBlock();
        //            tb.Text = MaterielName + " " + MaterielCode + " 0/" + MaterielCount;
        //            tb.Foreground = Brushes.Red;
        //            tb.VerticalAlignment = VerticalAlignment.Top;
        //            itemList.Add(tb);

        //        }
        //    }
        //    else if (StoreInfoModel.MaterialInfo != null)
        //    {
        //        string MaterielName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName;
        //        string MaterielCount = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity.ToString();
        //        string MaterielCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartCode;
        //        TextBlock tb = new TextBlock();
        //        tb.Text = MaterielName + " " + MaterielCode + " 0/" + MaterielCount;
        //        tb.Foreground = Brushes.Red;
        //        tb.VerticalAlignment = VerticalAlignment.Top;
        //        itemList.Add(tb);

        //    }
        //    //for (int i = 0; i < 50; i++)
        //    //{
        //    //    TextBlock tb = new TextBlock();
        //    //    tb.Text = "测试物料" + "0/" + i + 1;
        //    //    tb.Foreground = Brushes.Red;
        //    //    itemList.Add(tb);
        //    //}
        //    return itemList;
        //}

        private List<Grid> GetMaterielItems()
        {
            materielItems.Height = materielList.Height;
            List<Grid> itemList = new List<Grid>();

            if (StoreInfoModel.MaterialChildInfos.Count > 0)
            {
                foreach (var item in StoreInfoModel.MaterialChildInfos)
                {
                    Grid grid = new Grid()
                    {
                        Margin = new Thickness(2, 4, 2, 4)
                    };
                    StackPanel stackPanel = new StackPanel();

                    string MaterielName = item.GetMaterialDetail.PartName;
                    string MaterielCount = item.GetMaterialDetail.PartQuantity.ToString();
                    //string MaterielCode = item.GetMaterialDetail.PartCode;
                    string MaterielCode = item.GetPart.FigureCode;

                    TextBlock tbA = new TextBlock();
                    tbA.Text = MaterielName;
                    tbA.Foreground = Brushes.Red;
                    tbA.VerticalAlignment = VerticalAlignment.Top;
                    stackPanel.Children.Add(tbA);

                    TextBlock tbB = new TextBlock();
                    tbB.Text = MaterielCode;
                    tbB.Foreground = Brushes.Red;
                    tbB.VerticalAlignment = VerticalAlignment.Top;
                    stackPanel.Children.Add(tbB);

                    TextBlock tbC = new TextBlock();
                    tbC.Text = MaterielCount;
                    tbC.Foreground = Brushes.Red;
                    tbC.VerticalAlignment = VerticalAlignment.Top;
                    stackPanel.Children.Add(tbC);

                    grid.Children.Add(stackPanel);

                    itemList.Add(grid);
                }
            }
            else if (StoreInfoModel.MaterialInfo != null)
            {
                Grid grid = new Grid()
                {
                    Margin = new Thickness(2, 4, 2, 4)
                };
                StackPanel stackPanel = new StackPanel();

                string MaterielName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName;
                string MaterielCount = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity.ToString();
                //string MaterielCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartCode;
                string MaterielCode = StoreInfoModel.MaterialInfo.GetPart.FigureCode;

                TextBlock tbA = new TextBlock();
                tbA.Text = MaterielName;
                tbA.Foreground = Brushes.Red;
                tbA.VerticalAlignment = VerticalAlignment.Top;
                stackPanel.Children.Add(tbA);

                TextBlock tbB = new TextBlock();
                tbB.Text = MaterielCode;
                tbB.Foreground = Brushes.Red;
                tbB.VerticalAlignment = VerticalAlignment.Top;
                stackPanel.Children.Add(tbB);

                TextBlock tbC = new TextBlock();
                tbC.Text = MaterielCount;
                tbC.Foreground = Brushes.Red;
                tbC.VerticalAlignment = VerticalAlignment.Top;
                stackPanel.Children.Add(tbC);

                grid.Children.Add(stackPanel);

                itemList.Add(grid);
            }

            return itemList;
        }

        //扫描物料后更新物料列表
        private void ScanMateriel(string partCode)
        {

            List<TextBlock> itemList = materielItems.ItemsSource as List<TextBlock>;
            var list = StoreInfoModel.MaterialInfo;
            foreach (var item in StoreInfoModel.MaterialChildInfos)
            {
                string MaterielName = item.GetMaterialDetail.PartName;
                string MaterielCount = item.GetMaterialDetail.PartQuantity.ToString();

                if (partCode != item.GetMaterialDetail.PartCode)
                {
                    MessageBox.Show("该物料不在需求列表中");

                }
                else
                {



                }


                TextBlock tb = new TextBlock();
                tb.Text = MaterielName + "0/" + MaterielCount;
                tb.Foreground = Brushes.Red;
                itemList.Add(tb);

            }



        }

        private void MainPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            int n = (int)e.Key;
            switch (e.Key)
            {
                case Key.Tab:
                    #region
                    focusArea = ((int)focusArea + 1).Equals(4) ? 0 : (FocusAreaEnum)((int)focusArea + 1);
                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            RemoveTabBorder(tbDrawingList);
                            AddTabBorder(tbWorkStepsInfo);
                            break;
                        case FocusAreaEnum.CenterFocus:
                            RemoveTabBorder(tbWorkStepsInfo);
                            AddTabBorder(mxDraw);
                            break;
                        case FocusAreaEnum.RightFocus:
                            RemoveTabBorder(mxDraw);
                            AddTabBorder(materielList);
                            break;
                        case FocusAreaEnum.DownFocus:
                            RemoveTabBorder(materielList);
                            AddTabBorder(tbDrawingList);
                            break;
                    }
                    #endregion
                    break;
                case Key.Up:

                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            //WorkStepBar.Prev();
                            ShowWorkStepsInfo();
                            //workStepIndex = (workStepIndex - 1) <= -1 ? (workStepCount - 1) : (workStepIndex - 1);
                            workStepIndex = (workStepIndex - 1) <= -1 ? 0 : (workStepIndex - 1);
                            dataContext.WorkStep = dataContext.WorkSteps[workStepIndex];
                            WorkStepsList.SelectedIndex = workStepIndex;
                            break;
                        case FocusAreaEnum.CenterFocus:
                            pointY -= drawMoveOffset;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:
                            //RemoveTabBorder(mxDraw);
                            //AddTabBorder(materielList);
                            break;
                        case FocusAreaEnum.DownFocus:

                            break;
                        default: break;
                    }

                    break;
                case Key.Down:

                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:
                            //WorkStepBar.Next();
                            ShowWorkStepsInfo();
                            //workStepIndex = (workStepIndex + 1) >= workStepCount ? 0 : (workStepIndex + 1);
                            workStepIndex = (workStepIndex + 1) >= workStepCount ? (workStepCount - 1) : (workStepIndex + 1);
                            dataContext.WorkStep = dataContext.WorkSteps[workStepIndex];
                            WorkStepsList.SelectedIndex = workStepIndex;
                            break;
                        case FocusAreaEnum.CenterFocus:
                            pointY += drawMoveOffset;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:
                            //RemoveTabBorder(mxDraw);
                            //AddTabBorder(materielList);
                            break;
                        case FocusAreaEnum.DownFocus:

                            break;
                        default: break;
                    }

                    break;
                case Key.Left:

                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:

                            break;
                        case FocusAreaEnum.CenterFocus:
                            pointX += drawMoveOffset;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:

                            break;
                        case FocusAreaEnum.DownFocus:
                            if (!drawingCount.Equals(0))
                            {
                                ContentPresenter cpA = DrawCardItems.ItemContainerGenerator.ContainerFromIndex(drawingIndex) as ContentPresenter;
                                List<TextBlock> gdsA = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cpA);
                                if (gdsA.Count > 0)
                                {
                                    gdsA[0].Background = new SolidColorBrush(Color.FromRgb(65, 65, 65));
                                }

                                //CheckDrawListBtn(false);
                                drawingIndex = (drawingIndex - 1).Equals(-1) ? (drawingCount - 1) : (drawingIndex - 1);
                                OpenDrawing(drawFilePaths[drawingIndex]);

                                ContentPresenter cpB = DrawCardItems.ItemContainerGenerator.ContainerFromIndex(drawingIndex) as ContentPresenter;
                                List<TextBlock> gdsB = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cpB);
                                if (gdsB.Count > 0)
                                {
                                    gdsB[0].Background = new SolidColorBrush(Color.FromRgb(250, 80, 70));
                                }

                                //OpenDrawing(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[drawingIndex].FileName));
                                //BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】切换了图纸！", StoreInfoModel.UserName));
                                //CheckDrawListBtn(true);
                            }
                            break;
                        default: break;
                    }

                    break;
                case Key.Right:

                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:

                            break;
                        case FocusAreaEnum.CenterFocus:
                            pointX -= drawMoveOffset;
                            axMxDrawX.ZoomCenter(pointX, pointY);
                            break;
                        case FocusAreaEnum.RightFocus:

                            break;
                        case FocusAreaEnum.DownFocus:
                            if (!drawingCount.Equals(0))
                            {

                                ContentPresenter cpA = DrawCardItems.ItemContainerGenerator.ContainerFromIndex(drawingIndex) as ContentPresenter;
                                List<TextBlock> gdsA = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cpA);
                                if (gdsA.Count > 0)
                                {
                                    gdsA[0].Background = new SolidColorBrush(Color.FromRgb(65, 65, 65));
                                }

                                //CheckDrawListBtn(false);
                                drawingIndex = (drawingIndex - 1).Equals(-1) ? (drawingCount - 1) : (drawingIndex - 1);
                                OpenDrawing(drawFilePaths[drawingIndex]);

                                ContentPresenter cpB = DrawCardItems.ItemContainerGenerator.ContainerFromIndex(drawingIndex) as ContentPresenter;
                                List<TextBlock> gdsB = Helpers.WpfHelper.FindVisualChildren<TextBlock>(cpB);
                                if (gdsB.Count > 0)
                                {
                                    gdsB[0].Background = new SolidColorBrush(Color.FromRgb(250, 80, 70));
                                }

                                //OpenDrawing(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[drawingIndex].FileName));
                                //var temp = BLL.Helpers.ClientHelper.RecordOperateLog(string.Format(@"用户【{0}】切换了图纸！", StoreInfoModel.UserName));
                                //CheckDrawListBtn(true);
                            }
                            break;
                        default: break;
                    }

                    break;
                //+
                case Key.OemPlus:
                    if (focusArea.Equals(FocusAreaEnum.CenterFocus))
                    {
                        MouseSimulateWheelEvent(120);
                    }
                    break;
                //-
                case Key.OemMinus:
                    if (focusArea.Equals(FocusAreaEnum.CenterFocus))
                    {
                        MouseSimulateWheelEvent(-120);
                    }
                    break;
                case Key.Enter:

                    switch (focusArea)
                    {
                        case FocusAreaEnum.LeftFocus:

                            break;
                        case FocusAreaEnum.CenterFocus:

                            break;
                        case FocusAreaEnum.RightFocus:

                            break;
                        default: break;
                    }

                    break;
                case Key.LeftCtrl:
                case Key.Multiply:

                    WinView.ProceStateWindow window = new WinView.ProceStateWindow();
                    window.ShowDialog();
                    MessageBoxResult dr = window.Result;

                    var temp = BLL.Helpers.ClientHelper.RecordOperateLog(new List<string>() {
                            string.Format(@"用户【{0}】在图纸上打开了选择生产状态窗口！", StoreInfoModel.UserName),
                            string.Format(@"用户【{0}】在选择生产状态窗口中选择了【{1}】！{2}",
                            StoreInfoModel.UserName,dr==MessageBoxResult.Yes?"完工":dr==MessageBoxResult.No?"暂停":"取消",dr==MessageBoxResult.Yes?string .Format (@"并且输入的数量是【{0}】！",Convert.ToInt32(window.ProduceNum)):string .Empty),
                        });

                    //完工
                    if (dr == MessageBoxResult.Yes)
                    {
                        string msg = string.Empty;
                        bool result = new ProduceProceInfoBLL().ProduceProceResult(ref msg, Convert.ToInt32(window.ProduceNum), ProduceProceInfoBLL.ProduceProceStateEnum.CompleteTask);


                        if (!result)
                        {
                            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        NavigationService.GetNavigationService(this).Navigate(new Uri("View/ProducePlanPage.xaml", UriKind.Relative));
                    }
                    //暂停
                    else if (dr == MessageBoxResult.No)
                    {
                        string msg = string.Empty;
                        bool result = new ProduceProceInfoBLL().ProduceProceResult(ref msg, 0, ProduceProceInfoBLL.ProduceProceStateEnum.PauseTask);

                        //跳转至选择任务计划页面
                        NavigationService.GetNavigationService(this).Navigate(new Uri("View/ProducePlanPage.xaml", UriKind.Relative));
                    }
                    //取消
                    else
                    {

                    }

                    break;
                case Key.ImeProcessed:

                    return;
                default: break;
            }

            e.Handled = true;
        }


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

        public void ShowWorkStepsInfo()
        {
            //workStepIndex = WorkStepBar.StepIndex;
            //var obj = WorkStepBar.Items[workStepIndex];
            //var attr = obj.GetType().GetProperty("WorkStepsInfo");
            //strWorkStepsInfo = attr.GetValue(obj, null) == null ? string.Empty : attr.GetValue(obj, null).ToString();
            //
            //if (WorkStepsInfoContainer.Children.Count.Equals(0))
            //{
            //    WorkStepsInfoContainer.Children.Add(new TextBlock()
            //    {
            //        Padding = new Thickness(8, 5, 2, 8),
            //        FontSize = 14,
            //        TextWrapping = TextWrapping.Wrap,
            //        Foreground = new SolidColorBrush(Colors.White),
            //        Text = strWorkStepsInfo,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        HorizontalAlignment = HorizontalAlignment.Center
            //    });
            //}
            //else
            //{
            //    for (int i = 0; i < WorkStepsInfoContainer.Children.Count; i++)
            //    {
            //        if (WorkStepsInfoContainer.Children[i] is TextBlock)
            //        {
            //            ((TextBlock)WorkStepsInfoContainer.Children[i]).Text = strWorkStepsInfo;
            //        }
            //    }
            //}
        }

        public void HiddenWorkStepsInfo()
        {
            //WorkStepsInfoContainer.Children.Clear();
        }

        public void CheckDrawListBtn(bool check)
        {
            ContentPresenter cp = DrawCardItems.ItemContainerGenerator.ContainerFromIndex(drawingIndex) as ContentPresenter;
            List<RadioButton> gds = Helpers.WpfHelper.FindVisualChildren<RadioButton>(cp);
            gds[0].IsChecked = check;
        }

        public void RemoveTabBorder(Grid grid)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] is Border)
                {
                    grid.Children.Remove(grid.Children[i]);
                }
            }
        }

        public void AddTabBorder(Grid grid)
        {
            Border tbBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 255)),
                BorderThickness = new Thickness(2)
            };
            grid.Children.Add(tbBorder);
        }

        public string ToUpper(object value)
        {
            try
            {
                string hash = double.Parse(value.ToString()).ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
                string results = Regex.Replace(hash, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[.]|$))))", "${b}${z}");
                hash = Regex.Replace(results, ".", delegate (Match m) { return "负点空〇一二三四五六七八九空空空空空空空分角十百千万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
                if (hash.Substring(hash.Length - 1, 1) == "点")
                {
                    hash = hash.Replace("点", "");
                    return hash;
                }
                else
                {
                    hash = hash.Replace("角", "").Replace("分", "");
                    return hash;
                }
            }
            catch (Exception)
            {
                return "〇";
            }
        }


    }
}
