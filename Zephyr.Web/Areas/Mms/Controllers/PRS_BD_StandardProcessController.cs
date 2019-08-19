
using Newtonsoft.Json.Linq;
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
    public class PRS_BD_StandardProcessController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/PRS_BD_StandardProcess",
    newkey = "/api/Mms/PRS_BD_StandardProcess/getnewkey",
    edit = "/api/Mms/PRS_BD_StandardProcess/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            ProcessCode = "" ,
            ProcessName = "" 
    },
    defaultRow = new {
        IsEnable=1,
        ProcessCode="系统生成"
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID" ,"ProcessCode" ,"ProcessName" ,"Instracutions","ProcessType" ,"IsEnable" ,"CreatePerson" ,"CreateTime" ,"ModifyPerson" ,"ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class PRS_BD_StandardProcessApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>PRS_BD_StandardProcess</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ProcessCode' 		 cp='like'></field>
                <field name='ProcessName' 		 cp='like'></field>
        </where>
    </settings>");
    var service = new PRS_BD_StandardProcessService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
    return result;
    }

    public string GetNewKey()
    {
    return new PRS_BD_StandardProcessService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_BD_StandardProcess
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new PRS_BD_StandardProcessService();

    if (data.list.inserted.ToString() != "[]")
    {
        string code =new PRS_BD_StandardProcessService().GetMaxCode();
        //var fno = dno.Substring(0, 9);
        //var con = dno.Substring(9, 3);

        foreach (JToken row in data["list"]["inserted"].Children())
        {
            row["ProcessCode"] = code;
            int intCon = Convert.ToInt32(code);
            intCon++;
            code = intCon < 10 ? "00" + intCon.ToString() : intCon < 100 ? "0" + intCon.ToString() : intCon.ToString();
        }
        var result = service.Edit(null, listWrapper, data);

    }
    else
        service.Edit(null, listWrapper, data);


    //var result = service.Edit(null, listWrapper, data);
    }

    

    public string PostChangeStandardProcess(int? id)
    {
        string msg = "";
        var res = new PRS_BD_StandardProcessService().PostChangeStandardProcess(id, out msg);
        return msg;
    }
    }
    }
