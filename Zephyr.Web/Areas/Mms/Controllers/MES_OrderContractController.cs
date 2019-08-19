
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
    public class MES_OrderContractController : Controller
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
                    query = "/api/Mms/MES_OrderContract",
                    newkey = "/api/Mms/MES_OrderContract/getnewkey",
                    edit = "/api/Mms/MES_OrderContract/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    OrderContractCode = "",
                    BillState=""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "OrderContractCode", "OrderContractName", "BillState", "IsEnable" }
                }
            };

            return View(model);
        }
    }

    public class MES_OrderContractApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>MES_OrderContract</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='OrderContractCode' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new MES_OrderContractService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable",1));
            return result;
        }

        public string GetNewKey()
        {
            return new MES_OrderContractService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public int GetDelete(string id)
        {
            return new MES_OrderContractService().GetDelete(id);
        }
        public string GetAuditSearchEditBillState(string whereSql)
        {
            return new MES_OrderContractService().AuditSearchEditBillState(whereSql);
        }
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            MES_OrderContract
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_OrderContractService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
