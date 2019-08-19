
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
    public class MES_BN_ForwardTracingController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_BN_ForwardTracing",
                    remove = "/api/Mms/MES_BN_ForwardTracing/",
                    billno = "/api/Mms/MES_BN_ForwardTracing/getnewbillno",
                    audit = "/api/Mms/MES_BN_ForwardTracing/audit/",
                    edit = "/Mms/MES_BN_ForwardTracing/edit/"
                },
                resx = new
                {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    InventoryCode = ""
                },
                idField = "ID"
            };

            return View(model);
        }
    }

    public class MES_BN_ForwardTracingApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new WMS_BN_BillDetailService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            if (query["InventoryCode"]==null)
                return null;
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(select t1.* from WMS_BN_BillDetail as t1
inner join WMS_BN_BillMain as t2 on t1.BillCode=t2.BillCode
where t2.BillType=2) as temp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='temp.InventoryCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new WMS_BN_BillDetailService();
            var pQuery = query.ToParamQuery() ;
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetAps(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>V_APS_ProjectProduceDetial</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ApsCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new APS_ProjectProduceDetialService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetProjectPart(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>V_PMS_BN_ProjectPart</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ContractCode'		cp='equal'></field> 
        <field name='ProjectDetailID'		cp='equal'></field>   
    </where>
</settings>");
            var service = new PMS_BN_ProjectPartService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
