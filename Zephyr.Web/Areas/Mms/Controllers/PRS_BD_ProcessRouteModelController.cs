
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
    public class PRS_BD_ProcessRouteModelController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/PRS_BD_ProcessRouteModel",
    newkey = "/api/Mms/PRS_BD_ProcessRouteModel/getnewkey",
    edit = "/api/Mms/PRS_BD_ProcessRouteModel/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            ProcessRouteCode = "" ,
            ProcessRouteName = "" 
    },
    defaultRow = new {
        IsEnable=true,
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID" ,"ProcessRouteCode" ,"ProcessRouteName" ,"ProcessCode" ,"ProcessLineCode" ,"Remark","IsEnable" ,"CraetePerson" ,"CreateTime" ,"ModifyPerson" ,"ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class PRS_BD_ProcessRouteModelApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>PRS_BD_ProcessRouteModel</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ProcessRouteCode' 		 cp='like'></field>
                <field name='ProcessRouteName' 		 cp='like'></field>
        </where>
    </settings>");
    var service = new PRS_BD_ProcessRouteModelService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new PRS_BD_ProcessRouteModelService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_BD_ProcessRouteModel
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new PRS_BD_ProcessRouteModelService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
