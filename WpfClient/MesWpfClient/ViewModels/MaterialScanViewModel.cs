using BLL.ExtraBLL;
using BLL.Helpers;
using MesWpfClient.API;
using MesWpfClient.Commands;
using MesWpfClient.Helpers;
using MesWpfClient.Models;
using MesWpfClient.Views.WinViews;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.DocumentStructures;

namespace MesWpfClient.ViewModels
{
    public class MaterialScanViewModel : ViewModelBase
    {
        private WindowMaterialScan window;

        public MaterialScanViewModel(WindowMaterialScan _window, string barCode)
        {
            window = _window;
            WindowHelper.IsOpenMaterialScanWindow = true;

            LoadData();

            if (!string.IsNullOrEmpty(barCode))
            {
                IsShowButton = "Hidden";
                BarCodeScan(barCode);
            }

            BarCodeScanCommand = new DelegateCommand(obj => BarCodeScan(obj));
            ExitCommand = new DelegateCommand(obj => Exit(obj));
            OperateCommand = new DelegateCommand(obj => Operate(obj));
        }

        //******************************************************************************************************************//

        private string barCode;

        public string BarCode
        {
            get
            {
                return barCode;
            }
            set
            {
                barCode = value;
                OnPropertyChanged(nameof(BarCode));
            }
        }

        private string isShowButton = "Visible";

        public string IsShowButton
        {
            get
            {
                return isShowButton;
            }
            set
            {
                isShowButton = value;
                OnPropertyChanged(nameof(IsShowButton));
            }
        }

        //******************************************************************************************************************//

        private ObservableCollection<MaterialScanModel> leftData = new ObservableCollection<MaterialScanModel>();

        public ObservableCollection<MaterialScanModel> LeftData
        {
            get
            {
                return leftData;
            }
            set
            {
                leftData = value;
                OnPropertyChanged(nameof(LeftData));
            }
        }

        private ObservableCollection<MaterialScanModel> centerData = new ObservableCollection<MaterialScanModel>();

        public ObservableCollection<MaterialScanModel> CenterData
        {
            get
            {
                return centerData;
            }
            set
            {
                centerData = value;
                OnPropertyChanged(nameof(CenterData));
            }
        }

        private ObservableCollection<MaterialScanModel> rightData = new ObservableCollection<MaterialScanModel>();

        public ObservableCollection<MaterialScanModel> RightData
        {
            get
            {
                return rightData;
            }
            set
            {
                rightData = value;
                OnPropertyChanged(nameof(RightData));
            }
        }

        //******************************************************************************************************************//

        public DelegateCommand BarCodeScanCommand { get; set; }

        public DelegateCommand ExitCommand { get; set; }

        public DelegateCommand OperateCommand { get; set; }

