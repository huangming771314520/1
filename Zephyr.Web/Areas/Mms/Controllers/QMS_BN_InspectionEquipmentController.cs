
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
    public class QMS_BN_InspectionEquipmentController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new {
                    query = "/api/Mms/QMS_BN_InspectionEquipment",
                    remove = "/api/Mms/QMS_BN_InspectionEquipment/",
                    billno = "/api/Mms/QMS_BN_InspectionEquipment/getnewbillno",
                    audit = "/api/Mms/QMS_BN_InspectionEquipment/audit/",
                    edit1 = "/Mms/QMS_BN_InspectionEquipment/edit/",
                    
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
                    InspectionEquipmentName = "" ,
                    InspectionEquipmenState = "" ,
                    IsEnable = "" 
                },
                idField="ID"
            };

            return View(model);
        }


        public ActionResult Edit(string id = "")
        {
            string inspectionEquipmentCode = new QMS_BN_InspectionEquipmentService().GetInspectionEquipmentCodeByID(id);
            string inspectionEquipmentName = new QMS_BN_InspectionEquipmentService().GetInspectionEquipmentNameByID(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/QMS_BN_InspectionEquipmentQualification/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/QMS_BN_InspectionEquipmentQualification/edit/",                      //数据保存api
                    audit = "/api/Mms/QMS_BN_InspectionEquipmentQualification/audit/",                    //审核api
                    newkey = "/api/Mms/QMS_BN_InspectionEquipmentQualification/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new QMS_BN_InspectionEquipmentQualificationApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new QMS_BN_InspectionEquipment().Extend(new { InspectionEquipmentCode = "", InspectionEquipmentName = "", InspectionEquipmenState = "", IsEnable = "", SpecModel = "", FactoryName = "", FactoryNumber = "", EquipmentCode = "", PrecisionLevel = "" , PurchaseDate =DateTime.Now.ToString("yyyy-MM-dd")}),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","InspectionEquipmentCode","InspectionEquipmentName","QualificationName","QualificationCode","CertificateCode","EffectiveBeginDate","EffectiveEndDate","IsEnable"},
                          defaults = new {ID = "",InspectionEquipmentCode = inspectionEquipmentCode,InspectionEquipmentName = inspectionEquipmentName,QualificationName="",QualificationCode = "",CertificateCode = "",EffectiveBeginDate = "",EffectiveEndDate = "",IsEnable = ""}
                        }
}
            };
            return View(model);
        }


    }

    public class QMS_BN_InspectionEquipmentApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new QMS_BN_InspectionEquipmentService().GetNewKey("ID", "Maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>QMS_BN_InspectionEquipment</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='InspectionEquipmentName'		cp='like'></field>   
        <field name='InspectionEquipmenState'		cp='equal'></field>   
        <field name='IsEnable'		cp='equal'></field>   
    </where>
</settings>");
            var service = new QMS_BN_InspectionEquipmentService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class QMS_BN_InspectionEquipmentQualificationApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new QMS_BN_InspectionEquipmentService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new QMS_BN_InspectionEquipmentQualificationService().GetDynamicList(pQuery)
            };
            return result;
        }

        

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("QMS_BN_InspectionEquipment")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new QMS_BN_InspectionEquipmentService();
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
                    var service0 = new QMS_BN_InspectionEquipmentQualificationService();
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
        QMS_BN_InspectionEquipment
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>QMS_BN_InspectionEquipmentQualification</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new QMS_BN_InspectionEquipmentService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
