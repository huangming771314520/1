
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
    public class WMS_BN_LockMaterialController : Controller
    {
    public ActionResult Index()
    {
        var code = new sys_codeService();
        var model = new
        {
            dataSource = new{
            lockType = code.GetValueTextListByType_Xttcqw("LockType")
        },
        urls = new{
            query = "/api/Mms/WMS_BN_LockMaterial",
            newkey = "/api/Mms/WMS_BN_LockMaterial/getnewkey",
            edit = "/api/Mms/WMS_BN_LockMaterial/edit",
            lockstate = "/api/Mms/WMS_BN_LockMaterial/confirmlockmaterial" 
        },
        resx = new{
            noneSelect = "请先选择一条数据！",
            editSuccess = "保存成功！",
            auditSuccess = "单据已审核！",
            lockmes="操作成功！"
        },
        form = new{
                IncentoryCode = "" ,
                IncentoryName = "",
                WarehouseCode = "" ,
                WarehouseName = "" ,
                LockState = "" 
        },
        defaultRow = new {
            LockState=0
        },
        setting = new{
            idField = "ID",
            postListFields = new string[] { "IncentoryCode", "IncentoryName", "LockQuantity", "WarehouseCode", "WarehouseName", "MaterialCategoryNum", "MaterialCategoryName", "LockState", "LockDescription", "UnLockDescription", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "ID" }
            }
        }; 
        return View(model);
        }
         

    }

    public class WMS_BN_LockMaterialApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>WMS_BN_LockMaterial</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='IncentoryCode' 		 cp='equal'></field>
                <field name='IncentoryName' 		 cp='equal'></field>
                <field name='WarehouseCode' 		 cp='equal'></field>
                <field name='WarehouseName' 		 cp='equal'></field>
                <field name='LockState' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new WMS_BN_LockMaterialService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new WMS_BN_LockMaterialService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }


    [System.Web.Http.HttpPost]
    public string ConfirmLockMaterial(string id, JObject data)
    {
        var result = "";
        var masterService = new WMS_BN_LockMaterialService();
        var pQuery = ParamQuery.Instance().AndWhere("ID", id);
        var lockinfo = new WMS_BN_LockMaterialService().GetModel(pQuery);
        if (lockinfo!=null)
        {
            lockinfo.LockState = Convert.ToInt32(data["type"]);
        }
        string msg = "";
        result = masterService.ConfirmLockState(lockinfo, out msg) > 0 ? "yes" : "no";
        if (result == "no")
        {
            return msg;
        }
        return result;
    }
        

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            WMS_BN_LockMaterial
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new WMS_BN_LockMaterialService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