        public void BarCodeScan(object obj)
        {
            try
            {
                string barCode = obj.ToString();
                BarCode = "";

                LogHelper.WriteLog($"条码：{barCode}");

                if (string.IsNullOrEmpty(barCode))
                {
                    return;
                }

                MaterialScanModel materialScan_Left = LeftData.SingleOrDefault(item => item.BarCode.Equals(barCode));

                LogHelper.WriteLog($"正在左侧搜索...");

                if (materialScan_Left == null)
                {
                    MaterialScanModel materialScan_Center = CenterData.SingleOrDefault(item => item.BarCode.Equals(barCode));

                    LogHelper.WriteLog($"正在中间搜索...");

                    if (materialScan_Center == null)
                    {
                        MaterialScanModel materialScan_Right = RightData.SingleOrDefault(item => item.BarCode.Equals(barCode));

                        LogHelper.WriteLog($"正在右侧搜索...");

                        if (materialScan_Right == null)
                        {
                            //查该条码为何物

                            LogHelper.WriteLog($"正在调用接口搜索...\r\n{string.Format(@"http://{0}/api/Mms/WinFormClient/GetPartAndPBomByBarCode?barCode={1}", ConfigInfoModel.API, barCode)}");

                            var result = new MaterialInfoBLL().GetPartAndPBomByBarCode(barCode);

                            LogHelper.WriteLog($"搜索结果：{result.Result},详情：{result.Msg ?? ""}");

                            if (result.Result)
                            {
                                RightData.Add(new MaterialScanModel()
                                {
                                    PartCode = result.Data.GetPart.PartCode ?? "",
                                    PartFigureCode = result.Data.GetProcessBom.PartFigureCode ?? "",
                                    AlreadyScanQuantity = 1,
                                    BarCode = barCode
                                });

                                var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode);
                                LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                            }
                            else
                            {
                                RightData.Add(new MaterialScanModel()
                                {
                                    PartCode = "",
                                    PartFigureCode = $"无图号({barCode})",
                                    AlreadyScanQuantity = 1,
                                    BarCode = barCode
                                });
                                //MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                        }
                        else
                        {
                            materialScan_Right.AlreadyScanQuantity++;

                            var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode);
                            LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                        }
                    }
                    else
                    {
                        if (materialScan_Center.IsCrux.Equals(1))
                        {
                            if (materialScan_Center.AlreadyScanQuantity < materialScan_Center.DemandScanQuantity)
                            {
                                //执行出库
                                var result = new MaterialInfoBLL().MaterialInventory(materialScan_Center.PartCode);

                                if (result.Result)
                                {
                                    //出库成功
                                    materialScan_Center.AlreadyScanQuantity++;
                                }
                                else
                                {
                                    //出库失败
                                    MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                }
                            }
                            else
                            {
                                materialScan_Center.AlreadyScanQuantity++;
                            }

                            var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode);
                            LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                        }
                        else
                        {
                            materialScan_Center.AlreadyScanQuantity = materialScan_Center.DemandScanQuantity;
                            var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode, materialScan_Center.DemandScanQuantity);
                            LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                        }

                    }
                }
                else
                {
                    LeftData.Remove(materialScan_Left);
                    CenterData.Add(materialScan_Left);

                    if (materialScan_Left.IsCrux.Equals(1))
                    {
                        //执行出库
                        var result = new MaterialInfoBLL().MaterialInventory(materialScan_Left.PartCode);

                        if (result.Result)
                        {
                            //出库成功
                            materialScan_Left.AlreadyScanQuantity++;
                        }
                        else
                        {
                            //出库失败
                            MessageBox.Show(result.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode);
                        LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                    }
                    else
                    {
                        materialScan_Left.AlreadyScanQuantity = materialScan_Left.DemandScanQuantity;
                        var aaa = new MaterialInfoBLL().SaveBarCodeScanLog(barCode, materialScan_Left.DemandScanQuantity);
                        LogHelper.WriteLog($"记录扫码日志结果：{aaa.Result}  {aaa.Msg ?? ""}");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"扫码异常：{ex}");
            }
        }

        public void Exit(object obj)
        {
            window.Close();
        }

        public void Operate(object obj)
        {
            window.OperateType = 1;
            window.Close();
        }

        //******************************************************************************************************************//

        public void LoadData()
        {
            if (StoreInfoModel.MaterialInfo.GetMaterialDetail.IsZhuDuanJian)
            {
                if (StoreInfoModel.MaterialInfo.GetPart == null)
                {
                    throw new Exception("本体为铸件锻件，但是在part表查不到信息！");
                }

                if (StoreInfoModel.MaterialInfo.GetMaterialDetail.PartAlreadyScanQuantity < StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity)
                {
                    LeftData.Add(new MaterialScanModel()
                    {
                        PartCode = StoreInfoModel.MaterialInfo.GetPart.InventoryCode ?? "",
                        PartFigureCode = StoreInfoModel.MaterialInfo.GetPart.Model ?? "",
                        BarCode = StoreInfoModel.MaterialInfo.GetPart.CorrespondingBarcode ?? "",
                        DemandScanQuantity = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartQuantity - StoreInfoModel.MaterialInfo.GetMaterialDetail.PartAlreadyScanQuantity,
                        IsCrux = StoreInfoModel.MaterialInfo.GetProcessBom.IsCrux ?? 0
                    });
                }
                else
                {
                    CenterData.Add(new MaterialScanModel()
                    {
                        PartCode = StoreInfoModel.MaterialInfo.GetPart.InventoryCode ?? "",
                        PartFigureCode = StoreInfoModel.MaterialInfo.GetPart.Model ?? "",
                        BarCode = StoreInfoModel.MaterialInfo.GetPart.CorrespondingBarcode ?? "",
                        IsCrux = StoreInfoModel.MaterialInfo.GetProcessBom.IsCrux ?? 0
                    });
                }
            }

            foreach (var item in StoreInfoModel.MaterialChildInfos)
            {
                if (item.GetMaterialDetail.PartAlreadyScanQuantity < item.GetMaterialDetail.PartQuantity)
                {
                    LeftData.Add(new MaterialScanModel()
                    {
                        PartCode = item.GetMaterialDetail.PartCode,
                        PartFigureCode = item.GetMaterialDetail.PartFigureCode,
                        BarCode = item.GetPart.CorrespondingBarcode ?? "",
                        DemandScanQuantity = item.GetMaterialDetail.PartQuantity - item.GetMaterialDetail.PartAlreadyScanQuantity,
                        IsCrux = item.GetProcessBom.IsCrux ?? 0
                    });
                }
                else
                {
                    CenterData.Add(new MaterialScanModel()
                    {
                        PartCode = item.GetMaterialDetail.PartCode,
                        PartFigureCode = item.GetMaterialDetail.PartFigureCode,
                        BarCode = item.GetPart.CorrespondingBarcode ?? "",
                        IsCrux = item.GetProcessBom.IsCrux ?? 0
                    });
                }
            }

            RightData.Clear();
        }

        //******************************************************************************************************************//

    }
}
