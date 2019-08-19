
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
    public class SYS_BN_UserController : Controller
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
                    query = "/api/Mms/SYS_BN_User",
                    newkey = "/api/Mms/SYS_BN_User/getnewkey",
                    edit = "/api/Mms/SYS_BN_User/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    UserName = "",
                    DepartmentCode = "",
                    IsEnable = ""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "UserCode", "UserName", "DepartmentCode", "DepartmentName", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
    }

    public class SYS_BN_UserApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>SYS_BN_User</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='UserName' 		 cp='like'></field>
                <field name='DepartmentCode' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_BN_UserService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new SYS_BN_UserService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_BN_User
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_BN_UserService();
            var result = service.Edit(null, listWrapper, data);
        }
        public string GetCode()
        {
            string documentNo = MmsHelper.GetOrderNumber("SYS_BN_User", "UserCode", "YGBH", "", "");
            return documentNo;
        }

    }
}
