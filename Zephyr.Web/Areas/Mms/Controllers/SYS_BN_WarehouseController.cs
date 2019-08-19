
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
    public class SYS_BN_WarehouseController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/SYS_BN_Warehouse",
    newkey = "/api/Mms/SYS_BN_Warehouse/getnewkey",
    edit = "/api/Mms/SYS_BN_Warehouse/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            WarehouseName = "" ,
            WarehouseType = "" ,
            IsEnable = "" 
    },
    defaultRow = new {
        WarehouseType=2,
        IsEnable=1
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "WarehouseCode", "WarehouseName", "UserCode", "StoreKeeper", "WarehouseType", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class SYS_BN_WarehouseApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>SYS_BN_Warehouse</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='WarehouseName' 		 cp='like'></field>
                <field name='WarehouseType' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new SYS_BN_WarehouseService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new SYS_BN_WarehouseService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_BN_Warehouse
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new SYS_BN_WarehouseService();
    var result = service.Edit(null, listWrapper, data);
    }
    public string GetWarehouseCode()
    {
        string documentNo = MmsHelper.GetOrderNumber("SYS_BN_Warehouse", "WarehouseCode", "CKBH", "", "");
        return documentNo;
    }
  
    }
    }
