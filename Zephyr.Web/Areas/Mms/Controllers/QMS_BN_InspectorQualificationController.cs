
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
    public class QMS_BN_InspectorQualificationController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/QMS_BN_InspectorQualification",
    newkey = "/api/Mms/QMS_BN_InspectorQualification/getnewkey",
    edit = "/api/Mms/QMS_BN_InspectorQualification/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            InspectorName = "" ,
            IsEnable = "" ,
            EffectiveEndDate = "" 
    },
    defaultRow = new {

    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID" ,"UserCode" ,"InspectorName" ,"QualificationCode" ,"CertificateCode" ,"EffectiveBegainDate" ,"EffectiveEndDate" ,"IsEnable" }
    }
    };

    return View(model);
    }
    }

    public class QMS_BN_InspectorQualificationApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>QMS_BN_InspectorQualification</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='InspectorName' 		 cp='like'></field>
                <field name='IsEnable' 		 cp='equal'></field>
                <field name='EffectiveEndDate' 		 cp='daterange'></field>
        </where>
    </settings>");
    var service = new QMS_BN_InspectorQualificationService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new QMS_BN_InspectorQualificationService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            QMS_BN_InspectorQualification
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new QMS_BN_InspectorQualificationService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
