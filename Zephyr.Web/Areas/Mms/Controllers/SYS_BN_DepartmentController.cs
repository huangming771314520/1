
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
    public class SYS_BN_DepartmentController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var department = new SYS_BN_DepartmentService();
            var model = new
            {
                dataSource = new
                {
                    dsData = department.GetIDNameList()
                },
                urls = new
                {
                    query = "/api/Mms/SYS_BN_Department",
                    newkey = "/api/Mms/SYS_BN_Department/getnewkey",
                    edit = "/api/Mms/SYS_BN_Department/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    DepartmentName = "",
                    IsWorkshop = "",
                    IsEnable = ""
                },
                defaultRow = new
                {
                    IsWorkshop = 1,
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "DepartmentName", "DepartmentCode", "ParentCode", "ParentName", "IsWorkshop", "IsEnable" }
                }
            };

            return View(model);
        }
    }

    public class SYS_BN_DepartmentApiController : ApiController
    {
        public dynamic GetDepartTree()
        {
            //return new SYS_BOMService().GetBoomTree(code);

            return new SYS_BN_DepartmentService().GetDepartTree();
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>SYS_BN_Department</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='DepartmentName' 		 cp='like'></field>
                <field name='IsWorkshop' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_BN_DepartmentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new SYS_BN_DepartmentService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_BN_Department
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_BN_DepartmentService();
            var result = service.Edit(null, listWrapper, data);
        }
        public string GetCode()
        {
            string documentNo = MmsHelper.GetOrderNumber("SYS_BN_Department", "DepartmentCode", "BMBH", "", "");
            return documentNo;
        }
    }
}
