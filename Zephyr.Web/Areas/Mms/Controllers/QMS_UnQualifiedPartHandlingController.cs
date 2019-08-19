
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
    public class QMS_UnQualifiedPartHandlingController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/QMS_UnQualifiedPartHandling",
    newkey = "/api/Mms/QMS_UnQualifiedPartHandling/getnewkey",
    edit = "/api/Mms/QMS_UnQualifiedPartHandling/edit",
    audit1 = "/api/Mms/QMS_UnQualifiedPartHandling/GetAudit1"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            BillCode = "" ,
            ProjectName = "" ,
            PartName = "" 
    },
    defaultRow = new {
        ApproveState = 0,
        IsEnable=1,
        

    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "BillCode", "ContractCode", "ProjectName", "PartCode", "PartName", "Model", "RejectQuantity", 
        "ProcessCode", "ProcessName", "RejectDescription", "HandlingType", "AnalysisReason", "ApprovedPerson", "ApprovedTime",
        "ApprovedState", "HandlingResult", "InspectorCode", "InspectorName", "ApproveState", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class QMS_UnQualifiedPartHandlingApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>QMS_UnQualifiedPartHandling</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='BillCode' 		 cp='like'></field>
                <field name='ProjectName' 		 cp='like'></field>
                <field name='PartName' 		 cp='like'></field>
        </where>
    </settings>");
    var service = new QMS_UnQualifiedPartHandlingService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new QMS_UnQualifiedPartHandlingService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            QMS_UnQualifiedPartHandling
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
   
    var service = new QMS_UnQualifiedPartHandlingService();
    var result = service.Edit(null, listWrapper, data);
    }
    public string GetCode()
    {
        string billCode = MmsHelper.GetOrderNumber("QMS_UnQualifiedPartHandling", "BillCode", "BHGPCL", "", "");
        return billCode;
    }
    public string GetAudit1(string whereSql)
    {
        return new QMS_UnQualifiedPartHandlingService().GetAudit1(whereSql);
    }
    }
    }
