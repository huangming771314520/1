
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
    public class MES_BN_MatchingTableDetailController : Controller
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
                    query = "/api/Mms/MES_BN_MatchingTableDetail",
                    newkey = "/api/Mms/MES_BN_MatchingTableDetail/getnewkey",
                    edit = "/api/Mms/MES_BN_MatchingTableDetail/edit",
                    audit1 = "/api/Mms/MES_BN_MatchingTableDetail/GetAudit1"
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
                    ProjectName = "",
                    ProductName = "",
                    ProjectDetailID = "",
                    ProductType = "",
                    Model = "",
                    Specifications = "",
                    ProductPlanCode = "",
                    DesignTaskCode = "",
                    Type=""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProjectName", "PPartCode", "PPartName", "PartCode", "PartName", "BomQuantity", "NeedQuantity", "Type", "TypeName", "IsMaterial", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "ProductPlanCode", "DesignTaskCode" }
                }
            };

            return View(model);
        }

        public ActionResult Index2()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/MES_BN_MatchingTableDetail",
                    newkey = "/api/Mms/MES_BN_MatchingTableDetail/getnewkey",
                    edit = "/api/Mms/MES_BN_MatchingTableDetail/edit",
                    audit1 = "/api/Mms/MES_BN_MatchingTableDetail/GetAudit1"
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
                    ProjectName = "",
                    ProductName = "",
                    ProjectDetailID = "",
                    ProductType = "",
                    Model = "",
                    Specifications = "",
                    ProductPlanCode = "",
                    DesignTaskCode = "",
                    Type = ""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectDetailID", "ProjectName", "PPartCode", "PPartName", "PartCode", "PartName", "BomQuantity", "NeedQuantity", "Type", "TypeName", "IsMaterial", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "ProductPlanCode", "DesignTaskCode" }
                }
            };

            return View(model);
        }
    }

    public class MES_BN_MatchingTableDetailApiController : ApiController
    {
        public string GetValue(string text)
        {
            return new sys_codeService().Getsys_codeByTypeAndtext("PeiTao", text);
        }
        public string PostSetType(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 2, 1);
            var result = new MES_BN_MatchingTableDetailService().SetType(ids, data["type"].ToString(), data["typeName"].ToString(), out msg);

            return msg;
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>V_MES_BN_MatchingTableDetail</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
<field name='ContractCode' 		 cp='equal'></field>
<field name='ProjectDetailID' 		 cp='equal'></field>
<field name='Type' 		 cp='equal'></field>
<field name='ProductType' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_BN_MatchingTableDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new MES_BN_MatchingTableDetailService().GetNewKey("ID", "maxplus").PadLeft(6, '0');
        }
        public string GetAudit1(string ContractCode, string ProjectDetailID)
        {
            return new MES_BN_MatchingTableDetailService().GetAudit(ContractCode, ProjectDetailID);
        }
        // public int GetIsExit(string ContractCode, string ProjectDetailID, string ProductPlanCode)
        public int GetIsExit(string ContractCode, string ProjectDetailID)
        {
            return new MES_BN_MatchingTableDetailService().GetIsExit(ContractCode, ProjectDetailID);
            //return new MES_BN_MatchingTableDetailService().GetIsExit(ContractCode, ProjectDetailID, ProductPlanCode);
        }



        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            //dynamic insert_list = data.list.inserted;
            //if (data.list.inserted.ToString() != "[]")
            //{
            //    var mate_list = new PRS_Process_BOMService().GetModelList();
            //    foreach (dynamic item in data.list.inserted)
            //    {
            //        string PartCode = item["PartCode"];
            //        var mate_where_list = mate_list.Where(p => p.PartCode == PartCode && !string.IsNullOrWhiteSpace(p.MaterialCode) && (p.MaterialCode.StartsWith("锻") || p.MaterialCode.StartsWith("Z") || p.MaterialCode.StartsWith("z")));
            //        if (mate_where_list.Count() > 0)
            //        {
            //            item["Type"] = "012401";
            //            item["TypeName"] = "铸件配套";
            //        }
            //    }
            //}

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            MES_BN_MatchingTableDetail
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_BN_MatchingTableDetailService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
