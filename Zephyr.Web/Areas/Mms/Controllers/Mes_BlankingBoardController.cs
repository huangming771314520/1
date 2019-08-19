
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
    public class Mes_BlankingBoardController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/Mes_BlankingBoard",
                    remove = "/api/Mms/Mes_BlankingBoard/",
                    billno = "/api/Mms/Mes_BlankingBoard/getnewbillno",
                    audit = "/api/Mms/Mes_BlankingBoard/audit/",
                    edit1 = "/Mms/Mes_BlankingBoard/edit/"
                },
                resx = new
                {
                    detailTitle = "下料板材管理明细",
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
                    ContractCode = "",
                    BoardInventoryName = "",
                    IsEnable=1
                },
                idField = "ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {

            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/Mes_BlankingResult/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Mes_BlankingResult/edit/",                      //数据保存api
                    audit = "/api/Mms/Mes_BlankingResult/audit/",                    //审核api
                    newkey = "/api/Mms/Mes_BlankingResult/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new Mes_BlankingResultApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new Mes_BlankingBoard().Extend(new {ID=id, ContractCode = "", PartCode = "", BoardInventoryCode = "", BoardInventoryName = "" , ProgramCode = "" , IsEnable=1}),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","BlankingResultID","IsEnable","CreateTime","CreatePerson","ModifyTime","ModifyPerson","BiankingSize","BlankingCode","IsBlanking","BoardCode","BlankingDetailID"},
                          defaults = new {ID = "",MainID=id,BlankingResultID = "",IsEnable = "",CreateTime = "",CreatePerson = "",ModifyTime = "",ModifyPerson = "",BiankingSize = "",BlankingCode="",IsBlanking=0,BoardCode="",BlankingDetailID=""}
                        }
}
            };
            return View(model);
        }
    }

    public class Mes_BlankingBoardApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new Mes_BlankingBoardService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>(SELECT t1.*,
       t2.Model,
       t2.BatchCode
FROM dbo.Mes_BlankingBoard t1
    INNER JOIN dbo.WMS_BN_RealStock t2
        ON t1.BoardInventoryCode = t2.InventoryCode
           AND t2.IsEnable = 1) as tmp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ContractCode'		cp='equal'></field>   
        <field name='BoardInventoryName'		cp='equal'></field>   
        <field name='IsEnable'		cp='equal'></field>  
    </where>
</settings>");
            var service = new Mes_BlankingBoardService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class Mes_BlankingResultApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new Mes_BlankingBoardService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var detail = new Mes_BlankingResultService().GetDynamicList(ParamQuery.Instance().AndWhere("MainID", id).AndWhere("IsEnable", 1));
            var query = new RequestWrapper();
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>V__Mes_BlankingResult</from>
</settings>");
            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new Mes_BlankingResultService().GetDynamicListWithPaging(query.ToParamQuery().AndWhere("MainID", id).AndWhere("IsEnable", 1))
                //new Mes_BlankingResultService().GetDynamicListWithPaging(query.ToParamQuery().AndWhere("MainID", id).AndWhere("IsEnable",1))
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
        public string GetNewRowId(string type, string key, int qty = 1)
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
            foreach (JToken tab in data["tabs"].Children())
            {

                foreach (JProperty item in tab.Children())
                {
                    int res = 0;
                    if (item.Name == "inserted")
                    {
                        foreach (var row in item.Value.Children())
                        {
                            using (var db = Db.Context("Mms"))
                            {
                                string mID = row["MainID"].ToString();
                                string nId = row["ID"].ToString();
                                res = db.Update("Mes_BlankingResult")
                                .Column("MainID", mID)
                                .Column("IsBlanking",1)
                                .Where("ID", nId)
                               .Execute();
                            }
                        }
                        item.Value = "[]";
                    }
                }
            }
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }
}
