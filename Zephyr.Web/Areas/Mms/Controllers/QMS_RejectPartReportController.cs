
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
    public class QMS_RejectPartReportController : Controller
    {
    public ActionResult Index()
    {
        
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/QMS_RejectPartReport",
    newkey = "/api/Mms/QMS_RejectPartReport/getnewkey",
    edit = "/api/Mms/QMS_RejectPartReport/edit",
    audit1 = "/api/Mms/QMS_RejectPartReport/GetAudit1"
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
    postListFields = new string[] { "ID" ,"BillCode" ,"ContractCode" ,"ProjectName" ,"PartCode" ,"PartName" ,"Model"
        ,"ProcessCode" ,"ProcessName" ,"RejectQuantity" ,"WorkTeamCode" ,"WorkTeamName" ,"TechCommand" ,"InspectionRecord" 
        ,"LeaderRemark" ,"InspectorCode" ,"InspectorName","ApproveState" ,"IsEnable" ,"CreatePerson" ,"CreateTime" ,"ModifyPerson" ,"ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class QMS_RejectPartReportApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>QMS_RejectPartReport</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='BillCode' 		 cp='like'></field>
                <field name='ProjectName' 		 cp='like'></field>
                <field name='PartName' 		 cp='like'></field>
        </where>
    </settings>");
    var service = new QMS_RejectPartReportService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new QMS_RejectPartReportService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            QMS_RejectPartReport
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");

   
    var service = new QMS_RejectPartReportService();
    var result = service.Edit(null, listWrapper, data);
    }

        public string GetCode()
    {
            string billCode = MmsHelper.GetOrderNumber("QMS_RejectPartReport", "BillCode", "BHGPTZ", "", "");
            return billCode;
    }
        public string GetAudit1(string whereSql)
        {
            return new QMS_RejectPartReportService().GetAudit1(whereSql);
        }
    }
    }
