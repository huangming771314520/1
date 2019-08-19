
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
    public class MES_MaterialPickMainController : Controller
    {
        public static sys_code uc = new sys_code();
        public ActionResult Index()
        {

            var loginer = FormsAuth.GetUserData<LoginerBase>();
            var user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
            var Department = new SYS_BN_DepartmentService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("DepartmentCode", user.DepartmentCode));
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_MaterialPickMain",
                    remove = "/api/Mms/MES_MaterialPickMain/",
                    billno = "/api/Mms/MES_MaterialPickMain/getnewbillno",
                    audit = "/api/Mms/MES_MaterialPickMain/audit/",
                    edit1 = "/Mms/MES_MaterialPickMain/edit/"
                },
                resx = new
                {
                    detailTitle = "车间领料明细",
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
                    DepartmentID = Department.DepartmentCode ?? "",
                    DepartmentName = Department.DepartmentName ?? "",
                    PickingDate = "",
                    BillState = "1",
                },
                idField = "ID"
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            //string billCode = new MES_MaterialPickMainService().GetBillCodeByID(id);
            var data = new MES_MaterialPickDetailApiController().GetPageData(id);
            //string documentNo = MmsHelper.GetOrderNumber("MES_MaterialPickMain", "BillCode", "CJLL", "", "");
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            var dpartment = new SYS_BN_UserService().GetDepartmentInfo(loginer.UserCode);
            SYS_BN_Warehouse warehouse = new SYS_BN_WarehouseService().GetModel(ParamQuery.Instance().AndWhere("ISEnable", 1).AndWhere("WarehouseName", dpartment.DepartmentName));
            ViewBag.IsWarehouse = warehouse == null ? 0 : 1;
            warehouse = warehouse ?? new SYS_BN_Warehouse();

            string departid = "";
            var departname = "";
            if (dpartment != null)
            {
                departid = dpartment.DepartmentCode;
                departname = dpartment.DepartmentName;

            }
            string dtNow = DateTime.Now.ToString("yyyy-MM-dd");
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/MES_MaterialPickDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/MES_MaterialPickDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/MES_MaterialPickDetail/audit/",                    //审核api
                    newkey = "/api/Mms/MES_MaterialPickDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/MES_MaterialPickDetail/PostStorage/"
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
                    defaults = new MES_MaterialPickMain().Extend(new
                    {
                        ID = id,
                        BillCode = data.form == null ? "系统生成" : data.form.BillCode,
                        DepartmentID = departid,
                        DepartmentName = departname,
                        PickingDate = dtNow,
                        IsEnable = 1,
                        BillState = 1,
                        ContractCode = "",
                        ProjectName = "",
                        WarehouseCode = warehouse.WarehouseCode,
                        WarehouseName = warehouse.WarehouseName
                    }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","ContractCode","ProjectName","InventoryCode","InventoryName","RequiredQuantity","IsEnable","Model","Unit","Material"},
                          defaults = new {ID = "",MainID = id,ContractCode = "",ProjectName = "",InventoryCode = "",InventoryName = "",RequiredQuantity = "",IsEnable = 1,Model="",Unit="",Material=""}
                        }
}
            };
            return View(model);
        }
    }

    public class MES_MaterialPickMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new MES_MaterialPickMainService().GetNewKey("ID", "maxplus");
        }

        //public dynamic GetMaterialDetailByPCode(int ppdID, string partCode, int productID, string productName)
        //{
        //    var data = new MES_MaterialPickMainService().GetMaterialDetailByPCode(ppdID, partCode, productID, productName);
        //    return data;

        //}

        public dynamic POSTMaterialDetailByIID(dynamic data)
        {
            var list = data.list;
            string batchingCode = list[0].BatchingCode;
            var dataA = new MES_MaterialPickMainService().GetMaterialDetailByPCode(batchingCode);
            return dataA;
            //return data;

        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>MES_MaterialPickMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='equal'></field>   
        <field name='DepartmentName'		cp='equal'></field>   
        <field name='PickingDate'		cp='daterange'></field>   
        <field name='BillState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new MES_MaterialPickMainService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }
    }

    public class MES_MaterialPickDetailApiController : ApiController
    {

        public dynamic GetPageData(string id)
        {

            var masterService = new MES_MaterialPickMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var mainData = masterService.GetModel(pQuery);
            if (mainData != null)
            {
                var pQuery2 = ParamQuery.Instance().AndWhere("MainID", mainData.ID).AndWhere("IsEnable", 1);
                var result = new
                {
                    //主表数据
                    form = mainData,
                    scrollKeys = masterService.ScrollKeys("ID", id),

                    //明细数据
                    tab0 = new MES_MaterialPickDetailService().GetDynamicList(pQuery2)
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
                    tab0 = ""
                };
                return result;
            }

        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("MES_MaterialPickMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new MES_MaterialPickMainService();
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
                    var service0 = new MES_MaterialPickDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }




        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var service = new MES_MaterialPickMainService();

            if (data.form["BillCode"] != "系统生成")
            {
                var pQuery = ParamQuery.Instance().Select("*").AndWhere("ID", data.form["ID"]);
                MES_MaterialPickMain model = service.GetModel(pQuery);
                if (model.BillState != null && model.BillState == 2)
                {
                    MmsHelper.ThrowHttpExceptionWhen(true, "已发布的不能修改！");
                    return;
                }
            }
            var ids = "(";
            foreach (JToken tab in data["tabs"].Children())
            {
                foreach (JProperty item in tab.Children())
                {
                    if (item.Name == "deleted")
                    {
                        foreach (var row in item.Value.Children())
                        {
                            ids += "'" + row["ID"] + "',";
                        }
                        item.Value = "[]";
                    }
                }
            }
            if (ids != "(")
            {
                ids = ids.Remove(ids.Length - 1, 1);
                ids += ")";
                using (var db = Db.Context("Mms"))
                {
                    string sql1 = string.Format("update MES_MaterialPickDetail set IsEnable=0 where ID in {0} ", ids);
                    db.Sql(sql1).Execute();

                }
            }

            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_MaterialPickMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");



            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_MaterialPickDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            if (data.form["BillCode"] == "系统生成")
            {
                string documentNo = MmsHelper.GetOrderNumber("MES_MaterialPickMain", "BillCode", "CJLL", "", "");
                data.form["BillCode"] = documentNo;

            }
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }

        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new MES_MaterialPickDetailService().AuditBillCode(data["BillCode"].ToString(), out msg);

            return msg;
        }
        public int GetDelete(string id)
        {
            return new MES_MaterialPickMainService().GetDelete2(id);
        }
    }
}
