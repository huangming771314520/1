
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
    public class QMS_BN_QualificationController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/QMS_BN_Qualification",
    newkey = "/api/Mms/QMS_BN_Qualification/getnewkey",
    edit = "/api/Mms/QMS_BN_Qualification/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            QualificationName = "" ,
            QualificationState = "" ,
            IsEnable = "" 
    },
    defaultRow = new {

    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID" ,"QualificationCode" ,"QualificationName" ,"IdentificationCycle" ,"QualificationState" ,"IsEnable", "InspectionBeginDate", "InspectionFinishDate" }
    }
    };

    return View(model);
    }
    }

    public class QMS_BN_QualificationApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>QMS_BN_Qualification</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='QualificationName' 		 cp='like'></field>
                <field name='QualificationState' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new QMS_BN_QualificationService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new QMS_BN_QualificationService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            QMS_BN_Qualification
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new QMS_BN_QualificationService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
