
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
    public class PMS_BN_ProjectPlanDetailUpdateController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/PMS_BN_ProjectPlanDetailUpdate",
    newkey = "/api/Mms/PMS_BN_ProjectPlanDetailUpdate/getnewkey",
    edit = "/api/Mms/PMS_BN_ProjectPlanDetailUpdate/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            IsEnable = "" ,
            PlanType = "" ,
            UpdatedPlanDate = "" 
    },
    defaultRow = new {

    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "ProjectDetailID", "ContractCode", "PlanType", "PlanDate", "UpdatedPlanDate", "FinishDate", "UpdatedFinishDate", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class PMS_BN_ProjectPlanDetailUpdateApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>(
select t1.*,t2.ProductName,t2.ProductType,t2.Model,t2.Specifications from PMS_BN_ProjectPlanDetailUpdate as t1
left join PMS_BN_ProjectDetail as t2 on t1.ProjectDetailID=t2.ID) as temp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='IsEnable' 		 cp='equal'></field>
                <field name='PlanType' 		 cp='equal'></field>
                <field name='UpdatedPlanDate' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new PMS_BN_ProjectPlanDetailUpdateService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new PMS_BN_ProjectPlanDetailUpdateService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PMS_BN_ProjectPlanDetailUpdate
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new PMS_BN_ProjectPlanDetailUpdateService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
