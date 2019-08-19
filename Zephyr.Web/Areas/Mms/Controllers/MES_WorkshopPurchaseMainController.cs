
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
    public class MES_WorkshopPurchaseMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_WorkshopPurchaseMain",
                    remove = "/api/Mms/MES_WorkshopPurchaseMain/",
                    billno = "/api/Mms/MES_WorkshopPurchaseMain/getnewbillno",
                    audit = "/api/Mms/MES_WorkshopPurchaseMain/audit/",
                    edit1 = "/Mms/MES_WorkshopPurchaseMain/edit/"
                },
                resx = new
                {
                    detailTitle = "车间请购明细",
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
                    WorkshopPurchaseCode = "",
                    WorkshopID = ""
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }
            if (string.IsNullOrEmpty(code))
            {
                return JavaScript("请先维护所属车间!");
            }
            var data = new MES_WorkshopPurchaseDetailApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_WorkshopPurchaseDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_WorkshopPurchaseDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_WorkshopPurchaseDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_WorkshopPurchaseDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/MES_WorkshopPurchaseDetail/PostStorage/"        //审核方法
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
                    defaults = new MES_WorkshopPurchaseMain().Extend(new { ID = id, WorkshopPurchaseCode = data.form == null ? "系统生成" : data.form.WorkshopPurchaseCode, WorkshopID = code, WorkshopName = name, PurchaseStatus = 1, ApproveState = 1, CreateTime = "", IsEnable = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","Model","Spec","Remark","InventoryCode","InventoryName","MeterialName","WriteModel","WriteSpec","PurchaseQuantity"},
                          defaults = new {ID = "",MainID = id,InventoryCode = "",InventoryName="",PurchaseQuantity = "",IsEnable=1}
                        }
}
            };
            return View(model);
        }
    }

    public class MES_WorkshopPurchaseMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new MES_WorkshopPurchaseMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>MES_WorkshopPurchaseMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='WorkshopPurchaseCode'		cp='equal'></field>   
        <field name='WorkshopID'		cp='equal'></field>   
    </where>
</settings>");
            var service = new MES_WorkshopPurchaseMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }

        public int GetDelete(string id)
        {
            return new MES_WorkshopPurchaseMainService().GetDelete2(id);
        }
    }

    public class MES_WorkshopPurchaseDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_WorkshopPurchaseMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id);
            //var form = masterService.GetModel(pQuery);
            //form.CreateTime=Convert.ToDateTime? form.CreateTime.ToString("yyyy-mm-dd");
            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new MES_WorkshopPurchaseDetailService().GetDynamicList(pQuery2)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(dynamic data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_WorkshopPurchaseMain")
                .Column("ApproveState", data.form.status.ToString())
                .Column("ApproveRemark", data.form.comment.ToString())
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", data.form.ID.ToString());

            var service = new MES_WorkshopPurchaseMainService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！");
        }

        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new MES_WorkshopPurchaseDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var service = new MES_WorkshopPurchaseMainService();
            if (data.form["WorkshopPurchaseCode"] != "系统生成")
            {
                var pQuery = ParamQuery.Instance().Select("*").AndWhere("ID", data.form["ID"]);
                MES_WorkshopPurchaseMain model = service.GetModel(pQuery);
                if (model.ApproveState != null && model.ApproveState == "2")
                {
                    MmsHelper.ThrowHttpExceptionWhen(true, "已发布的不能修改！");
                    return;
                }
            }


            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_WorkshopPurchaseMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_WorkshopPurchaseDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["WorkshopPurchaseCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("MES_WorkshopPurchaseMain", "WorkshopPurchaseCode", "CJQG", "", "");
                data.form["WorkshopPurchaseCode"] = documentNo;

            }
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new MES_WorkshopPurchaseDetailService().AuditBillCode(data["BillCode"].ToString(), out msg);

            return msg;
        }
    }
}
