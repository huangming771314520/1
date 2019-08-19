
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class WMS_BN_RealStockController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/WMS_BN_RealStock",
                    remove = "/api/Mms/WMS_BN_RealStock/",
                    billno = "/api/Mms/WMS_BN_RealStock/getnewbillno",
                    audit = "/api/Mms/WMS_BN_RealStock/audit/",
                    edit = "/Mms/WMS_BN_RealStock/edit/"
                },
                resx = new {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    ID = "" ,
                    InventoryCode = "",
                    InventoryName = "",
                    Model="",
                    RealStock = "" ,
                    TotalStock = "" ,
                    LockStock = "" ,
                    WarehouseCode = "" ,
                    WarehouseName = "" ,
                    BatchCode = "" ,
                    Unit = "" ,
                    Remark = "" ,
                    CreatePerson = "" ,
                    CreateTime = "" ,
                    ModifyPerson = "" ,
                    ModifyTime = "" 
                },
                idField="ID"
            };

            return View(model);
        }
    }

    public class WMS_BN_RealStockApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new WMS_BN_RealStockService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>WMS_BN_RealStock</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ID'		cp='equal'></field>   
        <field name='InventoryCode'		cp='equal'></field>   
        <field name='InventoryName'		cp='equal'></field>  
        <field name='Model'		cp='equal'></field>   
        <field name='RealStock'		cp='equal'></field>   
        <field name='TotalStock'		cp='equal'></field>   
        <field name='LockStock'		cp='equal'></field>   
        <field name='WarehouseCode'		cp='equal'></field>   
        <field name='WarehouseName'		cp='equal'></field>   
        <field name='BatchCode'		cp='equal'></field>   
        <field name='Unit'		cp='equal'></field>   
        <field name='Remark'		cp='equal'></field>   
        <field name='CreatePerson'		cp='equal'></field>   
        <field name='CreateTime'		cp='equal'></field>   
        <field name='ModifyPerson'		cp='equal'></field>   
        <field name='ModifyTime'		cp='equal'></field>   
    </where>
</settings>");
            var service = new WMS_BN_RealStockService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
