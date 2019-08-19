
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
    public class WMS_BN_TransfersMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/WMS_BN_TransfersMain",
                    remove = "/api/Mms/WMS_BN_TransfersMain/",
                    billno = "/api/Mms/WMS_BN_TransfersMain/getnewbillno",
                    audit = "/api/Mms/WMS_BN_TransfersMain/audit/",
                    edit1 = "/Mms/WMS_BN_TransfersMain/edit/"
                },
                resx = new
                {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                    IsEnable = 1,
                },
                form = new
                {
                    BillCode = "",
                    OutWarehouseName = "",
                    InWarehouseName = ""
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            var data = new WMS_BN_TransfersDetailApiController().GetPageData(id);
            string billCode = new WMS_BN_TransfersMainService().GetBillCodeByID(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/WMS_BN_TransfersDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/WMS_BN_TransfersDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/WMS_BN_TransfersDetail/audit/",                    //审核api
                    newkey = "/api/Mms/WMS_BN_TransfersDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/WMS_BN_TransfersDetail/PostStorage/"
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
                    pageData = data,
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new WMS_BN_TransfersMain().Extend(new { BillCode = data.form == null ? "系统生成" : billCode, OutWarehouseCode = "", OutWarehouseName = "", InWarehouseCode = "", InWarehouseName = "", IsEnable = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","BillCode","InventoryCode","InventoryName","Unit","MateNum","ConfirmNum","BatchCode","Remark","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime","Model","UnitPrice","TotalPrice"},
                          defaults = new {ID = "",BillCode = "",InventoryCode = "",InventoryName = "",Unit = "",MateNum = "",ConfirmNum = "",BatchCode = "",Remark = "",IsEnable = 1,CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = "",Model="",UnitPrice="",TotalPrice=""}
                        }
}
            };
            return View(model);
        }

    }

    public class WMS_BN_TransfersMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new WMS_BN_TransfersMainService().GetNewKey("ID", "Maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>WMS_BN_TransfersMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='like'></field>   
        <field name='OutWarehouseName'		cp='like'></field>   
        <field name='InWarehouseName'		cp='like'></field>   
    </where>
</settings>");
            var service = new WMS_BN_TransfersMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
    public class WMS_BN_TransfersDetailApiController : ApiController
    {
        //public dynamic GetPageData(string id)
        //{
        //    var masterService = new WMS_BN_TransfersMainService();
        //    var pQuery = ParamQuery.Instance().AndWhere("ID", id);

        //    var result = new
        //    {
        //        //主表数据
        //        form = masterService.GetModel(pQuery),
        //        scrollKeys = masterService.ScrollKeys("ID", id),

        //        //明细数据
        //        tab0 = new WMS_BN_TransfersDetailService().GetDynamicList(pQuery)
        //    };
        //    return result;
        //}
        public dynamic GetPageData(string id)
        {

            var masterService = new WMS_BN_TransfersMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var mainData = masterService.GetModel(pQuery);
            if (mainData != null)
            {
                var pQuery2 = ParamQuery.Instance().AndWhere("BillCode", mainData.BillCode);
                var result = new
                {
                    //主表数据
                    form = mainData,
                    scrollKeys = masterService.ScrollKeys("ID", id),

                    //明细数据
                    tab0 = new WMS_BN_TransfersDetailService().GetDynamicList(pQuery2)
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
                .Update("WMS_BN_TransfersMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new WMS_BN_TransfersMainService();
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
                    var service0 = new WMS_BN_TransfersDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }
        public string PostStorage(string billCode)
        {
            string msg = "";

            var result = new WMS_BN_TransfersMainService().PostBuild(billCode, out msg);

            return msg;
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        WMS_BN_TransfersMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>WMS_BN_TransfersDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new WMS_BN_TransfersMainService();
            if (data.form["BillCode"] == "系统生成")
            {

                string documentNo = MmsHelper.GetOrderNumber("WMS_BN_TransfersMain", "BillCode", "PDD", "", "");
                data.form["BillCode"] = documentNo;
                foreach (JToken tab in data["tabs"].Children())
                {
                    foreach (JProperty item in tab.Children())
                    {
                        if (item.Name == "inserted")
                        {
                            foreach (var row in item.Value.Children())
                            {
                                row["BillCode"] = documentNo;
                            }
                        }
                    }
                }
            }

            var result = service.EditPage(data, formWrapper, tabsWrapper);

        }
    }

}
