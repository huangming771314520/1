using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;
namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class PRS_Process_BOMController : Controller
    {
        /// <summary>
        /// 生产管理：铸件、锻件、其他
        /// 工艺定料功能 菜单改为工艺备料功能 下料尺寸改为备料尺寸 下料数量改为备料数量
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            bool userIsProcessDept = false;

            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"SELECT ID,DepartmentName,DepartmentCode FROM dbo.SYS_BN_Department WHERE IsEnable = 1 AND DepartmentCode = (SELECT DepartmentCode FROM dbo.SYS_BN_User WHERE IsEnable = 1 AND UserCode = '{0}')", MmsHelper.GetUserCode());
                var dept = db.Sql(sql).QuerySingle<dynamic>();

                if (dept != null && dept.DepartmentName == "工艺处")
                {
                    userIsProcessDept = true;
                }
                else
                {
                    userIsProcessDept = false;
                }
            }

            ViewBag.UserIsProcessDept = userIsProcessDept;
            return View();
        }

        /// <summary>
        /// 车间管理：板材、棒材、型材
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2()
        {
            return View();
        }
    }
    public class PRS_Process_BOMApiController : ApiController
    {
        public dynamic GetIntoryNameByCode(string InventoryCode)
        {
            return new SYS_PartService().GetModelList(
                 ParamQuery.Instance()
                .AndWhere("InventoryCode", InventoryCode)
                ).Select(a => new { value = a.InventoryCode, label = a.InventoryName });
        }
        [System.Web.Http.HttpGet]
        public dynamic GetPRS_Process_BOMDetail(string PartCode)
        {
            return new PRS_Process_BOMService().GetModel(ParamQuery.Instance().AndWhere("PartCode", PartCode));
        }

        public int PostSavePartCode(dynamic data)
        {
            string PartCode = data.PartCode;
            string SetMateName = data.SetMateName;
            float SetMateNum = data.SetMateNum;
            string InPlanceSize = data.InPlanceSize;
            string BlankingSize = data.BlankingSize;
            string ContractCode = data.ContractCode;
            string FigureCode = data.FigureCode;
            string PartName = data.PartName;
            int Quantity = data.Quantity == null ? 0 : data.Quantity;
            int PartQuantity = data.PartQuantity == null ? 0 : data.PartQuantity;
            string MaterialCode = data.MaterialCode;
            string New_Specs = data.New_Specs;
            string New_Model = data.New_Model;
            string New_PartName = data.New_PartName;
            int MateType = data.MateType;
            string MateParamValue = data.MateParamValue;
            bool IsSetNewSMP = data.IsSetNewSMP;

            //int BlankingType = data.BlankingType;
            using (var db = Db.Context("Mms"))
            {
                db.UseTransaction(true);
                try
                {
                    //int MainID = new APS_ProductPurchaseMainService().GetModelList().Count > 0 ?
                    //   new APS_ProductPurchaseMainService().GetModelList().Max(a => a.ID) + 1 : 1;
                    int res = 0;

                    if(IsSetNewSMP)
                    {
                        string newInventoryCode = SetMateName;
                        //先获取关联物料的存货名称，型号规格信息
                        SYS_Part part = new SYS_PartService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("InventoryCode", SetMateName));
                        bool modelTrue = New_Model.Equals(part.Model);
                        bool nameTrue = New_PartName.Equals(part.InventoryName);
                        //判断页面传入的零件的星海规格和名称是否与part表中存储的数据一致
                        if (!(modelTrue&& nameTrue))
                        {
                            //不一致，就说明需要新增零件，此时需要保证订料材料的New_InventoryCode 为空，并保存前台用户维护后的型号规格和物料名称
                            res = db.Update("PRS_Process_BOM")
                       .Column("SetMateName", SetMateName)
                       .Column("SetMateNum", SetMateNum)
                       .Column("InPlanceSize", InPlanceSize)
                       .Column("BlankingSize", BlankingSize)
                       .Column("New_Specs", New_Specs)
                       .Column("New_Model", New_Model)
                       .Column("New_PartName", New_PartName)
                       //.Column("New_InventoryCode", newInventoryCode)
                       .Column("MateType", MateType)
                       .Column("MateParamValue", MateParamValue)
                       //.Column("BlankingType", BlankingType)
                       .Where("PartCode", PartCode).Execute();
                        }
                        else
                        {
                            //一致，就说明并不需要新增零件，所以New_InventoryCode就是订料材料的SetMateName
                            New_PartName = "";
                            res = db.Update("PRS_Process_BOM")
                       .Column("SetMateName", SetMateName)
                       .Column("SetMateNum", SetMateNum)
                       .Column("InPlanceSize", InPlanceSize)
                       .Column("BlankingSize", BlankingSize)
                       .Column("New_Specs", New_Specs)
                       .Column("New_Model", New_Model)
                       .Column("New_PartName", New_PartName)
                       .Column("New_InventoryCode", newInventoryCode)
                       .Column("MateType", MateType)
                       .Column("MateParamValue", MateParamValue)
                       //.Column("BlankingType", BlankingType)
                       .Where("PartCode", PartCode).Execute();
                        }
                        
                    }
                    else
                    {
                        res = db.Update("PRS_Process_BOM")
                       .Column("SetMateName", SetMateName)
                       .Column("SetMateNum", SetMateNum)
                       .Column("InPlanceSize", InPlanceSize)
                       .Column("BlankingSize", BlankingSize)
                       //当用户没有选择新增零件时，SetMateName及为新物料编码New_InventoryCode
                       .Column("New_InventoryCode", SetMateName)
                       .Column("MateType", MateType)
                       .Column("MateParamValue", MateParamValue)
                       //.Column("BlankingType", BlankingType)
                       .Where("PartCode", PartCode).Execute();
                    }
                    
                    if (res <= 0)
                    {
                        db.Rollback();
                        return 0;
                    }
                    string CreateType = data.MateType;
                    int total = Quantity * PartQuantity;

                    if (CreateType == "1")
                    {
                        var PRS_BlankingRecord = new PRS_BlankingRecordService();
                        PRS_BlankingRecord blankRecode = new PRS_BlankingRecord();
                        blankRecode = PRS_BlankingRecord.GetModel(ParamQuery.Instance().AndWhere("PartCode", PartCode).AndWhere("IsEnable", 1));

                        if (blankRecode == null)
                        {
                            res = db.Insert("PRS_BlankingRecord")
                           .Column("ContractCode", ContractCode)
                           .Column("InventoryCode", SetMateName)
                           .Column("PartCode", PartCode)
                           .Column("FigureCode", FigureCode)
                           .Column("PartName", PartName)
                           .Column("IsEnable", 1)
                           .Column("SingleQuantity", PartQuantity)
                           .Column("TotalQuantity", total)
                           .Column("MaterialCode", MaterialCode)
                           .Column("InPlanceSize", InPlanceSize)
                           .Column("BlankingSize", BlankingSize)
                           .Column("BlankedQuantity", 0)
                           .Column("NoBlankingQuantity", total)
                           .Column("CreateTime", DateTime.Now)
                           .Column("CreatePerson", MmsHelper.GetUserName())
                           .Column("ModifyTime", DateTime.Now)
                           .Column("ModifyPerson", MmsHelper.GetUserName()).Execute();
                            if (res <= 0)
                            {
                                db.Rollback();
                                return 0;
                            }
                        }
                        else
                        {
                            res = db.Update("PRS_BlankingRecord")
                            .Column("InventoryCode", SetMateName)
                            .Column("SingleQuantity", PartQuantity)
                            .Column("TotalQuantity", total)
                            .Column("InPlanceSize", InPlanceSize)
                            .Column("BlankingSize", BlankingSize)
                            .Column("MaterialCode", MaterialCode)
                            .Column("ModifyTime", DateTime.Now)
                            .Column("ModifyPerson", MmsHelper.GetUserName())
                  .Where("PartCode", PartCode).Execute();
                            if (res <= 0)
                            {
                                db.Rollback();
                                return 0;
                            }
                        }
                    }

                    //if (BlankingType == 1 || BlankingType == 2)
                    //{
                    //    DateTime newDT = DateTime.Now;
                    //    MES_BN_ProductProcessRoute oldPPR = db.Sql(string.Format(@"SELECT * FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND PartCode = '{0}' AND ProcessModelType = '1' AND ProcessLineCode =1", PartCode)).QuerySingle<MES_BN_ProductProcessRoute>();
                    //    PMS_BN_Project project = db.Sql(string.Format(@"SELECT * FROM dbo.PMS_BN_Project WHERE IsEnable = 1 AND ContractCode ='{0}'", ContractCode)).QuerySingle<PMS_BN_Project>();
                    //    PRS_BD_StandardProcess standardProcessXL = db.Sql(@"SELECT * FROM dbo.PRS_BD_StandardProcess WHERE IsEnable = 1 AND ProcessName LIKE '切割%'").QuerySingle<PRS_BD_StandardProcess>();
                    //    PRS_BD_StandardProcess standardProcessJ = db.Sql(@"SELECT * FROM dbo.PRS_BD_StandardProcess WHERE IsEnable = 1 AND ProcessName LIKE '锯%'").QuerySingle<PRS_BD_StandardProcess>();
                    //    SYS_BN_Department departmentXL = db.Sql(@"SELECT * FROM dbo.SYS_BN_Department WHERE IsEnable = 1 AND DepartmentName LIKE '%大阀下料车间%'").QuerySingle<SYS_BN_Department>();
                    //    SYS_BN_Department departmentJ = db.Sql(@"SELECT * FROM dbo.SYS_BN_Department WHERE IsEnable = 1 AND DepartmentName LIKE '%大阀棒材库%'").QuerySingle<SYS_BN_Department>();

                    //    MES_BN_ProductProcessRoute newPPR = new MES_BN_ProductProcessRoute()
                    //    {
                    //        ContractCode = ContractCode,
                    //        ProjectName = project.ProjectName,
                    //        PartCode = PartCode,
                    //        ProcessCode = BlankingType.Equals(1) ? standardProcessXL.ProcessCode : standardProcessJ.ProcessCode,
                    //        ProcessName = BlankingType.Equals(1) ? standardProcessXL.ProcessName : standardProcessJ.ProcessName,
                    //        ProcessLineCode = 1,
                    //        WorkshopID = BlankingType.Equals(1) ? departmentXL.DepartmentCode : departmentJ.DepartmentCode,
                    //        WorkshopName = BlankingType.Equals(1) ? departmentXL.DepartmentName : departmentJ.DepartmentName,
                    //        FigureCode = FigureCode,
                    //        IsEnable = 1,
                    //        Unit = 1,
                    //        IsInspectionReport = 2,
                    //        CreatePerson = MmsHelper.GetUserName(),
                    //        CreateTime = newDT,
                    //        ModifyPerson = MmsHelper.GetUserName(),
                    //        ModifyTime = newDT,
                    //        ProcessModelType = "1"
                    //    };
                    //    PRS_ProcessWorkSteps prsStep = new PRS_ProcessWorkSteps()
                    //    {
                    //        ProcessRouteID = new MES_BN_ProductProcessRouteService().GetModelList().Count > 0 ?
                    //    new MES_BN_ProductProcessRouteService().GetModelList().Max(a => a.ID) + 1 : 1,
                    //        WorkStepsLineCode = 1,
                    //        //WorkStepsName = newPPR.ProcessName,
                    //        IsEnable = 1,
                    //        Unit = 0,
                    //        CreatePerson = MmsHelper.GetUserName(),
                    //        CreateTime = newDT,
                    //        ModifyPerson = MmsHelper.GetUserName(),
                    //        ModifyTime = newDT
                    //    };


                    //    string sqlPPR = string.Empty;
                    //    string sqlStep = string.Empty;

                    //    if (oldPPR == null)
                    //    {
                    //        sqlStep = WinFormClientService.GetInsertSQL(prsStep);
                    //        sqlPPR = WinFormClientService.GetInsertSQL(newPPR);
                    //    }
                    //    else
                    //    {
                    //        sqlPPR = WinFormClientService.GetUpdateSQL(oldPPR, newPPR);

                    //    }

                    //    if (db.Sql(sqlPPR).Execute() <= 0)
                    //    {
                    //        db.Rollback();
                    //        return 0;
                    //    }

                    //    if (!string.IsNullOrEmpty(sqlStep))
                    //    {
                    //        if (db.Sql(sqlStep).Execute() <= 0)
                    //        {
                    //            db.Rollback();
                    //            return 0;
                    //        }
                    //    }

                    //    string sqlA = string.Format(@"SELECT ISNULL(COUNT(ID),0) FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND PartCode = '{0}' AND ProcessModelType = '{1}'", PartCode, 1);
                    //    string sqlB = string.Format(@"UPDATE dbo.PRS_Process_BOM SET {0} = {1} WHERE PartCode = '{2}'", "Blanking", db.Sql(sqlA).QuerySingle<int>(), PartCode);

                    //    if (db.Sql(sqlB).Execute() <= 0)
                    //    {
                    //        db.Rollback();
                    //        return 0;
                    //    }
                    //}

                    db.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    db.Rollback();
                    return 0;
                }
            }
        }



    }
}
