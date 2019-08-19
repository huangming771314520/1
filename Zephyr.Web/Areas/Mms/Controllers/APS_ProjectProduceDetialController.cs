
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
    public class APS_ProjectProduceDetialController : Controller
    {
        public ActionResult Index()
        {
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
            }
            var model = new
            {
                dataSource = new
                {
                   
                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit",
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = "",
                    PartCode = "",
                    ProductName = "",
                    WorkshopID = code,
                    WorkshopID2 = "",
                    EquipmentID = "",
                    WorkGroupID = "",
                    PlanedStartTime = "",
                    PlanedFinishTime = "",
                    PlanState = "1",
                    ProductPlanCode = "",
                    PlanType = "1",
                    DesignTaskCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType" }
                }
            };

            return View(model);
        }
        public ActionResult Index2()
        {
            var model = new
            {
                dataSource = new
                {
                    ContractCode = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_Project", "ContractCode value,ProjectName text"),
                    ProductID = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_ProjectDetail", "ID value,ProductName text")
                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit",
                    audit1 = "/api/Mms/APS_ProjectProduceDetial/GetAudit1"

                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ID="",
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
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType", "ProcessName", "ProductPlanCode", "IsEnable", "DesignTaskCode", "BomQty", "MainID", "RootPartCode", "MonthPlanCode", "ProcessModelType" }
                }
            };

            return View(model);
        }
        public ActionResult PurrcchasePlan()
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit",
                    audit1 = "/api/Mms/APS_ProjectProduceDetial/GetAudit1"

                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
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
                    DesignTaskCode = ""

                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType", "ProcessName", "ProductPlanCode", "IsEnable", "DesignTaskCode", "BomQty" }
                }
            };

            return View(model);
        }
        public ActionResult Index_Produce()//生产计划
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanedStartTime = DateTime.Now.ToShortDateString(),
                    PlanedFinishTime = DateTime.Now.ToShortDateString(),

                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState" }
                }
            };

            return View(model);
        }
        public ActionResult Index_Plan(string PartCode)
        {
            ViewData["PartCode"] = PartCode;
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanedStartTime = "",
                    PlanedFinishTime = "",
                    PartCode = PartCode
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState" }
                }
            };

            return View(model);
        }
        public ActionResult APS_Plan(string PartCode)//生产计划_new
        {
            ViewData["PartCode"] = PartCode;
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                },
                resx = new
                {

                },
                form = new
                {
                    PlanedStartTime = "",
                    PlanedFinishTime = "",
                    PartCode = PartCode,
                    ContractCode = "",
                    ProjectDetailID = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                }
            };

            return View(model);
        }
        public ActionResult Index_Workshop()//车间资源
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanedStartTime = "",
                    PlanedFinishTime = "",
                    WorkshopID = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState" }
                }
            };

            return View(model);
        }
        public ActionResult Index_Equipment(string equipmentID, DateTime planedStartTime, DateTime planedFinishTime)//设备资源
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial/Get2",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit",
                    audit1 = "/api/Mms/APS_ProjectProduceDetial/GetAudit1"

                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    EquipmentID = equipmentID,
                    PlanedStartTime = planedStartTime.ToString("yyyy-MM-dd 00:00:00"),
                    PlanedFinishTime = planedFinishTime.ToString("yyyy-MM-dd 23:59:59"),

                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType", "ProcessName", "ProductPlanCode", "IsEnable" }
                }
            };

            return View(model);
        }
        public ActionResult Index_WorkGroup()//班组资源
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanedStartTime = DateTime.Now.AddDays(-7).ToShortDateString(),
                    PlanedFinishTime = DateTime.Now.AddDays(7).ToShortDateString(),
                    WorkGroupName = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState" }
                }
            };

            return View(model);
        }
        public ActionResult demo()//生产计划_new
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceDetial",
                    newkey = "/api/Mms/APS_ProjectProduceDetial/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceDetial/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanedStartTime = DateTime.Now.ToShortDateString(),
                    PlanedFinishTime = DateTime.Now.ToShortDateString(),

                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "WorkshopID", "EquipmentID", "WorkGroupID", "WorkshopName", "EquipmentName", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState" }
                }
            };

            return View(model);
        }
        /// <summary>
        /// 选择生产计划根零件弹窗
        /// </summary>
        /// <returns></returns>
        [MvcMenuFilter(false)]
        public ActionResult ChooseMonthPlanItem()
        {
            return View();
        }

        [MvcMenuFilter(false)]
        public ActionResult ChooseMonthPlanItem_Better()
        {
            return View();
        }
    }

    public class APS_ProjectProduceDetialApiController : ApiController
    {
        public string GetPurchaseAdvanceTime(string PartCode, DateTime PlanedStartTime)
        {
            var part_list = new SYS_PartService().GetModelList(ParamQuery.Instance().AndWhere("PartCode", PartCode));
            int PurchaseAdvanceTime = part_list.Count > 0 ? Convert.ToInt32(part_list.First().PurchaseAdvanceTime) : 0;
            var PlanedFinishTime = PlanedStartTime.AddDays(PurchaseAdvanceTime);
            return PlanedFinishTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public int GetIsExit(string ContractCode, string ProjectDetailID, string PlanType)
        {
            return new APS_ProjectProduceDetialService().GetIsExit(ContractCode, ProjectDetailID, PlanType);
        }
        public string GetAuditSearchEditBillState(string whereSql)
        {
            return new APS_ProjectProduceDetialService().AuditSearchEditBillState(whereSql);
        }
        public dynamic Get2(RequestWrapper query)
        {
            string sql = string.Format(@"
select * from V_APS_ProjectProduceDetial where EquipmentID='{0}' and PlanedStartTime>='{1}' and PlanedFinishTime<='{2}'", query["EquipmentID"].ToString(), query["PlanedStartTime"].ToString(), query["PlanedFinishTime"].ToString());
            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QueryMany<dynamic>();
            }
        }
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='ID'>
                <select>DISTINCT *</select>
                <from>V_APS_ProjectProduceDetial</from>
                <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
                    <field name='ID' 		 cp='In'></field>
                    <field name='ContractCode' 		 cp='equal'></field>
                    <field name='ProjectDetailID' 		 cp='equal'></field>
                    <field name='PlanType' 		 cp='equal'></field>
<field name='PlanedStartTime' 		 cp='daterange'></field>
<field name='PlanedFinishTime' 		 cp='daterange'></field>
                    <field name='WorkshopID' 		 cp='equal'></field>
                    <field name='PlanState' 		 cp='equal'></field>
                    <field name='MonthPlanCode' 		 cp='equal'></field>
                    <field name='RootPartCode' 		 cp='equal'></field>
                    <field name='ProcessModelType' 		 cp='In'></field>
                </where>
            </settings>").ToParamQuery();
            var service = new APS_ProjectProduceDetialService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new APS_ProjectProduceDetialService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public string GetApsCode()
        {
            string documentNo = MmsHelper.GetOrderNumber("APS_ProjectProduceDetial", "ApsCode", "SCJH", "", "");
            return documentNo;
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
                    string MonthPlanStr = row.MonthPlanCode;
                    var MonthPlanList = MonthPlanStr.Split(',');
                    for (int i = 0; i < MonthPlanList.Length; i++)
                    {
                        new APS_ProduceAndMonthPlanService().Insert(
                            ParamInsert.Instance()
                            .Insert("APS_ProduceAndMonthPlan")
                            .Column("ProducePlanCode", ApsCode)
                            .Column("MonthPlanCode", MonthPlanList[i])
                            .Column("CreateTime", DateTime.Now)
                            .Column("CreatePerson", MmsHelper.GetUserCode()));
                    }
                    StartNumber++;
                }
            }
            var result = service.Edit(null, listWrapper, data);
        }
        public string PostPlanRelease(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.LastIndexOf(','),1);
            var result = new APS_ProjectProduceDetialService().PostPlanRelease("1", ids, out msg);

            return msg;
        }
        public string PostPlanWork(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 1, 1);
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
            var result = new APS_ProjectProduceDetialService().PostBuildPlan(data["planType"].ToString(), data["ContractCode"].ToString(), data["ProjectDetailID"].ToString(), data["PartCode"].ToString(), Convert.ToDateTime(data["StartPlanTime"]), data["MonthPlanCode"].ToString(), out msg);
            return msg;
        }

        public string PostBuildPlan2(dynamic data)
        {
            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();

            foreach (var item in data.list)
            {
                APS_ProjectProduceDetial aps = new APS_ProjectProduceDetial();
                aps.ContractCode = item.ContractCode;
                aps.ProjectDetailID = item.ProjectDetailID;
                aps.PartCode = item.PartCode;
                aps.ProcessCode = item.ProcessCode;
                aps.Quantity = item.Quantity;
                aps.PlanType = item.PlanType;
                aps.ProcessName = item.ProcessName;
                aps.ProductPlanCode = item.ProductPlanCode;
                aps.IsEnable = item.IsEnable;

                list.Add(aps);
            }

            if (data["EndPlanTime"] == "")
            {
                return "计划日期不能为空！";
            }
            string msg = "";
            var result = new APS_ProjectProduceDetialService().PostBuildPlan2(data["planType"].ToString(), data["ContractCode"].ToString(), data["ProjectDetailID"].ToString(), data["PartCode"].ToString(), Convert.ToDateTime(data["EndPlanTime"]), data["ProductPlanCode"].ToString(), data["DesignTaskCode"].ToString(), out msg);
            return msg;
        }
        //生产计划
        public dynamic GetProduceInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime)
        {
            return new APS_ProjectProduceDetialService().GetProduceInfo(PlanedStartTime, PlanedFinishTime);
        }
        //生产计划_new
        public dynamic GetPlanInfo(DateTime? PlanedStartTime, DateTime? PlanedFinishTime, string PartCode)
        {
            return new APS_ProjectProduceDetialService().GetPlanInfo(PlanedStartTime, PlanedFinishTime, PartCode);
        }
        //生产计划_new
        public dynamic GetPlanInfo_New(DateTime? PlanedStartTime, DateTime? PlanedFinishTime, string ContractCode, string ProjectDetailID, string PartCode = "")
        {
            return new APS_ProjectProduceDetialService().GetPlanInfo_New(PlanedStartTime, PlanedFinishTime, ContractCode, ProjectDetailID, PartCode);
        }
        public List<dynamic> GetProduceInfo(string workGroupID)
        {
            return new APS_ProjectProduceDetialService().GetProduceInfo(workGroupID);
        }

        //车间资源
        public dynamic GetWorkshopInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string WorkshopID)
        {
            return new APS_ProjectProduceDetialService().GetWorkshopInfo(PlanedStartTime, PlanedFinishTime, WorkshopID);
        }
        //设备资源
        public dynamic GetEquipmentInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string EquipmentID)
        {
            return new APS_ProjectProduceDetialService().GetEquipmentInfo(PlanedStartTime, PlanedFinishTime, EquipmentID);
        }
        //班组资源
        public dynamic GetWorkGroupInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string WorkGroupID)
        {
            return new APS_ProjectProduceDetialService().GetWorkGroupInfo(PlanedStartTime, PlanedFinishTime, WorkGroupID);
        }
    }
}
