
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
    public class APS_ProjectProduceTemporaryTaskController : Controller
    {
        public ActionResult Index()
        {
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }
            if (string.IsNullOrEmpty(code))
            {
                return JavaScript("请先维护所属车间!");
            }
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceTemporaryTask",
                    newkey = "/api/Mms/APS_ProjectProduceTemporaryTask/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceTemporaryTask/edit",
                    audit1 = "/api/Mms/APS_ProjectProduceTemporaryTask/GetAudit1"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = ""
                },
                defaultRow = new
                {
                    BillCode = "系统生成"
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "TemporaryTaskDec", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "WorkshopName", "EquipmentID", "EquipmentName", "WorkGroupID", "WorkGroupName", "Quantity", "ManHour", "Unit", "EarliestStartTime", "LatestStartTime", "PlanedStartTime", "EarliestFinishTime", "LatestFinishTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "FloatingHour", "PlanColor", "PlanState", "PlanType", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "ApproveState", "ApprovePerson", "ApproveDate", "ApproveRemark", "ApsCode", "BillCode" }
                }
            };

            return View(model);
        }
    }

    public class APS_ProjectProduceTemporaryTaskApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>APS_ProjectProduceTemporaryTask</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_ProjectProduceTemporaryTaskService();
            var pQuery = query.ToParamQuery().AndWhere("IsEnable", 1);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new APS_ProjectProduceTemporaryTaskService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public string GetAudit1(string whereSql)
        {
            return new APS_ProjectProduceTemporaryTaskService().GetAudit1(whereSql);
        }
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
             dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            APS_ProjectProduceTemporaryTask
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_ProjectProduceTemporaryTaskService();
            
            if (data.list.inserted.ToString() != "[]")
            {
                var dno = MmsHelper.GetOrderNumber("APS_ProjectProduceTemporaryTask", "BillCode", "LLRW", "", "");
                var fno = dno.Substring(0, 10);
                var con = dno.Substring(10, 3);

                foreach (JToken row in data["list"]["inserted"].Children())
                {
                    var pQuery = ParamQuery.Instance().Select("ApproveState").AndWhere("ID", row["ID"]);
                    var re = new APS_ProjectProduceTemporaryTaskService().GetModel(pQuery);
                    if (re!=null && re.ApproveState == "1")
                    {
                        MmsHelper.ThrowHttpExceptionWhen(true, "以审核不能保存", 0);
                        return;
                    }

                    row["BillCode"] = fno + con;
                    int intCon = Convert.ToInt32(con);
                    intCon++;
                    var zeros = 3 - intCon.ToString().Length;
                    con = "";
                    for (int i = 1; i <= zeros; i++)
                        con += "0";
                    con += intCon.ToString();
                    row["LaunchWorkshopID"] = code;
                    row["LaunchWorkshopName"] = name;
                    row["IsEnable"] = 1;
                }
            } 
            if (data.list.deleted.ToString() != "[]")
            {
                foreach (JToken row in data["list"]["deleted"].Children())
                {
                    var pQuery = ParamQuery.Instance().Select("ApproveState").AndWhere("ID", row["ID"]);
                    var re = new APS_ProjectProduceTemporaryTaskService().GetModel(pQuery);
                    if (re != null && re.ApproveState == "1")
                    {
                        MmsHelper.ThrowHttpExceptionWhen(true, "以审核不能删除", 0);
                        return;
                    }
                    row["IsEnable"] = 0;
                }
            }
            
            var result = service.Edit(null, listWrapper, data);


        }
    }
}
