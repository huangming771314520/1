
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
    public class PRS_ProcessWorkStepsController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/PRS_ProcessWorkSteps",
                    remove = "/api/Mms/PRS_ProcessWorkSteps/",
                    billno = "/api/Mms/PRS_ProcessWorkSteps/getnewbillno",
                    audit = "/api/Mms/PRS_ProcessWorkSteps/audit/",
                    edit = "/Mms/PRS_ProcessWorkSteps/edit/"
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
                    WorkStepsName = "" 
                },
                idField="id"
            };

            return View(model);
        }
    }

    public class PRS_ProcessWorkStepsApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new PRS_ProcessWorkStepsService().GetNewKey("id", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='id'>
    <select>*</select>
    <from>PRS_ProcessWorkSteps</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='WorkStepsName'		cp='equal'></field>   
    </where>
</settings>");
            var service = new PRS_ProcessWorkStepsService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
