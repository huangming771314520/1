
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class Mes_PurchaseProcessController : Controller
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
                    query = "/api/Mms/Mes_PurchaseProcess",
                    newkey = "/api/Mms/Mes_PurchaseProcess/getnewkey",
                    edit = "/api/Mms/Mes_PurchaseProcess/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = ""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    sostListFields = new string[] { "ID", "ContractCode", "ProductID", "UserCode", "SaleMan", "InventoryCode", "InventoryName", "Model", "MaterialCode", "Quantity", "ArrivalQuantity", "SupplierCode", "PurchaseCode", "PlanArrivelDate", "PrchaseDate", "PickMaterialDate", "FinishDate", "ActualFinishDate", "CurrentState", "UnqualityQuantity", "RectificationNumber", "Remark", "IsEnable", "InvoiceAmount", "InvoiceTime" }
                }
            };

            return View(model);
        }
        public ActionResult Edit(string id = "")
        {
            Mes_PurchaseProcess purchaseProcess = new Mes_PurchaseProcessService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", id));
            ViewBag.purchaseProcess = purchaseProcess;
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/Mes_PurchaseProcessDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/Mes_PurchaseProcessDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/Mes_PurchaseProcessDetail/audit/",                    //审核api
                    newkey = "/api/Mms/Mes_PurchaseProcessDetail/GetNewRowId/",            //获取新的明细数据的主键(日语叫采番)
                    storageSave = "/api/Mms/Mes_PurchaseProcessDetail/PostStorage/"      //审核单据
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
                    pageData = new Mes_PurchaseProcessDetailApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                /*
                 PickMaterialState     
                 ActualFinishState     
                 InvoiceState          
                 UnqualityQuantityState
                 RectificationState    
                 */
                form = new
                {
                    defaults = new Mes_PurchaseProcess().Extend(
                        new
                        {
                            ContractCode = "",
                            ProductID = "",
                            UserCode = "",
                            SaleMan = "",
                            InventoryCode = "",
                            InventoryName = "",
                            Model = "",
                            MaterialCode = "",
                            Quantity = "",
                            ArrivalQuantity = "",
                            SupplierCode = "",
                            PurchaseCode = "",
                            PlanArrivelDate = "",
                            PrchaseDate = "",
                            PickMaterialDate = "",
                            FinishDate = "",
                            ActualFinishDate = "",
                            InvoiceAmount = "",
                            InvoiceTime = "",
                            CurrentState = "",
                            UnqualityQuantity = "",
                            RectificationNumber = "",
                            Remark = "",
                            PickMaterialState = "",
                            ActualFinishState = "",
                            InvoiceState = "",
                            UnqualityQuantityState = "",
                            RectificationState = ""
                        }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "empty",
                          defaults = new {}
                        }
}
            };
            return View(model);
        }
    }

    public class Mes_PurchaseProcessApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>(SELECT t1.*,
       t2.SupplierName
FROM dbo.Mes_PurchaseProcess t1
    LEFT JOIN dbo.SYS_BN_Supplier t2
        ON t1.SupplierCode = t2.SupplierCode
           AND t2.IsEnable = 1) as tmp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new Mes_PurchaseProcessService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }
        public int GetDelete(string id)
        {
            return new Mes_PurchaseProcessService().GetDelete(id);
        }
        public string GetNewKey()
        {
            return new Mes_PurchaseProcessService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            (SELECT ID
      ,[ContractCode]
      ,[ProductID]
      ,[UserCode]
      ,[SaleMan]
      ,[InventoryCode]
      ,[InventoryName]
      ,[Model]
      ,[MaterialCode]
      ,[Quantity]
      ,[ArrivalQuantity]
      ,[SupplierCode]
      ,[PurchaseCode]
	  ,[SigningTime]
	  ,[InvoiceAmount]
      ,[InvoiceTime]
      ,[PlanArrivelDate]
      ,[PrchaseDate]
      ,[PickMaterialDate]
      ,[FinishDate]
      ,[ActualFinishDate]
      ,[CurrentState]
      ,[UnqualityQuantity]
      ,[RectificationNumber]
      ,[Remark]
      ,[IsEnable]
      ,[CreatePerson]
      ,[CreateTime]
      ,[ModifyPerson]
      ,[ModifyTime]
      ,PickMaterialState     
      ,ActualFinishState     
      ,InvoiceState          
      ,UnqualityQuantityState
      ,RectificationState    
  FROM [HBHC_Mes].[dbo].[Mes_PurchaseProcess]) as t
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new Mes_PurchaseProcessService();
            var result = service.Edit(null, listWrapper, data);
        }
    }

    public class Mes_PurchaseProcessDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var query = new RequestWrapper();
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>(SELECT t1.*,
       t2.SupplierName
FROM dbo.Mes_PurchaseProcess t1
    LEFT JOIN dbo.SYS_BN_Supplier t2
        ON t1.SupplierCode = t2.SupplierCode
           AND t2.IsEnable = 1) as tmp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
        </where>
    </settings>");
            //var service = new Mes_PurchaseProcessService();
            //var pQuery = query.ToParamQuery();
            //var result = service.GetModel(pQuery.AndWhere("IsEnable", 1).AndWhere("ID",id));
            //return result;

            var masterService = new Mes_PurchaseProcessService();
            var pQuery = query.ToParamQuery().AndWhere("ID", id).AndWhere("IsEnable", 1);

            var result = new
            {
                //主表数据
                form = masterService.GetDynamic(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
            };
            return result;
        }


        public string PostStorage(dynamic data)
        {
            string msg = "";

            var result = new Mes_PurchaseProcessService().AuditBillCode(data["mainID"].ToString(), out msg);

            return msg;
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        Mes_PurchaseProcess
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            data.form.CurrentState = 0;
            data.form.PickMaterialState = 0;
            data.form.ActualFinishState = 0;
            data.form.InvoiceState = 0;
            data.form.UnqualityQuantityState = 0;
            data.form.RectificationState = 0;
            var service = new Mes_PurchaseProcessService();
            if (data._changed == "true")
            {
                foreach (JProperty item in data.form.Children())
                {
                    //领料日期
                    if (item.Name == "PickMaterialDate")
                    {
                        data.form.CurrentState = 1;
                        data.form.PickMaterialState = 0;
                    }
                    //实际交货日期
                    if (item.Name == "ActualFinishDate"|| item.Name == "ArrivalQuantity")
                    {
                        data.form.CurrentState = 2;
                        data.form.ActualFinishState = 0;
                    }
                    //开票时间
                    if (item.Name == "InvoiceTime"|| item.Name == "InvoiceAmount")
                    {
                        data.form.CurrentState = 3;
                        data.form.InvoiceState = 0;
                    }
                    //PickMaterialState = "",
                    //        ActualFinishState = "",
                    //        InvoiceState = "",
                    //        UnqualityQuantityState = "",
                    //        RectificationState = ""
                    //不合格数量
                    if (item.Name == "UnqualityQuantity")
                    {
                        data.form.CurrentState = 4;
                        data.form.UnqualityQuantityState = 0;
                    }
                    //整改次数
                    if (item.Name == "RectificationNumber")
                    {
                        data.form.CurrentState = 5;
                        data.form.RectificationState = 0;
                    }
                }

            }

            var result = service.EditPage(data, formWrapper, tabsWrapper);



        }
    }
}
