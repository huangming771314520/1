
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class SYS_CreateSysPartController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    QuantityUnit = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.SYS_Part", "distinct QuantityUnit text")
                },
                urls = new
                {
                    query = "/api/Mms/SYS_CreateSysPart",
                    newkey = "/api/Mms/SYS_CreateSysPart/getnewkey",
                    edit = "/api/Mms/SYS_CreateSysPart/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    InventoryName = "",
                    Model = "",
                    Spec = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "InventoryCode", "InventoryName", "Model", "Spec", "PartType", "WarehouseCode", "WarehouseName", "QuantityUnit" }
                }
            };

            return View(model);
        }
    }

    public class SYS_CreateSysPartApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='PartType'>
        <select>ID,text,case when InventoryCode=ReferenceCode then '' else InventoryCode end InventoryCode,InventoryName,Model,Spec,ReferenceCode,ReferenceName,ReferenceModel,PartType,WarehouseCode,WarehouseName,QuantityUnit</select>
        <from>(SELECT *, CASE WHEN PartType='PRS_BoardCreateMate' THEN  '板材报料'
 WHEN PartType='MES_WorkshopPurchaseDetail'  THEN  '车间请购'
  WHEN PartType='PRS_Process_BOM'  THEN  '工艺定料' END AS text
  FROM [V_CreateSysPart])  as tmp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='InventoryName' 		 cp='like'></field>
                <field name='Model' 		 cp='like'></field>
                <field name='Spec' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new SYS_PartService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_Process_BOMService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public dynamic Edit(dynamic data)
        {
            dynamic list = data.list.updated;
            string referenceCode = data.referenceCode;
            return new SYS_PartService().CreateInventoryCode(list, referenceCode);
        }
    }
}
