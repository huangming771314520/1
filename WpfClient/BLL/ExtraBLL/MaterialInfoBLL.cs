using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Helpers;
using Model;
using Newtonsoft.Json;

namespace BLL.ExtraBLL
{
    public class MaterialInfoBLL
    {
        public enum MaterialStateEnum
        {
            /// <summary>
            /// 其他出库
            /// </summary>
            OtherOutput = 0,

            /// <summary>
            /// 转序入库
            /// </summary>
            TurnInput = 1
        }



        /// <summary>
        /// 获取某生产计划所用的物料详情
        /// </summary>
        public ResultModel GetMaterialDetail()
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetMaterialDetailByPCode?ppdID={1}&partCode={2}", ConfigInfoModel.API, StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkTicketCode, StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode);
                var result = Helpers.HttpHelper.GetTByUrl<MaterialInfoModel>(url);

                if (!result.Result)
                {
                    throw new Exception(result.Msg);
                }

                StoreInfoModel.MaterialInfos = result.Data;

                //要加工的零件本体
                StoreInfoModel.MaterialInfo = StoreInfoModel.MaterialInfos.FirstOrDefault(item => item.GetMaterialDetail.IsOneSelf);
                //要加工的零件子集
                StoreInfoModel.MaterialChildInfos = StoreInfoModel.MaterialInfos.Where(item => !item.GetMaterialDetail.IsOneSelf).ToList();

                if (result.Result)
                {
                    return new ResultModel() { Result = true };
                }
                else
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = result.Msg
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 根据条码 查Part和ProcessBOM
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel<MaterialScanInfoModel.DataModel> GetPartAndPBomByBarCode(string barCode)
        {
            try
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetPartAndPBomByBarCode?barCode={1}", ConfigInfoModel.API, barCode);
                var result = Helpers.HttpHelper.GetTByUrl<MaterialScanInfoModel>(url);

                if (!result.Result)
                {
                    throw new Exception(result.Msg);
                }

                return new ResultModel<MaterialScanInfoModel.DataModel>()
                {
                    Result = true,
                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<MaterialScanInfoModel.DataModel>()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }


        /// <summary>
        /// 物料其他出入库，转序出库
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ResultModel MaterialInventory(MaterialStateEnum state)
        {
            string url = string.Empty;
            string json = string.Empty;

            switch (state)
            {
                case MaterialStateEnum.OtherOutput:
                    #region 其他出库 旧方法-一次性出库所有
                    /*
                    url = "http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostMaterialOtherOutputStorage";

                    List<object> list = new List<object>();
                    foreach (var item in StoreInfoModel.MaterialInfos)
                    {
                        if (item.GetPart == null)
                        {
                            continue;
                        }

                        list.Add(new
                        {
                            InventoryCode = item.GetPart.InventoryCode,
                            InventoryName = item.GetPart.InventoryName,
                            Specs = item.GetPart.Model,
                            Unit = item.GetPart.QuantityUnit,
                            MateNum = item.GetMaterialDetail.PartQuantity,
                            ConfirmNum = item.GetMaterialDetail.PartQuantity,
                            UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                            PBillCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ApsCode,
                            BatchCode = "NewBatchCode"
                        });
                    }
                    json = Helpers.HttpHelper.PostJSON(url, new
                    {
                        mainData = new
                        {
                            ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
                            ProjectName = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName,
                            WarehouseCode = StoreInfoModel.WorkGroupInfo.GetWarehouse.WarehouseCode,
                            WarehouseName = StoreInfoModel.WorkGroupInfo.GetWarehouse.WarehouseName,
                            UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                            ApproveState = 2
                        },
                        detailData = list
                    });
                    */
                    #endregion

                    break;
                case MaterialStateEnum.TurnInput:
                    url = "http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostMaterialTurnInputStorage";

                    #region 转序入库

                    var pprModel = HttpHelper.GetTByUrl<ResultModel<dynamic>>(string.Format(@"http://{0}/api/Mms/WinFormClient/GetTurnTarget?ppdID={1}", ConfigInfoModel.API, StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ID));

                    //dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(json);

                    if (pprModel.Result)
                    {
                        json = BLL.Helpers.HttpHelper.PostJSON(url, new
                        {
                            mainData = new
                            {
                                ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
                                ProjectName = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName,
                                //WarehouseCode = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.WarehouseID,
                                //WarehouseName = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.WarehouseName,
                                WarehouseCode = pprModel.Data.ID,
                                WarehouseName = pprModel.Data.Name,
                                UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                                ApproveState = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.WarehouseID.Equals(StoreInfoModel.WorkGroupInfo.GetWarehouse.WarehouseCode) ? 2 : 1
                            },
                            detailData = new
                            {
                                InventoryCode = StoreInfoModel.MaterialInfo.GetPart.InventoryCode,
                                InventoryName = StoreInfoModel.MaterialInfo.GetPart.InventoryName,
                                Specs = StoreInfoModel.MaterialInfo.GetPart.Model,
                                Unit = StoreInfoModel.MaterialInfo.GetPart.QuantityUnit,
                                //MateNum = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
                                //ConfirmNum = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
                                MateNum = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity,
                                ConfirmNum = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity,
                                UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                                PBillCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ApsCode,
                                BatchCode = "NewBatchCode"
                            }
                        });
                    }
                    else
                    {
                        return new ResultModel()
                        {
                            Result = false,
                            Msg = pprModel.Msg
                        };
                    }

                    #endregion

                    break;
                default:

                    #region 参数异常

                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"内部参数错误！"
                    };

                    #endregion

            }

            return JsonConvert.DeserializeObject<ResultModel>(json);
        }

