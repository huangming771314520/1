using MesClient.API;
using MesClient.Commands;
using MesClient.Common;
using MesClient.Models;
using MesClient.Views.WinViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MesClient.ViewModels
{
    public class MaterialScanViewModel : ViewModelBase
    {
        public MaterialScanViewModel(WindowMaterialScan _window, string barCode = "")
        {
            WindowHelper.IsOpenMaterialScanWindow = true;

            LoadData();

            if (!string.IsNullOrEmpty(barCode))
            {
                IsShowButton = "Hidden";
                BarCodeScan(barCode);
            }

            BarCodeScanCommand = new DelegateCommand(obj => BarCodeScan(obj));
            ExitCommand = new DelegateCommand(new Action<object>(obj => { _window.Close(); }));
            OperateCommand = new DelegateCommand(new Action<object>(obj => { _window.OperateType = 1; _window.Close(); }));
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

                if (string.IsNullOrEmpty(barCode))
                {
                    return;
                }

                MaterialScanModel leftModel = LeftData.SingleOrDefault(item => item.BarCode.Equals(barCode));
                if (leftModel == null)
                {
                    MaterialScanModel centerModel = CenterData.SingleOrDefault(item => item.BarCode.Equals(barCode));
                    if (centerModel == null)
                    {
                        MaterialScanModel rightModel = RightData.SingleOrDefault(item => item.BarCode.Equals(barCode));
                        if (rightModel == null)
                        {
                            //查该条码为何物
                            var result = ApiManager.GetPartCodeAndFigureNoByBarCode(barCode);

                            if (result.Result)
                            {
                                dynamic dy = JsonConvert.DeserializeObject<dynamic>(result.Data.ToString());
                                RightData.Add(new MaterialScanModel()
                                {
                                    PartCode = dy.PartCode,
                                    PartFigureCode = dy.FigureNo,
                                    AlreadyScanQuantity = 1,
                                    BarCode = barCode
                                });
                                ApiManager.SaveBarCodeScanLog(barCode);
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
                            }
                        }
                        else
                        {
                            rightModel.AlreadyScanQuantity++;
                            //记录扫码日志
                            var result = ApiManager.SaveBarCodeScanLog(rightModel.BarCode);
                        }
                    }
                    else
                    {
                        if (centerModel.IsCrux.Equals(1))
                        {
                            if (centerModel.AlreadyScanQuantity < centerModel.DemandScanQuantity)
                            {
                                //执行出库
                                var resultA = ApiManager.MaterialScanOutput(centerModel.PartCode);
                                if (resultA.Result)
                                {
                                    //出库成功
                                    centerModel.AlreadyScanQuantity++;
                                }
                                else
                                {
                                    //出库失败
                                    MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                }
                            }
                            else
                            {
                                centerModel.AlreadyScanQuantity++;
                            }

                            //记录扫码日志
                            var resultB = ApiManager.SaveBarCodeScanLog(centerModel.BarCode);
                        }
                        else
                        {
                            centerModel.AlreadyScanQuantity = centerModel.DemandScanQuantity;
                            //记录扫码日志
                            var resultB = ApiManager.SaveBarCodeScanLog(centerModel.BarCode, centerModel.DemandScanQuantity);
                        }
                    }
                }
                else
                {
                    LeftData.Remove(leftModel);
                    CenterData.Add(leftModel);

                    if (leftModel.IsCrux.Equals(1))
                    {
                        //执行出库
                        var resultA = ApiManager.MaterialScanOutput(leftModel.PartCode);
                        if (resultA.Result)
                        {
                            //出库成功
                            leftModel.AlreadyScanQuantity++;
                        }
                        else
                        {
                            //出库失败
                            MessageBox.Show(resultA.Msg, "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        //记录扫码日志
                        var resultB = ApiManager.SaveBarCodeScanLog(leftModel.BarCode);
                    }
                    else
                    {
                        leftModel.AlreadyScanQuantity = leftModel.DemandScanQuantity;
                        //记录扫码日志
                        var resultB = ApiManager.SaveBarCodeScanLog(leftModel.BarCode, leftModel.DemandScanQuantity);
                    }
                }
            }
            catch
            {

            }
        }

        //******************************************************************************************************************//

        public void LoadData()
        {
            if (StoreInfo.MaterialOneSelf.IsCastingOrForging)
            {
                MaterialScanModel model = new MaterialScanModel()
                {
                    PartCode = StoreInfo.MaterialOneSelf.MaterialInfo.PartCode ?? "",
                    PartFigureCode = StoreInfo.MaterialOneSelf.MaterialInfo.FigureNo ?? "",
                    BarCode = StoreInfo.MaterialOneSelf.MaterialInfo.ExtraInfo.PartInfo.CorrespondingBarcode ?? "",
                    IsCrux = StoreInfo.MaterialOneSelf.MaterialInfo.ExtraInfo.BomInfo.IsCrux ?? 0
                };

                if (StoreInfo.MaterialOneSelf.MaterialInfo.AlreadyScanQuantity < StoreInfo.MaterialOneSelf.MaterialInfo.TotalQuantity)
                {
                    model.DemandScanQuantity = StoreInfo.MaterialOneSelf.MaterialInfo.TotalQuantity - StoreInfo.MaterialOneSelf.MaterialInfo.AlreadyScanQuantity;
                    LeftData.Add(model);
                }
                else
                {
                    CenterData.Add(model);
                }
            }

            foreach (var item in StoreInfo.MaterialChild.MaterialInfos)
            {
                MaterialScanModel model = new MaterialScanModel()
                {
                    PartCode = item.PartCode ?? "",
                    PartFigureCode = item.FigureNo ?? "",
                    BarCode = item.ExtraInfo.PartInfo.CorrespondingBarcode ?? "",
                    IsCrux = item.ExtraInfo.BomInfo.IsCrux ?? 0
                };

                if (item.AlreadyScanQuantity < item.TotalQuantity)
                {
                    model.DemandScanQuantity = item.TotalQuantity - item.AlreadyScanQuantity;
                    LeftData.Add(model);
                }
                else
                {
                    CenterData.Add(model);
                }
            }

            RightData.Clear();
        }

    }
}
