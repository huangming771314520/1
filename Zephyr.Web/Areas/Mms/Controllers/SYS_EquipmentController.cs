
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
    public class SYS_EquipmentController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/SYS_Equipment",
                    remove = "/api/Mms/SYS_Equipment/",
                    billno = "/api/Mms/SYS_Equipment/getnewbillno",
                    audit = "/api/Mms/SYS_Equipment/audit/",
                    edit1 = "/Mms/SYS_Equipment/edit/"
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
                    EquipmentName = "" ,
                    AffiliatedWorkshopID = "" ,
                    EquipmentState = "" 
                },
                idField="ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/SYS_EquipmentDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/SYS_EquipmentDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/SYS_EquipmentDetail/audit/",                    //审核api
                    newkey = "/api/Mms/SYS_EquipmentDetail/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new SYS_EquipmentDetailApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new SYS_Equipment().Extend(new { ID=id, EquipmentCode = "", EquipmentName = "", EquipmentType = "", AffiliatedWorkshopID = "", AffiliatedWorkshopName = "", EquipmentParms = "", EquipmentState = "",IsEnable=1,TeamCode="" }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","ProcessType","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {ID = "",MainID = id,ProcessType = "",IsEnable = 1,CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = ""}
                        }
}
            };
            return View(model);
        }
    }

    public class SYS_EquipmentApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new SYS_EquipmentService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>SYS_Equipment</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EquipmentName'		cp='equal'></field>   
        <field name='AffiliatedWorkshopID'		cp='equal'></field>   
        <field name='EquipmentState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new SYS_EquipmentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class SYS_EquipmentDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new SYS_EquipmentService();
            var pQuery = ParamQuery.Instance().AndWhere("id", id);
            var pQuery1 = ParamQuery.Instance().AndWhere("MainID", id);
            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new SYS_EquipmentDetailService().GetDynamicList(pQuery1)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("SYS_Equipment")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new SYS_EquipmentService();
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
                    var service0 = new SYS_EquipmentDetailService();
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
        SYS_Equipment
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>SYS_EquipmentDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new SYS_EquipmentService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
