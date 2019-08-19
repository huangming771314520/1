
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
    public class PMS_BN_ProjectPartController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {
                    pageData = new PMS_BN_ProjectPartApiController().GetPageData(),
                },
                urls = new
                {
                    query = "/api/Mms/PMS_BN_ProjectPart",
                    newkey = "/api/Mms/PMS_BN_ProjectPart/getnewkey",
                    edit = "/api/Mms/PMS_BN_ProjectPart/edit"
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
                    ProductName=""
                },
                defaultRow = new
                {
                    IsEnable=1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ProjectDetailID", "ContractCode", "ProjectID", "ProjectName","VersionCode", "PartCode", "PartName", "FileName", "FileAddress",  "IsEnable" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID", 
                          relationId = "ID",
                          postFields = new string[] { "ID","ContractCode","ProductPlanMainID","PartCode","ProcessCode","WorkshopID","EquipmentID","WorkGroupID","Quantity","PlanedStartTime","PlanedFinishTime","ActualStartTime","ActualFinishTime","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {ID = "",ContractCode = "",ProductPlanMainID = "",PartCode = "",ProcessCode = "",WorkshopID = "",EquipmentID = "",WorkGroupID = "",Quantity = "",PlanedStartTime = "",PlanedFinishTime = "",ActualStartTime = "",ActualFinishTime = "",CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = ""}
                        }
}
            };

            return View(model);
        }
    }

    public class PMS_BN_ProjectPartApiController : ApiController
    {
        public dynamic GetPageData(string PartCode = "", string ContractCode = "", string tabName = "")
        {
            var pQuery = ParamQuery.Instance();
            if (PartCode == "")
            {
                var result = new
                {
                    tab0 = "",
                };
                return result;
            }
            else
            {
                List<dynamic> data = new List<dynamic>();
                if (tabName == "项目生产明细计划")
                {
                    data = new APS_ProjectProduceDetialService().GetDynamicList(pQuery.AndWhere("ContractCode", ContractCode).AndWhere("PartCode", PartCode));
                }
                //else if (tabName == "工艺路线管理")
                //{
                //    data = new MES_BD_TechnologyService().GetDynamicList(pQuery.AndWhere("LineCode", lineCode));
                //}
                var result = new
                {
                    rows = data,
                    total = data.Count
                };
                return result;
            }
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>V_PMS_BN_ProjectPart</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
<field name='ProductName' 		 cp='like'></field>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='PartCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectPartService();
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
            PMS_BN_ProjectPart
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectPartService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
