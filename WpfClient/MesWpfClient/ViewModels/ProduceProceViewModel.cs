using BLL.ExtraBLL;
using BLL.Helpers;
using MesWpfClient.API;
using MesWpfClient.AppStart;
using MesWpfClient.Commands;
using MesWpfClient.Common;
using MesWpfClient.Helpers;
using MesWpfClient.Models;
using MesWpfClient.Views.WinViews;
using Model;
using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;

namespace MesWpfClient.ViewModels
{
    public class ProduceProceViewModel : ViewModelBase
    {
        #region 模拟 滚轮操作图纸 放大&缩小

        [DllImport("user32.dll")]
        private static extern int mouse_event(MouseEventFlag dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

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

        public static void MouseSimulateWheelEvent(int value)
        {
            int pointX = ConfigInfoModel.WinScreenWidth / 2;
            int pointY = ConfigInfoModel.WinScreenHeight / 2;
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / ConfigInfoModel.WinScreenWidth, pointY * 65536 / ConfigInfoModel.WinScreenHeight, 0, 0);
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / ConfigInfoModel.WinScreenWidth, pointY * 65536 / ConfigInfoModel.WinScreenHeight, value, 0);
        }

        #endregion

        private readonly string[] fileType = new string[] { ".jpg", ".JPG", ".png", ".PNG", ".dwg", ".DWG" };
        private AxMxDrawXLib.AxMxDrawX axMxDrawX;
        private double pLbx = 0, pLby = 0, pRtx = 0, pRty = 0;//图纸大小
        private double pointX = 0, pointY = 0;//图纸中心点
        private int drawMoveOffset = 70;//图纸移动偏移量

