
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
    public class WMS_BN_BillMainController : Controller
    {
        public static sys_code uc = new sys_code();
        public ActionResult Index(int id = 0)
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            SYS_BN_Department department = new SYS_BN_Department();
            if (id==10)
            {
                var user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
                department = new SYS_BN_DepartmentService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("DepartmentCode", user.DepartmentCode));
            }
            //var dapart=new SYS_BN_DepartmentService().get
            var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(loginer.UserCode);
            uc = new sys_codeService().Getsys_codeByTypeAndID("BillType", id);
            ViewBag.uc = uc;
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/WMS_BN_BillMain",
                    remove = "/api/Mms/WMS_BN_BillMain/",
                    billno = "/api/Mms/WMS_BN_BillMain/getnewbillno",
                    audit = "/api/Mms/WMS_BN_BillMain/audit/",
                    edit1 = "/Mms/WMS_BN_BillMain/edit/",
                    edit2 = "/Mms/WMS_BN_BillMain/customerEdit/?id=newid&type=" + id
                },
                resx = new
                {
                    detailTitle = uc.Text + "明细",
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
                    ContractCode = "",
                    ProjectName = "",
                    DepartmentID = department.DepartmentCode??"",
                    DepartmentName = department.DepartmentName ?? "",
                    SupplierCode = "",
                    SupplierName = "",
                    WarehouseCode = "",
                    WarehouseName = "",
                    ApproveState = 1
                },
                idField = "ID"
            };

            return View(model);
        }

        public ActionResult customerEdit(string id = "", string type = "")
        {
            uc = new sys_codeService().Getsys_codeByTypeAndID("BillType", int.Parse(type));
            ViewBag.uc = uc;
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            var user=new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", loginer.UserCode));
            //var department = new SYS_BN_UserService().GetDepartmentInfo(MmsHelper.GetUserCode());
            var department = new SYS_BN_DepartmentService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("DepartmentCode", user.DepartmentCode));
            SYS_BN_Warehouse warehouse = new SYS_BN_Warehouse();
            if (department.DepartmentCode=="0107")
            {
                warehouse = new SYS_BN_WarehouseService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
            }
            else
            {
                warehouse = new SYS_BN_WarehouseService().GetModel(ParamQuery.Instance().AndWhere("ISEnable", 1).AndWhere("WarehouseName", department.DepartmentName));
            }
            //var department=new SYS_BN_DepartmentService().GetIDNameList(MmsHelper.GetUserCode())
            
            
            
            warehouse = warehouse ?? new SYS_BN_Warehouse();
            ViewBag.user = warehouse.WarehouseCode == null ? 0 : 1;
           
            var data = new WMS_BN_BillDetailApiController().GetPageData(id);
            string billCode = new WMS_BN_BillMainService().GetBillCodeByID(id);
            //string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", uc.Description, "", "");
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/WMS_BN_BillDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/WMS_BN_BillDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/WMS_BN_BillDetail/audit/",                    //审核api
                    newkey = "/api/Mms/WMS_BN_BillDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/WMS_BN_BillDetail/PostStorage/"
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

                    DepartmentID = department.DepartmentCode,
                    DepartmentName = department.DepartmentName,
                    WarehouseCode = warehouse.WarehouseCode,
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new WMS_BN_BillMain().Extend(new { ID = data.scrollKeys.current, BillCode = data.form == null ? "系统生成" : data.form.BillCode, BillType = uc.Value, ContractCode = "", DepartmentID = "", DepartmentName = "", SupplierCode = "", WarehouseCode = warehouse.WarehouseCode, WarehouseName = warehouse.WarehouseName, ApproveState = "1", ApprovePerson = "", ApproveDate = "", ApproveRemark = "", Remark = "", CreatePerson = MmsHelper.GetUserName(), CreateTime = DateTime.Now, ModifyPerson = MmsHelper.GetUserName(), ModifyTime = DateTime.Now }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","BillCode","OrderBillCode","InventoryCode","InventoryName","Specs","Unit","MateNum","ConfirmNum","UnitPrice","TotalPrice","PackageCode","BatchCode","PBillCode","AccountabilityCode","Remark","CreatePerson","CreateTime","ModifyPerson","ModifyTime","IsEnable"},
                          defaults =new {ID = "",BillCode = billCode,InventoryCode = "",InventoryName = "",Unit = "",MateNum = "",ConfirmNum = "",PackageCode = "",BatchCode = "",PBillCode="",AccountabilityCode = "",Remark = "",CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = "",IsEnable=1}
                        }
}
            };
            return View("Edit", model);
        }

        //        public ActionResult Edit(string id = "")
        //        {
        //            var loginer = FormsAuth.GetUserData<LoginerBase>();
        //            //var department = new SYS_BN_UserService().GetDepartmentInfo(MmsHelper.GetUserCode());
        //            var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(loginer.UserCode);

        //            //if (warehouse == "0")
        //            //{
        //            //    return Redirect("Index");
        //            //}
        //            string billCode = new WMS_BN_BillMainService().GetBillCodeByID(id);
        //            string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", uc.Description, "", "");
        //            var model = new
        //            {
        //                urls = new
        //                {
        //                    getdata = "/api/Mms/WMS_BN_BillDetail/GetPageData/",        //获取主表数据及数据滚动数据api
        //                    edit = "/api/Mms/WMS_BN_BillDetail/edit/",                      //数据保存api
        //                    audit = "/api/Mms/WMS_BN_BillDetail/audit/",                    //审核api
        //                    newkey = "/api/Mms/WMS_BN_BillDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
        //                    storageSave = "/api/Mms/WMS_BN_BillDetail/PostStorage/"
        //                },
        //                resx = new
        //                {
        //                    rejected = "已撤消修改！",
        //                    editSuccess = "保存成功！",
        //                    auditPassed = "单据已通过审核！",
        //                    auditReject = "单据已取消审核！"
        //                },
        //                dataSource = new
        //                {
        //                    WarehouseCode = warehouse.WarehouseCode,
        //                    pageData = new WMS_BN_BillDetailApiController().GetPageData(id)
        //                    //payKinds = codeService.GetValueTextListByType("PayType")
        //                },
        //                form = new
        //                {
        //                    defaults = new WMS_BN_BillMain().Extend(new { ID = "", BillCode = billCode == string.Empty ? documentNo : billCode, BillType = uc.Value, ContractCode = "", DepartmentID = "", DepartmentName = "", SupplierCode = "", WarehouseCode = warehouse.WarehouseCode, WarehousName = warehouse.WarehousName, ApproveState = "1", ApprovePerson = MmsHelper.GetUserName(), ApproveDate = DateTime.Now, ApproveRemark = "", Remark = "" }),
        //                    primaryKeys = new string[] { "ID" }
        //                },
        //                tabs = new object[]{
        //                        new{
        //                          type = "grid",
        //                          rowId = "ID",
        //                          relationId = "ID",
        //                          postFields = new string[] { "ID","BillCode","OrderBillCode","InventoryCode","InventoryName","Unit","MateNum","ConfirmNum","PackageCode","BatchCode","PBillCode","AccountabilityCode","Remark","CreatePerson","CreateTime","ModifyPerson","ModifyTime","IsEnable"},
        //                          defaults = new {ID = "",BillCode = billCode,InventoryCode = "",InventoryName = "",Unit = "",MateNum = "",ConfirmNum = "",PackageCode = "",BatchCode = "",PBillCode="",AccountabilityCode = "",Remark = "",CreatePerson = "",CreateTime = "",ModifyPerson = "",ModifyTime = ""}
        //                        }
        //}
        //            };
        //            return View(model);
        //        }

        public ActionResult SearchInWarehouseData()
        {
            return View();
        }

    }

    public class WMS_BN_BillMainApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>WMS_BN_BillMain</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillCode'		cp='like'></field>   
        <field name='ContractCode'		cp='equal'></field>   
        <field name='DepartmentID'		cp='equal'></field>   
        <field name='SupplierCode'		cp='equal'></field>   
        <field name='WarehouseCode'		cp='equal'></field>   
        <field name='ApproveState'		cp='equal'></field>   
    </where>
