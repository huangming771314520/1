using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Zephyr.Areas;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WinFormClientService : ServiceBase<WinFormClient>
    {
        //**********************************************************************************************************//
        #region 查数据

        /// <summary>
        /// 根据用户/员工编号获取该班组关联的所有信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public dynamic GetWorkGroupInfoByUCode(string barCode)
        {
            try
            {
                using (var db = Db.Context("Mms"))
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
                                                GetToken = token,
                                                GetWorkGroupDetail = listA[0],
                                                GetWorkGroup = listB[0],
                                                GetWarehouse = listC[0]
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
        /// 根据班组编号获取该班组生产计划关联的所有信息
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public dynamic GetProducePlanInfoByTCode(string teamCode)
        {
            try
            {
                //查字典表
                List<sys_code> sysCodeList = null;
                using (var db = Db.Context("Sys"))
                {
                    //var sqlC = string.Format(@"SELECT * FROM dbo.sys_code WHERE IsEnable = 1 AND CodeType = 'ProductType'");
                    var sqlSysCode = string.Format(@"SELECT * FROM dbo.sys_code WHERE IsEnable = 1 ");
                    sysCodeList = db.Sql(sqlSysCode).QueryMany<sys_code>();
                }
                sysCodeList = sysCodeList.Where(item => item.CodeType.Equals("ProductType")).ToList();

                using (var db = Db.Context("Mms"))
                {
                    List<dynamic> data = new List<dynamic>();

                    //查班组生产计划
                    string sqlA = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PlanState = 2 AND ActualFinishTime IS NULL AND WorkGroupID = '{0}' ORDER BY PlanedStartTime ", teamCode);
                    var listA = db.Sql(sqlA).QueryMany<APS_ProjectProduceDetial>();

                    if (listA.Count <= 0)
                    {
                        throw new Exception(@"该班组当前没有生产计划！");
                    }

                    //查产品名称、产品类型、型号
                    var sqlB = string.Format(@"SELECT * FROM PMS_BN_ProjectDetail WHERE IsEnable = 1");
                    var listB = db.Sql(sqlB).QueryMany<PMS_BN_ProjectDetail>();
                    var sqlC = string.Format(@"SELECT * FROM dbo.PMS_BN_Project WHERE IsEnable = 1");
                    var listC = db.Sql(sqlC).QueryMany<PMS_BN_Project>();

                    //工艺
                    string sqlD = string.Format(@"SELECT * FROM MES_BN_ProductProcessRoute WHERE IsEnable = 1");
                    var listD = db.Sql(sqlD).QueryMany<MES_BN_ProductProcessRoute>();
                    string sqlE = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteDetail WHERE IsEnable = 1");
                    var listE = db.Sql(sqlE).QueryMany<MES_BN_ProductProcessRouteDetail>();
                    string sqlF = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteEquipment WHERE IsEnable = 1");
                    var listF = db.Sql(sqlF).QueryMany<MES_BN_ProductProcessRouteEquipment>();

                    //图纸
                    //设计图纸
                    string sqlG = string.Format(@"SELECT * FROM [dbo].[PMS_BN_PartFile] tbA WHERE tbA.FileName = ( SELECT MAX(FileName) FROM [dbo].[PMS_BN_PartFile] WHERE IsEnable = 1 AND PartCode = tbA.PartCode );");
                    var listG = db.Sql(sqlG).QueryMany<PMS_BN_PartFile>();
                    //工序图纸
                    string sqlJ = string.Format(@"SELECT * FROM dbo.QMS_QualityReportDoc WHERE IsEnable = 1 AND FileType = 5");
                    var listJ = db.Sql(sqlJ).QueryMany<QMS_QualityReportDoc>();
                    //工艺图纸
                    string sqlL = string.Format(@"SELECT * FROM dbo.PRS_ProcessFigure WHERE IsEnable = 1");
                    var listL = db.Sql(sqlL).QueryMany<PRS_ProcessFigure>();

                    //图号
                    string sqlH = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1");
                    var listH = db.Sql(sqlH).QueryMany<PRS_Process_BOM>();

                    //工步
                    string sqlI = string.Format(@"SELECT * FROM PRS_ProcessWorkSteps WHERE IsEnable = 1 ORDER BY WorkStepsLineCode ASC");
                    var listI = db.Sql(sqlI).QueryMany<PRS_ProcessWorkSteps>();

                    foreach (var item in listA)
                    {
                        var projectDetailModel = listB.FirstOrDefault(s => s.ID.Equals(Convert.ToInt32(item.ProjectDetailID)));
                        var productProcessRoute = listD.FirstOrDefault(s =>
                        {
                            bool a = s.ContractCode == null ? false : s.ContractCode.Equals(item.ContractCode);
                            bool b = s.PartCode == null ? false : s.PartCode.Equals(item.PartCode);
                            bool c = s.ProcessCode == null ? false : s.ProcessCode.Equals(item.ProcessCode);
                            bool d = s.ProcessLineCode == null ? false : s.ProcessLineCode.Equals(item.ProcessLineCode);
                            return a && b && c && d;
                        });
                        if (productProcessRoute == null)
                        {
                            throw new Exception(@"生产计划未查得工艺！");
                        }

                        var listK = listD.Where(s => s.PartCode.Equals(productProcessRoute.PartCode) && s.ProcessLineCode > productProcessRoute.ProcessLineCode).OrderBy(i => i.ProcessLineCode).ToList();
                        if (listK.Count > 0)
                        {
                            productProcessRoute.WarehouseID = listK[0].WarehouseID;
                            productProcessRoute.WarehouseName = listK[0].WarehouseName;
                        }

                        var code = sysCodeList.FirstOrDefault(s => s.Value.Equals(projectDetailModel.ProductType.ToString()));
                        if (code == null)
                        {
                            throw new Exception(@"生产计划未查得生产类型！");
                        }

                        var project = listC.FirstOrDefault(s => s.ContractCode.Equals(item.ContractCode));
                        if (project == null)
                        {
                            throw new Exception(@"生产计划未查得项目信息！");
                        }

                        var productProcessRouteDetail = listE.FirstOrDefault(s => s.ProcessRouteID.Equals(productProcessRoute.ID));
                        if (project == null)
                        {
                            //throw new Exception(@"产计划未查得工艺明细！");
                        }

                        var productProcessRouteEquipment = listF.FirstOrDefault(s => s.ProcessRouteID.Equals(productProcessRoute.ID));
                        if (productProcessRouteEquipment == null)
                        {
                            //throw new Exception(@"生产计划未查得设备详情！");
                        }

                        var partFiles = listG.Where(s => s.PartCode.Equals(item.PartCode)).ToList();
                        var qualityReportDocs = listJ.Where(s => s.SourceID.Equals(productProcessRoute.ID.ToString())).ToList();
                        var processFigures = listL.Where(s =>
                        {
                            bool a = s.PartCode == null ? false : s.PartCode.Equals(productProcessRoute.PartCode);
                            bool b = s.ContractCode == null ? false : s.ContractCode.Equals(productProcessRoute.ContractCode);
                            bool c = s.ProductID == null ? false : s.ProductID.Equals(item.ProjectDetailID);
                            return a && b && c;
                        }).ToList();

                        var processBOM = listH.FirstOrDefault(s => s.PartCode.Equals(item.PartCode));
                        if (processBOM == null)
                        {
                            throw new Exception(@"生产计划未查得图号信息！");
                        }

                        var processWorkSteps = listI.Where(s =>
                        {
                            bool a = s.ProcessRouteID == null ? false : true;
                            bool b = s.ProcessRouteID.Equals(productProcessRoute.ID);
                            return a && b;
                        }).ToList();
                        if (processWorkSteps.Count <= 0)
                        {
                            //throw new Exception(@"生产计划未查得工步信息！");
                        }

                        data.Add(new
                        {
                            GetProject = project,
                            GetProjectDetail = projectDetailModel,
                            GetProjectProduceDetial = item,
                            GetProductProcessRoute = productProcessRoute,
                            GetProductProcessRouteDetail = productProcessRouteDetail,
                            GetProductProcessRouteEquipment = productProcessRouteEquipment,
                            GetCode = code,
                            GetPartFiles = partFiles,
                            GetQualityReportDocs = qualityReportDocs,
                            GetProcessFigures = processFigures,
                            GetProcessBOM = processBOM,
                            GetProcessWorkSteps = processWorkSteps
                        });
                    }

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


        public dynamic GetProducePlanInfoByTCode_2(string teamCode)
        {
            try
            {
                //查字典表
                List<sys_code> sysCodeList = null;
                using (var db = Db.Context("Sys"))
                {
                    //var sqlC = string.Format(@"SELECT * FROM dbo.sys_code WHERE IsEnable = 1 AND CodeType = 'ProductType'");
                    var sqlSysCode = string.Format(@"SELECT * FROM dbo.sys_code WHERE IsEnable = 1 ");
                    sysCodeList = db.Sql(sqlSysCode).QueryMany<sys_code>();
                }
                sysCodeList = sysCodeList.Where(item => item.CodeType.Equals("ProductType")).ToList();

                using (var db = Db.Context("Mms"))
                {
                    List<dynamic> data = new List<dynamic>();

                    //查班组生产计划
                    //string sqlA = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PlanState = 2 AND ActualFinishTime IS NULL AND WorkGroupID = '{0}' ORDER BY PlanedStartTime ", teamCode);
                    string sqlA = string.Format(@"
                    SELECT a.*,b.ProjectDetailID,b.ContractCode,b.PartCode,b.ProcessLineCode FROM dbo.MES_WorkingTicket a join APS_ProjectProduceDetial b on a.ApsCode=b.ApsCode
                    WHERE a.IsEnable = 1 AND a.ApproveState = 2 AND a.TeamCode = '{0}' ORDER BY b.PlanedStartTime", teamCode);

                    var listA = db.Sql(sqlA).QueryMany<dynamic>();

                    if (listA.Count <= 0)
                    {
                        throw new Exception(@"该班组当前没有生产计划！");
                    }

                    //查产品名称、产品类型、型号
                    var sqlB = string.Format(@"SELECT * FROM PMS_BN_ProjectDetail WHERE IsEnable = 1");
                    var listB = db.Sql(sqlB).QueryMany<PMS_BN_ProjectDetail>();
                    var sqlC = string.Format(@"SELECT * FROM dbo.PMS_BN_Project WHERE IsEnable = 1");
                    var listC = db.Sql(sqlC).QueryMany<PMS_BN_Project>();

                    //工艺
                    string sqlD = string.Format(@"SELECT * FROM MES_BN_ProductProcessRoute WHERE IsEnable = 1");
                    var listD = db.Sql(sqlD).QueryMany<MES_BN_ProductProcessRoute>();
                    string sqlE = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteDetail WHERE IsEnable = 1");
                    var listE = db.Sql(sqlE).QueryMany<MES_BN_ProductProcessRouteDetail>();
                    string sqlF = string.Format(@"SELECT * FROM MES_BN_ProductProcessRouteEquipment WHERE IsEnable = 1");
                    var listF = db.Sql(sqlF).QueryMany<MES_BN_ProductProcessRouteEquipment>();

                    //图纸
                    //设计图纸
                    string sqlG = string.Format(@"SELECT * FROM [dbo].[PMS_BN_PartFile] tbA WHERE tbA.FileName = ( SELECT MAX(FileName) FROM [dbo].[PMS_BN_PartFile] WHERE IsEnable = 1 AND PartCode = tbA.PartCode );");
                    var listG = db.Sql(sqlG).QueryMany<PMS_BN_PartFile>();
                    //工序图纸
                    string sqlJ = string.Format(@"SELECT * FROM dbo.QMS_QualityReportDoc WHERE IsEnable = 1 AND FileType = 5");
                    var listJ = db.Sql(sqlJ).QueryMany<QMS_QualityReportDoc>();
                    //工艺图纸
                    string sqlL = string.Format(@"SELECT * FROM dbo.PRS_ProcessFigure WHERE IsEnable = 1");
                    var listL = db.Sql(sqlL).QueryMany<PRS_ProcessFigure>();

                    //图号
                    string sqlH = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1");
                    var listH = db.Sql(sqlH).QueryMany<PRS_Process_BOM>();

                    //工步
                    string sqlI = string.Format(@"SELECT * FROM PRS_ProcessWorkSteps WHERE IsEnable = 1 ORDER BY WorkStepsLineCode ASC");
                    var listI = db.Sql(sqlI).QueryMany<PRS_ProcessWorkSteps>();

                    foreach (var item in listA)
                    {
                        var projectDetailModel = listB.FirstOrDefault(s => s.ID.Equals(Convert.ToInt32(item.ProjectDetailID)));
                        var productProcessRoute = listD.FirstOrDefault(s =>
                        {
                            bool a = s.ContractCode == null ? false : s.ContractCode.Equals(item.ContractCode);
                            bool b = s.PartCode == null ? false : s.PartCode.Equals(item.PartCode);
                            bool c = s.ProcessCode == null ? false : s.ProcessCode.Equals(item.ProcessCode);
                            bool d = s.ProcessLineCode == null ? false : s.ProcessLineCode.Equals(item.ProcessLineCode);
                            return a && b && c && d;
                        });
                        if (productProcessRoute == null)
                        {
                            throw new Exception(@"生产计划未查得工艺！");
                        }

                        var listK = listD.Where(s => s.PartCode.Equals(productProcessRoute.PartCode) && s.ProcessLineCode > productProcessRoute.ProcessLineCode).OrderBy(i => i.ProcessLineCode).ToList();
                        if (listK.Count > 0)
                        {
                            productProcessRoute.WarehouseID = listK[0].WarehouseID;
                            productProcessRoute.WarehouseName = listK[0].WarehouseName;
                        }

                        var code = sysCodeList.FirstOrDefault(s => s.Value.Equals(projectDetailModel.ProductType.ToString()));
                        if (code == null)
                        {
                            throw new Exception(@"生产计划未查得生产类型！");
                        }

                        var project = listC.FirstOrDefault(s => s.ContractCode.Equals(item.ContractCode));
                        if (project == null)
                        {
                            throw new Exception(@"生产计划未查得项目信息！");
                        }

                        var productProcessRouteDetail = listE.FirstOrDefault(s => s.ProcessRouteID.Equals(productProcessRoute.ID));
                        if (project == null)
                        {
                            //throw new Exception(@"产计划未查得工艺明细！");
                        }

                        var productProcessRouteEquipment = listF.FirstOrDefault(s => s.ProcessRouteID.Equals(productProcessRoute.ID));
                        if (productProcessRouteEquipment == null)
                        {
                            //throw new Exception(@"生产计划未查得设备详情！");
                        }

                        var partFiles = listG.Where(s => s.PartCode.Equals(item.PartCode)).ToList();
                        var qualityReportDocs = listJ.Where(s => s.SourceID.Equals(productProcessRoute.ID.ToString())).ToList();
                        var processFigures = listL.Where(s =>
                        {
                            bool a = s.PartCode == null ? false : s.PartCode.Equals(productProcessRoute.PartCode);
                            bool b = s.ContractCode == null ? false : s.ContractCode.Equals(productProcessRoute.ContractCode);
                            bool c = s.ProductID == null ? false : s.ProductID.Equals(item.ProjectDetailID);
                            return a && b && c;
                        }).ToList();

                        var processBOM = listH.FirstOrDefault(s => s.PartCode.Equals(item.PartCode));
                        if (processBOM == null)
                        {
                            throw new Exception(@"生产计划未查得图号信息！");
                        }

                        var processWorkSteps = listI.Where(s =>
                        {
                            bool a = s.ProcessRouteID == null ? false : true;
                            bool b = s.ProcessRouteID.Equals(productProcessRoute.ID);
                            return a && b;
                        }).ToList();
                        if (processWorkSteps.Count <= 0)
                        {
                            //throw new Exception(@"生产计划未查得工步信息！");
                        }

                        data.Add(new
                        {
                            GetProject = project,
                            GetProjectDetail = projectDetailModel,
                            GetProjectProduceDetial = item,
                            GetProductProcessRoute = productProcessRoute,
                            GetProductProcessRouteDetail = productProcessRouteDetail,
                            GetProductProcessRouteEquipment = productProcessRouteEquipment,
                            GetCode = code,
                            GetPartFiles = partFiles,
                            GetQualityReportDocs = qualityReportDocs,
                            GetProcessFigures = processFigures,
                            GetProcessBOM = processBOM,
                            GetProcessWorkSteps = processWorkSteps
                        });
                    }

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
        /// 根据零件编码查物料
        /// </summary>
        /// <param name="ppdID">工票编码</param>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetMaterialDetailByPCode(string ppdID, string partCode)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sqlA = string.Format(@"select * from MES_WorkTicketMate where IsEnable=1 and WorkTicketCode='{0}'", ppdID);
                    List<MES_WorkTicketMate> dataA = db.Sql(sqlA).QueryMany<MES_WorkTicketMate>();
                    //string sqlA = string.Format(@"SELECT * FROM dbo.MES_BN_MatchingTableDetail WHERE IsEnable = 1 AND ( PartCode = '{0}' OR PPartCode = '{0}')", partCode);
                    //List<MES_BN_MatchingTableDetail> dataA = db.Sql(sqlA).QueryMany<MES_BN_MatchingTableDetail>();

                    //string sqlC = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND ( PartCode = '{0}' OR ParentCode = '{0}' )", partCode);
                    //                    string sqlC = string.Format(@"
                    //SELECT DISTINCT
                    //       tbA.*
                    //FROM dbo.PRS_Process_BOM tbA,
                    //(
                    //    SELECT *
                    //    FROM dbo.MES_BN_MatchingTableDetail
                    //    WHERE IsEnable = 1
                    //          AND
                    //          (
                    //              PartCode = '{0}'
                    //              OR PPartCode = '{0}'
                    //          )
                    //) tbB
                    //WHERE tbA.PartCode = tbB.PartCode
                    //      AND tbA.ParentCode = tbB.PPartCode
                    //      AND tbA.IsEnable = 1
                    //UNION
                    //SELECT *
                    //FROM dbo.PRS_Process_BOM
                    //WHERE PartCode = '{0}'
                    //      AND IsEnable = 1;
                    //", partCode);
                    string sqlC = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE PartCode IN (SELECT PartCode FROM MES_WorkTicketMate WHERE WorkTicketCode='{0}')", ppdID);
                    List<PRS_Process_BOM> pBomsList = db.Sql(sqlC).QueryMany<PRS_Process_BOM>();

                    List<string> inventoryCodes = new List<string>();
                    if (pBomsList.Count > 0)
                    {
                        for (int i = 0; i < pBomsList.Count; i++)
                        {
                            if ((pBomsList[i].InventoryCode != "") && (pBomsList[i].InventoryCode != null))
                            {
                                inventoryCodes.Add("'" + pBomsList[i].InventoryCode + "'");
                            }
                        }
                    }
                    List<SYS_Part> partList = new List<SYS_Part>();
                    string sqlB = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode in ({0})", string.Join(",", inventoryCodes));
                    if (inventoryCodes.Count > 0)
                    {
                        partList = db.Sql(sqlB).QueryMany<SYS_Part>();
                    }

                    //string sqlD = string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE ID = {0}", ppdID);
                    //APS_ProjectProduceDetial ppdModel = db.Sql(sqlD).QuerySingle<APS_ProjectProduceDetial>();

                    //                    string sqlE = string.Format(@"
                    //SELECT InventoryCode,
                    //       SUM(ConfirmNum) AS ScanQuantity
                    //FROM dbo.WMS_BN_BillDetail
                    //WHERE IsEnable = 1
                    //      AND BillCode IN
                    //          (
                    //              SELECT BillCode
                    //              FROM dbo.WMS_BN_BillMain
                    //              WHERE IsEnable = 1
                    //                    AND BillType = 7
                    //          )
                    //      AND PBillCode =
                    //      (
                    //          SELECT ApsCode FROM dbo.APS_ProjectProduceDetial WHERE ID = {0}
                    //      )
                    //GROUP BY InventoryCode;
                    //", ppdID);

                    string sqlE = string.Format(@"
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
", ppdID);

                    List<dynamic> dynamics = db.Sql(sqlE).QueryMany<dynamic>();
                    List<dynamic> warehouseList = new List<dynamic>();
                    List<dynamic> list = new List<dynamic>();

                    int[] types = new[] { 4, 5 };
                    //var benshen = dataA.SingleOrDefault(i => i.PartCode == partCode);
                    var benshen = dataA.FirstOrDefault(i => i.PartCode == partCode);

                    foreach (var item in dataA)
                    {
                        var isOneSelf = false;
                        var isZhuDuanJian = false;
                        var boms = pBomsList.Where(i => i.PartCode.Equals(item.PartCode)).ToList();
                        if (boms.Count <= 0)
                        {
                            throw new Exception(@"该零件在PBom表查不到详细信息！");
                        }

                        var parts = new List<SYS_Part>();

                        if (item.ID.Equals(benshen.ID))
                        {
                            isOneSelf = true;
                            if (types.Contains(boms[0].MateType ?? -1))
                            {
                                isZhuDuanJian = true;
                                parts = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", boms[0].New_InventoryCode ?? "")).QueryMany<SYS_Part>();
                                if (parts.Count <= 0)
                                {
                                    throw new Exception(@"本体为铸件锻件，但是在Part查不到信息！");
                                }
                            }
                        }
                        else
                        {
                            //var pItem = prsbomServices.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", item.PartCode));
                            var pItem = pBomsList.FirstOrDefault(i => i.PartCode.Equals(item.PartCode) && !string.IsNullOrEmpty(i.InventoryCode));
                            parts = partList.Where(i => i.InventoryCode == pItem.InventoryCode).ToList();
                            if (parts.Count <= 0)
                            {
                                throw new Exception(@"" + item.PartCode + "在Part表查不到详细信息！");
                            }
                        }

                        //零件数量
                        int partQuantity = 0;
                        partQuantity = item.RequiredQuantity ?? 0;
                        //if (item.Type.Equals("1"))
                        //{
                        //    partQuantity = ppdModel.Quantity ?? 0;
                        //}
                        //else
                        //{
                        //    partQuantity = db.Sql(string.Format(@"SELECT COUNT(ID) FROM dbo.APS_ProduceAndMonthPlan WHERE ProducePlanCode = (SELECT ApsCode FROM dbo.APS_ProjectProduceDetial WHERE ID = {0})", ppdID)).QuerySingle<int>();
                        //    //partQuantity = (prsbomServices.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", item.PartCode)).PartQuantity ?? 0) * partQuantity;
                        //    partQuantity = (boms[0].PartQuantity ?? 0) * partQuantity;//bom单台数量*产品数量
                        //}

                        //已扫描数量

                        int partAlreadyScanQuantity = 0;

                        if ((item.IsCrux ?? -1).Equals(0))
                        {
                            string sqlalreadySQL = string.Format(@"SELECT COUNT(ID) FROM dbo.MES_MateSweepCodeLog WHERE IsEnable = 1 AND ApsCode = '{0}' AND MateBarCode = '{1}'", ppdID, parts.Count < 0 ? "-1" : parts[0].CorrespondingBarcode);
                            if (db.Sql(sqlalreadySQL).QuerySingle<int>() > 0)
                            {
                                partAlreadyScanQuantity = partQuantity;
                            }
                        }
                        else
                        {
                            var temp = dynamics.SingleOrDefault(i => i.InventoryCode == boms[0].InventoryCode);
                            partAlreadyScanQuantity = temp == null ? 0 : Convert.ToInt32(temp.ScanQuantity);
                        }

                        list.Add(new
                        {
                            GetMaterialDetail = new
                            {
                                PartCode = item.PartCode,
                                //PartName = item.PartName,
                                PartName = boms[0].PartName,
                                PartFigureCode = boms[0].PartFigureCode,
                                //ParentCode = item.PPartCode,
                                ParentCode = boms[0].ParentCode,
                                PartQuantity = partQuantity,
                                PartAlreadyScanQuantity = partAlreadyScanQuantity,
                                MaterialCode = isZhuDuanJian ? (parts.Count <= 0 ? "" : parts[0].InventoryCode) : (boms[0].MaterialCode ?? ""),
                                IsOneSelf = isOneSelf,
                                IsZhuDuanJian = isZhuDuanJian
                            },
                            GetPart = parts.Count <= 0 ? null : parts[0],
                            GetProcessBom = boms[0]
                        });

                    }

                    return new ResultModel()
                    {
                        Result = true,
                        Data = list
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
        /// 判断某生产计划是否是最后一工序
        /// </summary>
        /// <param name="ppdID"></param>
        /// <returns></returns>
        public dynamic GetIsLastProcess(int ppdID)
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
", ppdID)).QuerySingle<APS_ProjectProduceDetial>();

                bool result = ppdLastModel.ID.Equals(ppdID);

                return new ResultModel()
                {
                    Result = true,
                    Data = result
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
        /// 车间根据生产计划领料
        /// </summary>
        /// <param name="ppdID"></param>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetPickMaterialDetailByPCode(string batchingCode)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sqlA = string.Format(@"SELECT * FROM dbo.MES_WorkshopBatching WHERE IsEnable = 1  and BatchingCode='{0}'", batchingCode);
                    MES_WorkshopBatching dataA = db.Sql(sqlA).QuerySingle<MES_WorkshopBatching>();
                    List<MES_WorkshopBatchingDetail> batchingDetailList = new MES_WorkshopBatchingDetailService().GetModelList(ParamQuery.Instance().AndWhere("BatchingCode", batchingCode)).ToList();
                    int contractQuantity = batchingDetailList[0].BatchingNum ?? 0;
                    List<string> prsboms = new List<string>();
                    if (batchingDetailList.Count > 0)
                    {
                        //配套管理车间领料中排除掉选中零件本身的数据
                        batchingDetailList.Remove(batchingDetailList.SingleOrDefault(item => item.PartCode.Equals(dataA.PartCode)));
                        prsboms = batchingDetailList.Select(item =>
                        { return "'" + item.PartCode + "'"; }
                        ).ToList();
                    }
                    else
                    {
                        throw new Exception(@"本体为铸件锻件，但是在Part查不到信息！");
                    }
                    //batchingDetailList.Remove(batchingDetailList.SingleOrDefault(item => item.PartCode.Equals(dataA.PartCode)));
                    //List<string> prsboms = batchingDetailList.Select(item => item.PartCode).ToList();
                    var prsbomServices = new PRS_Process_BOMService();
                    List<PRS_Process_BOM> pBomsList = new List<PRS_Process_BOM>();
                    string sqlC = string.Format(@"SELECT DISTINCT PartCode,PartName,InventoryCode,InventoryName
FROM dbo.PRS_Process_BOM
WHERE IsEnable = 1
      AND  PartCode in ({0})", string.Join(",", prsboms));
                    List<string> inventoryCodes = new List<string>();
                    if (prsboms.Count > 0)
                    {
                        //查询除了本身外的所有的工艺bom数据
                        pBomsList = db.Sql(sqlC).QueryMany<PRS_Process_BOM>();
                        inventoryCodes = pBomsList.Select(item => { return "'" + item.InventoryCode + "'"; }).ToList();
                    }


                    List<dynamic> warehouseList = new List<dynamic>();
                    List<dynamic> list = new List<dynamic>();
                    var parts = new List<SYS_Part>();

                    int[] typesA = new[] { 4, 5 };
                    string[] typesB = new string[] { "0", "1" };
                    var benshen = new PRS_Process_BOMService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", dataA.PartCode));
                    string projectName = new PMS_BN_ProjectService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ContractCode", dataA.ContractCode)).ProjectName;
                    string productName = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", dataA.ProductID)).ProductName;
                    var warehouseService = new SYS_BN_WarehouseService();
                    //判断本身是否为铸锻件
                    if (typesA.Contains(Convert.ToInt16(benshen.MateType ?? -1)))
                    {
                        //为铸锻件时，将铸锻件的物料信息加入到返回列表中
                        var part = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", benshen.New_InventoryCode ?? "")).QuerySingle<SYS_Part>();
                        if (string.IsNullOrEmpty(part.InventoryCode))
                        {
                            throw new Exception(@"本体为铸件锻件，但是在Part查不到信息！");
                        }
                        else
                        {
                            if (contractQuantity > 0)
                            {
                                parts.Add(part);
                                list.Add(new
                                {
                                    InventoryCode = part.InventoryCode,
                                    InventoryName = part.InventoryName,
                                    WarehouseCode = part.WarehouseCode,
                                    WarehouseName = part.WarehouseName,
                                    InventoryQuantity = contractQuantity,
                                    ContractCode = dataA.ContractCode,
                                    ProjectName = projectName,
                                    Model = part.Model,
                                    ProductID = dataA.ProductID,
                                    ProductName = productName,
                                    Unit = part.QuantityUnit
                                });
                                warehouseList.Add(new
                                {
                                    InventoryCode = part.InventoryCode,
                                    InventoryName = part.InventoryName,
                                    WarehouseCode = part.WarehouseCode,
                                    WarehouseName = part.WarehouseName,
                                    InventoryQuantity = contractQuantity,
                                    ContractCode = dataA.ContractCode,
                                    ProjectName = projectName,
                                    ProductID = dataA.ProductID,
                                    ProductName = productName,
                                    Model = part.Model,
                                    Unit = part.QuantityUnit
                                });
                            }

                        }

                    }
                    PRS_Process_BOM pbom = new PRS_Process_BOM();
                    PRS_Process_BOMService pbomServices = new PRS_Process_BOMService();
                    //循环生产配套_车间领料表中除了本身外此零件所有的下一级
                    foreach (var item in batchingDetailList)
                    {
                        if (item.BatchingNum == null || item.BatchingNum == 0)
                        {
                            continue;
                        }
                        else
                        {
                            //查询此配套零件对应的u8数据
                            pbom = pbomServices.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", item.PartCode));
                            //判断是否为自制件，且没有说明出库仓库,自制件跳出循环，不会加入生成领料单数据
                            if (pbom.IsSelfMade == "1" && string.IsNullOrEmpty(item.OutDeptCode))
                            {
                                continue;
                            }
                            //否则查询出物料的详细信息
                            else
                            {
                                List<SYS_Part> part = new List<SYS_Part>();

                                part = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", pbom.InventoryCode ?? "")).QueryMany<SYS_Part>();

                                if (part.Count <= 0)
                                {
                                    string msg = item.PartName + "(" + item.PartFigureNumber + ")" + @"在part表查不到详细信息，请先同步工艺BOM数据！";
                                    throw new Exception(msg);
                                }
                                else
                                {
                                    string warehouseCode = item.OutDeptCode ?? part[0].WarehouseCode;
                                    string warehouseName = warehouseService.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("WarehouseCode", warehouseCode)).WarehouseName;
                                    list.Add(new
                                    {
                                        InventoryCode = part[0].InventoryCode,
                                        InventoryName = part[0].InventoryName,
                                        WarehouseCode = warehouseCode,
                                        WarehouseName = warehouseName,
                                        InventoryQuantity = item.BatchingNum,
                                        ContractCode = dataA.ContractCode,
                                        ProjectName = projectName,
                                        ProductID = dataA.ProductID,
                                        ProductName = productName,
                                        Model = part[0].Model,
                                        Unit = part[0].QuantityUnit
                                    });

                                    warehouseList.Add(new
                                    {
                                        InventoryCode = part[0].InventoryCode,
                                        InventoryName = part[0].InventoryName,
                                        WarehouseCode = warehouseCode,
                                        WarehouseName = warehouseName,
                                        InventoryQuantity = item.BatchingNum,
                                        ContractCode = dataA.ContractCode,
                                        ProjectName = projectName,
                                        ProductID = dataA.ProductID,
                                        ProductName = productName,
                                        Model = part[0].Model,
                                        Unit = part[0].QuantityUnit
                                    });

                                }
                            }
                        }


                    }
                    if (list.Count == 0)
                    {
                        throw new Exception(@"此零件下所有的物料都不需要领料，生成领料单失败！");
                    }
                    return new ResultModel
                    {
                        Data = new
                        {
                            listmain = warehouseList,
                            listDetail = list
                        },
                        Result = true,
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
        /// 转序入库时，根据生产计划ID查转序目标
        /// </summary>
        /// <param name="ppdID"></param>
        /// <returns></returns>
        public dynamic GetTurnTarget(int ppdID)
        {
            try
            {
                var ppdModel = db.Sql(string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND ID = {0}", ppdID)).QuerySingle<APS_ProjectProduceDetial>();
                var pprModel = db.Sql(string.Format(@"SELECT * FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND PartCode = '{0}' AND ProcessModelType = {1} AND ProcessLineCode = '{2}'", ppdModel.PartCode, ppdModel.ProcessModelType, ppdModel.ProcessLineCode)).QuerySingle<MES_BN_ProductProcessRoute>();
                var pprList = db.Sql(string.Format(@"SELECT * FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND PartCode = '{0}' AND ProcessModelType = {1} AND ProcessLineCode > {2} ORDER BY ProcessLineCode ASC", ppdModel.PartCode, ppdModel.ProcessModelType, ppdModel.ProcessLineCode)).QueryMany<MES_BN_ProductProcessRoute>();

                //有下一工序  取下一工序  车间 WorkshopID
                if (pprList.Count > 0)
                {
                    var nextProcess = pprList[0];
                    return new ResultModel()
                    {
                        Result = true,
                        Data = new
                        {
                            ID = nextProcess.WorkshopID,
                            Name = nextProcess.WorkshopName
                        }
                    };
                }
                else
                {
                    //否则 父级编码查
                    var pbomList = db.Sql(string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND PartCode = '{0}'", ppdModel.PartCode)).QueryMany<PRS_Process_BOM>();
                    if (pbomList.Count.Equals(1))
                    {
                        //父级编码 生产计划
                        var ppdList = db.Sql(string.Format(@"SELECT * FROM dbo.APS_ProjectProduceDetial WHERE IsEnable = 1 AND PartCode = '{0}' ORDER BY ProcessModelType,ProcessLineCode", pbomList[0].ParentCode)).QueryMany<APS_ProjectProduceDetial>();

                        if (ppdList.Count <= 0)
                        {
                            if (string.IsNullOrEmpty(pprModel.WarehouseID))
                            {
                                throw new Exception(@"无转序目标，请确认数据！");
                            }
                            else
                            {
                                //WarehouseID
                                return new ResultModel()
                                {
                                    Result = true,
                                    Data = new
                                    {
                                        ID = pprModel.WarehouseID,
                                        Name = pprModel.WarehouseName
                                    }
                                };
                            }
                        }
                        else
                        {
                            //返回  第一道工序加工车间WorkshopID
                            return new ResultModel()
                            {
                                Result = true,
                                Data = new
                                {
                                    ID = ppdList[0].WorkshopID,
                                    Name = ppdList[0].WorkshopName
                                }
                            };
                        }
                    }
                    else
                    {
                        throw new Exception("未查到工艺BOM！");
                    }
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
        public dynamic GetPartAndPBomByBarCode(string barCode)
        {
            try
            {
                if (string.IsNullOrEmpty(barCode))
                {
                    throw new Exception("参数无效！");
                }

                string sqlA = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE CorrespondingBarcode = '{0}'", barCode);

                var part = db.Sql(sqlA).QuerySingle<SYS_Part>();

                if (part == null)
                {
                    throw new Exception("该条码在Part表查不到信息！");
                }

                string sqlB = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE PartCode = '{0}'", part.PartCode ?? "");

                var pbom = db.Sql(sqlB).QuerySingle<PRS_Process_BOM>();

                if (pbom == null)
                {
                    throw new Exception("Part与ProcessBOM为关联！");
                }

                return new ResultModel()
                {
                    Result = true,
                    Data = new
                    {
                        GetPart = part,
                        GetProcessBom = pbom
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
        /// 根据条码 查用户信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public dynamic GetUserInfoByBarCode(string barCode)
        {
            try
            {
                if (string.IsNullOrEmpty(barCode))
                {
                    throw new Exception(@"条码不能为空！");
                }

                string sql = string.Format(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserBarcode = '{0}'", barCode);

                var model = db.Sql(sql).QuerySingle<SYS_BN_User>();

                bool result = model != null;

                return new ResultModel()
                {
                    Result = result,
                    Data = model,
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

        /// <summary>
        /// 根据班组查设备及保养维护信息
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public dynamic GetEquipmentMaintainByTCode(string teamCode)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    //设备
                    string sqlA = string.Format(@"SELECT * FROM dbo.SYS_Equipment WHERE IsEnable = 1 ");
                    List<SYS_Equipment> listA = db.Sql(sqlA).QueryMany<SYS_Equipment>();
                    //设备明细
                    string sqlB = string.Format(@"SELECT * FROM dbo.SYS_EquipmentDetail WHERE IsEnable = 1 ");
                    List<SYS_EquipmentDetail> listB = db.Sql(sqlB).QueryMany<SYS_EquipmentDetail>();
                    //当天所有保养计划
                    string sqlC = string.Format(@"SELECT * FROM EQP_EquipmentMaintenancePlan WHERE IsEnable = 1 AND MaintenanceState = 2 AND DATEDIFF(DAY,PlanedStartTime,GETDATE()) = 0  AND ActualFinishTime IS NULL ");
                    List<EQP_EquipmentMaintenancePlan> listC = db.Sql(sqlC).QueryMany<EQP_EquipmentMaintenancePlan>();

                    List<dynamic> data = new List<dynamic>();

                    foreach (var item in listC)
                    {
                        var Equipments = listA.Where(i =>
                        {
                            bool a = i.EquipmentCode == null ? false : true;
                            bool b = i.EquipmentCode.Equals(item.EquipmentCode);
                            return a && b;
                        }).ToList();
                        if (Equipments.Count <= 0)
                        {
                            throw new Exception(@"数据异常：设备保养计划找不到该设备信息！");
                        }
                        else if (Equipments.Count.Equals(1))
                        {
                            if (Equipments[0].TeamCode.Equals(teamCode))
                            {
                                var EquipmentDetails = listB.Where(i => i.MainID.Equals(Equipments[0].ID.ToString())).ToList();

                                data.Add(new
                                {
                                    GetEquipment = Equipments[0],
                                    GetEquipmentDetail = EquipmentDetails,
                                    GetEquipmentMaintenancePlan = item
                                });
                            }
                        }
                        else
                        {
                            throw new Exception(@"数据异常：设备保养计划找到多台设备信息！");
                        }
                    }

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
        /// 根据零件编码、零件类型查零件信息
        /// </summary>
        /// <param name="partCode"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        public dynamic GetPartByPCodeAndPType(string partCode, string partType)
        {
            try
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(partCode))
                {
                    where += string.Format(" AND tbA.PartCode LIKE '%{0}%' ", partCode);
                }
                if (!string.IsNullOrEmpty(partType))
                {
                    where += string.Format(" AND tbA.PartTypeID LIKE '%{0}%' ", partType);
                }

                string sql = string.Format(@"
SELECT tbA.*,
       tbB.TypeName
FROM
(SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1) tbA
    LEFT JOIN
    (SELECT * FROM dbo.SYS_PartType WHERE IsEnable = 1) tbB
        ON tbA.PartTypeID = tbB.PartTypeCode
WHERE 1 = 1 {0}
", where);

                using (var db = Db.Context("Mms"))
                {
                    return new ResultModel()
                    {
                        Result = true,
                        Data = new
                        {
                            GetSysParts = db.Sql(sql).QueryMany<dynamic>()
                        }
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
        /// 查所有生产任务
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public dynamic GetTaskInfoByKeyWord(string url)
        {
            try
            {
                List<sys_code> taskTypeList = null;
                List<sys_code> designDeptList = null;

                using (var db = Db.Context("Sys"))
                {
                    var sqlA = string.Format(@"SELECT * FROM sys_code WHERE IsEnable = 1 AND CodeType = 'TaskType'");
                    var sqlB = string.Format(@"SELECT * FROM sys_code WHERE IsEnable = 1 AND CodeType = 'DesignDepartment'");

                    taskTypeList = db.Sql(sqlA).QueryMany<sys_code>();
                    designDeptList = db.Sql(sqlB).QueryMany<sys_code>();
                }

                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"
SELECT DISTINCT
       tbA.ID AS TaskID,
       tbA.TaskType,
       tbA.TaskFinishDate,
       tbA.ActualFinishTime,
       tbA.DesignType,
       tbA.BillCode,
       tbA.DesignDepartment,
       tbB.ID AS ProjectDetailID,
       tbB.ProductName,
       tbB.Model,
       tbB.Specifications,
       tbC.ContractCode,
       tbC.ProjectID,
       tbC.ProjectName,
       tbD.[FileName],
       tbD.FileAddress,
       tbD.DocName
FROM PMS_DesignTaskDetail tbA
    INNER JOIN PMS_BN_ProjectDetail tbB
        ON tbA.ProductID = tbB.ID
           AND tbB.IsEnable = 1
    LEFT JOIN PMS_BN_Project tbC
        ON tbB.MainID = tbC.ProjectID
           AND tbC.IsEnable = 1
    LEFT JOIN PRS_DesignChangeRequest tbD
        ON tbA.BillCode = tbD.RequestCode
           AND tbD.IsEnable = 1
WHERE tbA.IsEnable = 1
ORDER BY ContractCode DESC,
         TaskFinishDate DESC;
");

                    var list = db.Sql(sql).QueryMany<dynamic>();
                    List<dynamic> items = new List<dynamic>();


                    list.ForEach(item =>
                    {
                        string path = string.Empty;
                        if ((!string.IsNullOrEmpty(item.FileAddress)) && File.Exists(item.FileAddress))
                        {
                            path = url + item.FileAddress.Substring(item.FileAddress.IndexOf("Upload")).Replace(@"\", "/");
                        }

                        items.Add(new
                        {
                            TaskID = item.TaskID,
                            ContractCode = item.ContractCode,
                            ProductName = item.ProductName,
                            Model = item.Model,
                            Specifications = item.Specifications,
                            TaskTypeName = taskTypeList.SingleOrDefault(i => i.Value.Equals(Convert.ToString(item.TaskType))).Text,
                            TaskFinishDate = item.TaskFinishDate,
                            ActualFinishTime = item.ActualFinishTime,
                            ProjectID = item.ProjectID,
                            ProjectName = item.ProjectName,
                            ProjectDetailID = item.ProjectDetailID,
                            DesignType = item.DesignType,
                            DesignTypeName = item.DesignType == null ? "" : item.DesignType == 1 ? "新建项目设计任务" : item.DesignType == 2 ? "设计更改申请任务" : "未知",
                            BillCode = item.BillCode,
                            FileName = item.FileName,
                            FileAddress = path,
                            DocName = item.DocName,
                            DesignDepartmentType = item.DesignDepartment,
                            DesignDepartmentName = item.DesignDepartment == null ? null : designDeptList.SingleOrDefault(i => i.Value.Equals(Convert.ToString(item.DesignDepartment))).Text,
                        });
                    });

                    return new ResultModel
                    {
                        Result = true,
                        Data = items
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
        /// 查所有图纸申请任务
        /// </summary>
        /// <returns></returns>
        public dynamic GetDrawTaskInfoByKeyWord(string url)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sqlA = string.Format(@"SELECT * FROM dbo.SYS_DrawingApplication WHERE IsEnable = 1 AND RequestStatus in (1,2)");
                    List<SYS_DrawingApplication> listA = db.Sql(sqlA).QueryMany<SYS_DrawingApplication>();

                    string sqlB = string.Format(@"SELECT * FROM dbo.SYS_FileManage WHERE BindTableName = 'SYS_DrawingApplication' AND FileType = 1");
                    List<SYS_FileManage> listB = db.Sql(sqlB).QueryMany<SYS_FileManage>();

                    List<dynamic> items = new List<dynamic>();
                    Random rd = new Random(Guid.NewGuid().GetHashCode());

                    listA.ForEach(item =>
                    {
                        SYS_FileManage fileModel = listB.SingleOrDefault(i => i.BindCode.Equals(item.ID.ToString()));
                        fileModel = fileModel ?? new SYS_FileManage();

                        string path = string.Empty;
                        if ((!string.IsNullOrEmpty(fileModel.FileAddress)) && File.Exists(fileModel.FileAddress))
                        {
                            path = url + fileModel.FileAddress.Substring(fileModel.FileAddress.IndexOf("Upload")).Replace(@"\", "/");
                        }

                        items.Add(new
                        {
                            ID = item.ID,
                            RequestCode = item.RequestCode,
                            ContractCode = item.ContractCode,
                            ProductName = item.ProductName,
                            PartName = item.PartName,
                            FigureCode = item.FigureCode,
                            ApplicationReason = item.ApplicationReason,
                            RequestStatus = item.RequestStatus,
                            RequestStatusName = item.RequestStatus == null ? "未知" : item.RequestStatus.Equals(1) ? "未上传" : "已上传",
                            FileName = fileModel.FileSuffix == null ? "无" : (DateTime.Now.ToString("yyyyMMddHHmmss") + rd.Next(1000, 10000) + "." + fileModel.FileSuffix),
                            FileAddress = path,
                            DocName = fileModel.FileName
                        });
                    });

                    return new ResultModel()
                    {
                        Result = true,
                        Data = items
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
        /// 获取所有设计任务结果类型
        /// </summary>
        /// <returns></returns>
        public dynamic GetDesignTaskResult()
        {
            try
            {
                using (var db = Db.Context("Sys"))
                {
                    string sql = string.Format(@"SELECT * FROM dbo.sys_code WHERE IsEnable = 1 AND CodeType = 'DesignTaskResult'");
                    var data = db.Sql(sql).QueryMany<sys_code>();

                    return new ResultModel()
                    {
                        Result = true,
                        Data = data.Select(item =>
                        {
                            return new
                            {
                                Value = item.Value,
                                Text = item.Text
                            };
                        }).ToList()
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
        /// 下料记录-左侧数据查询
        /// </summary>
        /// <returns></returns>
        public dynamic GetBlankingRecordLeftData(string fCode)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"
SELECT a.ContractCode,
       a.FigureCode,
       a.PartName,
       a.BlankingSize,
       c.BiankingSize,
       c.ID
FROM dbo.Mes_BlankingResult c
    INNER JOIN dbo.PRS_BlankingResult b
        ON c.BlankingResultID = b.ID
    INNER JOIN dbo.PRS_BlankingRecord a
        ON b.MainID = a.ID
WHERE c.IsPicking = 0
      AND c.IsEnable = 1
      AND a.FigureCode = '{0}';
", fCode);

                    return new ResultModel()
                    {
                        Result = true,
                        Data = db.Sql(sql).QueryMany<dynamic>()
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
        /// 下料记录-右侧数据查询
        /// </summary>
        /// <param name="iCode"></param>
        /// <param name="bNumber"></param>
        /// <returns></returns>
        public dynamic GetBlankingRecordRightData(string iCode, string bNumber)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"
SELECT c.BiankingSize,
       c.ID
FROM Mes_BlankingResult c
    LEFT JOIN PRS_BlankingResult b
        ON c.BlankingResultID = b.ID
    LEFT JOIN PRS_BlankingRecord a
        ON b.MainID = a.ID
WHERE c.IsPicking = 1
      AND c.InventoryCode = '{0}'
      AND c.BatchNumber = '{1}';
", iCode, bNumber);

                    return new ResultModel()
                    {
                        Result = true,
                        Data = db.Sql(sql).QueryMany<dynamic>()
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


        #endregion
        //**********************************************************************************************************//
        #region 操作数据

        /// <summary>
        /// 物料其他出库/转需入库
        /// </summary>
        /// <param name="bMainModel"></param>
        /// <param name="bDetailModels"></param>
        /// <param name="type">0:其他出库 1:转序入库</param>
        /// <returns></returns>
        public dynamic MaterialInventory(WMS_BN_BillMain bMainModel, List<WMS_BN_BillDetail> bDetailModels, int type)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    try
                    {
                        //库存表
                        List<WMS_BN_RealStock> realStockData = db.Sql("SELECT * FROM WMS_BN_RealStock WHERE IsEnable = 1").QueryMany<WMS_BN_RealStock>();

                        db.UseTransaction(true);

                        //db.Insert("WMS_BN_BillMain", bMainModel).Execute();
                        int tempA = db.Sql(@"insert into WMS_BN_BillMain ([ID] ,[BillCode] ,[BillType] ,[ContractCode] ,[ProjectName] ,[SalesmanCode] ,[Salesman] ,[DepartmentID] ,[DepartmentName] ,[SupplierCode] ,[SupplierName] ,[WarehouseCode] ,[WarehouseName] ,[PickPersonCode] ,[PickPerson] ,[ApproveState] ,[ApprovePerson] ,[ApproveDate] ,[ApproveRemark] ,[OrderBillCode] ,[Remark] ,[CreatePerson] ,[CreateTime] ,[ModifyPerson] ,[ModifyTime] ,[IsEnable]) values(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25)",
                        bMainModel.ID, bMainModel.BillCode, bMainModel.BillType, bMainModel.ContractCode, bMainModel.ProjectName, bMainModel.SalesmanCode, bMainModel.Salesman, bMainModel.DepartmentID, bMainModel.DepartmentName, bMainModel.SupplierCode, bMainModel.SupplierName, bMainModel.WarehouseCode, bMainModel.WarehouseName, bMainModel.PickPersonCode, bMainModel.PickPerson, bMainModel.ApproveState, bMainModel.ApprovePerson, bMainModel.ApproveDate, bMainModel.ApproveRemark, bMainModel.OrderBillCode, bMainModel.Remark, bMainModel.CreatePerson, bMainModel.CreateTime, bMainModel.ModifyPerson, bMainModel.ModifyTime, bMainModel.IsEnable).Execute();

                        //***** 暂未测试 *****//
                        //int tempA = db.Sql(GetInsertSQL(bMainModel)).Execute();

                        StringBuilder sb = new StringBuilder();

                        foreach (var item in bDetailModels)
                        {
                            int tempB = db.Insert("WMS_BN_BillDetail", item).AutoMap(x => x.ID).Execute();

                            var realStocks = realStockData.Where(s =>
                            {
                                bool a = s.WarehouseCode == null ? false : s.WarehouseCode.Equals(bMainModel.WarehouseCode);
                                bool b = s.InventoryCode == null ? false : s.InventoryCode.Equals(item.InventoryCode);
                                bool c = s.BatchCode == null ? false : s.BatchCode.Equals(item.BatchCode);
                                return a && b && c;
                            }).ToList();

                            //物料其他出库
                            if (type.Equals(0))
                            {
                                #region
                                if (realStocks.Count <= 0)
                                {
                                    throw new Exception(@"仓库没有物料！");
                                }
                                else if (realStocks.Count.Equals(1))
                                {
                                    if (realStocks[0].RealStock == null)
                                    {
                                        throw new Exception(@"仓库物料数量异常！");
                                    }
                                    else
                                    {
                                        if (realStocks[0].RealStock < item.ConfirmNum)
                                        {
                                            throw new Exception(@"仓库物料数量不够，无法出库！");
                                        }
                                        else
                                        {
                                            sb.Append(string.Format("UPDATE WMS_BN_RealStock SET RealStock = {0},TotalStock = {1},ModifyPerson = '{2}',ModifyTime = '{3}' WHERE ID = {4} ;\r\n",
                                                realStocks[0].RealStock - item.ConfirmNum, realStocks[0].TotalStock - item.ConfirmNum, item.ModifyPerson, item.ModifyTime, realStocks[0].ID));
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception(@"仓库物料种类繁多，可惜不知取哪一种！");
                                }
                                #endregion
                            }
                            //转序入库
                            else if (type.Equals(1))
                            {
                                #region
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
                                            InventoryCode = item.InventoryCode,
                                            InventoryName = item.InventoryName,
                                            Model = item.Specs,
                                            RealStock = (double)item.ConfirmNum,
                                            TotalStock = (double)item.ConfirmNum,
                                            WarehouseCode = bMainModel.WarehouseCode,
                                            WarehouseName = bMainModel.WarehouseName,
                                            BatchCode = item.BatchCode,
                                            Unit = item.Unit,
                                            CreatePerson = bMainModel.CreatePerson,
                                            CreateTime = bMainModel.CreateTime,
                                            ModifyPerson = bMainModel.CreatePerson,
                                            ModifyTime = bMainModel.CreateTime,
                                            IsEnable = 1
                                        };
                                        int tempC = db.Insert("WMS_BN_RealStock", realStockModel).AutoMap(x => x.ID).Execute();
                                    }
                                    //有则更新数量
                                    else if (realStocks.Count.Equals(1))
                                    {
                                        sb.Append(string.Format("UPDATE WMS_BN_RealStock SET RealStock = {0},TotalStock = {1},ModifyPerson = '{2}',ModifyTime = '{3}' WHERE ID = {4} ;\r\n",
                                                realStocks[0].RealStock + item.ConfirmNum, realStocks[0].TotalStock + item.ConfirmNum, item.ModifyPerson, item.ModifyTime, realStocks[0].ID));
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
                                #endregion
                            }
                            else
                            {
                                throw new Exception(@"内部参数错误！");
                            }
                        }

                        //增减库存
                        string sql = sb.ToString();
                        if (!string.IsNullOrEmpty(sql))
                        {
                            db.Sql(sql).Execute();
                        }

                        db.Commit();

                        return new ResultModel()
                        {
                            Result = true
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
        /// 物料其他出库
        /// </summary>
        /// <param name="bMainModel"></param>
        /// <param name="bDetailModel"></param>
        /// <returns></returns>
        public dynamic MaterialInventory(WMS_BN_BillMain bMainModel, WMS_BN_BillDetail bDetailModel)
        {
            using (var db = Db.Context("Mms"))
            {
                try
                {
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
                        Result = result
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

        public dynamic SaveBarCodeScanLog(MES_MateSweepCodeLog scanLog)
        {
            try
            {
                bool result = db.Sql(GetInsertSQL(scanLog)).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "写入扫码日志成功！" : "写入扫码日志失败！"
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
        /// 生产需要报检-生成报检单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostCheckRequest(dynamic data)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string documentNoA = MmsHelper.GetOrderNumber("MES_ProcessInspectionRequest", "BillCode", "GCBJ", "", "");
                    string documentNoB = MmsHelper.GetOrderNumber("QMS_ProcessInspection", "BillCode", "GCJY", "", "");

                    #region MES_ProcessInspectionRequest表

                    string sqlA = string.Format(@"
INSERT INTO dbo.MES_ProcessInspectionRequest
(
    BillCode,
    ContractCode,
    ProjectName,
    ProductName,
    ProductFigureNumber,
    PartCode,
    PartName,
    partFigure,
    MaterialCode,
    CheckQuantity,
    BillState,
    IsEnable,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    ProductPlanCode,
    OperatorCode,
    DepartmentCode,
    DepartmentName,
    Unit,
    EquipmentCode,
    EquipmentName,
    ProductProcessRouteID
)
VALUES
(   '{0}',        -- BillCode - varchar(50)
    '{1}',        -- ContractCode - varchar(50)
    '{2}',        -- ProjectName - varchar(50)
    '{3}',        -- ProductName - varchar(50)
    '{4}',        -- ProductFigureNumber - varchar(50)
    '{5}',        -- PartCode - varchar(50)
    '{6}',        -- PartName - varchar(50)
    '{7}',        -- partFigure - varchar(50)
    '{8}',        -- MaterialCode - varchar(50)
    {9},          -- CheckQuantity - int
    {10},         -- BillState - int
    {11},         -- IsEnable - int
    '{12}',       -- CreatePerson - varchar(50)
    '{13}',       -- CreateTime - datetime
    '{14}',       -- ModifyPerson - varchar(50)
    '{15}',        -- ModifyTime - datetime
    '{16}',
    '{17}',
    '{18}',
    '{19}',
    '{20}',
    '{21}',
    '{22}',
    '{23}'
    );
", documentNoA, data["mesPIR"]["ContractCode"], data["mesPIR"]["ProjectName"], data["mesPIR"]["ProductName"], data["mesPIR"]["ProductFigureNumber"], data["mesPIR"]["PartCode"],
        data["mesPIR"]["PartName"], data["mesPIR"]["partFigure"], data["mesPIR"]["MaterialCode"], data["mesPIR"]["CheckQuantity"], data["mesPIR"]["BillState"],
        data["mesPIR"]["IsEnable"], data["mesPIR"]["CreatePerson"], data["mesPIR"]["CreateTime"], data["mesPIR"]["ModifyPerson"], data["mesPIR"]["ModifyTime"], data["mesPIR"]["ProductPlanCode"],
        data["mesPIR"]["OperatorCode"], data["mesPIR"]["DepartmentCode"], data["mesPIR"]["DepartmentName"], data["mesPIR"]["Unit"], data["mesPIR"]["EquipmentCode"], data["mesPIR"]["EquipmentName"], data["mesPIR"]["ProductProcessRouteID"]);

                    #endregion

                    //***** 暂未测试 *****//
                    //string sqlA = GetInsertSQL(new MES_ProcessInspectionRequest()
                    //{
                    //    BillCode = documentNoA,
                    //    ContractCode = data["mesPIR"]["ContractCode"],
                    //    ProjectName = data["mesPIR"]["ProjectName"],
                    //    ProductName = data["mesPIR"]["ProductName"],
                    //    ProductFigureNumber = data["mesPIR"]["ProductFigureNumber"],
                    //    PartCode = data["mesPIR"]["PartCode"],
                    //    PartName = data["mesPIR"]["PartName"],
                    //    partFigure = data["mesPIR"]["partFigure"],
                    //    MaterialCode = data["mesPIR"]["MaterialCode"],
                    //    CheckQuantity = data["mesPIR"]["CheckQuantity"],
                    //    BillState = data["mesPIR"]["BillState"],
                    //    IsEnable = data["mesPIR"]["IsEnable"],
                    //    CreatePerson = data["mesPIR"]["CreatePerson"],
                    //    CreateTime = data["mesPIR"]["CreateTime"],
                    //    ModifyPerson = data["mesPIR"]["ModifyPerson"],
                    //    ModifyTime = data["mesPIR"]["ModifyTime"],
                    //    ProductPlanCode = data["mesPIR"]["ProductPlanCode"],
                    //    OperatorCode = data["mesPIR"]["OperatorCode"],
                    //    DepartmentCode = data["mesPIR"]["DepartmentCode"],
                    //    DepartmentName = data["mesPIR"]["DepartmentName"],
                    //    Unit = data["mesPIR"]["Unit"],
                    //    EquipmentCode = data["mesPIR"]["EquipmentCode"],
                    //    EquipmentName = data["mesPIR"]["EquipmentName"],
                    //    ProductProcessRouteID = data["mesPIR"]["ProductProcessRouteID"]
                    //});

                    #region QMS_ProcessInspection

                    string sqlB = string.Format(@"
DECLARE @id int = (SELECT ISNULL(MAX(ID),1)+1 FROM dbo.QMS_ProcessInspection );
INSERT INTO dbo.QMS_ProcessInspection
(
    ID,
    BillCode,
    ContractCode,
    ProjectName,
    ProductName,
    ProductFigureNumber,
    PartCode,
    PartName,
    partFigure,
    MaterialCode,
    CheckQuantity,
    QualifiedQuntity,
    IsQualified,
    BillState,
    IsEnable,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    ProductPlanCode,
    OperatorCode,
    DepartmentCode,
    DepartmentName,
    PBillCode,
    Unit,
    EquipmentCode,
    EquipmentName
)
VALUES
(   @id,         -- ID - int
    '{0}',        -- BillCode - varchar(50)
    '{1}',        -- ContractCode - varchar(50)
    '{2}',        -- ProjectName - varchar(50)
    '{3}',        -- ProductName - varchar(50)
    '{4}',        -- ProductFigureNumber - varchar(50)
    '{5}',        -- PartCode - varchar(50)
    '{6}',        -- PartName - varchar(50)
    '{7}',        -- partFigure - varchar(50)
    '{8}',        -- MaterialCode - varchar(50)
    {9},         -- CheckQuantity - int
    {10},         -- QualifiedQuntity - int
    '{11}',        -- IsQualified - varchar(50)
    {12},         -- BillState - int
    {13},         -- IsEnable - int
    '{14}',        -- CreatePerson - varchar(50)
    '{15}', -- CreateTime - datetime
    '{16}',        -- ModifyPerson - varchar(50)
    '{17}', -- ModifyTime - datetime
    '{18}',        -- ProductPlanCode - varchar(50)
    '{19}',        -- OperatorCode - varchar(50)
    '{20}',        -- DepartmentCode - varchar(50)
    '{21}',        -- DepartmentName - varchar(50)
    '{22}',        -- PBillCode - varchar(50)
    '{23}',        -- Unit - varchar(50)
    '{24}',        -- EquipmentCode - varchar(50)
    '{25}'         -- EquipmentName - varchar(50)
    )
", documentNoB, data["qmsPI"]["ContractCode"], data["qmsPI"]["ProjectName"], data["qmsPI"]["ProductName"], data["qmsPI"]["ProductFigureNumber"],
        data["qmsPI"]["PartCode"], data["qmsPI"]["PartName"], data["qmsPI"]["partFigure"], data["qmsPI"]["MaterialCode"], data["qmsPI"]["CheckQuantity"], data["qmsPI"]["QualifiedQuntity"],
        data["qmsPI"]["IsQualified"], data["qmsPI"]["BillState"], data["qmsPI"]["IsEnable"], data["qmsPI"]["CreatePerson"], data["qmsPI"]["CreateTime"], data["qmsPI"]["ModifyPerson"],
        data["qmsPI"]["ModifyTime"], data["qmsPI"]["ProductPlanCode"], data["qmsPI"]["OperatorCode"], data["qmsPI"]["DepartmentCode"], data["qmsPI"]["DepartmentName"], documentNoA,
        data["qmsPI"]["Unit"], data["qmsPI"]["EquipmentCode"], data["qmsPI"]["EquipmentName"]);

                    #endregion

                    //***** 暂未测试 *****//
                    //string sqlB = GetInsertSQL(new QMS_ProcessInspection()
                    //{
                    //    ID = db.Sql("SELECT ISNULL(MAX(ID),1)+1 FROM dbo.QMS_ProcessInspection").QuerySingle<int>(),
                    //    BillCode = documentNoB,
                    //    ContractCode = data["qmsPI"]["ContractCode"],
                    //    ProjectName = data["qmsPI"]["ProjectName"],
                    //    ProductName = data["qmsPI"]["ProductName"],
                    //    ProductFigureNumber = data["qmsPI"]["ProductFigureNumber"],
                    //    PartCode = data["qmsPI"]["PartCode"],
                    //    PartName = data["qmsPI"]["PartName"],
                    //    partFigure = data["qmsPI"]["partFigure"],
                    //    MaterialCode = data["qmsPI"]["MaterialCode"],
                    //    CheckQuantity = data["qmsPI"]["CheckQuantity"],
                    //    QualifiedQuntity = data["qmsPI"]["QualifiedQuntity"],
                    //    IsQualified = data["qmsPI"]["IsQualified"],
                    //    BillState = data["qmsPI"]["BillState"],
                    //    IsEnable = data["qmsPI"]["IsEnable"],
                    //    CreatePerson = data["qmsPI"]["CreatePerson"],
                    //    CreateTime = data["qmsPI"]["CreateTime"],
                    //    ModifyPerson = data["qmsPI"]["ModifyPerson"],
                    //    ModifyTime = data["qmsPI"]["ModifyTime"],
                    //    ProductPlanCode = data["qmsPI"]["ProductPlanCode"],
                    //    OperatorCode = data["qmsPI"]["OperatorCode"],
                    //    DepartmentCode = data["qmsPI"]["DepartmentCode"],
                    //    DepartmentName = data["qmsPI"]["DepartmentName"],
                    //    PBillCode = documentNoA,
                    //    Unit = data["qmsPI"]["Unit"],
                    //    EquipmentCode = data["qmsPI"]["EquipmentCode"],
                    //    EquipmentName = data["qmsPI"]["EquipmentName"]
                    //});

                    db.UseTransaction(true);

                    int nA = db.Sql(sqlA).Execute();
                    int nB = db.Sql(sqlB).Execute();
                    bool result = nA > 0 && nB > 0;

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
                        Result = result
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
        /// 录入计划实际开始与结束时间
        /// </summary>
        /// <param name="ppdID">ID</param>
        /// <param name="type">0:开始  1:结束</param>
        /// <returns></returns>
        public dynamic GetProducePlanActualTime(int ppdID, int type)
        {
            try
            {
                string sql = string.Empty;
                DateTime dTime = DateTime.Now;

                if (type.Equals(0))
                {
                    sql = string.Format(@"UPDATE dbo.APS_ProjectProduceDetial SET ActualStartTime = '{0}' WHERE ID = {1}", dTime, ppdID);
                }
                else if (type.Equals(1))
                {
                    sql = string.Format(@"UPDATE dbo.APS_ProjectProduceDetial SET ActualFinishTime = '{0}' WHERE ID = {1}", dTime, ppdID);
                }
                else
                {
                    throw new Exception(@"参数错误！");
                }

                using (var db = Db.Context("Mms"))
                {
                    int count = db.Sql(sql).Execute();
                    return new ResultModel()
                    {
                        Result = count > 0
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
        /// 执行设备保养计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic EQPMaintenancePlan(dynamic data)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    int type = Convert.ToInt32(data["Type"]);

                    string strEQPMaintenancePlansql = string.Empty;
                    string strEQPsql = string.Empty;

                    //开始保养
                    if (type.Equals(0))
                    {
                        strEQPMaintenancePlansql = string.Format(@"UPDATE EQP_EquipmentMaintenancePlan SET ActualStartTime = '{0}',MaintenanceMan = '{1}',ModifyTime = '{0}' WHERE ID = {2} ",
                        DateTime.Now, data["EQPMaintenancePlanData"]["MaintenanceMan"], data["EQPMaintenancePlanData"]["ID"]);

                        strEQPsql = string.Format(@"UPDATE dbo.SYS_Equipment SET EquipmentState = '{0}',ModifyTime = '{1}' WHERE ID = {2} ", 2, DateTime.Now, data["EQPData"]["ID"]);

                    }
                    //结束保养
                    else if (type.Equals(1))
                    {
                        strEQPMaintenancePlansql = string.Format(@"UPDATE [EQP_EquipmentMaintenancePlan] SET ActualFinishTime = '{0}',MaintenanceMan = '{1}',ModifyTime = '{0}' WHERE ID = {2} ",
                        DateTime.Now, data["EQPMaintenancePlanData"]["MaintenanceMan"], data["EQPMaintenancePlanData"]["ID"]);

                        strEQPsql = string.Format(@"UPDATE dbo.SYS_Equipment SET EquipmentState = '{0}',ModifyTime = '{1}' WHERE ID = {2} ", 1, DateTime.Now, data["EQPData"]["ID"]);
                    }
                    else
                    {
                        throw new Exception(@"内部参数错误！");
                    }

                    try
                    {
                        db.UseTransaction(true);
                        bool result = true;

                        int countA = db.Sql(strEQPMaintenancePlansql).Execute();
                        int countB = db.Sql(strEQPsql).Execute();

                        if (countA > 0 && countB > 0)
                        {
                            db.Commit();
                        }
                        else
                        {
                            db.Rollback();
                            result = false;
                        }

                        return new ResultModel()
                        {
                            Result = result
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ResultModel()
                        {
                            Result = false,
                            Msg = ex.ToString()
                        };
                    }

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
        /// 根据零件编码生成条码
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCodeByPCode(string partCode)
        {
            try
            {
                if (string.IsNullOrEmpty(partCode))
                {
                    throw new Exception(@"参数错误！");
                }

                string sqlA = string.Format(@"
SELECT *
FROM dbo.SYS_Part
WHERE IsEnable = 1
      AND PartCode = '{0}'
      AND
      (
          CorrespondingBarcode IS NULL
          OR CorrespondingBarcode = ''
      );
", partCode);

                using (var db = Db.Context("Mms"))
                {
                    if (db.Sql(sqlA).QueryMany<dynamic>().Count > 0)
                    {
                        string sqlB = string.Format(@"
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
WHERE PartCode = '{0}'
      AND
      (
          CorrespondingBarcode IS NULL
          OR CorrespondingBarcode = ''
      );
SELECT @barcode;
", partCode);
                        return new ResultModel
                        {
                            Result = true,
                            Data = new
                            {
                                GetBarCode = db.Sql(sqlB).QuerySingle<string>()
                            }
                        };
                    }
                    else
                    {
                        return new ResultModel
                        {
                            Result = false,
                            Msg = @"零件编码不存在！"
                        };
                    }
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
        /// 根据零件InventoryCode生成条码
        /// </summary>
        /// <param name="inventoryCode"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCodeByICode(string inventoryCode)
        {
            try
            {
                if (string.IsNullOrEmpty(inventoryCode))
                {
                    throw new Exception(@"参数错误！");
                }

                string sqlA = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", inventoryCode);

                using (var db = Db.Context("Mms"))
                {
                    var partList = db.Sql(sqlA).QueryMany<SYS_Part>();

                    if (partList.Count > 0)
                    {
                        if (string.IsNullOrEmpty(partList[0].CorrespondingBarcode))
                        {
                            string sqlB = string.Format(@"
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
WHERE InventoryCode = '{0}'
SELECT @barcode;
", inventoryCode);
                            return new ResultModel
                            {
                                Result = true,
                                Data = db.Sql(sqlB).QuerySingle<string>()
                            };
                        }
                        else
                        {
                            return new ResultModel
                            {
                                Result = true,
                                Data = partList[0].CorrespondingBarcode
                            };
                        }
                    }
                    else
                    {
                        return new ResultModel
                        {
                            Result = false,
                            Msg = @"零件InventoryCode不存在！"
                        };
                    }
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
        /// 下料记录-更新左侧点击数据
        /// </summary>
        /// <param name="iCode"></param>
        /// <param name="bNumber"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetUpdateBlankingRecordLeftData(string iCode, string bNumber, int id)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"
UPDATE c
SET c.IsPicking = 1,
    c.InventoryCode = '{0}',
    c.BatchNumber = '{1}'
FROM Mes_BlankingResult c
WHERE c.ID = '{2}';
", iCode, bNumber, id);

                    bool result = db.Sql(sql).Execute() > 0;

                    return new ResultModel()
                    {
                        Result = result
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
        /// 下料记录-更新右侧点击数据
        /// </summary>
        /// <param name="iCode"></param>
        /// <param name="bNumber"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetUpdateBlankingRecordRightData(string iCode, string bNumber, int id)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"
UPDATE c
SET c.IsPicking = 0,
    c.InventoryCode = '{0}',
    c.BatchNumber = '{1}'
FROM Mes_BlankingResult c
WHERE c.ID = '{2}';
", iCode, bNumber, id);

                    bool result = db.Sql(sql).Execute() > 0;

                    return new ResultModel()
                    {
                        Result = result
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


        #endregion
        //**********************************************************************************************************//
        #region 查文件路径

        /// <summary>
        /// 根据ID获取 PartFile 表 文件路径
        /// </summary>
        /// <param name="id"></param>
        public string GetDrawingPathByID(int id)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"SELECT FileAddress FROM dbo.PMS_BN_PartFile WHERE ID = {0}", id);

                    return db.Sql(sql).QuerySingle<string>();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据ID获取 QMS_QualityReportDoc 表 文件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetQualityReportDocFilePathByID(int id)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"SELECT FileAddress FROM dbo.QMS_QualityReportDoc WHERE IsEnable = 1 AND ID = {0}", id);

                    return db.Sql(sql).QuerySingle<string>();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据ID获取 PRS_ProcessFigure 表 文件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetProcessFigureFilePathByID(int id)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string sql = string.Format(@"SELECT FileAddress FROM dbo.PRS_ProcessFigure WHERE IsEnable = 1 AND ID = {0}", id);

                    return db.Sql(sql).QuerySingle<string>();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion
        //**********************************************************************************************************//
        #region PDM文件上传

        private List<PartFileModel> Docs = new List<PartFileModel>();

        private List<BomModel> Parts = new List<BomModel>();

        /// <summary>
        /// 导入xml到数据库
        /// </summary>
        public ResultModel ImportDatabase(string strXML, long taskID, long projectID, string projectName, long projectDetailID, string contractCode, int designType, string designTaskResultType, string designTaskResultText)
        {
            try
            {
                //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strXML));
                //RootNewBModel root = new XmlSerializer(typeof(RootNewBModel)).Deserialize(stream) as RootNewBModel;

                RootNewModel GetRoot = GetProductDataByLocalXML(strXML);
                DateTime newDT = DateTime.Now;

                using (var db = Db.Context("Mms"))
                {
                    var partFiles = db.Sql(string.Format(@"SELECT * FROM PMS_BN_PartFile WHERE IsEnable = 1")).QueryMany<PMS_BN_PartFile>();
                    Dictionary<int, string> docNames = new Dictionary<int, string>();
                    partFiles.ForEach(item =>
                    {
                        docNames.Add(item.ID, item.DocName);
                    });

                    var boms = db.Sql(string.Format(@"SELECT * FROM SYS_BOM WHERE IsEnable = 1")).QueryMany<SYS_BOM>();
                    Dictionary<int, string> partCodeVersions = new Dictionary<int, string>();
                    boms.ForEach(item =>
                    {
                        partCodeVersions.Add(item.ID, item.PartCode + item.VersionCode + item.ParentCode);
                    });

                    StringBuilder sbPartFileSQL = new StringBuilder();
                    StringBuilder sbBomSQL = new StringBuilder();

                    int sort = 0;

                    for (int i = 0; i < Parts.Count; i++)
                    {
                        Interlocked.Increment(ref sort);
                        Parts[i].Sort = sort;
                    }

                    foreach (var item in Docs)
                    {
                        //已存在，需要更新
                        if (docNames.Values.Contains(item.DocName))
                        {
                            int id = docNames.FirstOrDefault(i => i.Value.Equals(item.DocName)).Key;

                            PMS_BN_PartFile oldModel = partFiles.SingleOrDefault(s => s.ID.Equals(id));
                            sbPartFileSQL.Append(GetUpdateSQL(oldModel, new
                            {
                                //ID = id,
                                ContractCode = contractCode,
                                ProjectID = projectID.ToString(),
                                ProjectDetailID = projectDetailID.ToString(),
                                ProjectName = projectName,
                                PPartCode = item.PPartCode,
                                PPartName = item.PPartName,
                                PartCode = item.PartCode,
                                PartName = item.PartName,
                                DocName = item.DocName,
                                FileName = item.FileName,
                                FileAddress = item.FileAddress,
                                State = item.State,
                                IsEnable = item.IsEnable,
                                ModifyPerson = item.ModifyPerson,
                                ModifyTime = DateTime.Now
                            }));
                        }
                        //不存在，需要新增
                        else
                        {
                            sbPartFileSQL.Append(GetInsertSQL(new PMS_BN_PartFile()
                            {
                                ContractCode = contractCode,
                                ProjectID = projectID.ToString(),
                                ProjectDetailID = projectDetailID.ToString(),
                                ProjectName = projectName,
                                PPartCode = item.PPartCode,
                                PPartName = item.PPartName,
                                PartCode = item.PartCode,
                                PartName = item.PartName,
                                DocName = item.DocName,
                                FileName = item.FileName,
                                FileAddress = item.FileAddress,
                                State = item.State,
                                IsEnable = item.IsEnable,
                                CreatePerson = item.CreatePerson,
                                CreateTime = DateTime.Now,
                                ModifyPerson = item.ModifyPerson,
                                ModifyTime = DateTime.Now,
                                FileType = "1"
                            }));
                        }
                    }

                    foreach (var item in Parts)
                    {
                        //已存在，需要更新
                        if (partCodeVersions.Values.Contains(item.PartCode + item.VersionCode + item.ParentCode))
                        {
                            int id = partCodeVersions.FirstOrDefault(i => i.Value.Equals(item.PartCode + item.VersionCode + item.ParentCode)).Key;

                            SYS_BOM oldModel = boms.SingleOrDefault(s => s.ID.Equals(id));
                            sbBomSQL.Append(GetUpdateSQL(oldModel, new
                            {
                                //ID = id,
                                PartFigureCode = item.PartFigureCode,
                                PartCode = item.PartCode,
                                PartName = item.PartName,
                                InventoryCode = item.InventoryCode,
                                InventoryName = item.InventoryName,
                                PartQuantity = item.PartQuantity,
                                ParentCode = item.ParentCode,
                                Weight = item.Weight,
                                Totalweight = item.Totalweight,
                                MaterialCode = item.MaterialCode,
                                MaterialInventoryCode = item.MaterialInventoryCode,
                                MaterialInventoryName = item.MaterialInventoryName,
                                MaterialQuantity = item.MaterialQuantity,
                                VersionCode = item.VersionCode,
                                IsEnable = 1,
                                ModifyPerson = item.ModifyPerson,
                                ModifyTime = DateTime.Now,
                                IsSelfMade = (item.PartType.Equals("WG") || item.PartType.Equals("wg")) ? "0" : "1",
                                PartType = item.PartType,
                                Remark = item.Remark
                            }));
                        }
                        //不存在，需要新增
                        else
                        {
                            sbBomSQL.Append(GetInsertSQL(new SYS_BOM()
                            {
                                PartFigureCode = item.PartFigureCode,
                                PartCode = item.PartCode,
                                PartName = item.PartName,
                                InventoryCode = item.InventoryCode,
                                InventoryName = item.InventoryName,
                                PartQuantity = item.PartQuantity,
                                ParentCode = item.ParentCode,
                                Weight = item.Weight,
                                Totalweight = item.Totalweight,
                                MaterialCode = item.MaterialCode,
                                MaterialInventoryCode = item.MaterialInventoryCode,
                                MaterialInventoryName = item.MaterialInventoryName,
                                MaterialQuantity = item.MaterialQuantity,
                                VersionCode = item.VersionCode,
                                IsEnable = 1,
                                CreatePerson = item.CreatePerson,
                                CreateTime = DateTime.Now,
                                ModifyPerson = item.ModifyPerson,
                                ModifyTime = DateTime.Now,
                                IsSelfMade = (item.PartType.Equals("WG") || item.PartType.Equals("wg")) ? "0" : "1",
                                PartType = item.PartType,
                                Remark = item.Remark,
                                Sort = item.Sort
                            }));
                        }
                    }

                    string strPartFileSQL = sbPartFileSQL.ToString();
                    string strBomSQL = sbBomSQL.ToString();
                    //string strDesignTaskDetailSQL = string.Format(@"UPDATE [PMS_DesignTaskDetail] SET [PartCode] = '{0}',[VersionCode] = '{1}',ActualFinishTime = GETDATE() WHERE ID = {2}", GetRoot.Product[0].Code, GetRoot.Product[0].Version, taskID);
                    string strDesignTaskDetailSQL = string.Format(@"UPDATE [PMS_DesignTaskDetail] SET [PartCode] = '{0}',[VersionCode] = '{1}' WHERE ID = {2}", GetRoot.Product[0].Code, GetRoot.Product[0].Version, taskID);
                    string strDesignTaskResultSQL = GetInsertSQL(new PMS_DesignTaskResult()
                    {
                        MainID = (int)taskID,
                        ContractCode = contractCode,
                        ProductID = projectDetailID.ToString(),
                        TaskType = designTaskResultType,
                        TaskDescription = designTaskResultText,
                        PartCode = GetRoot.Product[0].Code,
                        ActualFinishTime = newDT,
                        VersionCode = GetRoot.Product[0].Version == null ? (int?)null : Convert.ToInt32(GetRoot.Product[0].Version),
                        IsEnable = 1,
                        CreateTime = newDT,
                        ModifyTime = newDT
                    });

                    db.UseTransaction(true);
                    int countA = db.Sql(strPartFileSQL).Execute();
                    int countB = db.Sql(strBomSQL).Execute();
                    int countC = db.Sql(strDesignTaskDetailSQL).Execute();
                    int countD = db.Sql(strDesignTaskResultSQL).Execute();

                    bool result = true;
                    if (countA > 0 && countB > 0 && countC > 0 && countD > 0)
                    {
                        db.Commit();
                    }
                    else
                    {
                        db.Rollback();
                        result = false;
                    }

                    return new ResultModel()
                    {
                        Result = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = "XML文件写入数据库失败！\n详情：" + ex.Message
                };
            }
        }

        /// <summary>
        /// 更新PDM文件上传任务 完成结束时间
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public ResultModel SaveProductDataUpKillDate(long taskID)
        {
            try
            {
                using (var db = Db.Context("Mms"))
                {
                    string strDesignTaskDetailSQL = string.Format(@"UPDATE [PMS_DesignTaskDetail] SET ActualFinishTime = GETDATE() WHERE ID = {0}", taskID);
                    int count = db.Sql(strDesignTaskDetailSQL).Execute();

                    return new ResultModel()
                    {
                        Result = count > 0,
                        Msg = count > 0 ? null : "更新实际结束时间失败！"
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
        /// 读取XML文件 -- 新建设计项目
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public RootNewModel GetProductDataByLocalXML(string strXML)
        {
            //xml文件读取的对象
            RootNewModel result = new RootNewModel();

            //文档对象
            XmlDocument doc = new XmlDocument();
            //加载xml文件
            doc.LoadXml(strXML);

            //查询项目节点（项目有一个，所以是对象）
            XmlNode projectNode = doc.SelectSingleNode("/ROOT/PROJECT");

            //保存项目属性信息
            result.Project.Projectid = projectNode.Attributes["PROJECTID"].Value;
            result.Project.Projectname = projectNode.Attributes["PROJECTNAME"].Value;

            //获取产品节点（产品有多个，所以是集合）
            XmlNodeList ProductNodes = doc.SelectNodes("/ROOT//PRODUCT");

            //遍历产品
            foreach (XmlNode itemA in ProductNodes)
            {
                //实例化一个产品对象
                RootNewModel.ProductModel productModel = new RootNewModel.ProductModel();

                //获取这个产品下的所有DOC文档节点
                XmlNodeList DOCNodesA = itemA.SelectNodes("DOC");
                //获取这个产品下的所有Part零件节点
                XmlNodeList PartNodesA = itemA.SelectNodes("PART");

                //获取产品信息（属性值）
                productModel.Code = itemA.Attributes["CODE"].Value;
                productModel.Code1 = itemA.Attributes["CODE1"].Value;
                productModel.Version = itemA.Attributes["VERSION"].Value;
                productModel.Name = itemA.Attributes["NAME"].Value;
                productModel.Quantity = itemA.Attributes["QUANTITY"].Value;
                productModel.Material = itemA.Attributes["MATERIAL"].Value;
                productModel.Sigweight = itemA.Attributes["SIGWEIGHT"].Value;
                productModel.TotWeight = itemA.Attributes["TOTWEIGHT"].Value;
                productModel.Remark = itemA.Attributes["REMARK"].Value;
                productModel.FaHCode = itemA.Attributes["FAHCODE"].Value;

                Parts.Add(new BomModel()
                {
                    PartFigureCode = productModel.Code1,
                    PartCode = productModel.Code,
                    PartName = productModel.Name,
                    PartQuantity = string.IsNullOrEmpty(productModel.Quantity) ? (int?)null : Convert.ToInt32(productModel.Quantity),
                    ParentCode = null,
                    Weight = productModel.Sigweight,
                    Totalweight = productModel.TotWeight,
                    MaterialCode = productModel.Material,
                    VersionCode = productModel.Version,
                    IsEnable = 1,
                    PartType = string.Empty,
                    Remark = productModel.Remark
                });

                //遍历产品文档
                foreach (XmlNode itemB1 in DOCNodesA)
                {
                    RootNewModel.ProductModel.DocModel docModel = new RootNewModel.ProductModel.DocModel()
                    {
                        Code = itemB1.Attributes["CODE"].Value,
                        Code1 = itemB1.Attributes["CODE1"].Value,
                        Name = itemB1.Attributes["NAME"].Value,
                        Version = itemB1.Attributes["VERSION"].Value,
                        Gcname = itemB1.Attributes["GCNAME"].Value,
                        Page = itemB1.Attributes["PAGE"].Value,
                        Totpage = itemB1.Attributes["TOTPAGE"].Value,
                        Filename = itemB1.Attributes["FILENAME"].Value
                    };
                    productModel.Doc.Add(docModel);

                    string mapPath = GetFileUpLoadPath(new
                    {
                        RootPath = "PMS_BN_PartFile",
                        FileType = Path.GetExtension(docModel.Filename)
                    }).Data;
                    Docs.Add(new PartFileModel()
                    {
                        PPartCode = productModel.Code,
                        PPartName = productModel.Name,
                        PartCode = docModel.Code,
                        PartName = docModel.Name,
                        DocName = docModel.Filename,
                        FileName = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), new Random(Guid.NewGuid().GetHashCode()).Next(10000, 100000), Path.GetExtension(docModel.Filename)),
                        FileAddress = mapPath + docModel.Filename
                    });
                }

                //遍历所有一级Part零件
                foreach (XmlNode itemB2 in PartNodesA)
                {
                    //当前零件的父级节点，即当前产品节点
                    XmlNode productNode = itemB2.ParentNode;

                    //通过递归方法获取当前零件下的所有Part零件以及DOC文档
                    productModel.Part.Add(GetPart(itemB2));
                }

                //将当前产品信息添加到xml对象的产品里
                result.Product.Add(productModel);

            }

            return result;
        }

        /// <summary>
        /// 递归-获取Part零件节点下的所有Part零件以及Doc文档
        /// </summary>
        /// <param name="partNode">Part零件节点</param>
        /// <returns></returns>
        public RootNewModel.ProductModel.PartModel GetPart(XmlNode partNode)
        {
            RootNewModel.ProductModel.PartModel result = new RootNewModel.ProductModel.PartModel();

            XmlNodeList DOCNodes = partNode.SelectNodes("DOC");
            XmlNodeList PartNodes = partNode.SelectNodes("PART");

            result.Code = partNode.Attributes["CODE"].Value;
            result.Code1 = partNode.Attributes["CODE1"].Value;
            result.Version = partNode.Attributes["VERSION"].Value;
            result.Name = partNode.Attributes["NAME"].Value;
            result.Quantity = partNode.Attributes["QUANTITY"].Value;
            result.Material = partNode.Attributes["MATERIAL"].Value;
            result.Sigweight = partNode.Attributes["SIGWEIGHT"].Value;
            result.TotWeight = partNode.Attributes["TOTWEIGHT"].Value;
            result.Remark = partNode.Attributes["REMARK"].Value;
            result.FaHCode = partNode.Attributes["FAHCODE"].Value;
            result.PartType = partNode.Attributes["PARTTYPE"].Value;

            foreach (XmlNode item in DOCNodes)
            {
                RootNewModel.ProductModel.DocModel docModel = new RootNewModel.ProductModel.DocModel()
                {
                    Code = item.Attributes["CODE"].Value,
                    Code1 = item.Attributes["CODE1"].Value,
                    Name = item.Attributes["NAME"].Value,
                    Version = item.Attributes["VERSION"].Value,
                    Gcname = item.Attributes["GCNAME"].Value,
                    Page = item.Attributes["PAGE"].Value,
                    Totpage = item.Attributes["TOTPAGE"].Value,
                    Filename = item.Attributes["FILENAME"].Value
                };
                result.Doc.Add(docModel);

                string mapPath = GetFileUpLoadPath(new
                {
                    RootPath = "PMS_BN_PartFile",
                    FileType = Path.GetExtension(docModel.Filename)
                }).Data;
                Docs.Add(new PartFileModel()
                {
                    PPartCode = partNode.ParentNode.Attributes["CODE"].Value,
                    PPartName = partNode.ParentNode.Attributes["NAME"].Value,
                    PartCode = docModel.Code,
                    PartName = docModel.Name,
                    DocName = docModel.Filename,
                    FileName = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), new Random(Guid.NewGuid().GetHashCode()).Next(10000, 100000), Path.GetExtension(docModel.Filename)),
                    FileAddress = mapPath + docModel.Filename
                });
            }

            Parts.Add(new BomModel()
            {
                PartFigureCode = result.Code1,
                PartCode = result.Code,
                PartName = result.Name,
                PartQuantity = string.IsNullOrEmpty(result.Quantity) ? (int?)null : Convert.ToInt32(result.Quantity),
                ParentCode = partNode.ParentNode.Attributes["CODE"].Value,
                VersionCode = result.Version,
                Weight = result.Sigweight,
                Totalweight = result.TotWeight,
                MaterialCode = result.Material,
                IsEnable = 1,
                PartType = result.PartType,
                Remark = result.Remark
            });

            foreach (XmlNode item in PartNodes)
            {
                result.Part.Add(GetPart(item));
            }

            return result;
        }

        /// <summary>
        /// 读取XML文件 -- 设计更改申请
        /// </summary>
        /// <param name="strXmlFilePath"></param>
        /// <returns></returns>
        public RootChangeModel GetDocDataByLocalXML(string strXML)
        {
            //xml文件读取的对象
            RootChangeModel result = new RootChangeModel();

            //文档对象
            XmlDocument doc = new XmlDocument();

            //加载xml文件
            doc.LoadXml(strXML);

            //查询文档节点
            XmlNode changeQNode = doc.SelectSingleNode("/root/changeq");
            XmlNode changeNode = doc.SelectSingleNode("/root/change");

            //保存文档属性信息
            result.ChangeQ = new RootChangeModel.FileModel() { FileName = changeQNode.Attributes["filename"].Value };
            result.Change = new RootChangeModel.FileModel() { FileName = changeNode.Attributes["filename"].Value };

            //获取Doc节点（Doc节点有多个，所以是集合）
            XmlNodeList DocNodes = doc.SelectNodes("/root//doc");

            //遍历Doc
            foreach (XmlNode itemA in DocNodes)
            {
                //实例化一个Doc对象
                RootChangeModel.DocModel docModel = new RootChangeModel.DocModel()
                {
                    Code = itemA.Attributes["code"].Value,
                    Code1 = itemA.Attributes["code1"].Value,
                    Name = itemA.Attributes["name"].Value,
                    Version = itemA.Attributes["version"].Value,
                    Gcname = itemA.Attributes["gcname"].Value,
                    Page = itemA.Attributes["page"].Value,
                    Totpage = itemA.Attributes["totpage"].Value,
                    Filename = itemA.Attributes["filename"].Value,
                    Part = new List<RootChangeModel.DocModel.PartModel>()
                };
                //获取所有Part零件节点
                XmlNodeList PartNodes = itemA.SelectNodes("part");

                //遍历产品文档
                foreach (XmlNode itemB1 in PartNodes)
                {
                    docModel.Part.Add(new RootChangeModel.DocModel.PartModel()
                    {
                        Code = itemB1.Attributes["code"].Value,
                        Code1 = itemB1.Attributes["code1"].Value,
                        Name = itemB1.Attributes["name"].Value,
                        Material = itemB1.Attributes["material"].Value,
                        Sigweight = itemB1.Attributes["sigweight"].Value,
                        TotWeight = itemB1.Attributes["totweight"].Value,
                        Remark = itemB1.Attributes["remark"].Value
                    });
                }

                //将当前Doc信息添加到xml对象的Doc里
                result.Doc.Add(docModel);
            }
            return result;
        }

        #endregion
        //**********************************************************************************************************//
        #region 路径配置

        /// <summary>
        /// 根据xml文件配置 动态获取文件上传路径
        /// </summary>
        /// <param name="obj">
        /// RootPath--参数是必须的，该值为path节点属性dir值
        /// FileType--可选参数，该值为文件扩展名，如.txt、txt
        /// </param>
        /// <returns></returns>
        public static ResultModel<string> GetFileUpLoadPath(object obj)
        {
            try
            {
                ResultModel<string> result = new ResultModel<string>();

                string xmlFilePath = HttpContext.Current.Server.MapPath("~/Views/Shared/UploadXml/FileUploadPathSetting.xml");
                string fileRootPath = HttpContext.Current.Server.MapPath("~/Upload/");
                RootFilePathModel rootModel = GetRoot(xmlFilePath);
                RootFilePathModel.PathModel pathModel;

                List<RootFilePathModel.PathModel> pathModels = new List<RootFilePathModel.PathModel>();

                string rootPathValue = string.Empty;

                var rootPathAttr = obj.GetType().GetProperty("RootPath");
                if (rootPathAttr == null)
                {
                    throw new Exception(string.Format(@"缺少参数[{0}]！", "RootPath"));
                }
                else
                {
                    var rootPathValueObj = rootPathAttr.GetValue(obj, null);
                    if (rootPathValueObj == null)
                    {
                        throw new Exception(string.Format(@"参数[{0}]值不能为空！", "RootPath"));
                    }
                    else
                    {
                        rootPathValue = rootPathValueObj.ToString();
                    }

                    if (string.IsNullOrEmpty(rootPathValue))
                    {
                        throw new Exception(string.Format(@"无效的[{0}]参数值！", "RootPath"));
                    }

                    pathModels = rootModel.Path.Where(item => item.Dir.Equals(rootPathValue)).ToList();
                }

                if (pathModels.Count <= 0)
                {
                    throw new Exception(string.Format(@"未读取到[{0}]路径！", rootPathValue));
                }
                else if (pathModels.Count.Equals(1))
                {
                    pathModel = pathModels[0];

                    var list = pathModel.Nodes.GroupBy(item => item.Level).Where(item => item.Count() > 1).ToList();
                    if (list.Count > 0)
                    {
                        throw new Exception(string.Format(@"XML文件配置错误：排序属性[level]有重复值，此值唯一！"));
                    }

                    pathModel.Nodes.Sort((a, b) => a.Level.CompareTo(b.Level));
                }
                else
                {
                    throw new Exception(string.Format(@"读取到多个[{0}]路径！", rootPathValue));
                }

                foreach (RootFilePathModel.PathModel.NodeModel node in pathModel.Nodes)
                {
                    object newAttr = obj.GetType().GetProperty(node.Parm);
                    string newValue = string.Empty;

                    if (newAttr == null)
                    {
                        throw new Exception(string.Format(@"缺少参数[{0}]！", node.Parm));
                    }
                    else
                    {
                        object newValueObj = obj.GetType().GetProperty(node.Parm).GetValue(obj, null);
                        if (newValueObj == null)
                        {
                            throw new Exception(string.Format(@"参数[{0}]值不能为空！", node.Parm));
                        }
                        else
                        {
                            newValue = newValueObj.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(newValue))
                    {
                        throw new Exception(string.Format(@"无效的[{0}]参数值！", node.Parm));
                    }

                    if (node.Parm.Equals("FileType") || node.Dir.Contains("文件类型"))
                    {
                        if (newValue.Contains("."))
                        {
                            int index = newValue.LastIndexOf('.');
                            newValue = newValue.Substring(index + 1, newValue.Length - index - 1);
                        }
                        newValue = newValue.ToUpper();
                    }

                    if (newValue.Contains('\\') || newValue.Contains('/') || newValue.Contains(':') || newValue.Contains('*') || newValue.Contains('?') ||
                        newValue.Contains('"') || newValue.Contains('<') || newValue.Contains('>') || newValue.Contains('|'))
                    {
                        throw new Exception(string.Format("无效的[{0}]参数值！\n提示：文件名/文件夹名不能包含【\\、/、:、*、?、\"、<、>、|】字符！", node.Parm));
                    }

                    result.Data += string.Format(@"{0}\", newValue);
                }

                result.Data = fileRootPath + pathModel.Dir + @"\" + result.Data;
                result.Result = true;

                if (!Directory.Exists(result.Data))
                {
                    Directory.CreateDirectory(result.Data);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel<string>()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <returns></returns>
        private static RootFilePathModel GetRoot(string filePath)
        {
            RootFilePathModel result = new RootFilePathModel();
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList pathNodes = doc.SelectNodes("/root/path");
            foreach (XmlNode pathNode in pathNodes)
            {
                RootFilePathModel.PathModel pathModel = new RootFilePathModel.PathModel();
                pathModel.Dir = pathNode.Attributes["dir"].Value;
                XmlNodeList nodes = pathNode.SelectNodes("node");
                foreach (XmlNode node in nodes)
                {
                    pathModel.Nodes.Add(new RootFilePathModel.PathModel.NodeModel()
                    {
                        Dir = node.Attributes["dir"].Value,
                        Level = Convert.ToInt32(node.Attributes["level"].Value),
                        Parm = node.Attributes["parm"].Value
                    });
                }
                result.Path.Add(pathModel);
            }
            return result;
        }

        #endregion
        //**********************************************************************************************************//
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

        public static string GetUpdateSQL(string tableName, Dictionary<string, object> keyValuePairs, object newModel)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbWheres = new StringBuilder();

            foreach (KeyValuePair<string, object> kvp in keyValuePairs)
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

        public static string GetUpdateSQL(string tableName, KeyValuePair<string, object> keyValuePair, object newModel)
        {
            StringBuilder sbValues = new StringBuilder();

            var newAttrArr = newModel.GetType().GetProperties();

            foreach (var item in newAttrArr)
            {
                var newValue = newModel.GetType().GetProperty(item.Name).GetValue(newModel, null);
                sbValues.Append(string.Format(@"[{0}] = '{1}',", item.Name, newValue));
            }

            string strValues = sbValues.ToString();

            return string.Format(@"update {0} set {1} where [{2}] = '{3}';", tableName, strValues.Substring(0, strValues.Length - 1), keyValuePair.Key, keyValuePair.Value);
        }

        #endregion
        //**********************************************************************************************************//
        #region 公用方法

        /// <summary>
        /// 获取字符串、文件流的MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5WithString(string input)
        {
            MD5 md5Hash = MD5.Create();
            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sb = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x4"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }
            // 返回十六进制字符串  
            return sb.ToString();
        }

        public static string GetMD5WithFile(FileStream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(stream);
            stream.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x4"));
            }
            return sb.ToString();
        }


        #endregion
        //**********************************************************************************************************//
    }

    public class WinFormClient : ModelBase
    {

    }

    public class ResultModel
    {
        public bool Result { get; set; }

        public object Data { get; set; }

        public string Msg { get; set; }
    }

    public class ResultModel<T> where T : class
    {
        public bool Result { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }
    }

    /// <summary>
    /// 文件路径 配置文件 XML模型
    /// </summary>
    public class RootFilePathModel
    {
        public RootFilePathModel()
        {
            Path = new List<PathModel>();
        }

        public List<PathModel> Path { get; set; }

        public class PathModel
        {
            public PathModel()
            {
                Nodes = new List<NodeModel>();
            }

            public string Dir { get; set; }

            public List<NodeModel> Nodes { get; set; }

            public class NodeModel
            {
                public string Dir { get; set; }

                public int Level { get; set; }

                public string Parm { get; set; }
            }
        }
    }

    /// <summary>
    /// 新建设计任务 XML模型
    /// </summary>
    public class RootNewModel
    {
        public RootNewModel()
        {
            Project = new ProjectModel();

            Product = new List<ProductModel>();
        }

        public ProjectModel Project { get; set; }

        public List<ProductModel> Product { get; set; }

        public class ProjectModel
        {
            public string Projectid { get; set; }

            public string Projectname { get; set; }
        }

        public class ProductModel
        {
            public ProductModel()
            {
                Doc = new List<DocModel>();

                Part = new List<PartModel>();
            }

            public List<DocModel> Doc { get; set; }

            public List<PartModel> Part { get; set; }

            public string Code { get; set; }

            public string Code1 { get; set; }

            public string Version { get; set; }

            public string Name { get; set; }

            public string Quantity { get; set; }

            public string Material { get; set; }

            public string Sigweight { get; set; }

            public string TotWeight { get; set; }

            public string Remark { get; set; }

            public string FaHCode { get; set; }


            public class DocModel
            {
                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Name { get; set; }

                public string Version { get; set; }

                public string Gcname { get; set; }

                public string Page { get; set; }

                public string Totpage { get; set; }

                public string Filename { get; set; }

            }

            public class PartModel
            {
                public PartModel()
                {
                    Doc = new List<DocModel>();

                    Part = new List<PartModel>();
                }

                public List<DocModel> Doc { get; set; }

                public List<PartModel> Part { get; set; }

                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Version { get; set; }

                public string Name { get; set; }

                public string Quantity { get; set; }

                public string Material { get; set; }

                public string Sigweight { get; set; }

                public string TotWeight { get; set; }

                public string Remark { get; set; }

                public string FaHCode { get; set; }

                public string PartType { get; set; }
            }

        }
    }

    [XmlRoot("ROOT")]
    public class RootNewBModel
    {
        public RootNewBModel()
        {
            Project = new ProjectModel();

            Product = new ProductModel();
        }

        [XmlElement("PROJECT")]
        public ProjectModel Project { get; set; }

        [XmlElement("PRODUCT")]
        public ProductModel Product { get; set; }

        public class ProjectModel
        {
            [XmlAttribute("PROJECTID")]
            public string Projectid { get; set; }

            [XmlAttribute("PROJECTNAME")]
            public string Projectname { get; set; }
        }

        public class ProductModel
        {
            public ProductModel()
            {
                Doc = new List<DocModel>();

                Part = new List<PartModel>();
            }

            [XmlElement("DOC")]
            public List<DocModel> Doc { get; set; }

            [XmlElement("PART")]
            public List<PartModel> Part { get; set; }

            [XmlAttribute("CODE")]
            public string Code { get; set; }

            [XmlAttribute("CODE1")]
            public string Code1 { get; set; }

            [XmlAttribute("VERSION")]
            public string Version { get; set; }

            [XmlAttribute("NAME")]
            public string Name { get; set; }

            [XmlAttribute("QUANTITY")]
            public string Quantity { get; set; }

            [XmlAttribute("MATERIAL")]
            public string Material { get; set; }

            [XmlAttribute("SIGWEIGHT")]
            public string Sigweight { get; set; }

            [XmlAttribute("TOTWEIGHT")]
            public string TotWeight { get; set; }

            [XmlAttribute("REMARK")]
            public string Remark { get; set; }

            [XmlAttribute("FAHCODE")]
            public string FaHCode { get; set; }


            public class DocModel
            {
                [XmlAttribute("CODE")]
                public string Code { get; set; }

                [XmlAttribute("CODE1")]
                public string Code1 { get; set; }

                [XmlAttribute("NAME")]
                public string Name { get; set; }

                [XmlAttribute("VERSION")]
                public string Version { get; set; }

                [XmlAttribute("GCNAME")]
                public string Gcname { get; set; }

                [XmlAttribute("PAGE")]
                public string Page { get; set; }

                [XmlAttribute("TOTPAGE")]
                public string Totpage { get; set; }

                [XmlAttribute("FILENAME")]
                public string Filename { get; set; }

            }

            public class PartModel
            {
                public PartModel()
                {
                    Doc = new List<DocModel>();

                    Part = new List<PartModel>();
                }

                [XmlElement("DOC")]
                public List<DocModel> Doc { get; set; }

                [XmlElement("PART")]
                public List<PartModel> Part { get; set; }

                [XmlAttribute("CODE")]
                public string Code { get; set; }

                [XmlAttribute("CODE1")]
                public string Code1 { get; set; }

                [XmlAttribute("VERSION")]
                public string Version { get; set; }

                [XmlAttribute("NAME")]
                public string Name { get; set; }

                [XmlAttribute("QUANTITY")]
                public string Quantity { get; set; }

                [XmlAttribute("MATERIAL")]
                public string Material { get; set; }

                [XmlAttribute("SIGWEIGHT")]
                public string Sigweight { get; set; }

                [XmlAttribute("TOTWEIGHT")]
                public string TotWeight { get; set; }

                [XmlAttribute("REMART")]
                public string Remark { get; set; }

                [XmlAttribute("FAHCODE")]
                public string FaHCode { get; set; }

                [XmlAttribute("PARTTYPE")]
                public string PartType { get; set; }
            }

        }
    }


    /// <summary>
    /// 设计任务更改申请 XML模型
    /// </summary>
    public class RootChangeModel
    {
        public RootChangeModel()
        {
            Doc = new List<DocModel>();
        }

        public FileModel ChangeQ { get; set; }

        public FileModel Change { get; set; }

        public List<DocModel> Doc { get; set; }

        public class FileModel
        {
            public string FileName { get; set; }
        }

        public class DocModel
        {
            public DocModel()
            {
                Part = new List<PartModel>();
            }

            public List<PartModel> Part { get; set; }

            public string Code { get; set; }

            public string Code1 { get; set; }

            public string Name { get; set; }

            public string Version { get; set; }

            public string Gcname { get; set; }

            public string Page { get; set; }

            public string Totpage { get; set; }

            public string Filename { get; set; }


            public class PartModel
            {
                public string Code { get; set; }

                public string Code1 { get; set; }

                public string Version { get; set; }

                public string Name { get; set; }

                public string Quantity { get; set; }

                public string Material { get; set; }

                public string Sigweight { get; set; }

                public string TotWeight { get; set; }

                public string Remark { get; set; }

                public string FaHCode { get; set; }

                public string PartType { get; set; }
            }

        }
    }

    public class PartFileModel
    {
        private int _state = 0;

        private int? _isEnable = 1;


        public int ID { get; set; }

        public string ContractCode { get; set; }

        public string ProjectID { get; set; }

        public string ProjectDetailID { get; set; }

        public string ProjectName { get; set; }

        public string PPartCode { get; set; }

        public string PPartName { get; set; }

        public string PartCode { get; set; }

        public string PartName { get; set; }

        public string DocName { get; set; }

        public string FileName { get; set; }

        public string FileAddress { get; set; }

        public int State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public int? IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable = value;
            }
        }

        public string CreatePerson { get; set; }

        public DateTime? CreateTime { get; set; }

        public string ModifyPerson { get; set; }

        public DateTime? ModifyTime { get; set; }

        public string FileCode { get; set; }

        public string VersionCode { get; set; }

        public string FileType { get; set; }

    }

    public class BomModel
    {
        public int ID { get; set; }

        public string PartFigureCode { get; set; }

        public string PartCode { get; set; }

        public string PartName { get; set; }

        public string InventoryCode { get; set; }

        public string InventoryName { get; set; }

        public int? PartQuantity { get; set; }

        public string ParentCode { get; set; }

        public string Weight { get; set; }

        public string Totalweight { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialInventoryCode { get; set; }

        public string MaterialInventoryName { get; set; }

        public int? MaterialQuantity { get; set; }

        public string VersionCode { get; set; }

        public int? IsEnable { get; set; }

        public string CreatePerson { get; set; }

        public DateTime? CreateTime { get; set; }

        public string ModifyPerson { get; set; }

        public DateTime? ModifyTime { get; set; }

        public string PartType { get; set; }

        public string Remark { get; set; }

        /// <summary>
		/// 排序
		/// </summary>
		public int? Sort { get; set; }
    }
}