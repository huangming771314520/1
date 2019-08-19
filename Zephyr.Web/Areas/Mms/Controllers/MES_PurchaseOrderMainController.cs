
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_PurchaseOrderMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_PurchaseOrderMain",
                    remove = "/api/Mms/MES_PurchaseOrderMain/",
                    billno = "/api/Mms/MES_PurchaseOrderMain/getnewbillno",
                    audit = "/api/Mms/MES_PurchaseOrderMain/audit/",
                    edit1 = "/Mms/MES_PurchaseOrderMain/edit/"
                },
                resx = new
                {
                    detailTitle = "采购订单明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    BillCode = "",
                    ProductPurchaseCode = "",
                    ContractCode = "",
                    BillState = 1
                },
                idField = "ID"
            };

            return View(model);
        }

        public ActionResult SearchPurchaseOrder()
        {
            return View();
        }
        public ActionResult Edit(string id = "")
        {
            var data = new MES_PurchaseOrderDetailApiController().GetPageData(id);
            bool isEdit = true;
            if(data.form==null)
            {
                isEdit = false;
                ViewBag.isEdit = isEdit;
            }
            else
            {
                ViewBag.isEdit = isEdit;
            }
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_PurchaseOrderDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_PurchaseOrderDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_PurchaseOrderDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_PurchaseOrderDetail/GetNewRowId/",           //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/MES_PurchaseOrderDetail/PostStorage/"      //审核单据
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed = "单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new
                {
                    pageData = data
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    
                    defaults = new MES_PurchaseOrderMain().Extend(new { ID = id, BillCode = data.form == null ? "系统生成" : data.form.BillCode, ProductPurchaseCode = "", ContractCode = "", OrderDate = "", SaleMan = MmsHelper.GetUserName(), UserCode = MmsHelper.GetUserCode(), BillState = 1, IsEnable = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                         // postFields = new string[] { "ID", "MainID", "InventoryCode", "InventoryName", "OrderQuantity", "GoodsQuantity", "IsFinish", "Model", "Unit", "UnitPrice", "TotalPrice", "IsEnable", "WarehouseID", "SupplierCode", "CheckedQuantity", "PurchaseContract" },
                          postFields = new string[] { "ID","MainID","InventoryCode","InventoryName","OrderQuantity","GoodsQuantity","IsFinish","Model","Unit","UnitPrice","TotalPrice","IsEnable","WarehouseID","SupplierCode","CheckedQuantity","PlanArrivelDate","MatchTableID"},
                          defaults = new {ID = "",MainID = id,InventoryCode = "",OrderQuantity = "",GoodsQuantity = "",Model="",Unit="",UnitPrice="",TotalPrice="",IsFinish = 1,IsEnable=1,SupplierCode="",WarehouseID="",PlanArrivelDate="",MatchTableID=""}
                        }
}
            };
            return View(model);
        }
    }

    public class MES_PurchaseOrderMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new MES_PurchaseOrderMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>MES_PurchaseOrderMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='equal'></field>   
        <field name='ProductPurchaseCode'		cp='equal'></field>   
        <field name='ContractCode'		cp='equal'></field>   
        <field name='BillState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new MES_PurchaseOrderMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }

        public dynamic GetPurchaseOrderData(DateTime? purchaseDate, string saleMan, string SupplierName)
        {
            using (var db = Db.Context("Mms"))
            {
                string where = string.Empty;
                if (purchaseDate != null)
                {
                    string strBeginData = Convert.ToDateTime(purchaseDate.ToString()).ToString("yyyy-MM-dd 00:00:00");
                    where += string.Format(@" and OrderDate='{0}'", strBeginData);
                }
                if (saleMan != null)
                {
                    saleMan = "%" + saleMan + "%";
                    where += string.Format(@" and t1.SaleMan like '{0}'", saleMan);
                }
                if (SupplierName != null)
                {
                    SupplierName = "%" + saleMan + "%";
                    where += string.Format(@" and t1.SupplierName like '{0}'", SupplierName);
                }
                string sql1 = string.Format(@"select 
t1.BillCode,
t1.ProductPurchaseCode,
t1.ContractCode, 
t1.OrderDate,
t1.WarehouseID,
t1.WarehouseName,
t1.SupplierCode,
t1.SupplierName,
t1.UserCode,
t1.SaleMan,
BillState,
t1.IsEnable,
t2.InventoryCode,
t2.InventoryName,
t2.OrderQuantity,
GoodsQuantity,
Model,
Unit,
UnitPrice,
TotalPrice,
IsFinish
from MES_PurchaseOrderMain t1 left join MES_PurchaseOrderDetail t2 on
 t1.id=t2.Mainid where t1.IsEnable=1 and t2.IsEnable=1 {0}
", where);


                List<dynamic> list1 = db.Sql(sql1).QueryMany<dynamic>();

                return list1;
            }
        }

    }

    public class MES_PurchaseOrderDetailApiController : ApiController
    {
        public int GetDelete(string id)
        {
            return new MES_PurchaseOrderMainService().GetDelete(id);
        }
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_PurchaseOrderMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id).AndWhere("IsEnable",1);
            var maindata = masterService.GetModel(pQuery);
            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new MES_PurchaseOrderDetailService().GetDynamicList(pQuery2)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_PurchaseOrderMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new MES_PurchaseOrderMainService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }

        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new MES_PurchaseOrderDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var id = data.form["ID"].ToString();
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("ID", id);
            var re = new MES_PurchaseOrderMainService().GetModelList(pQuery);

            if (re.Count > 0 && re[0].BillState == 2)
            {
                MmsHelper.ThrowHttpExceptionWhen(true, "已审核数据不能修改！");
                return;
            }
            var ids = "(";
            foreach (JToken tab in data["tabs"].Children())
            {
                foreach (JProperty item in tab.Children())
                {
                    if (item.Name == "deleted")
                    {
                        foreach (var row in item.Value.Children())
                        {
                            ids += "'" + row["ID"] + "',";
                        }
                        item.Value = "[]";
                    }
                }
            }
            if (ids != "(")
            {
                ids = ids.Remove(ids.Length - 1, 1);
                ids += ")";
                using (var db = Db.Context("Mms"))
                {
                    string sql1 = string.Format("update MES_PurchaseOrderDetail set IsEnable=0 where ID in {0} ", ids);
                    db.Sql(sql1).Execute();

                }
            }
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_PurchaseOrderMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_PurchaseOrderDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("MES_PurchaseOrderMain", "BillCode", "CGDD", "", "");
                data.form["BillCode"] = documentNo;

            }
            var service = new MES_PurchaseOrderMainService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new MES_PurchaseOrderDetailService().AuditBillCode(data["mainID"].ToString(), out msg);

            return msg;
        }
        public string GetContractInfo(string contractCode)
        {
            return new MES_PurchaseOrderDetailService().GetContractInfo(contractCode);
        }
        public dynamic GetProductPurchase(string CodeArr)
        {
            return new APS_ProductPurchaseDetailService().GetProductPurchase(CodeArr);

        }
    }
}
