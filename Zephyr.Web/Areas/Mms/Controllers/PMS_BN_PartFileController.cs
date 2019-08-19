
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

using System.Text;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace Zephyr.Areas.Mms.Controllers
{
    public class PMS_BN_PartFileController : Controller
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
                    query = "/api/Mms/PMS_BN_PartFile",
                    newkey = "/api/Mms/PMS_BN_PartFile/getnewkey",
                    edit = "/api/Mms/PMS_BN_PartFile/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = "",
                    ProductName = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectID", "ProjectDetailID", "ProjectName", "PPartCode", "PPartName", "PartCode", "PartName", "DocName", "FileName", "FileAddress", "State" }
                }
            };

            return View(model);
        }
    }

    public class PMS_BN_PartFileApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>V_PMS_BN_PartFile</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
<field name='ProductName' 		 cp='like'></field>
                <field name='ContractCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_PartFileService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("State", "0"));
            return result;
        }

        public string GetNewKey()
        {
            return new PMS_BN_PartFileService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PMS_BN_PartFile
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_PartFileService();
            var result = service.Edit(null, listWrapper, data);
        }

        public dynamic GetBom(string parentCode)
        {
            return new PMS_BN_PartFileService().GetBom(parentCode);

        }
        public string PostSubmit(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 2, 1);
            var result = new PMS_BN_PartFileService().PartFileSubmit(ids, out msg);

            return msg;
        }


       
    }




}
