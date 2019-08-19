
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class sys_SystemabnormallogController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
    dsPricing = code.GetValueTextListByType("Pricing")
    },
    urls = new{
    query = "/api/Sys/sys_Systemabnormallog",
    newkey = "/api/Sys/sys_Systemabnormallog/getnewkey",
    edit = "/api/Sys/sys_Systemabnormallog/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            UnabnormalReason = "" ,
            UnabnormalDescription = "" ,
            Remark = "" ,
            Create_Time = "" ,
            Create_Person = "" 
    },
    defaultRow = new {

    },
    setting = new{
    idField = "Object_ID",
    postListFields = new string[] { "Object_ID" ,"RowID" ,"UnabnormalReason" ,"UnabnormalDescription" ,"Remark" ,"Modify_Time" ,"Modify_Person" ,"Create_Time" ,"Create_Person" }
    }
    };

    return View(model);
    }
    }

    public class sys_SystemabnormallogApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='Object_ID'>
        <select>*</select>
        <from>sys_systemabnormallog</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='UnabnormalReason' 		 cp='equal'></field>
                <field name='UnabnormalDescription' 		 cp='equal'></field>
                <field name='Remark' 		 cp='equal'></field>
                <field name='Create_Time' 		 cp='daterange'></field>
                <field name='Create_Person' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new sys_systemabnormallogService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new sys_systemabnormallogService().GetNewKey("Object_ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            sys_systemabnormallog
        </table>
        <where>
            <field name='Object_ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new sys_systemabnormallogService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
