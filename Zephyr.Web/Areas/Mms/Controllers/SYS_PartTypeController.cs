
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
    public class SYS_PartTypeController : Controller
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
                    query = "/api/Mms/SYS_PartType",
                    newkey = "/api/Mms/SYS_PartType/getnewkey",
                    edit = "/api/Mms/SYS_PartType/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PartTypeCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "PartTypeCode", "TypeName", "PPartTypeCode", "PTypeName", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
    }

    public class SYS_PartTypeApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {

            if (query["PartTypeCode"] != "")
            {
                query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>(select * from SYS_PartType where  PartTypeCode like 'CCC%') as temp</from>
    </settings>");
                var pQuery = query.ToParamQuery();
                pQuery.GetData().From = pQuery.GetData().From.Replace("CCC", query["PartTypeCode"].ToString());
                var service = new SYS_PartTypeService();
                var result = service.GetDynamicListWithPaging(pQuery);
                return result;

            }
            else
            {
                query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>SYS_PartType </from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='PartTypeCode' 		 cp='like'></field>
        </where>
    </settings>");
                var pQuery = query.ToParamQuery();
                var service = new SYS_PartTypeService();
                var result = service.GetDynamicListWithPaging(pQuery);
                return result;

            }


        }

        public string GetNewKey()
        {
            return new SYS_PartTypeService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_PartType
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_PartTypeService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
