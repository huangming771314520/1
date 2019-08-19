
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
    public class PRS_ProcessRouteModelMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/PRS_ProcessRouteModelMain",
                    remove = "/api/Mms/PRS_ProcessRouteModelMain/",
                    billno = "/api/Mms/PRS_ProcessRouteModelMain/getnewbillno",
                    audit = "/api/Mms/PRS_ProcessRouteModelMain/audit/",
                    edit1 = "/Mms/PRS_ProcessRouteModelMain/edit/"
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
                },
                form = new
                {
                    ProcessRouteCode = ""
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            //string processRouteCode = new PRS_ProcessRouteModelMainService().GetMaxCode();
            //processRouteCode = "GYMX" + processRouteCode;
            var data = new PRS_ProcessRouteModelDetailApiController().GetPageData(id);
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/PRS_ProcessRouteModelDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/PRS_ProcessRouteModelDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/PRS_ProcessRouteModelDetail/audit/",                    //审核api
                    newkey = "/api/Mms/PRS_ProcessRouteModelDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = data
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new PRS_ProcessRouteModelMain().Extend(new { ID = id, ProcessRouteCode = data.form == null ? "系统生成" : data.form.ProcessRouteCode, ProcessRouteName = "", IsEnable = 1, BillState = 0, }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","ProcessCode","ProcessName","ProcessLineCode","Remark","IsEnable","ContractCode","PartCode","PartFigure"},
                          defaults = new {ID = "",MainID=id,ProcessCode = "",ProcessName="",ProcessLineCode = "",Remark = "",IsEnable = "",ContractCode="",PartCode="",PartFigure=""}
                        },
                         new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","WorkStepsLineCode","WorkStepsName","WorkStepsContent","ManHours","IsEnable","Unit"},
                          defaults = new {}
                        }
}
            };
            return View(model);
        }
    }

    public class PRS_ProcessRouteModelMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new PRS_ProcessRouteModelMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ID'>
    <select>*</select>
    <from>(select t1.*,t2.ProjectName from PRS_ProcessRouteModelMain t1
left join PMS_BN_Project t2 on t1.ContractCode=t2.ContractCode) as temp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='temp.ProcessRouteCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new PRS_ProcessRouteModelMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable",1));
            return result;
        }
    }

    public class PRS_ProcessRouteModelDetailApiController : ApiController
    {
        public dynamic GetDetail(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>PRS_ProcessRouteModelDetail</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='MainID'		cp='equal'></field>   
    </where>
</settings>");
            var service = new PRS_ProcessRouteModelDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }
        public dynamic GetProjectDetail(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.ProjectID'>
    <select>*</select>
    <from>(
select t1.ProjectID,t1.ContractCode,t1.ProjectName,t2.ProductName,t2.FigureNumber,t2.Model,t2.Specifications,t2.ProductType from PMS_BN_Project t1 
left join PMS_BN_ProjectDetail t2 on t1.ProjectID=t2.MainID) as temp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='temp.ContractCode'		cp='equal'></field> 
        <field name='temp,FigureNumber'		cp='equal'></field> 
    </where>
</settings>");
            var service = new PMS_BN_ProjectPartService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetPageData(string id)
        {
            var masterService = new PRS_ProcessRouteModelMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
             //var pQuery = ParamQuery.Instance();
             if (id == "")
             {
                 var result = new
                 {
                     tab0 = "",
                     tab1=""
                 };
                 return result;
             }
             else
             {



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
                         tab0 = new PRS_ProcessRouteModelDetailService().GetDynamicList(pQuery2),
                        
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
                         tab0 = "",tab1=""
                     };
                     return result;
                 }
             }



        }

        public dynamic GetStepsData(string id)
        {
            var pQuery = ParamQuery.Instance().AndWhere("MainID", id);
            return new PRS_ProcessModelWorkStepsService().GetDynamicList(pQuery);
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("PRS_ProcessRouteModelMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new PRS_ProcessRouteModelMainService();
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
                    var service0 = new PRS_ProcessRouteModelDetailService();
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
            var re = new PRS_ProcessRouteModelMainService().GetModelList(pQuery);

            if (re.Count > 0 && re[0].BillState == 1)
            {
                MmsHelper.ThrowHttpExceptionWhen(true, "已审核数据不能修改！");
                return;
            }
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        PRS_ProcessRouteModelMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>PRS_ProcessRouteModelDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new PRS_ProcessRouteModelMainService();

            if (data.form["ProcessRouteCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetLSNumber("PRS_ProcessRouteModelMain", "ProcessRouteCode", "GYMX", "", "");
                data.form["ProcessRouteCode"] = documentNo;
              
            }


            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new PRS_ProcessRouteModelDetailService().AuditBillCode(data["ID"].ToString(), out msg);

            return msg;
        }
        public int GetDelete(string id)
        {
            return new PRS_ProcessRouteModelMainService().GetDelete(id);
        }
        public int GetAud(string id)
        {
            return new PRS_ProcessRouteModelMainService().GetAud(id);
        }
    }
}
