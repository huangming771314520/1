
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
    public class WMS_BN_TakeStockMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/WMS_BN_TakeStockMain",
                    remove = "/api/Mms/WMS_BN_TakeStockMain/",
                    billno = "/api/Mms/WMS_BN_TakeStockMain/getnewbillno",
                    audit = "/api/Mms/WMS_BN_TakeStockMain/audit/",
                    edit1 = "/Mms/WMS_BN_TakeStockMain/edit/"
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
                    tackStockState = new sys_codeService().GetValueTextListByType("TackStockState"),
                    takeStockType = new sys_codeService().GetValueTextListByType("TakeStockType")
                },
                form = new
                {
                    BillCode = "",
                    BillState = "",
                    TakeStockType = "",
                    TakeStockYear = "",
                    TakeStockMonth = ""
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            var data = new WMS_BN_TakeStockMainApiController().GetPageData(id);
            
            var service = new WMS_BN_TakeStockMainService();
            var info = service.GetModelById(Convert.ToInt32(id));
           
           
            //service.GetMaxBillCode(info.TakeStockYear.Value,info.TakeStockMonth.Value);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/WMS_BN_TakeStockMain/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/WMS_BN_TakeStockMain/edit/",                      //数据保存api
                    audit = "/api/Mms/WMS_BN_TakeStockMain/audit/",                    //审核api
                    newkey = "/api/Mms/WMS_BN_TakeStockMain/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    tackStockState = new sys_codeService().GetValueTextListByType("TackStockState"),

                    takeStockType = new sys_codeService().GetValueTextListByType("TakeStockType")
                },
                form = new
                {

                    defaults = new WMS_BN_TakeStockMain().Extend(new
                    {
                        ID = data.scrollKeys.current,
                        BillCode = "系统生成",
                        BillState = 1,
                        TakeStockType = 1,
                        TakeStockYear = DateTime.Now.Year,
                        TakeStockMonth = DateTime.Now.Month,
                        Remark = ""
                    }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "BillCode",
                          postFields = new string[] { "ID","BillCode","InventoryCode","InventoryName","WarehouseCode","WarehouseName","RealNum","TakeStockNum","DValue","BatchCode","Remark"},
                          defaults = new {ID = "",BillCode =data.form.BillCode,InventoryCode = "",InventoryName = "",WarehouseCode = "",WarehouseName = "",RealNum = "",TakeStockNum = "",DValue = "",BatchCode = "",Remark = ""}
                        }
}
            };
            return View(model);
        }
        public string ConfirmLockMaterial(string id, JObject data)
        {
            var result = "";
            var masterService = new WMS_BN_LockMaterialService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var lockinfo = new WMS_BN_LockMaterialService().GetModel(pQuery);
            if (lockinfo != null)
            {
                lockinfo.LockState = Convert.ToInt32(data["type"]);
            }
            string msg = "";
            result = masterService.ConfirmLockState(lockinfo,out msg) > 0 ? "yes" : "no";
            if (result=="no")
            {
                return msg;
            }
            return result;
        }

    }

    public class WMS_BN_TakeStockDetailApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>WMS_BN_TakeStockDetail</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='like'></field>   
    </where>