        public ProduceProceViewModel(AxMxDrawXLib.AxMxDrawX _axMxDrawX)
        {
            try
            {
                axMxDrawX = _axMxDrawX;

                LogHelper.WriteLog($"加载完成mxCAD控件");

                workSteps = GetWorkStepsData();
                LogHelper.WriteLog($"加载工步完成");
                drawings = GetDrawingsData();
                LogHelper.WriteLog($"加载图纸完成");
                maintains = ConvertCommon.ToObservable(GetMaintainData());
                LogHelper.WriteLog($"加载物料完成");

                Task.Run(() =>
                {
                    Execute.OnUIThread(() =>
                    {
                        ProjectName = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName.Trim();
                        LogHelper.WriteLog($"获取到项目名");
                        PartName = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartName.Trim();
                        LogHelper.WriteLog($"获取到零件名");
                        PartFigure = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartFigureCode.Trim();
                        LogHelper.WriteLog($"获取到图号");

                        if (WorkSteps.Count > 0)
                        {
                            LogHelper.WriteLog($"工步数量：{WorkSteps.Count}");
                            SelectWorkStepsIndex = 0;
                        }

                        if (Maintains.Count > 0)
                        {
                            LogHelper.WriteLog($"物料数量：{Maintains.Count}");
                            SelectMaintainIndex = 0;
                        }

                        if (Drawings.Count > 0)
                        {
                            LogHelper.WriteLog($"图纸数量：{Drawings.Count}");
                            SelectDrawingsIndex = 0;
                        }
                    });
                });

                LogHelper.WriteLog($"正在绑定命令");
                QrCodeScanCommand = new DelegateCommand(obj => QrCodeScan(obj));
                MxDrawCommand = new DelegateCommand(obj => MxDrawOperate(obj));
                LogHelper.WriteLog($"绑定命令完成");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"初始化生产看图纸页面出现异常：{ex}");
            }
        }

        //******************************************************************************************************************//

        private string projectName;

        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string partName;

        public string PartName
        {
            get
            {
                return partName;
            }
            set
            {
                partName = value;
                OnPropertyChanged(nameof(PartName));
            }
        }

        private string partFigure;

        public string PartFigure
        {
            get
            {
                return partFigure;
            }
            set
            {
                partFigure = value;
                OnPropertyChanged(nameof(PartFigure));
            }
        }

        private string qrCode;

        public string QrCode
        {
            get
            {
                return qrCode;
            }
            set
            {
                qrCode = value;
                OnPropertyChanged(nameof(QrCode));
            }
        }

        //******************************************************************************************************************//

        private int selectWorkStepsIndex = -1;

        public int SelectWorkStepsIndex
        {
            get
            {
                return selectWorkStepsIndex;
            }
            set
            {
                selectWorkStepsIndex = value;
                WorkStep = WorkSteps[value];
                OnPropertyChanged(nameof(SelectWorkStepsIndex));
            }
        }


        private WorkStepsModel workStep;

        public WorkStepsModel WorkStep
        {
            get
            {
                return workStep;
            }
            set
            {
                workStep = value;
                OnPropertyChanged(nameof(WorkStep));
            }
        }


        private List<WorkStepsModel> workSteps = new List<WorkStepsModel>();

        public List<WorkStepsModel> WorkSteps
        {
            get
            {
                return workSteps;
            }
            set
            {
                workSteps = value;
                OnPropertyChanged(nameof(WorkSteps));
            }
        }

        //******************************************************************************************************************//

        private int selectMaintainIndex = -1;

        public int SelectMaintainIndex
        {
            get
            {
                return selectMaintainIndex;
            }
            set
            {
                selectMaintainIndex = value;
                //if (value.Equals(-1) && Maintains.Count > 0)
                //{
                //Maintain = Maintains[0];
                //}
                //else
                //{
                //Maintain = Maintains[value];
                //}
                OnPropertyChanged(nameof(SelectMaintainIndex));
            }
        }


        private MaterialModel maintain;

        public MaterialModel Maintain
        {
            get
            {
                return maintain;
            }
            set
            {
                maintain = value;
                OnPropertyChanged(nameof(Maintain));
            }
        }


        private ObservableCollection<MaterialModel> maintains = new ObservableCollection<MaterialModel>();

        public ObservableCollection<MaterialModel> Maintains
        {
            get
            {
                return maintains;
            }
            set
            {
                maintains = value;
                OnPropertyChanged(nameof(Maintains));
            }
        }

        //******************************************************************************************************************//

        private List<DrawingModel> drawings = new List<DrawingModel>();

        public List<DrawingModel> Drawings
        {
            get
            {
                return drawings;
            }
            set
            {
                drawings = value;
                OnPropertyChanged(nameof(Drawings));
            }
        }


        private DrawingModel drawing;

        public DrawingModel Drawing
        {
            get
            {
                return drawing;
            }
            set
            {
                drawing = value;

                axMxDrawX.OpenDwgFile(value.DrawPath);
                axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
                pointX = (pRtx - pLbx) / 2;
                pointY = (pRty - pLby) / 2;
                axMxDrawX.ZoomCenter(pointX, pointY);
                axMxDrawX.AutoZoomAll = true;

                OnPropertyChanged(nameof(Drawing));
            }
        }


        private int selectDrawingsIndex = -1;

        public int SelectDrawingsIndex
        {
            get
            {
                return selectDrawingsIndex;
            }
            set
            {
                selectDrawingsIndex = value;
                Drawing = Drawings[value];
                OnPropertyChanged(nameof(SelectDrawingsIndex));
            }
        }

        //******************************************************************************************************************//

        public DelegateCommand QrCodeScanCommand { get; set; }

        public DelegateCommand MxDrawCommand { get; set; }

        public void QrCodeScan(object obj)
        {
            string code = obj.ToString();

            QrCode = "";

            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            WindowHelper.MaterialScanWindow = new WindowMaterialScan(code);
            WindowHelper.MaterialScanWindow.ShowDialog();
            new SerialPortScanCode().Start();
            WindowHelper.IsOpenMaterialScanWindow = false;

            new MaterialInfoBLL().GetMaterialDetail();
            Maintains = ConvertCommon.ToObservable(GetMaintainData());
            if (Maintains.Count > 0)
            {
                SelectMaintainIndex = 0;
            }

        }

        //梦想CAD控件操作
        public void MxDrawOperate(object obj)
        {
            //https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.input.key?view=netframework-4.5

            switch (obj)
            {
                case Key.Enter:

                    //弹出扫码界面
                    //WindowMaterialScan windowA = new WindowMaterialScan();
                    //windowA.ShowDialog();
                    WindowHelper.MaterialScanWindow = new WindowMaterialScan();
                    WindowHelper.MaterialScanWindow.ShowDialog();
                    new SerialPortScanCode().Start();
                    WindowHelper.IsOpenMaterialScanWindow = false;

                    //重加载物料数据
                    new MaterialInfoBLL().GetMaterialDetail();
                    Maintains = ConvertCommon.ToObservable(GetMaintainData());
                    if (Maintains.Count > 0)
                    {
                        SelectMaintainIndex = 0;
                    }

                    if ((WindowHelper.MaterialScanWindow as WindowMaterialScan).OperateType.Equals(0))
                    {
                        //不操作
                    }
                    else
                    {
                        //弹出选择 完工/暂停/取消 窗口
                        WindowProceState windowB = new WindowProceState();
                        windowB.ShowDialog();

                        //完工
                        if (windowB.SelectState.Equals(0))
                        {
                            ResultModel resultA = ProduceProceInfoBLL.IsLastProcess();
                            if (resultA.Result)
                            {
                                //是最后一道工序
                                if (Convert.ToBoolean(resultA.Data))
                                {
                                    //判断物料是否扫码完成
                                    bool isComplete = true;
                                    foreach (var item in Maintains)
                                    {
                                        if (!item.PartQuantity.Equals(item.PartAlreadyScanQuantity))
                                        {
                                            isComplete = false;
                                            break;
                                        }
                                    }

                                    if (!isComplete)
                                    {
                                        MessageBox.Show(@"物料未扫完，禁止完工！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                    else
                                    {
                                        //提示打码
                                        MessageBoxResult boxResult = MessageBox.Show("是否打码？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                                        if (boxResult == MessageBoxResult.OK)
                                        {
                                            //获取条码
                                            ResultModel resultE = ProduceProceInfoBLL.GenerateBarCode(StoreInfoModel.MaterialInfo.GetProcessBom.InventoryCode);
                                            if (resultE.Result)
                                            {
                                                int PointX = -25;//起始点X坐标
                                                int PointY = 0;//起始点Y坐标
                                                //string PrintName = "Honeywell PC42t (203 dpi) - DP";//打印机名称
                                                string PrintName = ConfigInfoModel.PrintMachineName;//打印机名称
                                                string ContractName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode; //合同
                                                string ProductName = StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName; //产品
                                                string PartCode = StoreInfoModel.MaterialInfo.GetProcessBom.PartFigureCode; //零件图号
                                                string PartName = StoreInfoModel.MaterialInfo.GetProcessBom.PartName; //零件名称
                                                string MaterialName = StoreInfoModel.MaterialInfo.GetProcessBom.MaterialCode; //材质
                                                string BarCode = resultE.Data.ToString();//条码
                                                bool isOK = CsBarCodePrint.BarCodePrint(PrintName, PointX, PointY, ContractName, ProductName, PartCode, PartName, MaterialName, BarCode);

                                                if (isOK)
                                                {
                                                    MessageBox.Show("打印成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                                                    ResultModel resultD = ProduceProceInfoBLL.ProduceProceResult(0, windowB.SelectNum, ProduceProceInfoBLL.ProduceProceStateEnum.CompleteTask);
                                                    if (resultD.Result)
                                                    {
                                                        WindowHelper.ShowPageProducePlan();
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(resultD.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("打印失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(resultE.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                        }
                                    }
                                }
                                //不是最后一道工序
                                else
                                {
                                    ResultModel resultB = ProduceProceInfoBLL.ProduceProceResult(0, windowB.SelectNum, ProduceProceInfoBLL.ProduceProceStateEnum.CompleteTask);
                                    if (resultB.Result)
                                    {
                                        WindowHelper.ShowPageProducePlan();
                                    }
                                    else
                                    {
                                        MessageBox.Show(resultB.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        //暂停
                        else if (windowB.SelectState.Equals(1))
                        {
                            ResultModel resultC = ProduceProceInfoBLL.ProduceProceResult(windowB.PauseReson, 0, ProduceProceInfoBLL.ProduceProceStateEnum.PauseTask);
                            if (resultC.Result)
                            {
                                WindowHelper.ShowPageProducePlan();
                            }
                            else
                            {
                                MessageBox.Show(resultC.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        //取消
                        else
                        {

                        }
                    }

                    break;
                case Key.Up:
                    pointY -= drawMoveOffset;
                    axMxDrawX.ZoomCenter(pointX, pointY);
                    break;
                case Key.Down:
                    pointY += drawMoveOffset;
                    axMxDrawX.ZoomCenter(pointX, pointY);
                    break;
                case Key.Left:
                    pointX += drawMoveOffset;
                    axMxDrawX.ZoomCenter(pointX, pointY);
                    break;
                case Key.Right:
                    pointX -= drawMoveOffset;
                    axMxDrawX.ZoomCenter(pointX, pointY);
                    break;
                case Key.OemPlus://+
                case Key.Add://+
                case Key.OemCloseBrackets://}
                    MouseSimulateWheelEvent(150);
                    break;
                case Key.OemMinus://-
                case Key.Subtract://-
                case Key.OemOpenBrackets://{
                    MouseSimulateWheelEvent(-120);
                    break;
                default: break;
            }

        }

        //******************************************************************************************************************//

        //******************************************************************************************************************//

        public List<WorkStepsModel> GetWorkStepsData()
        {
            List<WorkStepsModel> result = new List<WorkStepsModel>();

            //for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps.Count; i++)
            //{
            //    result.Add(new WorkStepsModel()
            //    {
            //        LineNum = i + 1,
            //        WorkStepsName = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsName,
            //        WorkStepsInfo = StoreInfoModel.ProducePlanInfo.GetProcessWorkSteps[i].WorkStepsContent
            //    });
            //}

            result.Add(new WorkStepsModel()
            {
                LineNum = 1,
                WorkStepsName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkStepsName,
                WorkStepsInfo = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkStepsContent
            });

            return result;
        }

        public List<MaterialModel> GetMaintainData()
        {
            try
            {
                bool isZhuDuanJian = StoreInfoModel.MaterialInfo.GetMaterialDetail.IsZhuDuanJian;
                List<MaterialModel> result = new List<MaterialModel>();

                LogHelper.WriteLog($"本体为铸件锻件：{isZhuDuanJian}");

                if (StoreInfoModel.MaterialInfo != null)
                {
                    if (isZhuDuanJian)
                    {
                        if (StoreInfoModel.MaterialInfo.GetPart == null)
                        {
                            LogHelper.WriteLog($"本体为铸件锻件，但是在part表查不到信息！");
                        }

                        result.Add(new MaterialModel()
                        {
                            PartCode = StoreInfoModel.MaterialInfo.GetPart.InventoryCode ?? "",
                            PartName = StoreInfoModel.MaterialInfo.GetPart.InventoryName ?? "",
                            PartFigureCode = StoreInfoModel.MaterialInfo.GetPart.Model ?? "",
                            ParentCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.ParentCode ?? "",
                            PartQuantity = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity,
                            PartAlreadyScanQuantity = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartAlreadyScanQuantity,
                            MaterialCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.MaterialCode,
                            Proportion = $"{StoreInfoModel.MaterialInfo.GetMaterialDetail.PartAlreadyScanQuantity}/{StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity}"
                        });
                    }
                }

                if (StoreInfoModel.MaterialChildInfos != null && StoreInfoModel.MaterialChildInfos.Count > 0)
                {
                    foreach (var item in StoreInfoModel.MaterialChildInfos)
                    {
                        result.Add(new MaterialModel()
                        {
                            PartCode = item.GetMaterialDetail.PartCode,
                            PartName = item.GetMaterialDetail.PartName,
                            PartFigureCode = item.GetMaterialDetail.PartFigureCode,
                            ParentCode = item.GetMaterialDetail.ParentCode,
                            PartQuantity = item.GetMaterialDetail.PartQuantity,
                            PartAlreadyScanQuantity = item.GetMaterialDetail.PartAlreadyScanQuantity,
                            MaterialCode = item.GetMaterialDetail.MaterialCode,
                            Proportion = $"{item.GetMaterialDetail.PartAlreadyScanQuantity}/{item.GetMaterialDetail.PartQuantity}"
                        });
                    }
                }

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].LineNum = i + 1;
                }

                //*********  生成条码测试  *********//
                /*
                StringBuilder sb = new StringBuilder();
                foreach (var item in StoreInfoModel.MaterialInfos)
                {
                    sb.Append($"合同：({StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode}) {StoreInfoModel.ProducePlanInfo.GetProject.ProjectName.Trim()}\r\n");
                    sb.Append($"产品：{StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName}\r\n");
                    sb.Append($"零件图号：{item.GetMaterialDetail.PartFigureCode}\r\n");
                    sb.Append($"零件名称：{item.GetMaterialDetail.PartName}\r\n");
                    sb.Append($"材质：{item.GetProcessBom.MaterialCode}\r\n");
                    sb.Append($"条码：{item.GetPart.CorrespondingBarcode ?? ""}\r\n");
                    sb.Append($"零件ProcessBomID：{item.GetProcessBom.ID}\r\n");
                    sb.Append($"零件InventoryCode：{item.GetPart.InventoryCode}\r\n");
                    sb.Append($"零件PartID：{item.GetPart.ID}\r\n");
                    sb.Append($"零件编码：{item.GetMaterialDetail.PartCode}\r\n");
                    sb.Append($"\r\n");
                }
                var temp = sb.ToString();
                */

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"绑定右侧物料出错：{ex}");

                List<MaterialModel> result = new List<MaterialModel>();
                return result;
            }
        }

        public List<DrawingModel> GetDrawingsData()
        {
            List<DrawingModel> result = new List<DrawingModel>();

            //for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetPartFiles.Count; i++)
            //{
            //    if (fileType.Contains(System.IO.Path.GetExtension(StoreInfoModel.ProducePlanInfo.GetPartFiles[i].FileName)))
            //    {
            //        result.Add(new DrawingModel()
            //        {
            //            DrawName = StoreInfoModel.ProducePlanInfo.GetPartFiles[i].FileName,
            //            DrawPath = string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, StoreInfoModel.ProducePlanInfo.GetPartFiles[i].FileName)
            //        });
            //    }
            //}

            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetProcessFigures.Count; i++)
            {
                string fileName = StoreInfoModel.ProducePlanInfo.GetProcessFigures[i].FileName;
                if (fileType.Contains(Path.GetExtension(fileName)))
                {
                    result.Add(new DrawingModel()
                    {
                        DrawName = fileName,
                        DrawPath = string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, fileName)
                    });
                }
                if (Path.GetExtension(fileName).ToLower().Equals(".pdf"))
                {
                    using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, fileName), FileMode.Open))
                    {
                        PDFFile pdfFile = PDFFile.Open(fs);
                        for (int j = 0; j < pdfFile.PageCount; j++)
                        {
                            Bitmap pageImage = pdfFile.GetPageImage(j, 56 * 10);
                            string newFileName = string.Format(@"{0}_{1}.{2}", Path.GetFileNameWithoutExtension(fileName), j, ImageFormat.Png.ToString());
                            pageImage.Save(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, newFileName), ImageFormat.Png);

                            result.Add(new DrawingModel()
                            {
                                DrawName = newFileName,
                                DrawPath = string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, newFileName)
                            });
                        }
                    }
                }
            }

            for (int i = 0; i < StoreInfoModel.ProducePlanInfo.GetQualityReportDocs.Count; i++)
            {
                string fileName = StoreInfoModel.ProducePlanInfo.GetQualityReportDocs[i].FileName;
                if (fileType.Contains(Path.GetExtension(fileName)))
                {
                    result.Add(new DrawingModel()
                    {
                        DrawName = fileName,
                        DrawPath = string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, fileName)
                    });
                }

                if (Path.GetExtension(fileName).ToLower().Equals(".pdf"))
                {
                    using (FileStream fs = new FileStream(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, fileName), FileMode.Open))
                    {
                        PDFFile pdfFile = PDFFile.Open(fs);
                        for (int j = 0; j < pdfFile.PageCount; j++)
                        {
                            Bitmap pageImage = pdfFile.GetPageImage(j, 56 * 10);
                            string newFileName = string.Format(@"{0}_{1}.{2}", Path.GetFileNameWithoutExtension(fileName), j + 1, ImageFormat.Png.ToString());
                            pageImage.Save(string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, newFileName), ImageFormat.Png);

                            result.Add(new DrawingModel()
                            {
                                DrawName = newFileName,
                                DrawPath = string.Format(@"{0}\{1}", ConfigInfoModel.DocDirName, newFileName)
                            });
                        }
                    }
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].LineNum = i + 1;
            }

            return result;
        }

    }
}
