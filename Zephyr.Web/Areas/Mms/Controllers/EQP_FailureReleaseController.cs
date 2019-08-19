
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
    public class EQP_FailureReleaseController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var loginer = FormsAuth.GetUserData<LoginerBase>();
    var model = new
    {
    dataSource = new{
        user = loginer.UserName,
    },
    urls = new{
    query = "/api/Mms/EQP_FailureRelease",
    newkey = "/api/Mms/EQP_FailureRelease/getnewkey",
    edit = "/api/Mms/EQP_FailureRelease/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            ReleaseTime = "" ,
            FailureState = "" 
    },
    defaultRow = new {
        FailRemoveMan = loginer.UserName,
        IsEnable=true,
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID" ,"FailureRemoveCode","FailureReportID" ,"ReleaseTime" ,"FailRemoveDescription" ,"FailRemoveMan" ,"FailureState" ,"IsEnable" ,"CreatePerson" ,"CreateTime" ,"ModifyPerson" ,"ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class EQP_FailureReleaseApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>EQP_FailureRelease</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ReleaseTime' 		 cp='daterange'></field>
                <field name='FailureState' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_FailureReleaseService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public List<EQP_FailureRelease> GetLoadData()
    {
    EQP_FailureReleaseService service = new EQP_FailureReleaseService();
    var result = service.GetLoadData();
    return result;
    }

        
    public string GetNewKey()
    {
    return new EQP_FailureReleaseService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            EQP_FailureRelease
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_FailureReleaseService();
    var result = service.Edit(null, listWrapper, data);
    }
    public string GetFailureRemoveCode()
    {
        string documentNo = MmsHelper.GetOrderNumber("EQP_FailureRelease", "FailureRemoveCode", "GZCL", "", "");
        return documentNo;
    }
    public string PostAcceptReportState(int id)
    {
        string msg = "";
        var result = new EQP_FailureReportService().AcceptReportState(id, out msg);
        return msg;
    }


    public string PostChangeFailureState(int id)
    {
        string msg = "";
        var result = new EQP_FailureReleaseService().ChangeFailureState(id, out msg);
        return msg;
    }
    public string PostRemoveFailureState(int id)
    {
        string msg = "";
        var result = new EQP_FailureReleaseService().RemoveFailureState(id,out msg);
        return msg;
    }
    }
    }
