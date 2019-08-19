
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
    public class SYS_BN_SupplierController : Controller
    {
    public ActionResult Index()
    {
    var code = new sys_codeService();
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/SYS_BN_Supplier",
    newkey = "/api/Mms/SYS_BN_Supplier/getnewkey",
    edit = "/api/Mms/SYS_BN_Supplier/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            SupplierName = "" ,
            IsEnable = "" 
    },
    defaultRow = new {
        IsEnable="true"
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "SupplierCode", "SupplierName", "SupplierForShort", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class SYS_BN_SupplierApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>SYS_BN_Supplier</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='SupplierName' 		 cp='like'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new SYS_BN_SupplierService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new SYS_BN_SupplierService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_BN_Supplier
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new SYS_BN_SupplierService();
    var result = service.Edit(null, listWrapper, data);
    }
    public string GetCode()
    {
        string documentNo = MmsHelper.GetOrderNumber("SYS_BN_Supplier", "SupplierCode", "GYBH", "", "");
        return documentNo;
    }
    }
    }
