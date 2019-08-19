
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
    public class MES_GoodsInspectionController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/MES_GoodsInspection",
                    remove = "/api/Mms/MES_GoodsInspection/",
                    billno = "/api/Mms/MES_GoodsInspection/getnewbillno",
                    audit = "/api/Mms/MES_GoodsInspection/audit/",
                    edit1 = "/Mms/MES_GoodsInspection/edit/"
                },
                resx = new {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new{
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new{
                    BillCode = "" ,
                    ContractCode = "" ,
                    SuppplierName = "" 
                },
                idField="ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new MES_GoodsInspectionDetailApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_GoodsInspectionDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_GoodsInspectionDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_GoodsInspectionDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_GoodsInspectionDetail/GetNewRowId/" ,           //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/MES_GoodsInspectionDetail/PostStorage/"      //审核单据
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
                    defaults = new MES_GoodsInspection().Extend(new {ID=id, BillCode = data.form == null ? "系统生成" : data.form.BillCode, PurchaseOrderCode = "", ProjectName = "", SuppplierName = "", DepartmentName = "", SaleMan = "", ArrivalDate = DateTime.Now.ToString("yyyy-MM-dd"), WarehouseName = "", IsEnable = 1, BillState =1}),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","InventoryCode","InVentoryName","Model","ArrivalQuatity","Unit","IsEnable","Material","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {ID = "",MainID = "",InventoryCode = "",InVentoryName = "",Model = "",ArrivalQuatity = "",Unit = "",IsEnable = 1,CreatePerson = "",Material="",CreateTime = "",ModifyPerson = "",ModifyTime = ""}
                        }
}
            };
            return View(model);
        }
    }

    public class MES_GoodsInspectionApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new MES_GoodsInspectionService().GetNewKey("ID", "Maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>MES_GoodsInspection</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='like'></field>   
        <field name='ContractCode'		cp='like'></field>   
        <field name='SuppplierName'		cp='equal'></field>   
    </where>
</settings>");
            var service = new MES_GoodsInspectionService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
    public class MES_GoodsInspectionDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_GoodsInspectionService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var mainData = masterService.GetModel(pQuery);
            if (mainData != null)
            {
                var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id);
                var result = new
                {
                    //主表数据
                    form = mainData,
                    scrollKeys = masterService.ScrollKeys("ID", id),
                    //明细数据
                    tab0 = new MES_GoodsInspectionDetailService().GetDynamicList(pQuery2)
                };
                return result;
            }
            else
            {
                var result = new
                 {
                     //主表数据
                     form = mainData,
                     scrollKeys = masterService.ScrollKeys("ID", id),

                     //明细数据
                     tab0 = ""
                 };
                return result;
            }
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_GoodsInspection")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new MES_GoodsInspectionService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }

        public dynamic GetPurchaseOrder(string mainID)
        {
            string msg = "";

            var result = new MES_PurchaseOrderDetailService().GetPurchaseOrder(mainID);

            return result;
        }

        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new MES_GoodsInspectionDetailService();
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
            var re = new MES_GoodsInspectionService().GetModelList(pQuery);

            if (re.Count > 0 && re[0].BillState == 2)
            {
                MmsHelper.ThrowHttpExceptionWhen(true, "已审核数据不能修改！");
                return;
            }
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_GoodsInspection
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_GoodsInspectionDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("MES_GoodsInspection", "BillCode", "CGBJ", "", "");
                data.form["BillCode"] = documentNo;

            }
            var service = new MES_GoodsInspectionService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new MES_GoodsInspectionDetailService().AuditBillCode(data["mainID"].ToString(), out msg);

            return msg;
        }
    }
}
