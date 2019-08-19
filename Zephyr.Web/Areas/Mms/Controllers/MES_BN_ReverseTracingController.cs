
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
    public class MES_BN_ReverseTracingController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_BN_ReverseTracing",
                    remove = "/api/Mms/MES_BN_ReverseTracing/",
                    billno = "/api/Mms/MES_BN_ReverseTracing/getnewbillno",
                    audit = "/api/Mms/MES_BN_ReverseTracing/audit/",
                    edit = "/Mms/MES_BN_ReverseTracing/edit/"
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
                    PartCode = ""
                },
                idField = "ID"
            };

            return View(model);
        }
    }

    public class MES_BN_ReverseTracingApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new WMS_BN_BillDetailService().GetNewKey("ID", "dateplus");
        }
        //工单
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(select * from V_APS_ProjectProduceDetial where ContractCode=(select ContractCode from PMS_BN_ProjectPart where PartCode='xxx')
and ProjectDetailID=(select ProjectDetailID from PMS_BN_ProjectPart where PartCode='xxx')) as temp</from>
</settings>");

            var service = new APS_ProjectProduceDetialService();
            var pQuery = query.ToParamQuery() ;
            var c = query["PartCode"].ToString().Length;
            if (c != 0)
            {
                pQuery.GetData().From = pQuery.GetData().From.Replace("xxx", query["PartCode"].ToString());
            }
            else
                return null;
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        //入库单
        public dynamic GetRKD(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(
select * from WMS_BN_BillDetail where billCode like 'CPRK%' and InventoryCode=(select InventoryCode from SYS_Part where PartCode='xxx')) as temp</from>
</settings>");
            var service = new WMS_BN_BillDetailService();
            var pQuery = query.ToParamQuery();
            var c = query["PartCode"].ToString().Length;
            if (c != 0)
            {
                pQuery.GetData().From = pQuery.GetData().From.Replace("xxx", query["PartCode"].ToString());
            }
            else
                return null;
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        //领料单
        public dynamic GetLLD(RequestWrapper query)
        {
            if (query["PBillCode"].ToString()=="")
            {
                return null;
            }
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(
select * from WMS_BN_BillDetail where billCode like 'LLCK%') as temp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='temp.PBillCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new WMS_BN_BillDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        //设备
        public dynamic GetEquipment(RequestWrapper query)
        {
            if (query["EquipmentCode"].ToString() == "")
            {
                return null;
            }
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>SYS_Equipment</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EquipmentCode'		cp='equal'></field> 
    </where>
</settings>");
            var service = new SYS_EquipmentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        //人员
        public dynamic GetPeople(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(
select t1.*,t2.TeamCode from SYS_WorkGroupDetail as t1 
inner join SYS_WorkGroup as t2 on t1.MainID=t2.ID
where t2.TeamCode='xxx') as temp</from>
</settings>");
            var service = new SYS_WorkGroupDetailService();
            var pQuery = query.ToParamQuery();
            var c = query["TeamCode"].ToString().Length;
            if (c != 0)
            {
                pQuery.GetData().From = pQuery.GetData().From.Replace("xxx", query["TeamCode"].ToString());
            }
            else
                return null;
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
