using MesClient.API;
using MesClient.AppStart;
using MesClient.Commands;
using MesClient.Common;
using MesClient.Entitys;
using MesClient.Models;
using MesClient.Views.WinViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MesClient.ViewModels
{
    public class PlanExecuteViewModel : ViewModelBase
    {
        private AxMxDrawXLib.AxMxDrawX axMxDrawX;
        private double pLbx = 0, pLby = 0, pRtx = 0, pRty = 0;//图纸大小
        private double pointX = 0, pointY = 0;//图纸中心点
        private int drawMoveOffset = ConfigInfo.MoveOffset;//图纸移动偏移量

        public PlanExecuteViewModel(AxMxDrawXLib.AxMxDrawX _axMxDrawX, ref string drawFilePath)
        {
            axMxDrawX = _axMxDrawX;

            StoreInfo.CurrentPlanFiles.ForEach(item =>
            {
                fileInfos.Add(new FileLineInfoEntity
                {
                    LineNum = FileInfos.Count + 1,
                    DocName = item.DocName,
                    FileName = item.FileName,
                    FileAddress = item.FileAddress,
                    WebAddressType = item.WebAddressType
                });
            });

            if (fileInfos.Count > 0)
            {
                fileSelectIndex = 0;
                drawFilePath = Path.Combine(ConfigInfo.DocDirName, fileInfos[0].FileName);
            }

            ResetMaterial();

            MxDrawCommand = new DelegateCommand(obj => MxDrawOperate(obj));
            QrCodeScanCommand = new DelegateCommand(obj => QrCodeScan(obj));
        }

        //******************************************************************************************************************//

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

        public string WorkStepsName
        {
            get
            {
                return workStepsName;
            }
            set
            {
                workStepsName = value;
                OnPropertyChanged(nameof(WorkStepsName));
            }
        }

        public string WorkStepsContent
        {
            get
            {
                return workStepsContent;
            }
            set
            {
                workStepsContent = value;
                OnPropertyChanged(nameof(WorkStepsContent));
            }
        }

        public string ProgramCode
        {
            get
            {
                return programCode;
            }
            set
            {
                programCode = value;
                OnPropertyChanged(nameof(ProgramCode));
            }
        }

        public string BatchCode
        {
            get
            {
                return batchCode;
            }
            set
            {
                batchCode = value;
                OnPropertyChanged(nameof(BatchCode));
            }
        }

        public string InventoryName
        {
            get
            {
                return inventoryName;
            }
            set
            {
                inventoryName = value;
                OnPropertyChanged(nameof(InventoryName));
            }
        }

        public string Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public string IsBlanking
        {
            get
            {
                return isBlanking;
            }
            set
            {
                isBlanking = value;
                OnPropertyChanged(nameof(IsBlanking));
            }
        }

        public string IsProduce
        {
            get
            {
                return isProduce;
            }
            set
            {
                isProduce = value;
                OnPropertyChanged(nameof(IsProduce));
            }
        }

        private string projectName = StoreInfo.CurrentWorkingTicket.ProjectName;
        private string partName = StoreInfo.CurrentWorkingTicket.PartName;
        private string partFigure = StoreInfo.CurrentWorkingTicket.FigureNo;
        private string workStepsName = StoreInfo.CurrentWorkingTicket.WorkStepsName;
        private string workStepsContent = StoreInfo.CurrentWorkingTicket.WorkStepsContent;
        private string programCode = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop) ? StoreInfo.BlankingMaterial == null ? "" : StoreInfo.BlankingMaterial.ProgramCode : "";
        private string batchCode = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop) ? StoreInfo.BlankingMaterial == null ? "" : StoreInfo.BlankingMaterial.BatchCode : "";
        private string inventoryName = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop) ? StoreInfo.BlankingMaterial == null ? "" : StoreInfo.BlankingMaterial.InventoryName : "";
        private string model = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop) ? StoreInfo.BlankingMaterial == null ? "" : StoreInfo.BlankingMaterial.Model : "";
        private string isBlanking = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop) ? "Visible" : "Hidden";
        private string isProduce = WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.ProduceShop) ? "Visible" : "Hidden";


        //******************************************************************************************************************//

        private ObservableCollection<FileLineInfoEntity> fileInfos = new ObservableCollection<FileLineInfoEntity>();

        public ObservableCollection<FileLineInfoEntity> FileInfos
        {
            get
            {
                return fileInfos;
            }
            set
            {
                fileInfos = value;
                OnPropertyChanged(nameof(FileInfos));
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

        private int fileSelectIndex = -1;

        public int FileSelectIndex
        {
            get
            {
                return fileSelectIndex;
            }
            set
            {
                fileSelectIndex = value;
                if (value >= 0 && value < FileInfos.Count)
                {
                    axMxDrawX.OpenDwgFile(Path.Combine(ConfigInfo.DocDirName, FileInfos[value].FileName));
                    axMxDrawX.GetMcDbDatabaseBound(ref pLbx, ref pLby, ref pRtx, ref pRty);
                    pointX = (pRtx - pLbx) / 2;
                    pointY = (pRty - pLby) / 2;
                    axMxDrawX.ZoomCenter(pointX, pointY);
                }
                OnPropertyChanged(nameof(FileSelectIndex));
            }
        }


        private int maintainSelectIndex = -1;

        public int MaintainSelectIndex
        {
            get
            {
                return maintainSelectIndex;
            }
            set
            {
                maintainSelectIndex = value;
                OnPropertyChanged(nameof(MaintainSelectIndex));
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

        public DelegateCommand MxDrawCommand { get; set; }

        public DelegateCommand QrCodeScanCommand { get; set; }

        public void MxDrawOperate(object obj)
        {
            //https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.input.key?view=netframework-4.5

            switch (obj)
            {
                case Key.Enter:

                    if (WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop))
                    {
                        //弹出选择 完工/暂停/取消 窗口
                        WindowPlanExecuteState window = new WindowPlanExecuteState();
                        window.ShowDialog();

                        //完工
                        if (window.SelectState.Equals(0))
                        {
                            var resultB = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.End);
                            var resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.End, window.SelectNum);

                            WindowHelper.ShowPagePlanCard();
                        }
                        //暂停
                        else if (window.SelectState.Equals(1))
                        {
                            ResultModel resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.Pause, window.SelectNum, (EnumManager.PauseResonEnum)window.PauseReson);
                            if (resultC.Result)
                            {
                                WindowHelper.ShowPagePlanCard();
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
                        return;
                    }

                    //弹出扫码界面
                    WindowHelper.MaterialScanWindow = new WindowMaterialScan();
                    WindowHelper.MaterialScanWindow.ShowDialog();
                    new SerialPortScanCode().Start();
                    WindowHelper.IsOpenMaterialScanWindow = false;

                    //重加载物料数据
                    ResetMaterial();

                    if ((WindowHelper.MaterialScanWindow as WindowMaterialScan).OperateType.Equals(0))
                    {
                        //不操作
                    }
                    else
                    {
                        //弹出选择 完工/暂停/取消 窗口
                        WindowPlanExecuteState window = new WindowPlanExecuteState();
                        window.ShowDialog();

                        //完工
                        if (window.SelectState.Equals(0))
                        {
                            ResultModel resultA = ApiManager.IsLastProcess();
                            if (resultA.Result)
                            {
                                //是最后一道工序
                                if (Convert.ToBoolean(resultA.Data))
                                {
                                    //判断物料是否扫码完成
                                    bool isComplete = true;
                                    foreach (var item in Maintains)
                                    {
                                        if (!item.TotalQuantity.Equals(item.AlreadyScanQuantity))
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
                                        #region 弃用的打印功能
                                        /*//提示打码
                                        MessageBoxResult boxResult = MessageBox.Show("是否打码？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                                        if (boxResult == MessageBoxResult.OK)
                                        {
                                            //获取条码
                                            ResultModel resultE = ApiManager.GenerateBarCode();
                                            if (resultE.Result)
                                            {
                                                int PointX = -25;//起始点X坐标
                                                int PointY = 0;//起始点Y坐标
                                                               //string PrintName = "Honeywell PC42t (203 dpi) - DP";//打印机名称
                                                string PrintName = ConfigInfo.PrintMachineName;//打印机名称
                                                string ContractName = StoreInfo.CurrentWorkingTicket.ContractCode; //合同
                                                string ProductName = StoreInfo.CurrentWorkingTicket.ProductName; //产品
                                                string PartCode = StoreInfo.CurrentWorkingTicket.FigureNo; //零件图号
                                                string PartName = StoreInfo.CurrentWorkingTicket.PartName; //零件名称
                                                string MaterialName = StoreInfo.MaterialOneSelf.MaterialInfo.MaterialCode; //材质
                                                string BarCode = resultE.Data.ToString();//条码
                                                bool isOK = CsBarCodePrint.BarCodePrint(PrintName, PointX, PointY, ContractName, ProductName, PartCode, PartName, MaterialName, BarCode);

                                                if (isOK)
                                                {
                                                    MessageBox.Show("打印成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                                                    var resultB = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.End);
                                                    var resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.End, window.SelectNum);

                                                    WindowHelper.ShowPagePlanCard();
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
                                        }*/
                                        #endregion

                                        var resultB = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.End);
                                        var resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.End, window.SelectNum);

                                        WindowHelper.ShowPagePlanCard();
                                    }
                                }
                                //不是最后一道工序
                                else
                                {
                                    var resultB = ApiManager.SetActualProducePlanDate(EnumManager.ProducePlanStateEnum.End);
                                    var resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.End, window.SelectNum);

                                    WindowHelper.ShowPagePlanCard();
                                }
                            }
                            else
                            {
                                MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        //暂停
                        else if (window.SelectState.Equals(1))
                        {
                            ResultModel resultC = ApiManager.SetProduceLog(EnumManager.ProduceLogTypeEnum.Pause, window.SelectNum, (EnumManager.PauseResonEnum)window.PauseReson);
                            if (resultC.Result)
                            {
                                WindowHelper.ShowPagePlanCard();
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
                    SimulationUserCommon.MouseSimulateWheelEvent(ConfigInfo.ZoomIn);
                    break;
                case Key.OemMinus://-
                case Key.Subtract://-
                case Key.OemOpenBrackets://{
                    SimulationUserCommon.MouseSimulateWheelEvent(ConfigInfo.ZoomOut);
                    break;
                default: break;
            }

        }

        public void QrCodeScan(object obj)
        {
            string code = obj.ToString();

            QrCode = "";

            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            if (WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop))
            {
                return;
            }

            WindowHelper.MaterialScanWindow = new WindowMaterialScan(code);
            WindowHelper.MaterialScanWindow.ShowDialog();
            new SerialPortScanCode().Start();
            WindowHelper.IsOpenMaterialScanWindow = false;

            ResetMaterial();
        }


        //******************************************************************************************************************//

        /// <summary>
        /// 重载物料信息
        /// </summary>
        private void ResetMaterial()
        {
            if (WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop))
            {
                return;
            }

            ApiManager.GetMaterialInfo();

            Maintains.Clear();

            if (StoreInfo.MaterialOneSelf.IsCastingOrForging)
            {
                Maintains.Add(new MaterialModel()
                {
                    LineNum = maintains.Count + 1,
                    PartCode = StoreInfo.MaterialOneSelf.MaterialInfo.PartCode,
                    PartName = StoreInfo.MaterialOneSelf.MaterialInfo.PartName,
                    BarCode = StoreInfo.MaterialOneSelf.MaterialInfo.ExtraInfo.PartInfo.CorrespondingBarcode,
                    PartFigureCode = StoreInfo.MaterialOneSelf.MaterialInfo.FigureNo,
                    MaterialCode = StoreInfo.MaterialOneSelf.MaterialInfo.MaterialCode,
                    AlreadyScanQuantity = StoreInfo.MaterialOneSelf.MaterialInfo.AlreadyScanQuantity,
                    TotalQuantity = StoreInfo.MaterialOneSelf.MaterialInfo.TotalQuantity,
                    Proportion = $"{StoreInfo.MaterialOneSelf.MaterialInfo.AlreadyScanQuantity}/{StoreInfo.MaterialOneSelf.MaterialInfo.TotalQuantity}"
                });
            }

            foreach (var item in StoreInfo.MaterialChild.MaterialInfos)
            {
                Maintains.Add(new MaterialModel()
                {
                    LineNum = maintains.Count + 1,
                    PartCode = item.PartCode,
                    PartName = item.PartName,
                    BarCode = item.ExtraInfo.PartInfo.CorrespondingBarcode,
                    PartFigureCode = item.FigureNo,
                    MaterialCode = item.MaterialCode,
                    AlreadyScanQuantity = item.AlreadyScanQuantity,
                    TotalQuantity = item.TotalQuantity,
                    Proportion = $"{item.AlreadyScanQuantity}/{item.TotalQuantity}"
                });
            }

            MaintainSelectIndex = Maintains.Count > 0 ? 0 : -1;

        }

    }
}
