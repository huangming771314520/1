
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
    public class SYS_DrawingApplicationController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/SYS_DrawingApplication",
                    newkey = "/api/Mms/SYS_DrawingApplication/getnewkey",
                    edit = "/api/Mms/SYS_DrawingApplication/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    RequestCode = "",
                    ContractCode = "",
                    ProductName = "",
                    FigureCode = "",
                    RequestStatus = ""
                },
                defaultRow = new
                {
                    IsEnable = 1,
                    RequestStatus = 0
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "RequestCode", "ContractCode", "ProductID", "ProductName", "RootPartCode", "PartCode", "PartName", "FigureCode", "ApplicationReason", "RequestStatus", "CreatePerson", "CreateTime", "IsEnable" }
                }
            };

            return View(model);
        }
    }

    public class SYS_DrawingApplicationApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='CreateTime desc'>
        <select>*</select>
        <from>SYS_DrawingApplication</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='RequestCode' 		 cp='like'></field>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='ProductName' 		 cp='like'></field>
                <field name='FigureCode' 		 cp='like'></field>
                <field name='RequestStatus' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_DrawingApplicationService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new SYS_DrawingApplicationService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            dynamic insert_list = data.list.inserted;
            if (data.list.inserted.ToString() != "[]")
            {
                var PlanCode = MmsHelper.GetOrderNumber("SYS_DrawingApplication", "RequestCode", "TZSQ", "", "");
                string PreCode = PlanCode.Substring(0, PlanCode.Length - 3);
                int StartNumber = Convert.ToInt32(PlanCode.Substring(PlanCode.Length - 3));
                foreach (dynamic item in data.list.inserted)
                {
                    item["RequestCode"] = PreCode + StartNumber.ToString().PadLeft(3, '0');
                    StartNumber++;
                }
            }
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_DrawingApplication
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_DrawingApplicationService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
