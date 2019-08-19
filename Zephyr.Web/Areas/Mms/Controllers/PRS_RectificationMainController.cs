
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
    public class PRS_RectificationMainController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/PRS_RectificationMain",
                    remove = "/api/Mms/PRS_RectificationMain/",
                    billno = "/api/Mms/PRS_RectificationMain/getnewbillno",
                    audit = "/api/Mms/PRS_RectificationMain/audit/",
                    edit1 = "/Mms/PRS_RectificationMain/edit/"
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
                    ContractCode = "",
                    ProductName = "",
                    IsEnable = "",
                    BillState = "0"
                },
                idField = "ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new PRS_RectificationDetailApiController().GetPageData(id);
            var model = new
            {

                urls = new
                {
                    getdata = "/api/Mms/PRS_RectificationDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/PRS_RectificationDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/PRS_RectificationDetail/audit/",                    //审核api
                    newkey = "/api/Mms/PRS_RectificationDetail/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = data,
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new PRS_RectificationMain().Extend(new { ID = id, BillCode = data.form == null ? "系统生成" : data.form.BillCode, ContractCode = "", ProjectName = "", ProductName = "", IsEnable = "1", ProjectDetailID = "", BillState = 0, }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","PartCode","ChargePerson","RectificationContent","ProcessCode","ProcessName","ProcessDesc","ProcessLineCode","WorkshopCode","EquipmentCode","WorkGroupCode","WarehouseCode","WorkshopName","EquipmentName","WorkGroupName","WarehouseName","Quantity","ManHour","Unit","FigureCode","ToolCode","IsEnable","IsCheck","CreatePerson","CreateTime","ModifyPerson","ModifyTime","ApproveState","ApprovePerson","ApproveDate","ApproveRemark"},
                          defaults = new {ID = "",MainID = id,PartCode = "",ChargePerson = "",RectificationContent = "",ProcessCode = "",ProcessName = "",ProcessDesc = "",ProcessLineCode = "",WorkshopCode = "",EquipmentCode = "",WorkGroupCode = "",WarehouseCode = "",WorkshopName = "",EquipmentName = "",WorkGroupName = "",WarehouseName = "",Quantity="",ManHour = "",Unit = "1",FigureCode = "",ToolCode = "",IsEnable = 1,IsCheck=0,CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = "",ApproveState = "",ApprovePerson = "",ApproveDate = "",ApproveRemark = ""}
                         }
}
            };
            return View(model);
        }

    }

    public class PRS_RectificationMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new PRS_RectificationMainService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>PRS_RectificationMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ContractCode'		cp='like'></field>   
        <field name='ProductName'		cp='like'></field>   
        <field name='IsEnable'		cp='equal'></field>   
    </where>
</settings>");
            var service = new PRS_RectificationMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }
    }

    public class PRS_RectificationDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new PRS_RectificationMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id).AndWhere("IsEnable", 1);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new PRS_RectificationDetailService().GetDynamicList(pQuery2)
            };
            return result;
        }

        //GetDeleteData
        public string GetDeleteData(string id)
        {
            string msg = string.Empty;
            int res = new PRS_RectificationDetailService().GetDeleteData(id, out msg);
            return msg;

        }

        public string PostBuildPlan(string id)
        {
            string msg = "";
            new APS_ProjectProduceDetialService().BuildPlan(id, out msg);
            return msg;
        }
        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new PRS_RectificationDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }
        public int GetDelete(string id)
        {
            return new PRS_RectificationMainService().GetDelete(id);
        }
        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var id = data.form["ID"].ToString();
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("ID", id);
            var re = new PRS_RectificationMainService().GetModelList(pQuery);

            if (re.Count > 0 && re[0].BillState == 1)
            {
                MmsHelper.ThrowHttpExceptionWhen(true, "已审核数据不能修改！");
                return;
            }
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        PRS_RectificationMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>PRS_RectificationDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("PRS_RectificationMain", "BillCode", "SCZG", "", "");
                data.form["BillCode"] = documentNo;

            }
            var service = new PRS_RectificationMainService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
    }


}
