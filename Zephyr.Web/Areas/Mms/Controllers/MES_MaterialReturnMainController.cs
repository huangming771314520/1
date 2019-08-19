
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
    public class MES_MaterialReturnMainController : Controller
    {
        public ActionResult Index()
        {

            var loginer = FormsAuth.GetUserData<LoginerBase>();
            SYS_BN_Department department = new SYS_BN_Department();

            var user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
            department = new SYS_BN_DepartmentService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("DepartmentCode", user.DepartmentCode));

            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_MaterialReturnMain",
                    remove = "/api/Mms/MES_MaterialReturnMain/",
                    billno = "/api/Mms/MES_MaterialReturnMain/getnewbillno",
                    audit = "/api/Mms/MES_MaterialReturnMain/audit/",
                    edit1 = "/Mms/MES_MaterialReturnMain/edit/"
                },
                resx = new
                {
                    detailTitle = "车间退料明细",
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
                    DepartmentID = department.DepartmentCode ?? "",
                    DepartmentName = department.DepartmentName ?? ""
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            var data = new MES_MaterialReturnDetailApiController().GetPageData(id);

            var dpartment = new SYS_BN_UserService().GetDepartmentInfo(MmsHelper.GetUserCode());
            var departid = "";
            var departname = "";
            if (dpartment.DepartmentCode != null)
            {
                departid = dpartment.DepartmentCode.ToString();
                departname = dpartment.DepartmentName;

            }
            string dtNow = DateTime.Now.ToString("yyyy-MM-dd");

            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_MaterialReturnDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_MaterialReturnDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_MaterialReturnDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_MaterialReturnDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/MES_MaterialReturnDetail/PostStorage/"
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

                    defaults = new MES_MaterialReturnMain().Extend(new
                    {
                        ID = id,
                        BillCode = data.form == null ? "系统生成" : data.form.BillCode,
                        PickBillCode = "",
                        DepartmentID = dpartment.DepartmentCode ?? "",
                        DepartmentName = dpartment.DepartmentName ?? "",
                        ReturnDate = dtNow,
                        ContractCode = "",
                        ProjectName = "",
                        WarehouseCode = "",
                        WarehouseName = "",
                        Billstate = 1,
                        IsEnable = 1
                    }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                           /*
          Unit VARCHAR(20) NULL
          Model VARCHAR(20) NULL
          Material VARCHAR(20) NUL
         */
                          postFields = new string[] { "ID","MainID","ContractCode","ProjectName","InventoryCode","InventoryName","ReturnQuantity","IsEnable","Unit","Model","Material"},
                          defaults = new {ID = "",MainID = id,ContractCode = "",ProjectName = "",InventoryCode = "",InventoryName = "",ReturnQuantity = "",IsEnable = 1,Unit="",Model="",Material=""}
                        }
}
            };
            return View(model);
        }
    }

    public class MES_MaterialReturnMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new MES_MaterialReturnMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>MES_MaterialReturnMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='equal'></field>   
        <field name='DepartmentName'		cp='like'></field>   
    </where>
</settings>");
            var service = new MES_MaterialReturnMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }



    public class MES_MaterialReturnDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_MaterialReturnMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new MES_MaterialReturnDetailService().GetDynamicList(pQuery2)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_MaterialReturnMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new MES_MaterialReturnMainService();
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
                    var service0 = new MES_MaterialReturnDetailService();
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
        MES_MaterialReturnMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_MaterialReturnDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("MES_MaterialReturnMain", "BillCode", "CJTL", "", "");
                data.form["BillCode"] = documentNo;

            }
            var service = new MES_MaterialReturnMainService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);

        }

        public dynamic PostStorage(dynamic data)
        {
            //string msg = "";

            //var result = new MES_MaterialReturnDetailService().AuditBillCode(data["BillCode"].ToString(), out msg);
            //var result = new MES_MaterialReturnDetailService().NewAuditBillCode(data["BillCode"].ToString());

            return new MES_MaterialReturnDetailService().NewAuditBillCode(data["BillCode"].ToString());
        }

        public dynamic GetMaterialPick(string id)
        {
            return new MES_MaterialReturnDetailService().GetMaterialReturnDetail(id);

        }

    }
}
