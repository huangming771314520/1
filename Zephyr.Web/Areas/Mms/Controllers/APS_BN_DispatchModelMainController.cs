
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
    public class APS_BN_DispatchModelMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/APS_BN_DispatchModelMain",
                    remove = "/api/Mms/APS_BN_DispatchModelMain/",
                    billno = "/api/Mms/APS_BN_DispatchModelMain/getnewbillno",
                    audit = "/api/Mms/APS_BN_DispatchModelMain/audit/",
                    edit1 = "/Mms/APS_BN_DispatchModelMain/edit/"
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
                    Name = "" ,
                    IsEnable=1
                },
                idField="ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new APS_BN_DispatchModelDetailApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/APS_BN_DispatchModelDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/APS_BN_DispatchModelDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/APS_BN_DispatchModelDetail/audit/",                    //审核api
                    newkey = "/api/Mms/APS_BN_DispatchModelDetail/GetNewRowId/" ,           //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/PRS_ProcessRouteModelDetail/PostStorage/"
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
                    pageData = new APS_BN_DispatchModelDetailApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new APS_BN_DispatchModelMain().Extend(new { Name = "", Code = data.form == null ? "系统生成" : data.form.Code ,IsEnable=1}),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","Name","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime","Remark","MainID","TaskCycle","Seq"},
                          defaults = new {ID = "",Name = "",Remark = "",CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = "",IsEnable = 1,MainID=id,TaskCycle="",Seq=""}
                        }
}
            };
            return View(model);
        }
    }

    public class APS_BN_DispatchModelMainApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new APS_BN_DispatchModelMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>APS_BN_DispatchModelMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='Name'		cp='like'></field>   
    </where>
</settings>");
            var service = new APS_BN_DispatchModelMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class APS_BN_DispatchModelDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new APS_BN_DispatchModelMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);

            //var result = new
            //{
            //    //主表数据
            //    form = masterService.GetModel(pQuery),
            //    scrollKeys = masterService.ScrollKeys("ID", id),

            //    //明细数据
            //    tab0 = new APS_BN_DispatchModelDetailService().GetDynamicList(ParamQuery.Instance().AndWhere("MainID", id))
            //};
            //return result;
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
                    tab0 = new APS_BN_DispatchModelDetailService().GetDynamicList(pQuery2),

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
                    tab0 = "",
                    tab1 = ""
                };
                return result;
            }
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("APS_BN_DispatchModelMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new APS_BN_DispatchModelMainService();
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
                    var service0 = new APS_BN_DispatchModelMainService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        public int GetDelete(string id)
        {
            return new APS_BN_DispatchModelMainService().GetDelete(id);
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        APS_BN_DispatchModelMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>APS_BN_DispatchModelDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new APS_BN_DispatchModelDetailService();

            if (data.form["Code"] == "系统生成")
            {
                string documentNo = MmsHelper.GetLSNumber("APS_BN_DispatchModelMain", "Code", "DDMX", "", "");
                data.form["Code"] = documentNo;

            }
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new APS_BN_DispatchModelDetailService().AuditBillCode(data["ID"].ToString(), out msg);

            return msg;
        }
    }
}
