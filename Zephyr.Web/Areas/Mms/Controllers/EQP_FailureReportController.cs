
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
    public class EQP_FailureReportController : Controller
    {
        public static sys_code uc = new sys_code();
          
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var report = new EQP_FailureReportService();
    var loginer = FormsAuth.GetUserData<LoginerBase>();
   
    var model = new
    {
    dataSource = new{
        failureReport = report.GetSelectReport(),
    },
    urls = new{
    query = "/api/Mms/EQP_FailureReport",
    newkey = "/api/Mms/EQP_FailureReport/getnewkey",
    edit = "/api/Mms/EQP_FailureReport/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            FailureReportCode = "" ,
            EquipmentName = "" ,
            FailReportState = "" 
    },
    defaultRow = new {
        
        IsEnable=true,
        FailReportState=1,
        FailTime=DateTime.Now,
        ReportPerson=loginer.UserName,
        
    },
    setting = new{
    idField = "ID",

    postListFields = new string[] { "ID" ,"FailureReportCode" ,"EquipmentID" ,"EquipmentName" ,"WorkshopID" ,"WorkshopName" ,"FailTime" ,"FailDescription" ,"ReportPerson" ,"FailReportState" ,"IsEnable" ,"CreatePerson" ,"CreateTime" ,"ModifyPerson" ,"ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class EQP_FailureReportApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>EQP_FailureReport</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='FailureReportCode' 		 cp='like'></field>
                <field name='EquipmentName' 		 cp='like'></field>
                <field name='FailReportState' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_FailureReportService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new EQP_FailureReportService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            EQP_FailureReport
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_FailureReportService();
    var result = service.Edit(null, listWrapper, data);
    }
    public string PostChangeReportState(int id)
    {
        string msg = "";
        var result = new EQP_FailureReportService().ChangeReportState(id, out msg);
        return msg;
    }


    
     public string GetFailureReportCode( )
    {
        string documentNo = MmsHelper.GetOrderNumber("EQP_FailureReport", "FailureReportCode", "GZBG", "", "");
        return documentNo;
    }
    }
    }
