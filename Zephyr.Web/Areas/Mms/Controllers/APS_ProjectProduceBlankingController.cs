
using Newtonsoft.Json.Linq;
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
    public class APS_ProjectProduceBlankingController : Controller
    {

        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    //ContractCode = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_Project", "ContractCode value,ProjectName text"),
                    //ProductID = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_ProjectDetail", "ID value,ProductName text")
                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceBlanking",
                    newkey = "/api/Mms/APS_ProjectProduceBlanking/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceBlanking/edit",
                    audit1 = "/api/Mms/APS_ProjectProduceBlanking/GetAudit1"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ID = "",
                    SavantCode = "",
                    ContractCode = "",
                    PartCode = "",
                    ProductName = "",
                    WorkshopID = "",
                    EquipmentID = "",
                    WorkGroupID = "",
                    PlanedStartTime = "",
                    PlanedFinishTime = "",
                    PlanState = "",
                    ProjectDetailID = "",
                    ProjectName = "",
                    ProductType = "",
                    Model = "",
                    Specifications = "",
                    ProductPlanCode = "",
                    DesignTaskCode = "",
                    Quantity = "",
                    MonthPlanCode = "",
                    RootPartCode = "",
                    ProcessModelType = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "SavantCode", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType", "ProcessName", "ProductPlanCode", "IsEnable", "DesignTaskCode", "BomQty", "MainID", "RootPartCode", "MonthPlanCode", "ProcessModelType" }
                }
            };
            return View(model);
        }
    }

    public class APS_ProjectProduceBlankingApiController : ApiController
    {
       
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='temp.ContractCode'>
                <select>*</select>
                <from>(SELECT distinct t1.ID, t1.ContractCode,t1.SavantCode, t1.ProjectDetailID, t1.ProductPlanMainID, t1.PartCode, t1.ProcessCode, t1.ProcessLineCode, 
                t1.WorkshopID, t1.WorkshopName, t1.EquipmentID, t1.EquipmentName, t1.WorkGroupID, t1.WorkGroupName, 
                t1.Quantity, t1.ManHour, t1.Unit, t1.EarliestStartTime, t1.LatestStartTime, t1.PlanedStartTime, t1.EarliestFinishTime, 
                t1.LatestFinishTime, t1.PlanedFinishTime, t1.ActualStartTime, t1.ActualFinishTime, t1.FloatingHour, t1.PlanColor, 
                t1.PlanState, t1.PlanType, t1.IsEnable, t1.CreatePerson, t1.CreateTime, t1.ModifyPerson, t1.ModifyTime, t1.ApproveState, 
                t1.ApprovePerson, t1.ApproveDate, t1.ApproveRemark, t1.ApsCode, t1.ProductPlanCode, t1.ProcessName, 
                t1.DesignTaskCode, t1.BomQty, t1.MainID, t2.ProductName, t2.ProductType, t2.Model, t2.Specifications, 
                t3.PartFigureCode, t4.eqpUseTime, t3.MaterialCode, P.PartName, t1.RootPartCode, t1.MonthPlanCode, 
                t1.ProcessModelType
FROM      dbo.APS_ProjectProduceDetial AS t1 LEFT OUTER JOIN
                dbo.PMS_BN_ProjectDetail AS t2 ON t1.ProjectDetailID = t2.ID LEFT OUTER JOIN
                dbo.PRS_Process_BOM AS t3 ON t1.PartCode = t3.PartCode LEFT OUTER JOIN
                    (SELECT   EquipmentID, SUM(CASE WHEN DATEDIFF(d, CONVERT(VARCHAR(10), PlanedStartTime, 121), 
                                     CONVERT(VARCHAR(10), PlanedFinishTime, 121)) > 0 THEN DATEDIFF(mi, PlanedStartTime, 
                                     (CONVERT(VARCHAR(10), PlanedStartTime, 121) + ' 17:00:00')) ELSE DATEDIFF(mi, PlanedStartTime, 
                                     PlanedFinishTime) END) AS eqpUseTime, CONVERT(VARCHAR(10), PlanedStartTime, 121) 
                                     AS PlanedStartTime
                     FROM      dbo.APS_ProjectProduceDetial
                     WHERE   (EquipmentID IS NOT NULL)
                     GROUP BY EquipmentID, CONVERT(VARCHAR(10), PlanedStartTime, 121)) AS t4 ON 
                t1.EquipmentID = t4.EquipmentID AND CONVERT(VARCHAR(10), t1.PlanedStartTime, 121) = CONVERT(VARCHAR(10), 
                t4.PlanedStartTime, 121) LEFT OUTER JOIN
                    (SELECT DISTINCT PartCode, MAX(PartName) AS PartName
                     FROM      dbo.SYS_Part
                     GROUP BY PartCode) AS P ON t1.PartCode = P.PartCode
WHERE   (t1.IsEnable = 1) AND (t3.IsEnable = 1) AND t1.ProcessModelType='1') as temp</from>
                <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
                    <field name='temp.ID' 		 cp='In'></field>
                    <field name='temp.ContractCode' 		 cp='equal'></field>
                    <field name='temp.SavantCode' 		 cp='equal'></field>
                    <field name='temp.ProjectDetailID' 		 cp='equal'></field>
                    <field name='temp.PlanType' 		 cp='equal'></field>
                    <field name='temp.WorkshopID' 		 cp='equal'></field>
                    <field name='temp.PlanState' 		 cp='equal'></field>
                    <field name='temp.MonthPlanCode' 		 cp='equal'></field>
                    <field name='temp.RootPartCode' 		 cp='equal'></field>
                    <field name='temp.ProcessModelType' 		 cp='In'></field>
                </where>
            </settings>").ToParamQuery();
            var service = new APS_ProjectProduceDetialService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            APS_ProjectProduceDetial
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_ProjectProduceDetialService();
            if (data.list.inserted.ToString() != "[]")
            {
                //APS明细表新增
                var PlanCode = MmsHelper.GetOrderNumber("APS_ProjectProduceDetial", "ApsCode", "SCJH", "", "");
                string PreCode = PlanCode.Substring(0, PlanCode.Length - 3);
                int StartNumber = Convert.ToInt32(PlanCode.Substring(PlanCode.Length - 3));
                foreach (dynamic row in data.list.inserted)
                {
                    var ApsCode = PreCode + StartNumber.ToString().PadLeft(3, '0');
                    row["ApsCode"] = ApsCode;
                    StartNumber++;
                }
            }
            var result = service.Edit(null, listWrapper, data);
        }
        public string PostPlanRelease(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 2, 1);
            var result = new APS_ProjectProduceDetialService().PostPlanRelease("1", ids, out msg);

            return msg;
        }
        public string PostPlanWork(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 2, 1);
            var result = new APS_ProjectProduceDetialService().PostPlanRelease("2", ids, out msg);

            return msg;
        }
        public string PostBuildPlan(dynamic data)
        {
            if (data["StartPlanTime"] == "")
            {
                return "计划开始日期不能为空！";
            }
            string msg = "";
            var result = new APS_ProjectProduceDetialService().PostBuildPlan_Blanking(data["planType"].ToString(), data["ContractCode"].ToString(), Convert.ToDateTime(data["StartPlanTime"]), out msg);
            return msg;
        }
        
    }
}
