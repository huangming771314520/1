using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Zephyr.Areas;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;
using Zephyr.Utils.NPOI.SS.Formula.Functions;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class ClientService : ServiceBase<Client>
    {
        #region

        public string GetTokenByUCode(string uCode, int type = 0)
        {
            var loginLog = db.Sql(string.Format(@"select top 1 * from LoginLog where [Type] = {0} and UserCode = '{1}' order by CreateDate desc", type, uCode)).QuerySingle<LoginLog>();
            return loginLog == null ? string.Empty : loginLog.Token;
        }

        #endregion

        #region 用户信息

        /// <summary>
        /// 登录，并查用户详细信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel GetUserDetailInfoByBarCode(string barCode)
        {
            try
            {
                string sqlD = string.Format("SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserBarcode = '{0}'", barCode);
                var listD = db.Sql(sqlD).QueryMany<SYS_BN_User>();
                if (listD.Count <= 0)
                {
                    throw new Exception(@"该条码不存在！");
                }
                else if (listD.Count.Equals(1))
                {
                    string userCode = listD[0].UserCode;

                    string sqlA = string.Format(@"SELECT * FROM SYS_WorkGroupDetail WHERE IsEnable = 1 AND UserCode = '{0}' ", userCode);
                    List<SYS_WorkGroupDetail> listA = db.Sql(sqlA).QueryMany<SYS_WorkGroupDetail>();

                    if (listA.Count <= 0)
                    {
                        throw new Exception(@"查无此人");
                    }
                    else if (listA.Count.Equals(1))
                    {
                        string sqlB = string.Format(@"SELECT * FROM dbo.SYS_WorkGroup WHERE IsEnable = 1 AND ID = {0} ", listA[0].MainID);
                        List<SYS_WorkGroup> listB = db.Sql(sqlB).QueryMany<SYS_WorkGroup>();

                        if (listB.Count <= 0)
                        {
                            throw new Exception(@"该用户未被分配到班组");
                        }
                        else if (listB.Count.Equals(1))
                        {
                            string sqlC = string.Format(@"SELECT * FROM dbo.SYS_BN_Warehouse WHERE IsEnable = 1 AND WarehouseType = 1 AND WarehouseName = '{0}' ", listB[0].DepartName);
                            List<SYS_BN_Warehouse> listC = db.Sql(sqlC).QueryMany<SYS_BN_Warehouse>();

                            if (listC.Count <= 0)
                            {
                                throw new Exception(@"仓库异常：此编号所在班组不存在仓库！");
                            }
                            else if (listC.Count.Equals(1))
                            {
                                string token = string.Empty;
                                if (new LoginLogService().Insert(userCode, ref token).Result)
                                {
                                    return new ResultModel()
                                    {
                                        Result = true,
                                        Data = new
                                        {
                                            Token = token,
                                            BarCode = barCode,
                                            UserCode = listA[0].UserCode,
                                            UserName = listA[0].UserName,
                                            ExtraInfo = new
                                            {
                                                TeamCode = listB[0].TeamCode,
                                                TeamName = listB[0].TeamName,
                                                DepartID = listB[0].DepartID,
                                                WarehouseCode = listC[0].WarehouseCode,
                                                WarehouseName = listC[0].WarehouseName
                                            }
                                        }
                                    };
                                }
                                else
                                {
                                    throw new Exception(@"Token生成失败！");
                                }
                            }
                            else
                            {
                                throw new Exception(@"仓库异常：此编号被分配到多个班组！");
                            }
                        }
                        else
                        {
                            throw new Exception(@"班组异常：此编号被分配到多个班组！");
                        }
                    }
                    else
                    {
                        throw new Exception(@"用户异常：此编号被多个用户使用！");
                    }
                }
                else
                {
                    throw new Exception(@"该条码被多个用户使用！");
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
        /// 根据条码查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoByBarCode(string barCode)
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserBarcode = '{0}'", barCode);

                var model = db.Sql(sql).QuerySingle<SYS_BN_User>();

                bool result = model != null;

                return new ResultModel()
                {
                    Result = result,
                    Data = result ? new
                    {
                        BarCode = model.UserBarcode,
                        UserCode = model.UserCode,
                        UserName = model.UserName
                    } : null,
                    Msg = result ? "查询成功！" : $"该条码【{barCode}】不存在！"
                };
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


        #endregion

        /// <summary>
        /// 根据班组编号查生产计划
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public ResultModel GetWorkingTicketDataByTCode(string teamCode)
        {
            try
            {
                string sql = string.Format(@"
SELECT 
tbC.ProjectID,
tbC.ContractCode,
tbC.ProjectName,
tbD.ID AS ProductID,
tbD.ProductName,
tbB.ID AS ProducePlanID,
tbB.Quantity,
tbB.RootPartCode,
tbB.ApsCode,
tbA.ID AS WorkTicketID,
tbA.WorkTicketCode,
tbA.PartCode,
tbA.WorkStepsName,
tbA.WorkStepsContent,
tbA.ApproveTime,
tbA.ApproveRemark,
tbA.TicketLevel,
tbA.TicketStatus,
tbE.ID AS ProcessRouteID,
tbE.ProcessName
FROM dbo.MES_WorkingTicket tbA --工票
LEFT JOIN dbo.APS_ProjectProduceDetial tbB --生产计划
ON tbA.ApsCode = tbB.ApsCode AND tbA.IsEnable = 1 AND tbB.IsEnable =1 AND tbA.ApproveState = 2 AND tbA.TeamCode ='{0}'
LEFT JOIN dbo.PMS_BN_Project tbC --项目
ON tbB.ContractCode = tbC.ContractCode AND tbC.IsEnable =1
LEFT JOIN dbo.PMS_BN_ProjectDetail tbD --产品
ON tbB.ProjectDetailID = tbD.ID AND tbD.IsEnable =1
LEFT JOIN dbo.MES_BN_ProductProcessRoute tbE --工艺路线
ON tbB.ContractCode = tbE.ContractCode AND tbB.PartCode = tbE.PartCode AND tbA.ProcessCode = tbE.ProcessCode AND tbB.ProcessLineCode = tbE.ProcessLineCode AND tbE.IsEnable =1
WHERE tbB.ID IS NOT NULL AND tbC.ProjectID IS NOT NULL AND tbD.ID IS NOT NULL AND tbE.ID IS NOT NULL
", teamCode);

                List<dynamic> items = db.Sql(sql).QueryMany<dynamic>();
                List<object> data = new List<object>();

                foreach (var item in items)
                {
                    string sqlPBom = string.Format(@"SELECT * FROM dbo.Get_ProcessBOM('{0}') WHERE PartCode = '{1}'", item.RootPartCode, item.PartCode);
                    var pBomModel = db.Sql(sqlPBom).QuerySingle<PRS_Process_BOM>();

                    data.Add(new
                    {
                        //项目
                        ProjectID = item.ProjectID,
                        ContractCode = item.ContractCode,
                        ProjectName = item.ProjectName,
                        //产品
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        //计划
                        ProducePlanID = item.ProducePlanID,
                        Quantity = item.Quantity,
                        ApsCode = item.ApsCode,
                        //工票
                        WorkTicketID = item.WorkTicketID,
                        WorkTicketCode = item.WorkTicketCode,
                        WorkStepsName = item.WorkStepsName,
                        WorkStepsContent = item.WorkStepsContent,
                        ApproveTime = item.ApproveTime,
                        ApproveRemark = item.ApproveRemark,
                        TicketLevel = item.TicketLevel,
                        TicketStatus = item.TicketStatus,
                        //工艺路线
                        ProcessRouteID = item.ProcessRouteID,
                        ProcessName = item.ProcessName,
                        //工艺bom
                        ProcessBomID = pBomModel.ID,
                        PartCode = pBomModel.PartCode,
                        PartName = pBomModel.PartName,
                        FigureNo = pBomModel.PartFigureCode
                    });
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = data
                };
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
        /// 根据工票ID查物料
        /// </summary>
        /// <param name="wTicketID"></param>
        /// <returns></returns>
        public ResultModel GetMaterialInfoByWTicketID(int wTicketID)
        {
            try
            {
                string sqlWTicket = string.Format(@"SELECT * FROM dbo.MES_WorkingTicket WHERE ID = {0}", wTicketID);
                var wTicketModel = db.Sql(sqlWTicket).QuerySingle<MES_WorkingTicket>();

                string sqlA = string.Format(@"SELECT * FROM dbo.MES_WorkTicketMate WHERE IsEnable = 1 AND WorkTicketCode = '{0}'", wTicketModel.WorkTicketCode);
                List<MES_WorkTicketMate> dataA = db.Sql(sqlA).QueryMany<MES_WorkTicketMate>();

                string sqlB = string.Format(@"
SELECT DISTINCT tbB.* FROM dbo.MES_WorkTicketMate tbA
LEFT JOIN dbo.PRS_Process_BOM tbB
ON tbA.PartCode = tbB.PartCode AND ISNULL(tbA.ParentCode,'') = ISNULL(tbB.ParentCode,'') AND tbB.IsEnable = 1
WHERE tbA.IsEnable = 1 AND tbA.WorkTicketCode ='{0}'
", wTicketModel.WorkTicketCode);
                List<PRS_Process_BOM> dataB = db.Sql(sqlB).QueryMany<PRS_Process_BOM>();

                string sqlC = string.Format(@"
SELECT DISTINCT tbC.* FROM dbo.MES_WorkTicketMate tbA
LEFT JOIN dbo.PRS_Process_BOM tbB
ON tbA.PartCode = tbB.PartCode AND ISNULL(tbA.ParentCode,'') = ISNULL(tbB.ParentCode,'') AND tbB.IsEnable = 1
LEFT JOIN dbo.SYS_Part tbC
ON tbB.InventoryCode = tbC.InventoryCode AND tbB.InventoryName = tbC.InventoryName AND tbC.IsEnable = 1
WHERE tbA.IsEnable = 1 AND tbA.WorkTicketCode ='{0}'
", wTicketModel.WorkTicketCode);
                List<SYS_Part> dataC = db.Sql(sqlC).QueryMany<SYS_Part>();

                string sqlD = string.Format(@"
SELECT InventoryCode,
       SUM(ConfirmNum) AS ScanQuantity
FROM dbo.WMS_BN_BillDetail
WHERE IsEnable = 1
      AND BillCode IN
          (
              SELECT BillCode
              FROM dbo.WMS_BN_BillMain
              WHERE IsEnable = 1
                    AND BillType = 7
          )
      AND PBillCode = '{0}'
GROUP BY InventoryCode;
", wTicketModel.WorkTicketCode);
                List<dynamic> dataD = db.Sql(sqlD).QueryMany<dynamic>();

                int[] types = new[] { 4, 5 };

                var oneSelf = dataA.Single(item => item.PartCode.Equals(wTicketModel.PartCode));
                dataA.Remove(oneSelf);

                var oneSelfPBomModel = dataB.Single(item => item.PartCode.Equals(oneSelf.PartCode));
                bool isCastingOrForging = new[] { 4, 5 }.Contains(oneSelfPBomModel.MateType ?? -1);
                SYS_Part oneSelfPartModel;
                if (isCastingOrForging)
                {
                    oneSelfPartModel = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", oneSelfPBomModel.New_InventoryCode ?? "")).QueryMany<SYS_Part>().FirstOrDefault();
                    if (oneSelfPartModel == null)
                    {
                        throw new Exception(@"本体为铸件锻件，但是在Part查不到信息！");
                    }
                }
                else
                {
                    oneSelfPartModel = dataC.Where(item => item.InventoryCode == oneSelfPBomModel.InventoryCode).ToList().FirstOrDefault();
                    if (oneSelfPartModel == null)
                    {
                        throw new Exception("本体\n零件编码：" + oneSelfPBomModel.PartCode + "\n存货编码：" + oneSelfPBomModel.InventoryCode + "\n在Part表查不到详细信息！");
                    }
                }

                var tempOneSelf = dataD.SingleOrDefault(i => i.InventoryCode == oneSelfPBomModel.InventoryCode);
                int partAlreadyScanQuantityOneSelf = 0;
                if ((oneSelf.IsCrux ?? -1).Equals(0))
                {
                    string sqlalreadySQL = string.Format(@"SELECT COUNT(ID) FROM dbo.MES_MateSweepCodeLog WHERE IsEnable = 1 AND ApsCode = '{0}' AND MateBarCode = '{1}'", wTicketModel.WorkTicketCode, oneSelfPartModel.CorrespondingBarcode);
                    if (db.Sql(sqlalreadySQL).QuerySingle<int>() > 0)
                    {
                        partAlreadyScanQuantityOneSelf = oneSelf.RequiredQuantity ?? 0;
                    }
                }
                else
                {
                    partAlreadyScanQuantityOneSelf = tempOneSelf == null ? 0 : Convert.ToInt32(tempOneSelf.ScanQuantity);
                }

                object oneSelfModel = new
                {
                    PartCode = oneSelf.PartCode,
                    PartName = oneSelfPBomModel.PartName,
                    FigureNo = oneSelfPBomModel.PartFigureCode,
                    TotalQuantity = oneSelf.RequiredQuantity ?? 0,
                    AlreadyScanQuantity = partAlreadyScanQuantityOneSelf,
                    MaterialCode = isCastingOrForging ? oneSelfPartModel.InventoryCode : oneSelfPBomModel.MaterialCode,
                    ExtraInfo = new
                    {
                        BomInfo = oneSelfPBomModel,
                        PartInfo = oneSelfPartModel
                    }
                };

                List<object> childModelList = new List<object>();

                foreach (var item in dataA)
                {
                    var childPBomModel = dataB.Single(i => i.PartCode.Equals(item.PartCode));
                    var childPartModel = dataC.Where(i => i.InventoryCode == childPBomModel.InventoryCode).ToList().FirstOrDefault();
                    if (childPartModel == null)
                    {
                        throw new Exception("本体\n零件编码：" + childPartModel.PartCode + "\n存货编码：" + childPartModel.InventoryCode + "\n在Part表查不到详细信息！");
                    }

                    var tempChild = dataD.SingleOrDefault(i => i.InventoryCode == childPBomModel.InventoryCode);
                    int partAlreadyScanQuantitychild = 0;
                    if ((item.IsCrux ?? -1).Equals(0))
                    {
                        string sqlalreadySQL = string.Format(@"SELECT COUNT(ID) FROM dbo.MES_MateSweepCodeLog WHERE IsEnable = 1 AND ApsCode = '{0}' AND MateBarCode = '{1}'", wTicketModel.WorkTicketCode, childPartModel.CorrespondingBarcode);
                        if (db.Sql(sqlalreadySQL).QuerySingle<int>() > 0)
                        {
                            partAlreadyScanQuantitychild = item.RequiredQuantity ?? 0;
                        }
                    }
                    else
                    {
                        partAlreadyScanQuantitychild = tempChild == null ? 0 : Convert.ToInt32(tempChild.ScanQuantity);
                    }

                    childModelList.Add(new
                    {
                        PartCode = item.PartCode,
                        PartName = childPBomModel.PartName,
                        FigureNo = childPBomModel.PartFigureCode,
                        TotalQuantity = item.RequiredQuantity ?? 0,
                        AlreadyScanQuantity = partAlreadyScanQuantitychild,
                        MaterialCode = childPBomModel.MaterialCode,
                        ExtraInfo = new
                        {
                            BomInfo = childPBomModel,
                            PartInfo = childPartModel
                        }
                    });
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = new
                    {
                        MaterialOneSelf = new
                        {
                            IsCastingOrForging = isCastingOrForging,
                            MaterialInfo = oneSelfModel
                        },
                        MaterialChild = new
                        {
                            MaterialInfos = childModelList
                        }
                    }
                };
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
        /// 根据工票ID查下料物料
        /// </summary>
        /// <param name="wTicketID"></param>
        /// <returns></returns>
        public ResultModel GetBlankingMaterialInfoByWTicketID(int wTicketID)
        {
            try
            {
                string sql = string.Format(@"
SELECT DISTINCT tbE.ProgramCode,tbF.BatchCode,tbG.InventoryCode,tbG.InventoryName,tbG.Model
FROM dbo.MES_WorkingTicket tbA
LEFT JOIN dbo.APS_ProjectProduceDetial tbB ON tbA.ApsCode = tbB.ApsCode AND tbB.IsEnable = 1
LEFT JOIN dbo.PRS_BlankingResult tbC ON tbB.SavantCode = tbC.SavantCode AND tbC.IsEnable = 1
LEFT JOIN dbo.Mes_BlankingResult tbD ON tbC.ID = tbD.BlankingResultID AND tbD.IsEnable = 1
LEFT JOIN dbo.Mes_BlankingBoard tbE ON tbD.MainID = tbE.ID AND tbE.IsEnable = 1
LEFT JOIN dbo.WMS_BN_RealStock tbF ON tbE.BoardInventoryCode = tbF.InventoryCode AND tbF.IsEnable = 1
LEFT JOIN dbo.SYS_Part tbG ON tbE.BoardInventoryCode = tbG.InventoryCode AND tbG.IsEnable = 1
WHERE tbA.IsEnable = 1 AND tbA.ID = {0}
", wTicketID);

                dynamic data = db.Sql(sql).QuerySingle<dynamic>();

                if (data == null)
                {
                    throw new Exception(@"未查出程序号、批次号等信息！");
                }
                else
                {
                    return new ResultModel()
                    {
                        Result = true,
                        Data = data
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
        /// 获取生产计划所需图纸文件
        /// </summary>
        /// <param name="pRouteID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public ResultModel GetPlanNeedFile(int pRouteID, int productID)
        {
            try
            {
                string sqlA = string.Format(@"SELECT * FROM dbo.QMS_QualityReportDoc WHERE IsEnable = 1 AND FileType = 5 AND SourceID = '{0}'", pRouteID);
                List<QMS_QualityReportDoc> dataA = db.Sql(sqlA).QueryMany<QMS_QualityReportDoc>();

                string sqlB = string.Format(@"
SELECT tbA.* FROM dbo.PRS_ProcessFigure tbA,
(SELECT * FROM dbo.MES_BN_ProductProcessRoute WHERE ID = {0}) tbB 
WHERE tbA.IsEnable = 1 AND tbA.ProductID = {1} AND tbA.ContractCode = tbB.ContractCode AND tbA.PartCode = tbB.PartCode
", pRouteID, productID);
                List<PRS_ProcessFigure> dataB = db.Sql(sqlB).QueryMany<PRS_ProcessFigure>();

                List<object> data = new List<object>();

                dataA.ForEach(item =>
                {
                    data.Add(new
                    {
                        DocName = item.DocName,
                        FileName = item.FileName,
                        FileAddress = item.FileAddress,
                        WebAddressType = item.FileAddress.ToLower().Contains("ftp://") ? 2 : item.FileAddress.ToLower().Contains("http://") ? 1 : 0
                    });
                });

                dataB.ForEach(item =>
                {
                    data.Add(new
                    {
                        DocName = item.DocName,
                        FileName = item.FileName,
                        FileAddress = item.FileAddress,
                        WebAddressType = item.FileAddress.ToLower().Contains("ftp://") ? 2 : item.FileAddress.ToLower().Contains("http://") ? 1 : 0
                    });
                });

                return new ResultModel()
                {
                    Result = true,
                    Data = data
                };
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
        /// <returns></returns>
        public ResultModel SetProduceLog(dynamic data)
        {
            try
            {
                int type = Convert.ToInt32(data["Type"]);
                int pauseReson = Convert.ToInt32(data["PauseReson"]);
                DateTime newDate = DateTime.Now;

                MES_OpreatorWorkingLog model = new MES_OpreatorWorkingLog()
                {
                    APSCode = data["WorkTicketCode"],
                    BegainTime = newDate,
                    IsEnable = 1,
                    CreatePerson = data["UserName"],
                    CreateTime = newDate,
                    OperatePerson = data["UserName"],
                    OpreateCode = data["UserCode"],
                    ModifyPerson = data["UserName"],
                    ModifyTime = newDate,
                    FinishQuantity = Convert.ToDecimal(data["Quantity"])
                };

                string sqlLog = string.Format(@"SELECT * FROM  dbo.MES_OpreatorWorkingLog WHERE IsEnable = 1 AND APSCode = '{0}' AND BegainTime IS NOT NULL AND FinishTime IS NULL ORDER BY CreateTime DESC", model.APSCode);
                MES_OpreatorWorkingLog logModel = db.Sql(sqlLog).QueryMany<MES_OpreatorWorkingLog>().FirstOrDefault();

                int logID = 0;

                switch (type)
                {
                    //新增
                    case 0:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualStartTime = newDate,
                            TicketStatus = 2
                        })).Execute();

                        model.FinishQuantity = null;
                        logID = db.Sql(WinFormClientService.GetInsertSQL(model) + "SELECT @@IDENTITY;").QuerySingle<int>();
                        break;
                    //暂停
                    case 1:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualFinishTime = newDate,
                            TicketStatus = 4
                        })).Execute();

                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_OpreatorWorkingLog), new KeyValuePair<string, object>("ID", logModel.ID), new
                        {
                            FinishTime = newDate,
                            WorkingHour = (int)Math.Floor(new TimeSpan(newDate.Ticks).Subtract(new TimeSpan(Convert.ToDateTime(logModel.BegainTime).Ticks)).Duration().TotalSeconds),
                            ModifyTime = newDate,
                            PauseType = 1,
                            PauseReson = pauseReson
                        })).Execute();

                        logID = logModel.ID;
                        break;
                    //完工
                    case 2:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualFinishTime = newDate,
                            TicketStatus = 3
                        })).Execute();

                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_OpreatorWorkingLog), new KeyValuePair<string, object>("ID", logModel.ID), new
                        {
                            FinishTime = newDate,
                            WorkingHour = (int)Math.Floor(new TimeSpan(newDate.Ticks).Subtract(new TimeSpan(Convert.ToDateTime(logModel.BegainTime).Ticks)).Duration().TotalSeconds),
                            ModifyTime = newDate,
                            PauseType = 2,
                            FinishQuantity = model.FinishQuantity
                        })).Execute();

                        logID = logModel.ID;
                        break;
                    default:
                        throw new Exception(@"参数错误！");
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = logID
                };
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
        /// 录入生产实际开始/结束时间
        /// </summary>
        /// <returns></returns>
        public ResultModel SetActualProducePlanDate(int planID, int state)
        {
            try
            {
                string sql = string.Empty;
                DateTime newDate = DateTime.Now;

                if (state.Equals(0))
                {
                    sql = string.Format(@"UPDATE dbo.APS_ProjectProduceDetial SET ActualStartTime = '{0}' WHERE ID = {1}", newDate, planID);
                }
                else if (state.Equals(1))
                {
                    sql = string.Format(@"UPDATE dbo.APS_ProjectProduceDetial SET ActualFinishTime = '{0}' WHERE ID = {1}", newDate, planID);
                }
                else
                {
                    throw new Exception(@"参数错误！");
                }

                bool result = db.Sql(sql).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? @"成功！" : "失败！"
                };
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
        /// <returns></returns>
        public dynamic SetProduceOperator(dynamic data)
        {
            try
            {
                string barCodeStr = data["BarCodes"];
                int logID = Convert.ToInt32(data["LogID"]);
                List<string> barCodes = barCodeStr.Split(',').ToList();
                var barCodeList = barCodes.Select(item => { return "'" + item + "'"; }).ToList();
                var userList = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserBarcode IN ({0})", string.Join(",", barCodeList))).QueryMany<SYS_BN_User>();

                StringBuilder sb = new StringBuilder();

                foreach (var item in barCodes)
                {
                    var userModel = userList.Single(i => i.UserBarcode.Equals(item));

                    sb.Append(WinFormClientService.GetInsertSQL(new MES_OperatorStatistical()
                    {
                        MainID = logID,
                        OperatorCode = userModel.UserCode,
                        OperatorName = userModel.UserName,
                        OperatorBarCode = userModel.UserBarcode
                    }));
                }

                bool result = db.Sql(sb.ToString()).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "保存成功！" : "保存失败！"
                };
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
        /// 物料扫码出库
        /// </summary>
        /// <returns></returns>
        public ResultModel MaterialOutput(dynamic data)
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

                db.UseTransaction(true);

                int tempA = db.Sql(GetInsertSQL(bMainModel)).Execute();

                var realStocks = db.Sql(string.Format(@"SELECT * FROM dbo.WMS_BN_RealStock WHERE IsEnable = 1 AND WarehouseCode = '{0}' AND InventoryCode = '{1}' ORDER BY BatchCode", bMainModel.WarehouseCode, bDetailModel.InventoryCode)).QueryMany<WMS_BN_RealStock>();
                WMS_BN_RealStock realStock = null;

                if (realStocks.Count <= 0)
                {
                    throw new Exception(@"仓库没有物料！");
                }
                else
                {
                    var num = Convert.ToInt32(bDetailModel.ConfirmNum ?? 1);

                    foreach (var item in realStocks)
                    {
                        if (item.RealStock == null)
                        {
                            throw new Exception(@"仓库物料数量异常！");
                        }
                        else
                        {
                            if ((item.RealStock ?? 0) < num)
                            {

                            }
                            else
                            {
                                realStock = item;
                                bDetailModel.BatchCode = item.BatchCode;
                                break;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(bDetailModel.BatchCode))
                    {
                        throw new Exception(@"仓库物料数量不够，无法出库！");
                    }
                }

                int tempB = db.Sql(GetInsertSQL(bDetailModel)).Execute();

                string sql = GetUpdateSQL(nameof(WMS_BN_RealStock), new KeyValuePair<string, object>("ID", realStock.ID), new
                {
                    RealStock = realStock.RealStock - bDetailModel.ConfirmNum,
                    TotalStock = realStock.TotalStock - bDetailModel.ConfirmNum,
                    ModifyPerson = bDetailModel.ModifyPerson,
                    ModifyTime = bDetailModel.ModifyTime,
                });

                int tempC = db.Sql(sql).Execute();

                bool result = tempA > 0 && tempB > 0 && tempC > 0;
                if (result)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? @"成功！" : "失败！"
                };
            }
            catch (Exception ex)
            {
                db.Rollback();

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
        public ResultModel SaveBarCodeScanLog(dynamic data)
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

                bool result = db.Sql(GetInsertSQL(scanLog)).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? @"成功！" : "失败！"
                };
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
        /// 根据条码 查PartCode、FigureNo
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public ResultModel GetPartCodeAndFigureNoByBarCode(string barCode)
        {
            try
            {
                if (string.IsNullOrEmpty(barCode))
                {
                    throw new Exception("参数无效！");
                }

                string sql = string.Format(@"
SELECT tbA.PartCode,tbA.PartName,tbA.PartFigureCode
FROM dbo.PRS_Process_BOM tbA JOIN dbo.SYS_Part tbB 
ON tbB.IsEnable = 1 AND tbB.CorrespondingBarcode = '{0}' AND tbA.InventoryCode = tbB.InventoryCode AND tbA.InventoryName = tbB.InventoryName
WHERE tbA.IsEnable = 1
", barCode);
                dynamic model = db.Sql(sql).QuerySingle<dynamic>();

                if (model == null)
                {
                    throw new Exception("该条码在Part表查不到信息！");
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = new
                    {
                        PartCode = model.PartCode,
                        PartName = model.PartName,
                        FigureNo = model.PartFigureCode
                    }
                };
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
        /// 判断某生产计划是否是最后一工序
        /// </summary>
        /// <returns></returns>
        public ResultModel GetIsLastProcess(int planID)
        {
            try
            {
                var ppdLastModel = db.Sql(string.Format(@"
SELECT TOP 1
       *
FROM dbo.APS_ProjectProduceDetial tbA,
(
    SELECT *
    FROM dbo.APS_ProjectProduceDetial
    WHERE IsEnable = 1
          AND ID = {0}
) tbB
WHERE tbA.IsEnable = 1
      AND tbA.ContractCode = tbB.ContractCode
      AND tbA.ProjectDetailID = tbB.ProjectDetailID
      AND tbA.PartCode = tbB.PartCode
      AND tbA.ProcessModelType = tbB.ProcessModelType
      AND tbA.PlanState = 2
ORDER BY tbA.ProcessLineCode DESC;
", planID)).QuerySingle<APS_ProjectProduceDetial>();

                bool result = ppdLastModel.ID.Equals(planID);

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? @"成功！" : "失败！"
                };
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
        /// 生成条码
        /// </summary>
        /// <param name="partID"></param>
        /// <returns></returns>
        public ResultModel GenerateBarCode(string partID)
        {
            try
            {
                string sqlPart = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE ID = {0}", partID);
                var partModel = db.Sql(sqlPart).QuerySingle<SYS_Part>();

                string barCode = string.Empty;

                if (string.IsNullOrEmpty(partModel.CorrespondingBarcode))
                {
                    string sqlA = string.Format(@"
DECLARE @barcode VARCHAR(50) =
        (
            SELECT RIGHT(REPLICATE('0', 12) +
                         (
                             SELECT CONVERT(VARCHAR(50), CONVERT(BIGINT, ISNULL(MAX(CorrespondingBarcode), '0')) + 1)
                             FROM dbo.SYS_Part
                         ), 12)
        );
UPDATE dbo.SYS_Part
SET CorrespondingBarcode = @barcode
WHERE ID = '{0}'
SELECT @barcode;
", partID);
                    barCode = db.Sql(sqlA).QuerySingle<string>();
                }
                else
                {
                    barCode = partModel.CorrespondingBarcode;
                }

                return new ResultModel()
                {
                    Result = true
                };
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



        #region 反射生成SQL语句

        /// <summary>
        /// 根据对象生成SQL插入语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetInsertSQL<T>(T obj) where T : class
        {
            List<string> colmonsList = new List<string>();
            StringBuilder sbValues = new StringBuilder();

            var attrArr = obj.GetType().GetProperties();

            foreach (var attr in attrArr)
            {
                bool isIdentity = attr.CustomAttributes.Where(item => item.AttributeType.Name.Equals("IdentityAttribute")).Count() > 0;

                if (isIdentity)
                {
                    continue;
                }
                else
                {
                    colmonsList.Add(attr.Name);

                    if (attr.GetValue(obj, null) == null)
                    {
                        sbValues.Append("NULL,");
                    }
                    else
                    {
                        sbValues.Append(string.Format(@"'{0}',", attr.GetValue(obj, null)));
                    }
                }
            }

            string strValues = sbValues.ToString();

            return string.Format(@"insert into {0}({1}) values({2});", obj.GetType().Name, string.Join(",", colmonsList), strValues.Substring(0, strValues.Length - 1));
        }

        /// <summary>
        /// 根据对象生成SQL更新语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetUpdateSQL<T>(T oldModel, object newModel) where T : class
        {
            StringBuilder sbValues = new StringBuilder();

            var primaryKeyColName = string.Empty;
            var primaryKeyColValue = string.Empty;

            var oldAttrArr = oldModel.GetType().GetProperties();
            var newAttrArr = newModel.GetType().GetProperties();

            foreach (var attr in oldAttrArr)
            {
                bool isPrimaryKey = attr.CustomAttributes.Where(item => item.AttributeType.Name.Equals("PrimaryKeyAttribute")).Count() > 0;
                if (isPrimaryKey)
                {
                    primaryKeyColName = attr.Name;
                    primaryKeyColValue = oldModel.GetType().GetProperty(attr.Name).GetValue(oldModel, null).ToString();
                    break;
                }
            }

            if (string.IsNullOrEmpty(primaryKeyColName))
            {
                throw new Exception("找不到主键！");
            }

            foreach (var item in newAttrArr)
            {
                if (item.Name.Equals(primaryKeyColName))
                {
                    continue;
                }

                var oldValue = oldModel.GetType().GetProperty(item.Name).GetValue(oldModel, null);
                var newValue = newModel.GetType().GetProperty(item.Name).GetValue(newModel, null);

                if (oldValue == null && newValue == null)
                {
                    continue;
                }
                else if (oldValue == null && newValue != null)
                {
                    sbValues.Append(string.Format(@"[{0}] = '{1}',", item.Name, newValue));
                }
                else if (oldValue != null && newValue == null)
                {
                    sbValues.Append(string.Format(@"[{0}] = NULL,", item.Name));
                }
                else
                {
                    if (oldValue.Equals(newValue))
                    {
                        continue;
                    }
                    else
                    {
                        sbValues.Append(string.Format(@"[{0}] = '{1}',", item.Name, newValue));
                    }
                }
            }

            string strSQL = sbValues.ToString();

            return string.Format(@"update {0} set {1} where {2} = {3};", oldModel.GetType().Name, strSQL.Substring(0, strSQL.Length - 1), primaryKeyColName, primaryKeyColValue);
        }

        public static string GetUpdateSQL(string tableName, Dictionary<string, object> where, object newModel)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbWheres = new StringBuilder();

            foreach (KeyValuePair<string, object> kvp in where)
            {
                sbWheres.Append(string.Format(@" and [{0}] = '{1}'", kvp.Key, kvp.Value));
            }

            var newAttrArr = newModel.GetType().GetProperties();

            foreach (var item in newAttrArr)
            {
                var newValue = newModel.GetType().GetProperty(item.Name).GetValue(newModel, null);
                sbValues.Append(string.Format(@"[{0}] = '{1}',", item.Name, newValue));
            }

            string strValues = sbValues.ToString();

            return string.Format(@"update {0} set {1} where 1 = 1 {2};", tableName, strValues.Substring(0, strValues.Length - 1), sbWheres.ToString());
        }

        public static string GetUpdateSQL(string tableName, KeyValuePair<string, object> where, object newModel)
        {
            StringBuilder sbValues = new StringBuilder();

            var newAttrArr = newModel.GetType().GetProperties();

            foreach (var item in newAttrArr)
            {
                var newValue = newModel.GetType().GetProperty(item.Name).GetValue(newModel, null);
                sbValues.Append(string.Format(@"[{0}] = '{1}',", item.Name, newValue));
            }

            string strValues = sbValues.ToString();

            return string.Format(@"update {0} set {1} where [{2}] = '{3}';", tableName, strValues.Substring(0, strValues.Length - 1), where.Key, where.Value);
        }

        #endregion

        /*
         try
            {



                return new ResultModel()
                {
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
         */
    }

    public class Client : ModelBase
    {


    }

}