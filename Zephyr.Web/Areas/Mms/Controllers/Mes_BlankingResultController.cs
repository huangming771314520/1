
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class Mes_BlankingResultController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new {
                    getdata = "/api/Mms/Mes_BlankingResult/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Mes_BlankingResult/edit/",                      //数据保存api
                    audit = "/api/Mms/Mes_BlankingResult/audit/",                    //审核api
                    newkey = "/api/Mms/Mes_BlankingResult/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
                },
                resx = new {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed ="单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new{
                    pageData=new Mes_BlankingResultApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new{
                    defaults = new Mes_BlankingBoard().Extend(new {  ContractCode = "",PartCode = "",BoardInventoryCode = "",BoardInventoryName = ""}),
                    primaryKeys = new string[]{"ID"}
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","BlankingResultID","InventoryCode","InventoryName","BlankingQuantity","BatchNumber","IsPicking","FileAddress","FileName","DocName","IsEnable","CreateTime","CreatePerson","ModifyTime","ModifyPerson","BiankingSize"},
                          defaults = new {ID = "",BlankingResultID = "",InventoryCode = "",InventoryName = "",BlankingQuantity = "",BatchNumber = "",IsPicking = "",FileAddress = "",FileName = "",DocName = "",IsEnable = "",CreateTime = "",CreatePerson = "",ModifyTime = "",ModifyPerson = "",BiankingSize = ""}
                        }
}
            };
            return View(model);
        }
    }

    public class Mes_BlankingResultApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new Mes_BlankingBoardService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);

             var result = new {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                    tab0 = new Mes_BlankingResultService().GetDynamicList(pQuery)
};
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("Mes_BlankingBoard")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new Mes_BlankingBoardService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
  
        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type,string key,int qty=1)
        {
            switch (type)
            {
    case "grid0":
        var service0 = new Mes_BlankingResultService();
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
        Mes_BlankingBoard
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>Mes_BlankingResult</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

var service = new Mes_BlankingBoardService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
