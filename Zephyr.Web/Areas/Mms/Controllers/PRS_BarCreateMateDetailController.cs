
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
    public class PRS_BarCreateMateDetailController : Controller
    {
        public ActionResult Index(string BomList = "")
        {

            var model = new
            {
                PRS_BarCreateMateList = new PRS_BarCreateMateService().GetBomList(BomList),
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/PRS_BarCreateMateDetail",
                    newkey = "/api/Mms/PRS_BarCreateMateDetail/getnewkey",
                    edit = "/api/Mms/PRS_BarCreateMateDetail/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "InventoryCode", "InventoryName", "Model", "Specs", "TotalNum", "RealNum", "NeedNum", "ID", "ContractCode", "InventoryNum", "Unit" }
                }
            };

            return View(model);
        }


    }

    public class PRS_BarCreateMateDetailApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>a.*,b.FigureCode</select>
        <from>PRS_BarCreateMate a left join SYS_Part b on a.InventoryCode=b.InventoryCode</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
            <field name='ContractCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_BarCreateMateService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_BarCreateMateService().GetNewKey("ID", "maxplus").PadLeft(6, '0');
        }

        public dynamic GetBomList(string BomList = "")
        {
            return new PRS_BarCreateMateService().GetBomList(BomList);
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_BarCreateMate
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_BarCreateMateService();
            var result = service.Edit(null, listWrapper, data);
        }


    }
}