</settings>");
            var service = new WMS_BN_BillMainService();
            //if (WMS_BN_BillMainController.uc.Value =="8")
            //{
            //var loginer = FormsAuth.GetUserData<LoginerBase>();
            //var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(loginer.UserCode);
            //var pQuery1 = query.ToParamQuery().AndWhere("ApproveState", 1).AndWhere("BillType","6").AndWhere("WarehouseCode", warehouse.WarehouseCode);
            //var result = service.GetDynamicListWithPaging(pQuery1);
            //return result;
            //}
            //else if (WMS_BN_BillMainController.uc.Value == "9")
            //{
            //    var loginer = FormsAuth.GetUserData<LoginerBase>();
            //    var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(loginer.UserCode);
            //    var pQuery1 = query.ToParamQuery().AndWhere("ApproveState", 1).AndWhere("BillType", "7").AndWhere("WarehouseCode", warehouse.WarehouseCode);
            //    var result = service.GetDynamicListWithPaging(pQuery1);
            //    return result;
            //}
            //else

            var loginer = FormsAuth.GetUserData<LoginerBase>();
            var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(loginer.UserCode);
            var pQuery = query.ToParamQuery().AndWhere("BillType", WMS_BN_BillMainController.uc.Value);
            //.AndWhere("WarehouseCode", warehouse.WarehouseCode);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;

        }

        public dynamic PostSetBillMainData(dynamic data)
        {
            return new WMS_BN_BillMainService().SetBillMainData(new WMS_BN_BillMain()
            {

            });
        }
    }

    public class WMS_BN_BillDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            //var department = new SYS_BN_UserService().GetDepartmentInfo(MmsHelper.GetUserCode());
            var warehouse = new SYS_BN_WarehouseService().GetWarehouseByCode(MmsHelper.GetUserCode());
            //if (warehouse == "0")
            //{
            //    MmsHelper.ThrowHttpExceptionWhen(true, "员工" + MmsHelper.GetUserName() + "不是仓储人员，无法进入出入库明细页面！！", 0);
            //}

            //MmsHelper.ThrowHttpExceptionWhen(warehouse == "0", "请注意，您不是仓储人员，无法进入出入库明细页面！", id);
            var masterService = new WMS_BN_BillMainService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var mainData = masterService.GetModel(pQuery);

            if (mainData != null)
            {
                var pQuery2 = ParamQuery.Instance().AndWhere("BillCode", mainData.BillCode);
                //mainData.WarehouseName = warehouse.WarehouseName;
                var result = new
                {
                    //主表数据
                    form = mainData,
                    scrollKeys = masterService.ScrollKeys("ID", id),
                    //明细数据
                    tab0 = new WMS_BN_BillDetailService().GetDynamicList(pQuery2)
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

        public dynamic GetBillDetail(string BillCode)
        {
            return new WMS_BN_BillDetailService().GetBillDetail(BillCode);
        }

        public dynamic GetIsExit(string BillCode)
        {
            return new WMS_BN_BillDetailService().GetIsExit(BillCode);
        }

        public dynamic GetSearchInWarehouseData(DateTime? storageInDate, string supplierName, string warehouseName, string storeKeeper)
        {
            using (var db = Db.Context("Mms"))
            {
                string where = string.Empty;

                if (storageInDate != null)
                {
                    string strBeginData = Convert.ToDateTime(storageInDate.ToString()).ToString("yyyy-MM-dd 00:00:00");
                    where += string.Format(@" and datepart(mm,'{0}') = datepart(mm,approveDate)", strBeginData);
                    where += string.Format(@" and datepart(yyyy,'{0}') = datepart(yyyy,approveDate)", strBeginData);
                }
                if (supplierName != null)
                {
                    supplierName = "%" + supplierName + "%";
                    where += string.Format(@" and t1.supplierName like '{0}'", supplierName);
                }
                if (warehouseName != null)
                {
                    warehouseName = "%" + warehouseName + "%";
                    where += string.Format(@" and t1.warehouseName like '{0}'", warehouseName);
                }
                if (storeKeeper != null)
                {
                    storeKeeper = "%" + storeKeeper + "%";
                    where += string.Format(@" and t3.storeKeeper like '{0}'", storeKeeper);
                }
                string sql1 = string.Format(@"select 
t1.BillCode,
t1.ContractCode,
t1.ProjectName,
t1.SupplierCode,
t1.SupplierName,
t1.WarehouseCode,
t1.WarehouseName,
t1.ApproveState,
t1.ApprovePerson,
t1.ApproveDate,
t2.InventoryCode,
t2.InventoryName,
t2.Specs,
t2.Unit,
t2.MateNum,
t2.ConfirmNum,
t2.UnitPrice,
t2.TotalPrice,
t2.BatchCode,
t3.UserCode,
t3.StoreKeeper
 from WMS_BN_BillMain t1 left join WMS_BN_BillDetail t2 
 on t1.BillCode=t2.BillCode 
  left join SYS_BN_Warehouse t3 on t1.WarehouseCode=t3.WarehouseCode
 where t1.IsEnable=1 and t2.IsEnable=1 
and BillType in (select value from HBHC_Sys.dbo.sys_code where text like '%入库%') 
 {0}", where);


                List<dynamic> list1 = db.Sql(sql1).QueryMany<dynamic>();

                return list1;
            }
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("WMS_BN_BillMain")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new WMS_BN_BillMainService();
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
                    var service0 = new WMS_BN_BillDetailService();
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
        WMS_BN_BillMain
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>WMS_BN_BillDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new WMS_BN_BillMainService();
            if (data.form["BillCode"] == "系统生成")
            {
                if (string.IsNullOrEmpty(WMS_BN_BillMainController.uc.Value))
                    return;
                string documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", WMS_BN_BillMainController.uc.Description, "", "");
                data.form["BillCode"] = documentNo;
                foreach (JToken tab in data["tabs"].Children())
                {
                    foreach (JProperty item in tab.Children())
                    {
                        if (item.Name == "inserted")
                        {
                            foreach (var row in item.Value.Children())
                            {
                                row["BillCode"] = documentNo;
                            }
                        }
                    }
                }
            }

            var result = service.EditPage(data, formWrapper, tabsWrapper);

        }
        public string PostStorage(dynamic data)
        {
            string msg = "";
            if (string.IsNullOrEmpty(WMS_BN_BillMainController.uc.Value))
            {
                return "数据错误！";
            }
            var result = new MmsService().WMS_OutAndIn(data["BillCode"].ToString(), WMS_BN_BillMainController.uc.Value, out msg);

            return msg;
        }
    }
}
