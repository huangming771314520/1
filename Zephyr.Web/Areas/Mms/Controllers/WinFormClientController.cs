using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class WinFormClientController : Controller
    {

    }

    public class WinFormClientApiController : ApiController
    {
        /// <summary>
        /// 获取客户端与服务器连接状态
        /// </summary>
        /// <returns></returns>
        public dynamic GetTestConnectionStatus()
        {
            return new
            {
                Result = true
            };
        }

        /// <summary>
        /// 获取客户端当前登录状态是否失效
        /// </summary>
        /// <param name="uCode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public dynamic GetUserLoginStatus(string uCode, string token)
        {
            //*******************************************************************************************
            //延时 30 秒 返回结果
            LoginLogService service = new LoginLogService();
            for (int i = 0; i < 30; i++)
            {
                if (service.GetTokenByUCode(uCode).Equals(token))
                {
                    Thread.Sleep(1000 * 1);
                }
                else
                {
                    return new
                    {
                        Result = false,
                        Msg = @"当前账户已在其他客户端登录！"
                    };
                }
            }
            return new
            {
                Result = true
            };
        }

        /// <summary>
        /// 根据用户/员工编号获取该班组关联的所有信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public dynamic GetWorkGroupInfoByUCode(string barCode)
        {
            return new WinFormClientService().GetWorkGroupInfoByUCode(barCode);
        }

        /// <summary>
        /// 根据班组编号获取该班组生产计划关联的所有信息
        /// </summary>
        /// <param name="teamCode">班组编号</param>
        /// <returns></returns>
        public dynamic GetProducePlanInfoByTCode(string teamCode)
        {
            return new WinFormClientService().GetProducePlanInfoByTCode_2(teamCode);
        }

        /// <summary>
        /// 根据零件编码查物料
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetMaterialDetailByPCode(string ppdID, string partCode)
        {
            return new WinFormClientService().GetMaterialDetailByPCode(ppdID, partCode);
        }

        /// <summary>
        /// 判断某生产计划是否是最后一工序
        /// </summary>
        /// <param name="ppdID"></param>
        /// <returns></returns>
        public dynamic GetIsLastProcess(int ppdID)
        {
            return new WinFormClientService().GetIsLastProcess(ppdID);
        }

        /// <summary>
        /// 转序入库时，根据生产计划ID查转序目标
        /// </summary>
        /// <param name="ppdID"></param>
        /// <returns></returns>
        public dynamic GetTurnTarget(int ppdID)
        {
            return new WinFormClientService().GetTurnTarget(ppdID);
        }

        /// <summary>
        /// 根据条码 查Part和ProcessBOM
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetPartAndPBomByBarCode(string barCode)
        {
            return new WinFormClientService().GetPartAndPBomByBarCode(barCode);
        }

        /// <summary>
        /// 根据条码 查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetUserInfoByBarCode(string barCode)
        {
            return new WinFormClientService().GetUserInfoByBarCode(barCode);
        }

        /// <summary>
        /// 物料其他出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostMaterialOtherOutputStorage(dynamic data)
        {
            try
            {
                DateTime newDT = DateTime.Now;
                string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "QTCK", "", "");

                WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
                {
                    ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
                    BillCode = documentNo,
                    BillType = 7,
                    IsEnable = 1,
                    ContractCode = data["mainData"]["ContractCode"],
                    ProjectName = data["mainData"]["ProjectName"],
                    WarehouseCode = data["mainData"]["WarehouseCode"],
                    WarehouseName = data["mainData"]["WarehouseName"],
                    ApproveState = data["mainData"]["ApproveState"],
                    ApprovePerson = data["mainData"]["UserName"],
                    ApproveDate = newDT,
                    CreatePerson = data["mainData"]["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["mainData"]["UserName"],
                    ModifyTime = newDT
                };

                List<WMS_BN_BillDetail> bDetailModels = new List<WMS_BN_BillDetail>();

                dynamic detailData = data["detailData"];
                foreach (var item in detailData)
                {
                    bDetailModels.Add(new WMS_BN_BillDetail()
                    {
                        BillCode = documentNo,
                        IsEnable = 1,
                        InventoryCode = item["InventoryCode"],
                        InventoryName = item["InventoryName"],
                        Specs = item["Specs"],
                        Unit = item["Unit"],
                        MateNum = item["MateNum"] == null ? null : Convert.ToDouble(item["MateNum"]),
                        ConfirmNum = item["ConfirmNum"] == null ? null : Convert.ToDouble(item["ConfirmNum"]),
                        CreatePerson = item["UserName"],
                        CreateTime = newDT,
                        ModifyPerson = item["UserName"],
                        ModifyTime = newDT,
                        BatchCode = item["BatchCode"],
                        PBillCode = item["PBillCode"]
                    });
                }

                return new WinFormClientService().MaterialInventory(bMainModel, bDetailModels, 0);
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
        /// 物料其他出库 - 单个物料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostMaterialOtherOutputStorageBySingle(dynamic data)
        {
            try
            {
                DateTime newDT = DateTime.Now;
                string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "QTCK", "", "");

                WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
                {
                    ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
                    BillCode = documentNo,
                    BillType = 7,
                    IsEnable = 1,
                    ContractCode = data["mainData"]["ContractCode"],
                    ProjectName = data["mainData"]["ProjectName"],
                    WarehouseCode = data["mainData"]["WarehouseCode"],
                    WarehouseName = data["mainData"]["WarehouseName"],
                    ApproveState = data["mainData"]["ApproveState"],
                    ApprovePerson = data["mainData"]["UserName"],
                    ApproveDate = newDT,
                    CreatePerson = data["userData"]["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["userData"]["UserName"],
                    ModifyTime = newDT
                };

                WMS_BN_BillDetail bDetailModel = new WMS_BN_BillDetail()
                {
                    BillCode = documentNo,
                    IsEnable = 1,
                    InventoryCode = data["detailData"]["InventoryCode"],
                    InventoryName = data["detailData"]["InventoryName"],
                    Specs = data["detailData"]["Specs"],
                    Unit = data["detailData"]["Unit"],
                    MateNum = data["detailData"]["MateNum"] == null ? null : Convert.ToDouble(data["detailData"]["MateNum"]),
                    ConfirmNum = data["detailData"]["ConfirmNum"] == null ? null : Convert.ToDouble(data["detailData"]["ConfirmNum"]),
                    CreatePerson = data["userData"]["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["userData"]["UserName"],
                    ModifyTime = newDT,
                    PBillCode = data["detailData"]["PBillCode"]
                };

                return new WinFormClientService().MaterialInventory(bMainModel, bDetailModel);
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
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostSaveBarCodeScanLog(dynamic data)
        {
            try
            {
                DateTime newDT = DateTime.Now;

                MES_MateSweepCodeLog scanLog = new MES_MateSweepCodeLog()
                {
                    UserBarCode = data["UserBarCode"],
                    ApsCode = data["ApsCode"],
                    MateBarCode = data["PartBarCode"],
                    MateQuantity = Convert.ToInt32(data["MateQuantity"]),
                    IsEnable = 1,
                    CreatePerson = data["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["UserName"],
                    ModifyTime = newDT
                };

                return new WinFormClientService().SaveBarCodeScanLog(scanLog);
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
        /// 物料转序入库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostMaterialTurnInputStorage(dynamic data)
        {
            try
            {
                DateTime newDT = DateTime.Now;
                string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "ZXRK", "", "");

                WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
                {
                    ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
                    BillCode = documentNo,
                    BillType = 8,
                    IsEnable = 1,
                    ContractCode = data["mainData"]["ContractCode"],
                    ProjectName = data["mainData"]["ProjectName"],
                    WarehouseCode = data["mainData"]["WarehouseCode"],
                    WarehouseName = data["mainData"]["WarehouseName"],
                    ApproveState = data["mainData"]["ApproveState"],
                    ApprovePerson = data["mainData"]["UserName"],
                    ApproveDate = newDT,
                    CreatePerson = data["mainData"]["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["mainData"]["UserName"],
                    ModifyTime = newDT
                };

                List<WMS_BN_BillDetail> bDetailModels = new List<WMS_BN_BillDetail>();
                bDetailModels.Add(new WMS_BN_BillDetail()
                {
                    BillCode = documentNo,
                    IsEnable = 1,
                    InventoryCode = data["detailData"]["InventoryCode"],
                    InventoryName = data["detailData"]["InventoryName"],
                    Specs = data["detailData"]["Specs"],
                    Unit = data["detailData"]["Unit"],
                    MateNum = data["detailData"]["MateNum"] == null ? null : Convert.ToDouble(data["detailData"]["MateNum"]),
                    ConfirmNum = data["detailData"]["ConfirmNum"] == null ? null : Convert.ToDouble(data["detailData"]["ConfirmNum"]),
                    CreatePerson = data["detailData"]["UserName"],
                    CreateTime = newDT,
                    ModifyPerson = data["detailData"]["UserName"],
                    ModifyTime = newDT,
                    BatchCode = data["detailData"]["BatchCode"],
                    PBillCode = data["detailData"]["PBillCode"]
                });

                return new WinFormClientService().MaterialInventory(bMainModel, bDetailModels, 1);
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
        /// 生产需要报检-生成报检单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostCheckRequest(dynamic data)
        {
            return new WinFormClientService().PostCheckRequest(data);
        }

        /// <summary>
        /// 录入生产计划实际开始/结束时间
        /// </summary>
        /// <param name="ppdID"></param>
        /// <param name="type">0:开始 1:结束</param>
        /// <returns></returns>
        public dynamic GetProducePlanActualTime(int ppdID, int type)
        {
            return new WinFormClientService().GetProducePlanActualTime(ppdID, type);
        }

        /// <summary>
        /// 根据班组查设备及保养维护信息
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public dynamic GetEquipmentMaintainByTCode(string teamCode)
        {
            return new WinFormClientService().GetEquipmentMaintainByTCode(teamCode);
        }

        /// <summary>
        /// 执行设备保养计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostEQPMaintenancePlan(dynamic data)
        {
            return new WinFormClientService().EQPMaintenancePlan(data);
        }

        /// <summary>
        /// 获取所有设计任务结果
        /// </summary>
        /// <returns></returns>
        public dynamic GetDesignTaskResult()
        {
            return new WinFormClientService().GetDesignTaskResult();
        }

        /// <summary>
        /// 根据零件编码、零件类型查零件信息
        /// </summary>
        /// <param name="partCode"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        public dynamic GetPartByPCodeAndPType(string partCode, string partType)
        {
            return new WinFormClientService().GetPartByPCodeAndPType(partCode, partType);
        }

        /// <summary>
        /// 根据零件编码生成条码
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCodeByPCode(string partCode)
        {
            return new WinFormClientService().GetUpdatePartBarCodeByPCode(partCode);
        }

        /// <summary>
        /// 根据零件InventoryCode生成条码
        /// </summary>
        /// <param name="inventoryCode"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCodeByICode(string inventoryCode)
        {
            return new WinFormClientService().GetUpdatePartBarCodeByICode(inventoryCode);
        }

        /// <summary>
        /// 查所有生产任务
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public dynamic GetTaskInfo()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.ToString();
                string api = url.Substring(0, url.IndexOf("api"));
                return new WinFormClientService().GetTaskInfoByKeyWord(api);
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
        /// 查所有图纸申请任务
        /// </summary>
        /// <returns></returns>
        public dynamic GetDrawTaskInfo()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.ToString();
                string api = url.Substring(0, url.IndexOf("api"));
                return new WinFormClientService().GetDrawTaskInfoByKeyWord(api);
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
        /// 上传文件后需要存入的XML
        /// </summary>
        /// <returns></returns>
        public dynamic PostProductDataByLocalXML(long taskID, long projectID, string projectName, long projectDetailID, string contractCode, int designType, string designTaskResultType, string designTaskResultText)
        {
            try
            {
                var data = HttpContext.Current.Request.InputStream;
                StreamReader readStream = new StreamReader(data, Encoding.UTF8);
                string strXML = readStream.ReadToEnd();

                int explain_S = strXML.IndexOf("<?");
                int explain_E = strXML.IndexOf("?>");
                string strExplain = string.Empty;
                if (!explain_S.Equals(-1))
                {
                    strExplain = strXML.Substring(explain_S, explain_E + 2);
                    strXML = strXML.Replace(strExplain, strExplain.ToLower());
                }

                return new WinFormClientService().ImportDatabase(strXML, taskID, projectID, projectName, projectDetailID, contractCode, designType, designTaskResultType, designTaskResultText);
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
        /// 更新上传PDM实际结束时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public dynamic GetProductDataUpKillDate(long taskID)
        {
            return new WinFormClientService().SaveProductDataUpKillDate(taskID);
        }

        /// <summary>
        /// 根据ID下载 PartFile 中图纸 的网络下载路径
        /// </summary>
        /// <param name="id"></param>
        public dynamic GetDrawingWebPathByID(int id)
        {
            try
            {
                string filePath = new WinFormClientService().GetDrawingPathByID(id);

                if (File.Exists(filePath))
                {
                    string url = HttpContext.Current.Request.Url.ToString();

                    string pathA = filePath.Substring(filePath.IndexOf("Upload")).Replace(@"\", "/");
                    string pathB = url.Substring(0, url.IndexOf("api"));

                    return new ResultModel<string>()
                    {
                        Result = true,
                        Data = pathB + pathA
                    };
                }
                else
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"文件不存在！"
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
        /// 根据ID下载 QMS_QualityReportDoc 中文件 的网络下载路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetDocWebPathByID(int id)
        {
            try
            {
                string filePath = new WinFormClientService().GetQualityReportDocFilePathByID(id);

                if (File.Exists(filePath))
                {
                    string url = HttpContext.Current.Request.Url.ToString();

                    string pathA = filePath.Substring(filePath.IndexOf("Upload")).Replace(@"\", "/");
                    string pathB = url.Substring(0, url.IndexOf("api"));

                    return new ResultModel<string>()
                    {
                        Result = true,
                        Data = pathB + pathA
                    };
                }
                else
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"文件不存在！"
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

        public dynamic GetFigureWebPathByID(int id)
        {
            try
            {
                string filePath = new WinFormClientService().GetProcessFigureFilePathByID(id);

                if (File.Exists(filePath))
                {
                    string url = HttpContext.Current.Request.Url.ToString();

                    string pathA = filePath.Substring(filePath.IndexOf("Upload")).Replace(@"\", "/");
                    string pathB = url.Substring(0, url.IndexOf("api"));

                    return new ResultModel<string>()
                    {
                        Result = true,
                        Data = pathB + pathA
                    };
                }
                else
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"文件不存在！"
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
        /// 记录生产计划开始暂停日志
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostProduceLog(dynamic data)
        {
            try
            {
                if (data["APSCode"] == null)
                {
                    throw new Exception(@"APSCode不能为空！");
                }

                int type = Convert.ToInt32(data["Type"]);
                int pauseReson = Convert.ToInt32(data["PauseReson"]);

                DateTime newData = DateTime.Now;

                MES_OpreatorWorkingLog model = new MES_OpreatorWorkingLog()
                {
                    APSCode = data["APSCode"],
                    BegainTime = newData,
                    IsEnable = 1,
                    CreatePerson = data["CreatePerson"],
                    CreateTime = newData,
                    OperatePerson = data["OperatePerson"],
                    OpreateCode = data["OpreateCode"],
                    ModifyPerson = data["ModifyPerson"],
                    ModifyTime = newData,
                    FinishQuantity = Convert.ToDecimal(data["FinishQuantity"])
                };

                return new MES_OpreatorWorkingLogService().SaveProduceLog(model, type, pauseReson);
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
        /// 记录计划执行操作人
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostProduceOperatorStatistical(dynamic data)
        {
            try
            {
                string barCodeStr = data["BarCodes"];
                int logID = Convert.ToInt32(data["LogID"]);

                List<string> barCodes = barCodeStr.Split(',').ToList();

                return new MES_OperatorStatisticalService().SetProduceOperatorStatistical(barCodes, logID);
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
        /// 记录客户端操作日志
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostSetOperateLog(dynamic data)
        {
            try
            {
                List<ClientOperateLog> models = new List<ClientOperateLog>();
                foreach (var item in data)
                {
                    models.Add(new ClientOperateLog()
                    {
                        AddressIP = item["AddressIP"],
                        Content = item["Content"],
                        CreatePersonCode = item["CreatePersonCode"],
                        CreatePersonName = item["CreatePersonName"],
                        CreateDate = item["CreateDate"]
                    });
                }

                return new ClientOperateLogService().SetClientOperateLog(models);
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = string.Format("写入操作日志错误！\n详情：{0}", ex.ToString())
                };
            }
        }


        public dynamic GetBlankingRecordLeftData(string fCode)
        {
            return new WinFormClientService().GetBlankingRecordLeftData(fCode);
        }

        public dynamic GetBlankingRecordRightData(string iCode, string bNumber)
        {
            return new WinFormClientService().GetBlankingRecordRightData(iCode, bNumber);
        }

        public dynamic GetUpdateBlankingRecordLeftData(string iCode, string bNumber, int id)
        {
            return new WinFormClientService().GetUpdateBlankingRecordLeftData(iCode, bNumber, id);
        }

        public dynamic GetUpdateBlankingRecordRightData(string iCode, string bNumber, int id)
        {
            return new WinFormClientService().GetUpdateBlankingRecordRightData(iCode, bNumber, id);
        }



        //**********************************************************************************************************//
        #region PDM文件上传 支持断点续传

        public dynamic GetResumFile(string fileName)
        {
            try
            {
                ResultModel<string> result = WinFormClientService.GetFileUpLoadPath(new
                {
                    RootPath = "PMS_BN_PartFile",
                    FileType = Path.GetExtension(fileName)
                });

                var saveFilePath = result.Data + fileName;
                long length = 0;

                if (File.Exists(saveFilePath))
                {
                    using (FileStream fs = File.OpenWrite(saveFilePath))
                    {
                        length = fs.Length;
                    }
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = length
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }
        }

        public dynamic PostRsume()
        {
            var file = HttpContext.Current.Request.InputStream;
            var fileName = HttpContext.Current.Request.QueryString["filename"];

            ResultModel<string> result = WinFormClientService.GetFileUpLoadPath(new
            {
                RootPath = "PMS_BN_PartFile",
                FileType = Path.GetExtension(fileName)
            });

            return new SYS_FileManageService().SaveAs(result.Data + fileName, file);
        }

        #endregion

    }


}