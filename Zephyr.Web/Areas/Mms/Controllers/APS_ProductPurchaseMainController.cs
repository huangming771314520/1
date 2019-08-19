
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
using System.Linq;
using Zephyr.Data;

namespace Zephyr.Areas.Mms.Controllers
{
    public class APS_ProductPurchaseMainController : Controller
    {
        public static int PurchaseType { get; set; } = 1;

        public ActionResult Index(int id = 1)
        {
            PurchaseType = id;
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/APS_ProductPurchaseMain",
                    remove = "/api/Mms/APS_ProductPurchaseMain/",
                    billno = "/api/Mms/APS_ProductPurchaseMain/getnewbillno",
                    audit = "/api/Mms/APS_ProductPurchaseMain/audit/",
                    edit1 = "/Mms/APS_ProductPurchaseMain/edit/"
                },
                resx = new
                {
                    detailTitle = "生产请购明细",
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
                    PurchaseDocumentCode = "",
                    ContractCode = "",
                    PurchaseDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    WarehouseID = ""
                },
                idField = "ID"
            };
            ViewBag.PurchaseType = id;
            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new APS_ProductPurchaseDetailApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/APS_ProductPurchaseDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/APS_ProductPurchaseDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/APS_ProductPurchaseDetail/audit/",                    //审核api
                    newkey = "/api/Mms/APS_ProductPurchaseDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/APS_ProductPurchaseDetail/PostStorage/"
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
                    defaults = new APS_ProductPurchaseMain().Extend(new { ID = id, PurchaseDocumentCode = data.form == null ? "系统生成" : data.form.PurchaseDocumentCode, ContractCode = "", ProductPlanCode = "", PurchaseDate = DateTime.Now, WarehouseID = "", IsEnable = 1, Billstate = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",

                          postFields = new string[] { "ID","MainID","InventoryCode","SingleProductRequestQuantity","TotalRequestQuantity","Remark","PurchaseState","IsEnable"
                              ,"PurchaseQuantity","StockQuantity","RequestedQuantity","SaleMan","DepartmentCode","DepartmentName","WarehouseCode","WarehouseName","SupplierCode","SupplierName"
                              ,"OrderQuantity","PurchaseFeedback","PurchaseRemark","PlanArrivelDate"},
                          defaults = new {ID = "",MainID = id,InventoryCode = "",SingleProductRequestQuantity = "",TotalRequestQuantity = "",Remark = "",PurchaseState = 0,IsEnable = 1}
                        }
}
            };
            return View(model);
        }
    }

    public class APS_ProductPurchaseMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new APS_ProductPurchaseMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>v_APS_ProductPurchaseMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='PurchaseDocumentCode'		cp='equal'></field>   
        <field name='ContractCode'		cp='equal'></field>   
        <field name='PurchaseDate'		cp='daterange'></field>   
        <field name='WarehouseID'		cp='equal'></field>   
    </where>
