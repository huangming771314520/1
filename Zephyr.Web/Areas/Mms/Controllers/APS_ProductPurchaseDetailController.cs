
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
    public class APS_ProductPurchaseDetailController : Controller
    {
        /// <summary>
        /// 采购分单主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProductPurchaseDetail1",
                    newkey = "/api/Mms/APS_ProductPurchaseDetail1/getnewkey",
                    edit = "/api/Mms/APS_ProductPurchaseDetail1/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WarehouseName = "",
                    PurchaseDate = "",
                    ContractCode = "",
                    ProjectName = "",
                    SaleMan = "",
                    UserCode = "",
                    DepartmentCode = code
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "MainID", "InventoryCode", "SingleProductRequestQuantity", "TotalRequestQuantity", "Remark", "PurchaseState", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "PurchaseQuantity", "StockQuantity", "RequestedQuantity", "SaleMan", "DepartmentCode", "DepartmentName", "WarehouseCode", "WarehouseName", "SupplierCode", "SupplierName", "OrderQuantity", "PurchaseFeedback", "PurchaseRemark", "UserCode", "PurchaseDate" }
                }
            };

            return View(model);
        }
        /// <summary>
        /// 采购接单主界面
        /// </summary>
        /// <returns></returns>

        public ActionResult Index2()
        {
            var code = new sys_codeService();
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string departcode = string.Empty;
            string departname = string.Empty;
            if (depart != null)
            {
                departcode = depart.DepartmentCode;
                departname = depart.DepartmentName;
            }
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_ProductPurchaseDetail1",
                    newkey = "/api/Mms/APS_ProductPurchaseDetail1/getnewkey",
                    edit = "/api/Mms/APS_ProductPurchaseDetail1/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WarehouseName = "",
                    PurchaseDate = "",
                    ContractCode = "",
                    ProjectName = "",
                    SaleMan = MmsHelper.GetUserName(),
                    UserCode = MmsHelper.GetUserCode(),
                    DepartmentCode = departcode
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "MainID", "InventoryCode", "SingleProductRequestQuantity", "TotalRequestQuantity", "Remark", "PurchaseState", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "PurchaseQuantity", "StockQuantity", "RequestedQuantity", "SaleMan", "DepartmentCode", "DepartmentName", "WarehouseCode", "WarehouseName", "SupplierCode", "SupplierName", "OrderQuantity", "PurchaseFeedback", "PurchaseRemark", "UserCode", "PurchaseDate" }
                }
            };

            return View(model);
        }

    }

    public class APS_ProductPurchaseDetail1ApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>V_ProductPurchaseDetailAndMain</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='WarehouseName' 		 cp='like'></field>
                <field name='PurchaseDate' 		 cp='equal'></field>
                <field name='UserCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_ProductPurchaseDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new APS_ProductPurchaseDetailService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            APS_ProductPurchaseDetail
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_ProductPurchaseDetailService();
            var result = service.Edit(null, listWrapper, data);
        }


        public string PostSetType(dynamic data)
        {
            string msg = "";
            string ids = data["ids"].ToString();
            ids = ids.Remove(ids.Length - 2, 1);
            if(data["UserCode"]==null||data["SaleMan"]==null)
            {
                msg = "请选择采购员";
            }
            var result = new APS_ProductPurchaseDetailService().SetType(ids, data["UserCode"].ToString(), data["SaleMan"].ToString(), out msg);

            return msg;
        }
    }
}
