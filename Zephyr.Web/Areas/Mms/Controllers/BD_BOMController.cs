
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class BD_BOMController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/BD_BOM",
                    remove = "/api/Mms/BD_BOM/",
                    billno = "/api/Mms/BD_BOM/getnewbillno",
                    audit = "/api/Mms/BD_BOM/audit/",
                    edit1 = "/Mms/BD_BOM/edit/"
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
                    PartCode = "",
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
                    getdata = "/api/Mms/BD_BOM1/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/BD_BOM1/edit/",                      //数据保存api
                    audit = "/api/Mms/BD_BOM1/audit/",                    //审核api
                    newkey = "/api/Mms/BD_BOM1/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new BD_BOM1ApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new SYS_BOM().Extend(new { PartCode = "" }),
                    primaryKeys = new string[] { "ID" },

                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","InventoryCode","InventoryName","Model","Spec","Weight","Totalweight"},
                          defaults = new {ID = "",InventoryCode = "",InventoryName = "",Model = "",Spec = "",Weight = "",Totalweight = ""}
                        }
}
            };
            return View(model);
        }

        public ActionResult NewIndex()
        {


            return View();
        }

    }

    public class BD_BOMApiController : ApiController
    {
        public dynamic GetBOMsByPCode(string partCode, string sort = "", string order = "desc")
        {
            var data = new PRS_Process_BOMService().GetBOMsByPCode_Two(partCode);
            //if ((!string.IsNullOrEmpty(sort)) && sort.Equals("InventoryCode"))
            //{
            //    if (order.Equals("desc"))
            //    {
            //        data = data.OrderByDescending(item => item.InventoryCode).ToList();
            //    }
            //    else
            //    {
            //        data = data.OrderBy(item => item.InventoryCode).ToList();
            //    }
            //}
            return data;
        }

        public string GetNewBillNo()
        {
            return new SYS_BOMService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>SYS_BOM</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='PartCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new SYS_BOMService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public dynamic GetBoomTree2(string PartCode)
        {
            return new SYS_BOMService().GetBoomTree2(PartCode);
        }
    }
    public class BD_BOM1ApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new SYS_BOMService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var data = masterService.GetModel(pQuery);
            var pQuery2 = ParamQuery.Instance().AndWhere("InventoryName", data.PartName);
            var result = new
            {
                //主表数据
                form = data,
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new SYS_PartService().GetDynamicList(pQuery2)
            };
            return result;
        }
        public dynamic getPart(string partName)
        {
            //var pQuery2 = ParamQuery.Instance().AndWhere("InventoryName", partName);
            return new SYS_BOMService().getPart(partName);
        }
        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("SYS_BOM")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new SYS_BOMService();
            var rowsAffected = service.Update(pUpdate);
            MmsHelper.ThrowHttpExceptionWhen(rowsAffected < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }

        //todo 改成支持多个Tab
        // 地址：GET api/mms/@(controller)/getnewrowid 预取得新的明细表的行号
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            new SYS_BOMService().updatePart(data["partCode"].ToString(), data["pareName"].ToString(), data["id"].ToString(), data["weight"].ToString());
        }


        public List<dynamic> GetMaterialDetailByPartCode(string partCode)
        {
            return new SYS_BOMService().GetMaterialDetailByPartCode(partCode);
        }


    }
}
