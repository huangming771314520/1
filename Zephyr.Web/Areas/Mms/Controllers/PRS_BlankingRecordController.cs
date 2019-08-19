
using Antlr.Runtime;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils.NPOI.HSSF.Record;

namespace Zephyr.Areas.Mms.Controllers
{
    public class PRS_BlankingRecordController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/PRS_BlankingRecord",
                    newkey = "/api/Mms/PRS_BlankingRecord/getnewkey",
                    edit = "/api/Mms/PRS_BlankingRecord/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    FigureCode = "",
                    ContractCode = "",
                    IsEnable = 1,
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "PartCode", "FigureCode", "PartName", "SingleQuantity", "TotalQuantity", "MaterialCode", "InPlanceSize", "BlankingSize", "Process", "BatchNumber", "Remark", "IsEnable", "CreateTime", "CreatePerson", "ModifyTime", "ModifyPerson" }
                }
            };

            return View(model);
        }
        public ActionResult Edit(string id,string newPName)
        {
            ViewBag.NewPName = newPName;
            var model = new
            {
                urls = new
                {
                    change_BlankingType = "/api/Mms/PRS_BlankingResult/PostChangeBlankingType/",
                    getdata = "/api/Mms/PRS_BlankingResult/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/PRS_BlankingResult/edit/",                      //数据保存api
                    //edit_toplist = "/api/Mms/PRS_BlankingResult/Edit_TopList",
                    audit = "/api/Mms/PRS_BlankingResult/audit/",                    //审核api
                    newkey = "/api/Mms/PRS_BlankingResult/GetNewRowId/",           //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/PRS_BlankingResult/PostCreateBoard/"
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
                    pageData = new PRS_BlankingResultApiController().GetPageData(id),
                    SavantCode = id
                },
                form = new
                {
                    defaults = new MES_SavantManage().Extend(new { SavantCode = "", SpareMateSize = "", SpareMateNum = "", SplitNum = "", Remark = "" }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","ResultSize","Remark","IsEnable","CreatePerson","CreateTime","MdifyPerson","ModifyTime","PartBlankingQuntity","SavantCode"},
                          defaults = new {ID = "",ResultSize = "",Remark = "",IsEnable = 1,CreatePerson = "",CreateTime = "",MdifyPerson = "",ModifyTime = "",PartBlankingQuntity="",SavantCode=id}
                        },
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID", "BlankingResultID", "BiankingSize", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "BlankingType"},
                          defaults = new {IsEnable = 1}
                        }
                }
            };
            return View(model);
        }

        public ActionResult newIndex()
        {
            return View();
        }

        public ActionResult BlankingProcessCard()
        {
            return View();
        }

    }

    public class PRS_BlankingRecordApiController : ApiController
    {
        //    public dynamic Get(RequestWrapper query)
        //    {
        //        query.LoadSettingXmlString(@"
        //<settings defaultOrderBy='ID'>
        //    <select>*</select>
        //    <from>PRS_BlankingRecord</from>
        //    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
        //            <field name='FigureCode' 		 cp='like'></field>
        //            <field name='ContractCode' 		 cp='equal'></field>
        //    </where>
        //</settings>");
        //        var service = new PRS_BlankingRecordService();
        //        var pQuery = query.ToParamQuery();
        //        var result = service.GetDynamicListWithPaging(pQuery);
        //        return result;
        //    }

        public string GetNewKey()
        {
            return new PRS_BlankingRecordService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        /// <summary>
        /// 获取拼板方案明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //public int GetDelete(string id)
        //{
        //    return new PRS_BlankingRecordService().GetDelete(id);
        //}

        //    [System.Web.Http.HttpPost]
        //    public void Edit(dynamic data)
        //    {
        //        var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
        //<settings>
        //    <table>
        //        PRS_BlankingRecord
        //    </table>
        //    <where>
        //        <field name='ID' cp='equal'></field>
        //    </where>
        //</settings>");
        //        var service = new PRS_BlankingRecordService();
        //        var result = service.Edit(null, listWrapper, data);
        //    }


        public dynamic GetSavantManageData()
        {
            return new PRS_BlankingRecordService().GetSavantManageData().Data;
        }

        public dynamic GetSavantAndPBomData(string savantCode)
        {
            return new PRS_BlankingRecordService().GetSavantAndPBomData(savantCode).Data;
        }

        public dynamic GetBlankingProcessCardData(string cCode, string productName, string pFigure, string pName, string savantCode)
        {
            return new PRS_BlankingRecordService().GetBlankingProcessCardData(cCode, productName, pFigure, pName, savantCode).Data;
        }

        public dynamic GetDelSavantAndPBomData(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = @"请选择要删除的数据！"
                };
            }
            else
            {
                return new PRS_BlankingRecordService().GetDelSavantAndPBomData(ids);
            }
        }

        public dynamic GetAddSavantAndPBomData(string ids, string savantCode)
        {
            return new PRS_BlankingRecordService().AddSavantAndPBomData(ids, savantCode);
        }

        public dynamic GetUpdateSavantAndPBomNum(long id, string num)
        {
            return new PRS_BlankingRecordService().UpdateSavantAndPBomNum(id, Convert.ToInt32(num));
        }

        public dynamic PostUpdateSavantManageData(dynamic data)
        {
            return new PRS_BlankingRecordService().UpdateSavantManageData(data);
        }

        public dynamic PostSaveBlankingPlanData(dynamic data)
        {
            return new PRS_BlankingRecordService().SaveBlankingPlanData(data);
        }
    }

    public class PRS_BlankingResultApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_SavantManageService();
            var pQuery = ParamQuery.Instance().AndWhere("SavantCode", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                //scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new PRS_BlankingResultService().GetDynamicList(ParamQuery.Instance().AndWhere("SavantCode", id).AndWhere("IsEnable", 1))
            };
            return result;
        }

        public dynamic GetPlateData(string id)
        {
            var pQuery = ParamQuery.Instance().AndWhere("BlankingResultID", id).AndWhere("IsEnable", 1);
            return new Mes_BlankingResultService().GetDynamicList(pQuery);
        }


        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("PRS_BlankingRecord")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new PRS_BlankingRecordService();
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
                    var service0 = new PRS_BlankingResultService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        public dynamic PostCreateBoard(dynamic data)
        {
            bool result = true;
            new Mes_BlankingResultService().PostCreateBoard(out result, data);
            return result;
        }

        public void PostChangeBlankingType(dynamic data)
        {
            string BlankingType = data.BlankingType;
            int ID = data.ID;
            new Mes_BlankingResultService().Update(
                ParamUpdate.Instance().Column("BlankingType", BlankingType)
                .AndWhere("ID", ID)
                );
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public int Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_SavantManage
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>PRS_BlankingResult</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>Mes_BlankingResult</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new PRS_BlankingRecordService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
            return 1;
        }
    }
}
