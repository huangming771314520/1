
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
    public class PMS_ProductTaskController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/PMS_ProductTask",
                    remove = "/api/Mms/PMS_ProductTask/",
                    billno = "/api/Mms/PMS_ProductTask/getnewbillno",
                    audit = "/api/Mms/PMS_ProductTask/audit/",
                    edit = "/Mms/PMS_ProductTask/edit/"
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
                    ProjectName = "",
                    ContractReceiveTime = "",
                    SJTaskType = "",
                    RWTaskType=""
                }
                //idField="ID"
            };

            return View(model);
        }
    }

    public class PMS_ProductTaskApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new PMS_ProductTaskService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ProductID'>
    <select>*</select>
    <from>V_PMS_ProjectTaskData</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ProjectName'		cp='like'></field>   
        <field name='ContractReceiveTime'		cp='equal'></field>  
<field name='SJTaskType'		cp='equal'></field>
<field name='RWTaskType'		cp='equal'></field>
    </where>
</settings>");
            var service = new PMS_ProductTaskService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
