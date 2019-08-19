using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.Mms.Controllers;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;
using System.Collections;
using System.Web.UI.WebControls;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_OpreatorWorkingLogService : ServiceBase<WinFormClient>
    {
        public dynamic SaveProduceLog(MES_OpreatorWorkingLog model, int type, int pauseReson)
        {
            try
            {
                string sqlA = string.Format(@"
SELECT * FROM  dbo.MES_OpreatorWorkingLog WHERE IsEnable = 1 AND APSCode = '{0}' AND BegainTime IS NOT NULL AND FinishTime IS NULL ORDER BY CreateTime DESC 
", model.APSCode);
                List<MES_OpreatorWorkingLog> listA = db.Sql(sqlA).QueryMany<MES_OpreatorWorkingLog>();

                string sql = string.Empty;
                DateTime newDate = DateTime.Now;

                switch (type)
                {
                    case 0:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualStartTime = newDate,
                            TicketStatus = 2
                        })).Execute();
                        break;
                    case 1:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualFinishTime = newDate,
                            TicketStatus = 4
                        })).Execute();
                        break;
                    case 2:
                        db.Sql(WinFormClientService.GetUpdateSQL(nameof(MES_WorkingTicket), new KeyValuePair<string, object>("WorkTicketCode", model.APSCode), new
                        {
                            ActualFinishTime = newDate,
                            TicketStatus = 3
                        })).Execute();
                        break;
                    default:
                        throw new Exception(@"参数错误！");
                }

                //新增记录
                if (type.Equals(0))
                {
                    model.FinishQuantity = null;
                    sql = WinFormClientService.GetInsertSQL(model);

                    sql += "SELECT @@IDENTITY;";

                    int id = db.Sql(sql).QuerySingle<int>();

                    return new ResultModel()
                    {
                        Result = id > 0,
                        Data = id
                    };

                }
                //更新记录
                else
                {
                    //暂停
                    if (type.Equals(1))
                    {
                        sql = WinFormClientService.GetUpdateSQL(nameof(MES_OpreatorWorkingLog), new KeyValuePair<string, object>("ID", listA[0].ID), new
                        {
                            FinishTime = newDate,
                            WorkingHour = GetDateDisparity(Convert.ToDateTime(listA[0].BegainTime), newDate),
                            ModifyTime = newDate,
                            PauseType = 1,
                            PauseReson = pauseReson
                        });
                    }
                    //完工
                    else if (type.Equals(2))
                    {
                        sql = WinFormClientService.GetUpdateSQL(nameof(MES_OpreatorWorkingLog), new KeyValuePair<string, object>("ID", listA[0].ID), new
                        {
                            FinishTime = newDate,
                            WorkingHour = GetDateDisparity(Convert.ToDateTime(listA[0].BegainTime), newDate),
                            ModifyTime = newDate,
                            PauseType = 2,
                            FinishQuantity = model.FinishQuantity
                        });
                    }
                    //参数错误
                    else
                    {
                        throw new Exception(@"参数错误！");
                    }

                    return new ResultModel()
                    {
                        Result = db.Sql(sql).Execute() > 0,
                        Data = listA[0].ID
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
        /// 获取时间差
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns>单位 分钟</returns>
        public int GetDateDisparity(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts2.Subtract(ts1).Duration();

            int sec = (int)Math.Floor(ts3.TotalSeconds);

            return sec / 60;
        }

        public bool Insert(MES_OpreatorWorkingLog model)
        {
            using (var db = Db.Context("Mms"))
            {
                #region
                string sql = string.Format(@"
INSERT INTO dbo.MES_OpreatorWorkingLog
(
    APSCode,
    BegainTime,
    FinishTime,
    WorkingHour,
    OpreateCode,
    OperatePerson,
    IsEnable,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime
)
VALUES
(   '{0}',        -- APSCode - varchar(50)
    '{1}', -- BegainTime - datetime
    '{2}', -- FinishTime - datetime
    {3},         -- WorkingHour - int
    '{4}',        -- OpreateCode - varchar(50)
    '{5}',        -- OperatePerson - varchar(50)
    {6},         -- IsEnable - int
    '{7}',        -- CreatePerson - varchar(50)
    GETDATE(), -- CreateTime - datetime
    '{8}',        -- ModifyPerson - varchar(50)
    GETDATE()  -- ModifyTime - datetime
    );
", model.APSCode, model.BegainTime, model.FinishTime, model.WorkingHour, model.OpreateCode, model.OperatePerson, 1,
model.CreatePerson, model.CreateTime, model.ModifyPerson, model.ModifyTime);
                #endregion

                int n = db.Sql(sql).Execute();

                return n > 0;
            }
        }

        public bool Update(MES_OpreatorWorkingLog model)
        {

            throw new Exception("暂未实现");
        }

        /// <summary>
        /// 获取完工确认 页面班组选项数据
        /// </summary>
        /// <returns></returns>
        public dynamic GetWorkGroupData()
        {
            try
            {
                string sql = string.Format(@"
SELECT TeamCode,
       TeamName
FROM dbo.SYS_WorkGroup
WHERE IsEnable = 1
      AND DepartID IN
          (
              SELECT DepartmentCode
              FROM dbo.SYS_BN_Department
              WHERE IsEnable = 1
                    AND IsWorkshop = 1
                    AND DepartmentCode IN
                        (
                            SELECT DepartmentCode
                            FROM dbo.SYS_BN_User
                            WHERE IsEnable = 1
                                  AND UserCode = '{0}'
                        )
          );
", MmsHelper.GetUserCode());

                var result = db.Sql(sql).QueryMany<dynamic>();

                return result;
            }
            catch (Exception ex)
            {
                var result = new List<dynamic>();

                return result;
            }
        }

        public dynamic GetProduceLogInfo(string workGroup, DateTime? beginDate, DateTime? finishDate)
        {
            using (var db = Db.Context("Mms"))
            {
                List<dynamic> result = new List<dynamic>();
                string sqlA = string.Format(@"
DECLARE @userCode varchar(50) = '{0}';
SELECT t1.*,t6.IsInspectionReport
FROM dbo.MES_OpreatorWorkingLog t1
    INNER JOIN dbo.SYS_BN_User t2
        ON t2.UserCode = @userCode
           AND t2.IsEnable = 1
    INNER JOIN dbo.SYS_BN_User t3
        ON t1.OpreateCode = t3.UserCode
           AND t2.DepartmentCode = t3.DepartmentCode
           AND t3.IsEnable = 1
    INNER JOIN dbo.MES_WorkingTicket t4
        ON t1.APSCode = t4.WorkTicketCode
           AND t4.IsEnable = 1
    INNER JOIN dbo.APS_ProjectProduceDetial t5
        ON t5.ApsCode = t4.ApsCode
           AND t5.IsEnable = 1
    INNER JOIN dbo.MES_BN_ProductProcessRoute t6
        ON t5.ProcessCode = t6.ProcessCode
           AND t5.ProcessModelType = t6.ProcessModelType
           AND t5.PartCode = t6.PartCode
           AND t6.ProcessLineCode = t5.ProcessLineCode
		   WHERE t1. IsEnable = 1
          AND PauseType = 2
          AND AuditState = 0
", MmsHelper.GetUserCode() /*"050311"*/);
                //string sqlA = string.Format("SELECT * FROM MES_OpreatorWorkingLog WHERE IsEnable = 1");
                //var listA = db.Sql(sqlA).QueryMany<dynamic>();
                result = db.Sql(sqlA).QueryMany<dynamic>();

                //if ((!string.IsNullOrEmpty(workGroup)) && (!workGroup.Equals("-1")))
                //{
                //    List<string> userCodes = db.Sql(string.Format(@"SELECT UserCode FROM dbo.SYS_WorkGroupDetail WHERE IsEnable = 1 AND MainID = (SELECT ID FROM dbo.SYS_WorkGroup WHERE IsEnable = 1 AND TeamCode = '{0}')", workGroup)).QueryMany<string>();
                //    listA = listA.Where(item => userCodes.Contains(item.OpreateCode)).ToList();
                //}

                //#region 根据生产计划时间查询
                ///*
                //if (beginDate == null && finishDate == null)
                //{

                //}
                //else
                //{
                //    List<string> ApsCodeList = new List<string>();

                //    if (beginDate == null && finishDate != null)
                //    {
                //        ApsCodeList = db.Sql(string.Format(@"SELECT ApsCode FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PlanedFinishTime < '{0}'", Convert.ToDateTime(finishDate).ToString("yyyy-MM-dd 23:59:59"))).QueryMany<string>();
                //    }
                //    else if (beginDate != null && finishDate == null)
                //    {
                //        ApsCodeList = db.Sql(string.Format(@"SELECT ApsCode FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PlanedStartTime > '{0}'", Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd 00:00:00"))).QueryMany<string>();
                //    }
                //    else
                //    {
                //        ApsCodeList = db.Sql(string.Format(@"SELECT ApsCode FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PlanedStartTime > '{0}' AND PlanedFinishTime < '{1}'", Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd 00:00:00"), Convert.ToDateTime(finishDate).ToString("yyyy-MM-dd 23:59:59"))).QueryMany<string>();
                //    }

                //    listA = listA.Where(item => ApsCodeList.Contains(item.APSCode)).ToList();
                //}
                //*/
                //#endregion

                //#region

                //if (beginDate == null && finishDate == null)
                //{

                //}
                //else
                //{
                //    if (beginDate == null && finishDate != null)
                //    {
                //        listA = listA.Where(item => item.FinishTime < finishDate).ToList();
                //    }
                //    else if (beginDate != null && finishDate == null)
                //    {
                //        listA = listA.Where(item => item.FinishTime > beginDate).ToList();
                //    }
                //    else
                //    {
                //        listA = listA.Where(item =>
                //        {
                //            bool a = item.FinishTime > beginDate;
                //            bool b = item.FinishTime < finishDate;
                //            return a && b;
                //        }).ToList();
                //    }
                //}

                //#endregion

                //string sqlB = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1");
                //var listB = db.Sql(sqlB).QueryMany<APS_ProjectProduceDetial>();

                ////工艺
                //string sqlD = string.Format(@"SELECT * FROM MES_BN_ProductProcessRoute WHERE IsEnable = 1");
                //var listD = db.Sql(sqlD).QueryMany<MES_BN_ProductProcessRoute>();
                ////string sqlE = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteDetail WHERE IsEnable = 1");
                ////var listE = db.Sql(sqlE).QueryMany<MES_BN_ProductProcessRouteDetail>();
                ////string sqlF = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteEquipment WHERE IsEnable = 1");
                ////var listF = db.Sql(sqlF).QueryMany<MES_BN_ProductProcessRouteEquipment>();

                //foreach (var item in listA)
                //{
                //    //生产计划
                //    var projectProduceDetial = listB.FirstOrDefault(s =>
                //    {
                //        bool a = s.ApsCode == null ? false : true;
                //        bool b = s.ApsCode.Equals(item.APSCode);
                //        return a && b;
                //    });

                //    //该计划对应的工艺
                //    var productProcessRoute = listD.FirstOrDefault(s =>
                //    {
                //        bool a = s.ContractCode == null ? false : s.ContractCode.Equals(projectProduceDetial.ContractCode);
                //        bool b = s.PartCode == null ? false : s.PartCode.Equals(projectProduceDetial.PartCode);
                //        bool c = s.ProcessCode == null ? false : s.ProcessCode.Equals(projectProduceDetial.ProcessCode);
                //        bool d = s.ProcessLineCode == null ? false : s.ProcessLineCode.Equals(projectProduceDetial.ProcessLineCode);
                //        return a && b && c && d;
                //    });

                //    result.Add(new
                //    {
                //        ID = item.ID,
                //        APSCode = item.APSCode,
                //        BegainTime = item.BegainTime == null ? "" : Convert.ToDateTime(item.BegainTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                //        FinishTime = item.FinishTime == null ? "" : Convert.ToDateTime(item.FinishTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                //        WorkingHour = item.WorkingHour,
                //        OpreateCode = item.OpreateCode,
                //        OperatePerson = item.OperatePerson,
                //        IsEnable = item.IsEnable,
                //        CreatePerson = item.CreatePerson,
                //        CreateTime = item.CreateTime == null ? "" : Convert.ToDateTime(item.CreateTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                //        ModifyPerson = item.ModifyPerson,
                //        ModifyTime = item.ModifyTime == null ? "" : Convert.ToDateTime(item.ModifyTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                //        PauseType = item.PauseType,
                //        FinishQuantity = Convert.ToInt32(item.FinishQuantity),
                //        AuditQuantity = item.AuditQuantity == null ? 0 : Convert.ToInt32(item.AuditQuantity),
                //        //AuditQuantity = Convert.ToInt32(item.FinishQuantity),
                //        AuditState = item.AuditState,
                //        IsInspectionReport = productProcessRoute == null ? -1 : (productProcessRoute.IsInspectionReport ?? 0)
                //    });
                //}

                return result;
            }
        }

        //public ResultModel InspectionReport(string apsCode, int auditNum = 0)
        //{
        //    try
        //    {
        //        //日志
        //        string sqlA = string.Format(@"SELECT * FROM [MES_OpreatorWorkingLog] WHERE IsEnable = 1 AND APSCode = '{0}'", apsCode);
        //        var opreatorWorkingLog = db.Sql(sqlA).QuerySingle<MES_OpreatorWorkingLog>();
        //        if (opreatorWorkingLog == null)
        //        {
        //            throw new Exception(@"查无生产日志！");
        //        }

        //        //计划
        //        string sqlB = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND ApsCode = '{0}'", apsCode);
        //        var projectProduceDetial = db.Sql(sqlB).QuerySingle<APS_ProjectProduceDetial>();
        //        if (projectProduceDetial == null)
        //        {
        //            throw new Exception(@"查无生产计划！");
        //        }

        //        //工艺
        //        string sqlC = string.Format(@"SELECT * FROM MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND ContractCode = '{0}' AND PartCode = '{1}' AND ProcessCode = '{2}' AND ProcessLineCode = '{3}'",
        //          projectProduceDetial.ContractCode, projectProduceDetial.PartCode, projectProduceDetial.ProcessCode, projectProduceDetial.ProcessLineCode);
        //        var productProcessRoute = db.Sql(sqlC).QuerySingle<MES_BN_ProductProcessRoute>();
        //        if (productProcessRoute == null)
        //        {
        //            throw new Exception(@"查无生产工艺！");
        //        }
        //        if (productProcessRoute.IsInspectionReport == null)
        //        {
        //            throw new Exception(@"该生产计划报检不明确！");
        //        }

        //        //产品
        //        string sqlD = string.Format(@"SELECT * FROM PMS_BN_ProjectDetail WHERE IsEnable = 1 AND ID = {0}", projectProduceDetial.ProjectDetailID);
        //        var projectDetail = db.Sql(sqlD).QuerySingle<PMS_BN_ProjectDetail>();
        //        if (projectDetail == null)
        //        {
        //            throw new Exception(@"查无产品信息！");
        //        }

        //        //项目
        //        string sqlH = string.Format(@"SELECT * FROM dbo.PMS_BN_Project WHERE IsEnable = 1 AND ProjectID = {0}", projectDetail.MainID);
        //        var project = db.Sql(sqlH).QuerySingle<PMS_BN_Project>();
        //        if (project == null)
        //        {
        //            throw new Exception(@"查无项目信息！");
        //        }

        //        //零件
        //        string sqlE = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND PartCode = '{0}'", projectProduceDetial.PartCode);
        //        var part = db.Sql(sqlE).QuerySingle<SYS_Part>();
        //        if (part == null)
        //        {
        //            return new ResultModel()
        //            {
        //                Result = false,
        //                Msg = @"查无零件信息！"
        //            };
        //        }

        //        //图纸
        //        string sqlF = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND PartCode = '{0}'", projectProduceDetial.PartCode);
        //        var processBOM = db.Sql(sqlF).QuerySingle<PRS_Process_BOM>();
        //        if (processBOM == null)
        //        {
        //            throw new Exception(@"查无图纸信息！");
        //        }

        //        //班组
        //        string sqlG = string.Format(@"SELECT * FROM dbo.SYS_WorkGroup WHERE IsEnable = 1 AND dbo.SYS_WorkGroup.TeamCode='{0}'", projectProduceDetial.WorkGroupID);
        //        var workGroup = db.Sql(sqlG).QuerySingle<SYS_WorkGroup>();
        //        if (workGroup == null)
        //        {
        //            throw new Exception(@"查无班组信息！");
        //        }

        //        //仓库
        //        string sqlI = string.Format(@"SELECT * FROM dbo.SYS_BN_Warehouse WHERE IsEnable = 1 AND WarehouseName = '{0}' ", workGroup.DepartName);
        //        var warehouse = db.Sql(sqlI).QuerySingle<SYS_BN_Warehouse>();
        //        if (warehouse == null)
        //        {
        //            throw new Exception(@"查无仓库信息！");
        //        }

        //        //开启事务
        //        db.UseTransaction(true);

        //        DateTime dTime = DateTime.Now;
        //        string documentNoA = MmsHelper.GetOrderNumber("MES_ProcessInspectionRequest", "BillCode", "GCBJ", "", "");
        //        string documentNoB = MmsHelper.GetOrderNumber("QMS_ProcessInspection", "BillCode", "GCJY", "", "");
        //        string documentNoC = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "ZXRK", "", "");

        //        //更新日志表
        //        string sqlUpdateLog = string.Format(@"update MES_OpreatorWorkingLog set AuditState = 1,AuditQuantity = {0} where IsEnable = 1 and PauseType = 2 and APSCode = '{1}' ", auditNum, apsCode);
        //        db.Sql(sqlUpdateLog).Execute();

        //        //不需要报检
        //        if (productProcessRoute.IsInspectionReport.Equals(1) || productProcessRoute.IsInspectionReport.Equals(2))
        //        {
        //            //查转序目标
        //            var turnTarget = new WinFormClientService().GetTurnTarget(projectProduceDetial.ID);
        //            if (!turnTarget.Result)
        //            {
        //                throw new Exception(turnTarget.Msg);
        //            }

        //            WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
        //            {
        //                ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
        //                BillCode = documentNoC,
        //                BillType = 8,
        //                IsEnable = 1,
        //                ContractCode = projectProduceDetial.ContractCode,
        //                ProjectName = project.ProjectName,
        //                WarehouseCode = turnTarget.Data.ID,
        //                WarehouseName = turnTarget.Data.Name,
        //                ApproveState = turnTarget.Data.ID == warehouse.WarehouseCode ? "2" : "1",
        //                ApprovePerson = MmsHelper.GetUserName(),
        //                ApproveDate = dTime,
        //                CreatePerson = MmsHelper.GetUserName(),
        //                CreateTime = dTime,
        //                ModifyPerson = MmsHelper.GetUserName(),
        //                ModifyTime = dTime
        //            };

        //            WMS_BN_BillDetail bDetailModel = new WMS_BN_BillDetail()
        //            {
        //                BillCode = documentNoC,
        //                IsEnable = 1,
        //                InventoryCode = part.InventoryCode,
        //                InventoryName = part.InventoryName,
        //                Specs = part.Model,
        //                Unit = part.QuantityUnit,
        //                MateNum = auditNum,
        //                ConfirmNum = auditNum,
        //                CreatePerson = MmsHelper.GetUserName(),
        //                CreateTime = dTime,
        //                ModifyPerson = MmsHelper.GetUserName(),
        //                ModifyTime = dTime,
        //                BatchCode = "NewBatchCode",
        //                PBillCode = apsCode
        //            };

        //            //出入库主表
        //            string sqlBMain = WinFormClientService.GetInsertSQL(bMainModel);
        //            db.Sql(sqlBMain).Execute();
        //            //出入库明细表
        //            string sqlBDetail = WinFormClientService.GetInsertSQL(bDetailModel);
        //            db.Sql(sqlBDetail).Execute();

        //            string sqlJ = string.Format(@"SELECT * FROM WMS_BN_RealStock WHERE IsEnable = 1 AND WarehouseCode = '{0}' AND InventoryCode = '{1}' AND BatchCode = '{2}'", bMainModel.WarehouseCode, bDetailModel.InventoryCode, bDetailModel.BatchCode);
        //            var realStocks = db.Sql(sqlJ).QueryMany<WMS_BN_RealStock>();

        //            if (bMainModel.ApproveState.Equals("1"))
        //            {
        //                //不作操作
        //            }
        //            else if (bMainModel.ApproveState.Equals("2"))
        //            {
        //                //没有库存则新增
        //                if (realStocks.Count <= 0)
        //                {
        //                    WMS_BN_RealStock realStockModel = new WMS_BN_RealStock()
        //                    {
        //                        InventoryCode = bDetailModel.InventoryCode,
        //                        InventoryName = bDetailModel.InventoryName,
        //                        Model = bDetailModel.Specs,
        //                        RealStock = bDetailModel.ConfirmNum,
        //                        TotalStock = bDetailModel.ConfirmNum,
        //                        WarehouseCode = bMainModel.WarehouseCode,
        //                        WarehouseName = bMainModel.WarehouseName,
        //                        BatchCode = bDetailModel.BatchCode,
        //                        Unit = bDetailModel.Unit,
        //                        CreatePerson = bMainModel.CreatePerson,
        //                        CreateTime = bMainModel.CreateTime,
        //                        ModifyPerson = bMainModel.CreatePerson,
        //                        ModifyTime = bMainModel.CreateTime,
        //                        IsEnable = 1
        //                    };

        //                    //库存表
        //                    string sqlRealStock = WinFormClientService.GetInsertSQL(realStockModel);
        //                    db.Sql(sqlRealStock).Execute();
        //                }
        //                //有则更新数量
        //                else if (realStocks.Count.Equals(1))
        //                {
        //                    //库存表
        //                    string sqlRealStock = WinFormClientService.GetUpdateSQL(nameof(WMS_BN_RealStock), new KeyValuePair<string, object>("ID", realStocks[0].ID), new
        //                    {
        //                        RealStock = realStocks[0].RealStock + auditNum,
        //                        TotalStock = realStocks[0].TotalStock + auditNum,
        //                        ModifyPerson = bDetailModel.ModifyPerson,
        //                        ModifyTime = dTime
        //                    });
        //                    db.Sql(sqlRealStock).Execute();
        //                }
        //                else
        //                {
        //                    throw new Exception(@"仓库物料种类繁多，可惜不知取哪一种！");
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception(@"ApproveState参数错误！");
        //            }
        //        }
        //        //需要报检
        //        else
        //        {
        //            MES_ProcessInspectionRequest pirModel = new MES_ProcessInspectionRequest()
        //            {
        //                BillCode = documentNoA,
        //                ContractCode = projectProduceDetial.ContractCode,
        //                ProjectName = productProcessRoute.ProjectName,
        //                ProductName = projectDetail.ProductName,
        //                ProductFigureNumber = projectDetail.FigureNumber,
        //                PartCode = projectProduceDetial.PartCode,
        //                PartName = part.PartName,
        //                partFigure = processBOM.PartFigureCode,
        //                MaterialCode = "",
        //                CheckQuantity = auditNum,
        //                BillState = 2,
        //                IsEnable = 1,
        //                CreatePerson = MmsHelper.GetUserName(),
        //                CreateTime = dTime,
        //                ModifyPerson = MmsHelper.GetUserName(),
        //                ModifyTime = dTime,
        //                ProductPlanCode = projectProduceDetial.ProductPlanCode,
        //                OperatorCode = MmsHelper.GetUserCode(),
        //                DepartmentCode = workGroup.TeamCode,
        //                DepartmentName = workGroup.TeamName,
        //                Unit = projectProduceDetial.Unit == null ? null : projectProduceDetial.Unit.ToString(),
        //                EquipmentCode = projectProduceDetial.EquipmentID,
        //                EquipmentName = projectProduceDetial.EquipmentName,
        //                ProductProcessRouteID = productProcessRoute.ID
        //            };

        //            QMS_ProcessInspection piModel = new QMS_ProcessInspection()
        //            {
        //                ID = db.Sql(@"SELECT ISNULL(MAX(ID),1)+1 FROM dbo.QMS_ProcessInspection").QuerySingle<int>(),
        //                BillCode = documentNoB,
        //                ContractCode = projectProduceDetial.ContractCode,
        //                ProjectName = productProcessRoute.ProjectName,
        //                ProductName = projectDetail.ProductName,
        //                ProductFigureNumber = projectDetail.FigureNumber,
        //                PartCode = projectProduceDetial.PartCode,
        //                PartName = part.PartName,
        //                partFigure = processBOM.PartFigureCode,
        //                MaterialCode = "",
        //                CheckQuantity = auditNum,
        //                QualifiedQuntity = 0,
        //                IsQualified = "1",
        //                BillState = 1,
        //                IsEnable = 1,
        //                CreatePerson = MmsHelper.GetUserName(),
        //                CreateTime = dTime,
        //                ModifyPerson = MmsHelper.GetUserName(),
        //                ModifyTime = dTime,
        //                ProductPlanCode = projectProduceDetial.ProductPlanCode,
        //                OperatorCode = MmsHelper.GetUserCode(),
        //                DepartmentCode = workGroup.TeamCode,
        //                DepartmentName = workGroup.TeamName,
        //                PBillCode = documentNoA,
        //                Unit = projectProduceDetial.Unit == null ? null : projectProduceDetial.Unit.ToString(),
        //                EquipmentCode = projectProduceDetial.EquipmentID,
        //                EquipmentName = projectProduceDetial.EquipmentName
        //            };

        //            //报检申请表
        //            string sqlPIR = WinFormClientService.GetInsertSQL(pirModel);
        //            db.Sql(sqlPIR).Execute();

        //            //质检表
        //            string sqlPI = WinFormClientService.GetInsertSQL(piModel);
        //            db.Sql(sqlPI).Execute();
        //        }

        //        db.Commit();

        //        return new ResultModel()
        //        {
        //            Result = true,
        //            Msg = @"审核成功！"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        db.Rollback();

        //        return new ResultModel()
        //        {
        //            Result = false,
        //            Msg = ex.Message
        //        };
        //    }
        //}

        public ResultModel InspectionReport(string workTicketCode, int auditNum = 0)
        {
            try
            {
                string apsCode = "";
                //工票
                string sql1 = string.Format("select * from MES_WorkingTicket where WorkTicketCode='{0}'", workTicketCode);
                var workTicketList = db.Sql(sql1).QueryMany<MES_WorkingTicket>();
                if (workTicketList.Count == 0)
                {
                    throw new Exception(@"查无工票！");
                }
                //获取工票表中的计划编号
                apsCode = workTicketList.FirstOrDefault().ApsCode;
                int WorkStepsLineCode = workTicketList.FirstOrDefault().WorkStepsLineCode ?? 0;//工步行号
                var WorkshopCode = workTicketList.FirstOrDefault().WorkshopCode;//当前所在车间
                var TeamCode = workTicketList.FirstOrDefault().TeamCode;//当前班组
                //var TurnTargetCode = workTicketList.FirstOrDefault().TurnTargetCode;//转序目标车间

                //日志
                string sqlA = string.Format(@"SELECT * FROM [MES_OpreatorWorkingLog] WHERE IsEnable = 1 AND APSCode = '{0}'", workTicketCode);
                var opreatorWorkingLog = db.Sql(sqlA).QuerySingle<MES_OpreatorWorkingLog>();
                if (opreatorWorkingLog == null)
                {
                    throw new Exception(@"查无生产日志！");
                }

                //计划
                string sqlB = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND ApsCode = '{0}'", apsCode);
                var projectProduceDetial = db.Sql(sqlB).QuerySingle<APS_ProjectProduceDetial>();
                if (projectProduceDetial == null)
                {
                    throw new Exception(@"查无生产计划！");
                }

                //工步编号
                string sql2 = string.Format(@"
SELECT MAX(WorkStepsLineCode) WorkStepsLineCode FROM PRS_ProcessWorkSteps WHERE ProcessRouteID IN
(SELECT ID FROM dbo.MES_BN_ProductProcessRoute WHERE ProcessCode='{0}' AND ContractCode='{1}')", projectProduceDetial.ProcessCode, projectProduceDetial.ContractCode);
                var WorkStepID = db.Sql(sql2).QuerySingle<int>();
                if (WorkStepsLineCode != WorkStepID)
                {
                    throw new Exception(@"该工票中的工步不是当前工序的最后一步，无需完工！");
                }

                //工艺
                string sqlC = string.Format(@"SELECT * FROM MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND ContractCode = '{0}' AND PartCode = '{1}' AND ProcessCode = '{2}' AND ProcessLineCode = '{3}'",
                  projectProduceDetial.ContractCode, projectProduceDetial.PartCode, projectProduceDetial.ProcessCode, projectProduceDetial.ProcessLineCode);
                var productProcessRoute = db.Sql(sqlC).QuerySingle<MES_BN_ProductProcessRoute>();
                if (productProcessRoute == null)
                {
                    throw new Exception(@"查无生产工艺！");
                }
                if (productProcessRoute.IsInspectionReport == null)
                {
                    throw new Exception(@"该生产计划报检不明确！");
                }

                //产品
                string sqlD = string.Format(@"SELECT * FROM PMS_BN_ProjectDetail WHERE IsEnable = 1 AND ID = {0}", projectProduceDetial.ProjectDetailID);
                var projectDetail = db.Sql(sqlD).QuerySingle<PMS_BN_ProjectDetail>();
                if (projectDetail == null)
                {
                    throw new Exception(@"查无产品信息！");
                }

                //项目
                string sqlH = string.Format(@"SELECT * FROM dbo.PMS_BN_Project WHERE IsEnable = 1 AND ProjectID = {0}", projectDetail.MainID);
                var project = db.Sql(sqlH).QuerySingle<PMS_BN_Project>();
                if (project == null)
                {
                    throw new Exception(@"查无项目信息！");
                }

                //零件
                string sqlE = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND PartCode = '{0}'", projectProduceDetial.PartCode);
                var part = db.Sql(sqlE).QuerySingle<SYS_Part>();
                if (part == null)
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"查无零件信息！"
                    };
                }

                //图纸
                string sqlF = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND PartCode = '{0}'", projectProduceDetial.PartCode);
                var processBOM = db.Sql(sqlF).QuerySingle<PRS_Process_BOM>();
                if (processBOM == null)
                {
                    throw new Exception(@"查无图纸信息！");
                }

                //班组
                string sqlG = string.Format(@"SELECT * FROM dbo.SYS_WorkGroup WHERE IsEnable = 1 AND dbo.SYS_WorkGroup.TeamCode='{0}'", TeamCode);
                var workGroup = db.Sql(sqlG).QuerySingle<SYS_WorkGroup>();
                if (workGroup == null && string.IsNullOrEmpty(TeamCode))
                {
                    throw new Exception(@"查无班组信息！");
                }

                //仓库
                string sqlI = string.Format(@"SELECT * FROM dbo.SYS_BN_Warehouse WHERE IsEnable = 1 AND WarehouseName = '{0}' ", workGroup.DepartName);
                var warehouse = db.Sql(sqlI).QuerySingle<SYS_BN_Warehouse>();
                if (warehouse == null)
                {
                    throw new Exception(@"查无仓库信息！");
                }

                //开启事务
                db.UseTransaction(true);

                DateTime dTime = DateTime.Now;
                string documentNoA = MmsHelper.GetOrderNumber("MES_ProcessInspectionRequest", "BillCode", "GCBJ", "", "");
                string documentNoB = MmsHelper.GetOrderNumber("QMS_ProcessInspection", "BillCode", "GCJY", "", "");
                string documentNoC = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "ZXRK", "", "");

                //更新日志表
                string sqlUpdateLog = string.Format(@"update MES_OpreatorWorkingLog set AuditState = 1,AuditQuantity = {0} where IsEnable = 1 and PauseType = 2 and APSCode = '{1}' ", auditNum, apsCode);
                db.Sql(sqlUpdateLog).Execute();

                //不需要报检
                if (productProcessRoute.IsInspectionReport.Equals(1))//不报检
                {
                    //查转序目标
                    var turnTarget = new WinFormClientService().GetTurnTarget(projectProduceDetial.ID);
                    if (!turnTarget.Result)
                    {
                        throw new Exception(turnTarget.Msg);
                    }
                    string turnTargetCode = turnTarget.Data.ID;
                    if (turnTargetCode != WorkshopCode)
                    {
                        WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
                        {
                            ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
                            BillCode = documentNoC,
                            BillType = 8,
                            IsEnable = 1,
                            ContractCode = projectProduceDetial.ContractCode,
                            ProjectName = project.ProjectName,
                            WarehouseCode = turnTarget.Data.ID,
                            WarehouseName = turnTarget.Data.Name,
                            ApproveState = turnTarget.Data.ID == warehouse.WarehouseCode ? "2" : "1",
                            ApprovePerson = MmsHelper.GetUserName(),
                            ApproveDate = dTime,
                            CreatePerson = MmsHelper.GetUserName(),
                            CreateTime = dTime,
                            ModifyPerson = MmsHelper.GetUserName(),
                            ModifyTime = dTime
                        };

                        WMS_BN_BillDetail bDetailModel = new WMS_BN_BillDetail()
                        {
                            BillCode = documentNoC,
                            IsEnable = 1,
                            InventoryCode = part.InventoryCode,
                            InventoryName = part.InventoryName,
                            Specs = part.Model,
                            Unit = part.QuantityUnit,
                            MateNum = auditNum,
                            ConfirmNum = auditNum,
                            CreatePerson = MmsHelper.GetUserName(),
                            CreateTime = dTime,
                            ModifyPerson = MmsHelper.GetUserName(),
                            ModifyTime = dTime,
                            BatchCode = "NewBatchCode",
                            PBillCode = apsCode
                        };

                        //出入库主表
                        string sqlBMain = WinFormClientService.GetInsertSQL(bMainModel);
                        db.Sql(sqlBMain).Execute();
                        //出入库明细表
                        string sqlBDetail = WinFormClientService.GetInsertSQL(bDetailModel);
                        db.Sql(sqlBDetail).Execute();

                        string sqlJ = string.Format(@"SELECT * FROM WMS_BN_RealStock WHERE IsEnable = 1 AND WarehouseCode = '{0}' AND InventoryCode = '{1}' AND BatchCode = '{2}'", bMainModel.WarehouseCode, bDetailModel.InventoryCode, bDetailModel.BatchCode);
                        var realStocks = db.Sql(sqlJ).QueryMany<WMS_BN_RealStock>();

                        if (bMainModel.ApproveState.Equals("1"))
                        {
                            //不作操作
                        }
                        else if (bMainModel.ApproveState.Equals("2"))
                        {
                            //没有库存则新增
                            if (realStocks.Count <= 0)
                            {
                                WMS_BN_RealStock realStockModel = new WMS_BN_RealStock()
                                {
                                    InventoryCode = bDetailModel.InventoryCode,
                                    InventoryName = bDetailModel.InventoryName,
                                    Model = bDetailModel.Specs,
                                    RealStock = bDetailModel.ConfirmNum,
                                    TotalStock = bDetailModel.ConfirmNum,
                                    WarehouseCode = bMainModel.WarehouseCode,
                                    WarehouseName = bMainModel.WarehouseName,
                                    BatchCode = bDetailModel.BatchCode,
                                    Unit = bDetailModel.Unit,
                                    CreatePerson = bMainModel.CreatePerson,
                                    CreateTime = bMainModel.CreateTime,
                                    ModifyPerson = bMainModel.CreatePerson,
                                    ModifyTime = bMainModel.CreateTime,
                                    IsEnable = 1
                                };

                                //库存表
                                string sqlRealStock = WinFormClientService.GetInsertSQL(realStockModel);
                                db.Sql(sqlRealStock).Execute();
                            }
                            //有则更新数量
                            else if (realStocks.Count.Equals(1))
                            {
                                //库存表
                                string sqlRealStock = WinFormClientService.GetUpdateSQL(nameof(WMS_BN_RealStock), new KeyValuePair<string, object>("ID", realStocks[0].ID), new
                                {
                                    RealStock = realStocks[0].RealStock + auditNum,
                                    TotalStock = realStocks[0].TotalStock + auditNum,
                                    ModifyPerson = bDetailModel.ModifyPerson,
                                    ModifyTime = dTime
                                });
                                db.Sql(sqlRealStock).Execute();
                            }
                            else
                            {
                                throw new Exception(@"仓库物料种类繁多，可惜不知取哪一种！");
                            }
                        }
                        else
                        {
                            throw new Exception(@"ApproveState参数错误！");
                        }
                    }
                }
                //需要报检
                else
                {
                    MES_ProcessInspectionRequest pirModel = new MES_ProcessInspectionRequest()
                    {
                        BillCode = documentNoA,
                        ContractCode = projectProduceDetial.ContractCode,
                        ProjectName = productProcessRoute.ProjectName,
                        ProductName = projectDetail.ProductName,
                        ProductFigureNumber = projectDetail.FigureNumber,
                        PartCode = projectProduceDetial.PartCode,
                        PartName = part.PartName,
                        partFigure = processBOM.PartFigureCode,
                        MaterialCode = "",
                        CheckQuantity = auditNum,
                        BillState = 2,
                        IsEnable = 1,
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = dTime,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = dTime,
                        ProductPlanCode = projectProduceDetial.ProductPlanCode,
                        OperatorCode = MmsHelper.GetUserCode(),
                        DepartmentCode = workGroup.TeamCode,
                        DepartmentName = workGroup.TeamName,
                        Unit = projectProduceDetial.Unit == null ? null : projectProduceDetial.Unit.ToString(),
                        EquipmentCode = projectProduceDetial.EquipmentID,
                        EquipmentName = projectProduceDetial.EquipmentName,
                        ProductProcessRouteID = productProcessRoute.ID
                    };

                    QMS_ProcessInspection piModel = new QMS_ProcessInspection()
                    {
                        ID = db.Sql(@"SELECT ISNULL(MAX(ID),1)+1 FROM dbo.QMS_ProcessInspection").QuerySingle<int>(),
                        BillCode = documentNoB,
                        ContractCode = projectProduceDetial.ContractCode,
                        ProjectName = productProcessRoute.ProjectName,
                        ProductName = projectDetail.ProductName,
                        ProductFigureNumber = projectDetail.FigureNumber,
                        PartCode = projectProduceDetial.PartCode,
                        PartName = part.PartName,
                        partFigure = processBOM.PartFigureCode,
                        MaterialCode = "",
                        CheckQuantity = auditNum,
                        QualifiedQuntity = 0,
                        IsQualified = "1",
                        BillState = 1,
                        IsEnable = 1,
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = dTime,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = dTime,
                        ProductPlanCode = projectProduceDetial.ProductPlanCode,
                        OperatorCode = MmsHelper.GetUserCode(),
                        DepartmentCode = workGroup.TeamCode,
                        DepartmentName = workGroup.TeamName,
                        PBillCode = documentNoA,
                        Unit = projectProduceDetial.Unit == null ? null : projectProduceDetial.Unit.ToString(),
                        EquipmentCode = projectProduceDetial.EquipmentID,
                        EquipmentName = projectProduceDetial.EquipmentName,
                        OuterFactoryBatch = apsCode
                    };

                    //创建报检申请表
                    string sqlPIR = WinFormClientService.GetInsertSQL(pirModel);
                    db.Sql(sqlPIR).Execute();

                    //创建质检表
                    string sqlPI = WinFormClientService.GetInsertSQL(piModel);
                    db.Sql(sqlPI).Execute();

                    if (productProcessRoute.IsInspectionReport.Equals(2))//检验记录,判断生产车间和转序目标一致则，生成转序单
                    {
                        //查转序目标
                        var turnTarget = new WinFormClientService().GetTurnTarget(projectProduceDetial.ID);
                        if (!turnTarget.Result)
                        {
                            throw new Exception(turnTarget.Msg);
                        }
                        string turnTargetCode = turnTarget.Data.ID;
                        if (turnTargetCode != WorkshopCode)
                        {
                            WMS_BN_BillMain bMainModel = new WMS_BN_BillMain()
                            {
                                ID = Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")),
                                BillCode = documentNoC,
                                BillType = 8,
                                IsEnable = 1,
                                ContractCode = projectProduceDetial.ContractCode,
                                ProjectName = project.ProjectName,
                                WarehouseCode = turnTarget.Data.ID,
                                WarehouseName = turnTarget.Data.Name,
                                ApproveState = turnTarget.Data.ID == warehouse.WarehouseCode ? "2" : "1",
                                ApprovePerson = MmsHelper.GetUserName(),
                                ApproveDate = dTime,
                                CreatePerson = MmsHelper.GetUserName(),
                                CreateTime = dTime,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = dTime
                            };

                            WMS_BN_BillDetail bDetailModel = new WMS_BN_BillDetail()
                            {
                                BillCode = documentNoC,
                                IsEnable = 1,
                                InventoryCode = part.InventoryCode,
                                InventoryName = part.InventoryName,
                                Specs = part.Model,
                                Unit = part.QuantityUnit,
                                MateNum = auditNum,
                                ConfirmNum = auditNum,
                                CreatePerson = MmsHelper.GetUserName(),
                                CreateTime = dTime,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = dTime,
                                BatchCode = "NewBatchCode",
                                PBillCode = apsCode
                            };

                            //出入库主表
                            string sqlBMain = WinFormClientService.GetInsertSQL(bMainModel);
                            db.Sql(sqlBMain).Execute();
                            //出入库明细表
                            string sqlBDetail = WinFormClientService.GetInsertSQL(bDetailModel);
                            db.Sql(sqlBDetail).Execute();

                            string sqlJ = string.Format(@"SELECT * FROM WMS_BN_RealStock WHERE IsEnable = 1 AND WarehouseCode = '{0}' AND InventoryCode = '{1}' AND BatchCode = '{2}'", bMainModel.WarehouseCode, bDetailModel.InventoryCode, bDetailModel.BatchCode);
                            var realStocks = db.Sql(sqlJ).QueryMany<WMS_BN_RealStock>();

                            if (bMainModel.ApproveState.Equals("1"))
                            {
                                //不作操作
                            }
                            else if (bMainModel.ApproveState.Equals("2"))
                            {
                                //没有库存则新增
                                if (realStocks.Count <= 0)
                                {
                                    WMS_BN_RealStock realStockModel = new WMS_BN_RealStock()
                                    {
                                        InventoryCode = bDetailModel.InventoryCode,
                                        InventoryName = bDetailModel.InventoryName,
                                        Model = bDetailModel.Specs,
                                        RealStock = bDetailModel.ConfirmNum,
                                        TotalStock = bDetailModel.ConfirmNum,
                                        WarehouseCode = bMainModel.WarehouseCode,
                                        WarehouseName = bMainModel.WarehouseName,
                                        BatchCode = bDetailModel.BatchCode,
                                        Unit = bDetailModel.Unit,
                                        CreatePerson = bMainModel.CreatePerson,
                                        CreateTime = bMainModel.CreateTime,
                                        ModifyPerson = bMainModel.CreatePerson,
                                        ModifyTime = bMainModel.CreateTime,
                                        IsEnable = 1
                                    };

                                    //库存表
                                    string sqlRealStock = WinFormClientService.GetInsertSQL(realStockModel);
                                    db.Sql(sqlRealStock).Execute();
                                }
                                //有则更新数量
                                else if (realStocks.Count.Equals(1))
                                {
                                    //库存表
                                    string sqlRealStock = WinFormClientService.GetUpdateSQL(nameof(WMS_BN_RealStock), new KeyValuePair<string, object>("ID", realStocks[0].ID), new
                                    {
                                        RealStock = realStocks[0].RealStock + auditNum,
                                        TotalStock = realStocks[0].TotalStock + auditNum,
                                        ModifyPerson = bDetailModel.ModifyPerson,
                                        ModifyTime = dTime
                                    });
                                    db.Sql(sqlRealStock).Execute();
                                }
                                else
                                {
                                    throw new Exception(@"仓库物料种类繁多，可惜不知取哪一种！");
                                }
                            }
                            else
                            {
                                throw new Exception(@"ApproveState参数错误！");
                            }
                        }
                    }
                }

                db.Commit();

                return new ResultModel()
                {
                    Result = true,
                    Msg = @"审核成功！"
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
    }

    public class MES_OpreatorWorkingLog : ModelBase
    {
        [Identity]
        [PrimaryKey]

        /// <summary>
		/// 主键ID
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 生产任务编码
        /// </summary>
        public string APSCode { get; set; }

        /// <summary>
        /// 加工开始时间
        /// </summary>
        public DateTime? BegainTime { get; set; }

        /// <summary>
        /// 加工结束时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 加工时间
        /// </summary>
        public int? WorkingHour { get; set; }

        /// <summary>
        /// 操作工编码
        /// </summary>
        public string OpreateCode { get; set; }

        /// <summary>
        /// 操作工
        /// </summary>
        public string OperatePerson { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 暂停类型 1：暂停 2：完工
        /// </summary>
        public string PauseType { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal? FinishQuantity { get; set; }

        /// <summary>
        /// 审核数量
        /// </summary>
        public decimal? AuditQuantity { get; set; }

        /// <summary>
        /// 审核状态 0:未审核  1:已审核
        /// </summary>
        public int AuditState { get; set; }
        public int? PauseReson { get; set; }
    }
}