        /// <summary>
        /// 其他出库 新方法-针对每个零件出库
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public ResultModel MaterialInventory(string partCode)
        {
            try
            {
                string url = "http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostMaterialOtherOutputStorageBySingle";

                MaterialInfoModel.DataModel MaterialInfo = StoreInfoModel.MaterialChildInfos.Single(item => item.GetMaterialDetail.PartCode.Equals(partCode));

                string json = Helpers.HttpHelper.PostJSON(url, new
                {
                    mainData = new
                    {
                        ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
                        ProjectName = StoreInfoModel.ProducePlanInfo.GetProject.ProjectName,
                        WarehouseCode = StoreInfoModel.WorkGroupInfo.GetWarehouse.WarehouseCode,
                        WarehouseName = StoreInfoModel.WorkGroupInfo.GetWarehouse.WarehouseName,
                        ApproveState = 2
                    },
                    detailData = new
                    {
                        InventoryCode = MaterialInfo.GetPart.InventoryCode,
                        InventoryName = MaterialInfo.GetPart.InventoryName,
                        Specs = MaterialInfo.GetPart.Model,
                        Unit = MaterialInfo.GetPart.QuantityUnit,
                        MateNum = 1,
                        ConfirmNum = 1,
                        //PBillCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ApsCode
                        PBillCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkTicketCode
                    },
                    userData = new
                    {
                        UserCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
                        UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName
                    }
                });

                return JsonConvert.DeserializeObject<ResultModel>(json);
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 记录扫码日志
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel SaveBarCodeScanLog(string barCode, int quantity = 1)
        {
            try
            {
                string url = "http://" + ConfigInfoModel.API + "/api/Mms/WinFormClient/PostSaveBarCodeScanLog";

                ResultModel result = Helpers.HttpHelper.PostTByUrl<ResultModel>(url, new
                {
                    UserBarCode = StoreInfoModel.UserBarCode,
                    PartBarCode = barCode,
                    ApsCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkTicketCode,
                    MateQuantity = quantity,
                    UserName = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName
                });

                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 需要报检，生成报检单
        /// </summary>
        /// <returns></returns>
        //public ResultModel InspectionReport()
        //{
        //    DateTime dTime = DateTime.Now;
        //    string url = string.Format(@"http://{0}/api/Mms/WinFormClient/PostCheckRequest", ConfigInfoModel.API);
        //    object parameter = new
        //    {
        //        mesPIR = new
        //        {
        //            ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
        //            ProjectName = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProjectName,
        //            ProductName = StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName,
        //            ProductFigureNumber = StoreInfoModel.ProducePlanInfo.GetProjectDetail.FigureNumber,
        //            PartCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode,
        //            PartName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName,
        //            partFigure = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartFigureCode,
        //            MaterialCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.MaterialCode,
        //            CheckQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
        //            BillState = 2,
        //            IsEnable = 1,
        //            CreatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
        //            CreateTime = dTime,
        //            ModifyPerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
        //            ModifyTime = dTime,
        //            ProductPlanCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ProductPlanCode,
        //            OperatorCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
        //            DepartmentCode = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode,
        //            DepartmentName = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamName,
        //            Unit = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Unit,
        //            EquipmentCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentID,
        //            EquipmentName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentName,
        //            ProductProcessRouteID = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ID
        //        },
        //        qmsPI = new
        //        {
        //            ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
        //            ProjectName = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProjectName,
        //            ProductName = StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName,
        //            ProductFigureNumber = StoreInfoModel.ProducePlanInfo.GetProjectDetail.FigureNumber,
        //            PartCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode,
        //            PartName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName,
        //            partFigure = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartFigureCode,
        //            MaterialCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.MaterialCode,
        //            CheckQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
        //            QualifiedQuntity = 0,
        //            IsQualified = 1,
        //            BillState = 1,
        //            IsEnable = 1,
        //            CreatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
        //            CreateTime = dTime,
        //            ModifyPerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
        //            ModifyTime = dTime,
        //            ProductPlanCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ProductPlanCode,
        //            OperatorCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
        //            DepartmentCode = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode,
        //            DepartmentName = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamName,
        //            Unit = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Unit,
        //            EquipmentCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentID,
        //            EquipmentName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentName
        //        }
        //    };
        //    return Helpers.HttpHelper.PostTByUrl<ResultModel>(url, parameter);
        //}


        public ResultModel InspectionReport()
        {
            DateTime dTime = DateTime.Now;
            string url = string.Format(@"http://{0}/api/Mms/WinFormClient/PostCheckRequest", ConfigInfoModel.API);
            object parameter = new
            {
                mesPIR = new
                {
                    ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
                    ProjectName = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProjectName,
                    ProductName = StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName,
                    ProductFigureNumber = StoreInfoModel.ProducePlanInfo.GetProjectDetail.FigureNumber,
                    PartCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode,
                    PartName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName,
                    partFigure = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartFigureCode,
                    MaterialCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.MaterialCode,
                    //CheckQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity,
                    CheckQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity,
                    BillState = 2,
                    IsEnable = 1,
                    CreatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                    CreateTime = dTime,
                    ModifyPerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                    ModifyTime = dTime,
                    //ProductPlanCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ProductPlanCode,
                    OperatorCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
                    DepartmentCode = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode,
                    DepartmentName = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamName,
                    Unit = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Unit,
                    EquipmentCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentCode,
                    EquipmentName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentName,
                    ProductProcessRouteID = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ID
                },
                qmsPI = new
                {
                    ContractCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ContractCode,
                    ProjectName = StoreInfoModel.ProducePlanInfo.GetProductProcessRoute.ProjectName,
                    ProductName = StoreInfoModel.ProducePlanInfo.GetProjectDetail.ProductName,
                    ProductFigureNumber = StoreInfoModel.ProducePlanInfo.GetProjectDetail.FigureNumber,
                    PartCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.PartCode,
                    PartName = StoreInfoModel.MaterialInfo.GetMaterialDetail.PartName,
                    partFigure = StoreInfoModel.ProducePlanInfo.GetProcessBOM.PartFigureCode,
                    MaterialCode = StoreInfoModel.MaterialInfo.GetMaterialDetail.MaterialCode,
                    CheckQuantity = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.WorkQuantity,
                    QualifiedQuntity = 0,
                    IsQualified = 1,
                    BillState = 1,
                    IsEnable = 1,
                    CreatePerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                    CreateTime = dTime,
                    ModifyPerson = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserName,
                    ModifyTime = dTime,
                    //ProductPlanCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.ProductPlanCode,
                    OperatorCode = StoreInfoModel.WorkGroupInfo.GetWorkGroupDetail.UserCode,
                    DepartmentCode = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamCode,
                    DepartmentName = StoreInfoModel.WorkGroupInfo.GetWorkGroup.TeamName,
                    Unit = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Unit,
                    //EquipmentCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentID,
                    EquipmentCode = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentCode,
                    EquipmentName = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.EquipmentName
                }
            };
            return Helpers.HttpHelper.PostTByUrl<ResultModel>(url, parameter);
        }

    }
}