</settings>");
            var service = new WMS_BN_TakeStockDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
    public class WMS_BN_TakeStockMainApiController : ApiController
    {
        public dynamic GetData(string tockType)
        {

            List<dynamic> data = new List<dynamic>();

            data = new WMS_BN_TakeStockMainService().GetData(tockType);

            var result = new
            {
                rows = data,
                total = data.Count
            };
            return result;
        }
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>WMS_BN_TakeStockMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ID'		cp='equal'></field>   
        <field name='BillCode'		cp='like'></field>   
        <field name='TakeStockYear'		cp='equal'>1</field>   
        <field name='TakeStockMonth'		cp='equal'></field>   
        <field name='TakeStockType'		cp='equal'></field>   
        <field name='BillState'		cp='equal'></field>    
    </where>
</settings>");
            var service = new WMS_BN_TakeStockMainService();
            var pQuery = query.ToParamQuery();//.AndWhere("BillType", WMS_In_InMaster_ComController.uc.Value);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetPageData(string id)
        {
            var masterService = new WMS_BN_TakeStockMainService();
            var masterServiceDetial = new WMS_BN_TakeStockDetailService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var data = masterService.GetModel(pQuery);
            var pQuery2 = ParamQuery.Instance().AndWhere("BillCode", data.BillCode);
            //var pQuery2 = ParamQuery.Instance().AndWhere("BillCode", id);
            string orderby = data.TakeStockType == 1 ? "InventoryCode" : "WarehouseCode";
            var result = new
            {
                //主表数据
                form = data,
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = masterServiceDetial.GetDynamicList(pQuery2.OrderBy(orderby)),
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("WMS_BN_TakeStockDetail")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new WMS_BN_TakeStockDetailService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
        public string GetNewBillNo()
        {
            return new WMS_BN_TakeStockMainService().GetNewKey("ID", "maxplus");
        }
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new WMS_BN_TakeStockDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var serviceMain = new WMS_BN_TakeStockMainService();
            var formWrapperMain = RequestWrapper.Instance().LoadSettingXmlString(@"
            <settings>
                <table>
                    WMS_BN_TakeStockMain
                </table>
                <where>
                    <field name='ID' cp='equal'></field>
                </where>
            </settings>
            ");

            var formWrapperDetail = RequestWrapper.Instance().LoadSettingXmlString(@"
                <settings>
                    <table>
                        WMS_BN_TakeStockDetail
                    </table>
                    <where>
                        <field name='ID' cp='equal'></field>
                    </where>
                </settings>
                ");

            //.GetNewKey("ID", "maxplus");
            //判断当前是否首次保存首次保存产生单号
            //var currentID = data["form"]["ID"];
            //var pQuery = ParamQuery.Instance().AndWhere("ID", currentID);
            //var currentInfo = serviceMain.GetModel(pQuery);

            //dynamic qm = new
            //{
            //    month = data["form"]["TakeStockYear"],
            //    year = data["form"]["TakeStockYear"]
            //};
            //if (currentInfo == null)
            //{

            //    var code = serviceMain.GetMaxBillCode(qm);
            //    data["form"]["BillCode"] = code;
            //}

            //判断是否是新增，是的话就判断一个月只能保存一条盘点数据
            dynamic qm = new
            {
                month = data["form"]["TakeStockMonth"],
                year = data["form"]["TakeStockYear"]
            };
            if (qm.month != null || qm.year != null)
            {
                if (serviceMain.GetTakeStock(Convert.ToInt32(qm.year), Convert.ToInt32(qm.month)) > 0)
                {
                    MmsHelper.ThrowHttpExceptionWhen(true, "一个月只能生成一次盘点！", "");
                }
            }
            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(formWrapperDetail);
            var service = new WMS_BN_TakeStockDetailService();
            var result = service.EditPage(data, formWrapperMain, tabsWrapper);

        }
        //审核，自动生成出入库单
        public string PostBuildBill(string billCode)
        {
            string msg = "";
            var result = new WMS_BN_TakeStockMainService().PostBuild(billCode, out msg);
            return msg;
        }

        public string GetCode()
        {
            string billcode = MmsHelper.GetOrderNumber("WMS_BN_TakeStockMain", "BillCode", "PDDH", "", "");
            return billcode;
        }
        public string PostBuildPD(dynamic data)
        {
            string msg = "";
            var result = new WMS_BN_TakeStockMainService().PostBuildPD(Convert.ToInt32(data["year"]), Convert.ToInt32(data["month"]), (int)data["tackStockType"],  out msg);
            return msg;
        }
    }
}
