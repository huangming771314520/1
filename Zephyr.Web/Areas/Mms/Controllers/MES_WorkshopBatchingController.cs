
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.CommonWrap;
using Zephyr.Areas.Psi.Controllers;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_WorkshopBatchingController : Controller
    {
        public ActionResult Index()
        {
            var user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
            string workshopName = new SYS_BN_DepartmentService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("DepartmentCode", user.DepartmentCode ?? "")).DepartmentName ?? "";
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_WorkshopBatching",
                    remove = "/api/Mms/MES_WorkshopBatching/",
                    billno = "/api/Mms/MES_WorkshopBatching/getnewbillno",
                    audit = "/api/Mms/MES_WorkshopBatching/audit/",
                    edit1 = "/Mms/MES_WorkshopBatching/edit/"
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
                    BatchingCode = "",
                    ContractCode = "",
                    DepartmentName = workshopName,
                    //WorkshopCode = "",
                },
                idField = "BatchingCode"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_WorkshopBatchingDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_WorkshopBatchingDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_WorkshopBatchingDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_WorkshopBatchingDetail/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new MES_WorkshopBatchingDetailApiController().GetPageData(id),
                    ContractCode = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_Project", "ContractCode value,ProjectName text"),
                    ProductID = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_ProjectDetail", "ID value,ProductName text"),
                    WorkshopCode = new SYS_BN_DepartmentService().GetDynamicList(ParamQuery.Instance().Select("DepartmentCode value,DepartmentName text").From("SYS_BN_Department").AndWhere("IsWorkshop", 1)),
                    PartName = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PRS_Process_BOM", "PartCode value,PartName text"),
                    OutDeptCode = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.SYS_BN_Warehouse", "WarehouseCode value,WarehouseName text")
                },
                form = new
                {
                    defaults = new MES_WorkshopBatching().Extend(new { BatchingCode = "", RootPartCode = "", PartCode = "", ContractCode = "", ProductID = "", WorkshopCode = "", IsEnable = 1, PartName = "", RootName = "" }),
                    primaryKeys = new string[] { "BatchingCode" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","PartCode","ParentCode","PartFigureNumber","PartName","MaterialCode","PartQuantity","Quantity","RequirementNum","BatchingNum","OutDeptCode","BatchingCode","CreatePerson","CreateTime","IsCrux"},
                          defaults = new {ID = "",PartCode = "",PartFigureNumber = "",PartName = "",MaterialCode = "",PartQuantity = "",Quantity = "",RequirementNum = "",BatchingNum = "",OutDeptCode = "",BatchingCode = "",CreatePerson = "",CreateTime = ""}
                        }
}
            };
            return View(model);
        }

        [MvcMenuFilter(false)]
        public ActionResult ChooseProcessBom()
        {
            return View();
        }

    }

    public class MES_WorkshopBatchingApiController : ApiController
    {
        public string GetNewBillNo()
        {
            return new MES_WorkshopBatchingService().GetNewKey("BatchingCode", "dateplus");
        }

        public int GetDelete(string id)
        {
            return new MES_WorkshopBatchingService().Update(ParamUpdate.Instance().Column("IsEnable", 0).AndWhere("BatchingCode", id));
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='BatchingCode'>
    <select>*</select>
    <from>(SELECT t1.* ,t2.PartName RootName FROM V_MesWorkshopBatching t1 INNER JOIN dbo.PRS_Process_BOM t2 ON t1.RootPartCode=t2.PartCode) as tmp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BatchingCode'		cp='like'></field>   
        <field name='ContractCode'		cp='like'></field>   
        <field name='DepartmentName'		cp='like'></field>
    </where>
</settings>");
            var service = new MES_WorkshopBatchingService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

    }

    public class MES_WorkshopBatchingDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new MES_WorkshopBatchingService();
            //var pQuery = ParamQuery.Instance().AndWhere("BatchingCode", id);
            RequestWrapper query = new RequestWrapper().LoadSettingXmlString(@"
<settings defaultOrderBy='BatchingCode'>
    <select>*</select>
    <from>(SELECT t1.* ,t2.PartName RootName FROM V_MesWorkshopBatching t1 INNER JOIN dbo.PRS_Process_BOM t2 ON t1.RootPartCode=t2.PartCode) as tmp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BatchingCode'		cp='like'></field>   
        <field name='ContractCode'		cp='like'></field>   
        <field name='DepartmentName'		cp='like'></field>
    </where>
</settings>");
            ParamQuery pQuery = query.ToParamQuery();
            pQuery.AndWhere("BatchingCode", id);
            var data = masterService.GetDynamic(pQuery);
            var result = new
            {
                //主表数据
                form = data,
                scrollKeys = masterService.ScrollKeys("BatchingCode", id),

                //明细数据
                tab0 = new MES_WorkshopBatchingDetailService().GetDynamicList(ParamQuery.Instance().AndWhere("BatchingCode", id))
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_WorkshopBatching")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("BatchingCode", id);

            var service = new MES_WorkshopBatchingService();
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
                    var service0 = new MES_WorkshopBatchingDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var BatchingCode = MmsHelper.GetOrderNumber("MES_WorkshopBatching", "BatchingCode", "PLD", "", "");
            data.form.BatchingCode = BatchingCode;
            dynamic InsertList = data.tabs[0].inserted;
            foreach (dynamic item in InsertList)
            {
                item.BatchingCode = BatchingCode;
            }

            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_WorkshopBatching
    </table>
    <where>
        <field name='BatchingCode' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_WorkshopBatchingDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new MES_WorkshopBatchingService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public dynamic GetProductTreeList([FromUri]TreeNodeModel model)
        {
            //var PMS_BN_ProjectDetailList = new PMS_BN_ProjectDetailService().GetModelList();
            //var PRS_Process_BOMList = new PRS_Process_BOMService().GetModelList();
            //var APS_ProjectProduceDetialList = new APS_ProjectProduceDetialService().GetModelList();

            var list = TreeNodeManage.GetTreeNodeList<dynamic>(
                  TreeNodeManage.Instance()
                  .SetNode(model.NodeField)
                  .SetParentNode(model.ParentNodeField, model.ParentNodeValue)
                  .SetTableName(model.TableName)
                  .SetNodeLevel(model.IsLevel)
                  .SetTreeSetting(model.TreeSetting)
                  .SetWhereSql(model.WhereSql));

            return list;
        }


        public dynamic GetData(string figureNo, string parentPartCode)
        {
            return new MES_WorkshopBatchingDetailService().GetData(figureNo, parentPartCode);
        }

    }
}
