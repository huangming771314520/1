
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class APS_ProjectProduceReportController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProjectProduceReport",
                    newkey = "/api/Mms/APS_ProjectProduceReport/getnewkey",
                    edit = "/api/Mms/APS_ProjectProduceReport/edit"
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
                    WorkshopName = "",
                    EquipmentID = "",
                    PlanedStartTime = "",
                    PlanedFinishTime = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProductPlanMainID", "PartCode", "ProcessCode", "ProcessLineCode", "WorkshopID", "WorkshopName", "EquipmentID", "EquipmentName", "WorkGroupID", "WorkGroupName", "Quantity", "ManHour", "Unit", "PlanedStartTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "MonthPlanCode", "ProcessModelType", "RootPartCode" }
                }
            };

            return View(model);
        }
    }

    public class APS_ProjectProduceReportApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>V_APS_ProjectProduceDetial</from>
        <where defaultForAll='false' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='PartFigureCode' 		 cp='like'></field>
                <field name='WorkshopName' 		 cp='like'></field>
                <field name='EquipmentName' 		 cp='like'></field>
                <field name='PlanedStartTime' 		 cp='daterange'></field>
                <field name='PlanedFinishTime' 		 cp='daterange'></field>
        </where>
    </settings>");
            var service = new APS_ProjectProduceDetialService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new APS_ProjectProduceDetialService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
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
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
