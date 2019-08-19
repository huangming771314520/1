
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
    public class QMS_ShipmentInspectionController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/QMS_ShipmentInspection",
                    remove = "/api/Mms/QMS_ShipmentInspection/",
                    billno = "/api/Mms/QMS_ShipmentInspection/getnewbillno",
                    audit = "/api/Mms/QMS_ShipmentInspection/audit/",
                    edit1 = "/Mms/QMS_ShipmentInspection/edit/"
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
                    ProductName = "" 
                },
                idField="ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new QMS_ShipmentInspectionItemApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/QMS_ShipmentInspectionItem/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/QMS_ShipmentInspectionItem/edit/",                      //数据保存api
                    audit = "/api/Mms/QMS_ShipmentInspectionItem/audit/",                    //审核api
                    newkey = "/api/Mms/QMS_ShipmentInspectionItem/GetNewRowId/",         //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/QMS_ShipmentInspectionItem/PostStorage/"
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
                    //pageData = new QMS_ShipmentInspectionItemApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new QMS_ShipmentInspection().Extend(new { ID = id, BillCode = data.form == null ? "系统生成" : data.form.BillCode, ContractCode = "", ProjectName = "", ProductName = "", Model = "", QualifiedQuntity = "", IsQualified = 1, IsEnable = 1, BillState = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","InspectionItemCode","InspectionItemName","InspectionStandard","ShipMentCheckItemCode","CheckRecord","Inspector","InspectionCode","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {ID = "",MainID = id,InspectionItemCode = "",InspectionItemName = "",InspectionStandard = "",ShipMentCheckItemCode = "",CheckRecord = "",Inspector = "",InspectionCode = "",IsEnable = 1,CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = ""}
                        }
}
            };
            return View(model);
        }
    }

    public class QMS_ShipmentInspectionApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new QMS_ShipmentInspectionService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>QMS_ShipmentInspection</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='equal'></field>   
        <field name='ProductName'		cp='equal'></field>   
    </where>
</settings>");
            var service = new QMS_ShipmentInspectionService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class QMS_ShipmentInspectionItemApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new QMS_ShipmentInspectionService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var mainData = masterService.GetModel(pQuery);
            if (mainData != null)
            {
                var pQuery2 = ParamQuery.Instance().AndWhere("MainID", mainData.ID);
                var result = new
                {
                    //主表数据
                    form = mainData,
                    scrollKeys = masterService.ScrollKeys("ID", id),

                    //明细数据
                    tab0 = new QMS_ShipmentInspectionItemService().GetDynamicList(pQuery2)
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
                .Update("QMS_ShipmentInspection")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new QMS_ShipmentInspectionService();
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
                    var service0 = new QMS_ShipmentInspectionItemService();
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
        QMS_ShipmentInspection
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>QMS_ShipmentInspectionItem</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("QMS_ShipmentInspection", "BillCode", "FYJY", "", "");
                data.form["BillCode"] = documentNo;
                //foreach (JToken tab in data["tabs"].Children())
                //{
                //    foreach (JProperty item in tab.Children())
                //    {
                //        if (item.Name == "inserted")
                //        {
                //            foreach (var row in item.Value.Children())
                //            {
                //                row["BillCode"] = documentNo;
                //            }
                //        }
                //    }
                //}
            }

            var service = new QMS_ShipmentInspectionService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new QMS_ShipmentInspectionService().AuditBillCode(data["BillCode"].ToString(), out msg);

            return msg;
        }
    }
}