</settings>");
            var service = new APS_ProductPurchaseMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("PurchaseType", APS_ProductPurchaseMainController.PurchaseType));
            return result;
        }

        public string GetInsertPurchase(string code, string deID,int? pType)
        {
            string msg = "";
            var result = new APS_ProductPurchaseMainService().InsertPurchase(code, deID, pType, out msg);
            return msg;
        }

        public void PostCreateProductRequest(dynamic data)
        {
            var list = data.list;
            foreach (dynamic item in list)
            {
                string ContractCode = item["ContractCode"];
                string CreateType = item["CreateType"];
                var BoardOrBarData = new List<dynamic>();
                switch (CreateType)
                {
                    case "板材":
                        BoardOrBarData = new PRS_BoardCreateMateService().GetDynamicList(ParamQuery.Instance().AndWhere("ContractCode", ContractCode));
                        break;
                    case "棒材":
                        BoardOrBarData = new PRS_BarCreateMateService().GetDynamicList(ParamQuery.Instance().AndWhere("ContractCode", ContractCode));
                        break;
                    case "车间":
                        int MainID = item["ID"];
                        BoardOrBarData = new MES_WorkshopPurchaseDetailService().GetDynamicList
                            (
                            ParamQuery
                            .Instance()
                            .Select("InventoryCode,PurchaseQuantity as InventoryNum,Remark")
                            .From("MES_WorkshopPurchaseDetail")
                            .AndWhere("MainID", MainID)
                            );
                        break;
                    default:
                        break;
                }
                using (var db = Db.Context("Mms"))
                {
                    db.UseTransaction(true);
                    try
                    {
                        int MainID = new APS_ProductPurchaseMainService().GetModelList().Count > 0 ?
                        new APS_ProductPurchaseMainService().GetModelList().Max(a => a.ID) + 1 : 1;
                        db.Insert("APS_ProductPurchaseMain")
                        .Column("ID", MainID)
                        .Column("PurchaseDocumentCode", MmsHelper.GetOrderNumber("APS_ProductPurchaseMain", "PurchaseDocumentCode", "SCQG", "", ""))
                        .Column("ContractCode", ContractCode)
                        .Column("DepartmentCode", "0202")
                        .Column("PurchaseDate", DateTime.Now)
                        .Column("IsEnable", 1)
                        .Column("BillState", 1)
                        .Column("PurchaseType", CreateType.Equals("板材") ? 2 : CreateType.Equals("棒材") ? 3 : CreateType.Equals("车间") ? 4 : 1)
                        .Column("CreateTime", DateTime.Now)
                        .Column("CreatePerson", MmsHelper.GetUserName()).Execute();
                        foreach (var item_board in BoardOrBarData)
                        {
                            double PurchaseQuantity = Convert.ToDouble(item_board.InventoryNum);
                            string InventoryCode = item_board.New_InventoryCode;
                            string Remark = item_board.Remark;
                            string unit = "";
                            if (CreateType != "车间")
                            {
                                unit = item_board.Unit;
                            }

                            db.Insert("APS_ProductPurchaseDetail")
                            .Column("MainID", MainID)
                            //.Column("InventoryCode", InventoryCode)
                            .Column("InventoryCode", InventoryCode)
                            .Column("TotalRequestQuantity", Convert.ToInt32(PurchaseQuantity))
                            .Column("PurchaseQuantity", PurchaseQuantity)
                            .Column("StockQuantity", 0)
                            .Column("PurchaseState", 0)
                            .Column("DepartmentCode", "0202")
                            .Column("DepartmentName", "供应")
                            .Column("IsEnable", 1)
                            .Column("CreateTime", DateTime.Now)
                            .Column("Remark", Remark)
                            .Column("Unit", unit)
                            .Column("CreatePerson", MmsHelper.GetUserName()).ExecuteReturnLastId<int>();
                        }
                        db.Commit();
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        break;
                    }
                }
            }
        }
    }

    public class APS_ProductPurchaseDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new APS_ProductPurchaseMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id);
            string sql = string.Format(@"SELECT DISTINCT t1.*,
       t3.InventoryName,
       t3.QuantityUnit,
       t3.Spec,
       t3.Model
FROM APS_ProductPurchaseDetail t1
    LEFT JOIN PRS_Process_BOM t2
        ON t1.InventoryCode = t2.InventoryCode
           AND t2.IsEnable = 1
    LEFT JOIN dbo.SYS_Part t3
        ON t3.InventoryCode = t1.InventoryCode
           AND t3.IsEnable = 1
WHERE t1.mainid={0}", id);

            var data = Db.Context("Mms").Sql(sql).QueryMany<dynamic>();

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = data
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("APS_ProductPurchaseMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new APS_ProductPurchaseMainService();
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
                    var service0 = new APS_ProductPurchaseDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }



        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        APS_ProductPurchaseMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>APS_ProductPurchaseDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["PurchaseDocumentCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("APS_ProductPurchaseMain", "PurchaseDocumentCode", "SCQG", "", "");
                data.form["PurchaseDocumentCode"] = documentNo;

            }
            var service = new APS_ProductPurchaseMainService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
        public dynamic GetBom(string parentCode, string ContractCode, string ProductName)
        {
            var list = new MES_BN_ProductProcessRouteService().GetGYBom(parentCode);
            //var notShowList = new MES_BN_MatchingTableDetailService().GetNotShowBom(parentCode, ContractCode, ProductName, "0");
            //foreach (var item in notShowList)
            //{
            //    var part = (from p in list where p.PartCode == item select p).FirstOrDefault();
            //    if (part != null)
            //    {
            //        list.Remove(part);
            //    }
            //}

            return list;
        }
        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new APS_ProductPurchaseDetailService().AuditBillCode(data["mainID"].ToString(), out msg);

            return msg;
        }
        public dynamic GetWorkshopPurchaseDetail(string mainID)
        {
            return new MES_WorkshopPurchaseDetailService().GetWorkshopPurchaseDetail(mainID);
        }

        public string GetDelete(string id)
        {
            string msg = "删除成功！";
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("ID", id);
            var re = new APS_ProductPurchaseMainService().GetModel(pQuery);
            if (re == null || re.BillState == 2)
            {
                msg = "已审核不能删除！";
            }
            else
            {
                var result = new APS_ProductPurchaseMainService().GetDelete(id);
                var pQuery2 = ParamQuery.Instance().Select("*").AndWhere("MainID", id);
                var services = new APS_ProductPurchaseDetailService();
                var list = services.GetModelList(pQuery2);
                foreach (var item in list)
                {
                    services.GetDelete(item.ID.ToString());
                }

            }

            return msg;
        }
    }
}
