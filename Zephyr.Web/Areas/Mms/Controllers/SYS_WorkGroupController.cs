
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
    public class SYS_WorkGroupController : Controller
    {
        public ActionResult Index()
        {
            var user = MmsHelper.GetUserCode();
            var department = new SYS_BN_UserService().GetDepartment(user);
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/SYS_WorkGroup",
                    remove = "/api/Mms/SYS_WorkGroup/",
                    billno = "/api/Mms/SYS_WorkGroup/getnewbillno",
                    audit = "/api/Mms/SYS_WorkGroup/audit/",
                    edit1 = "/Mms/SYS_WorkGroup/edit/"
                },
                resx = new
                {
                    detailTitle = "班组管理明细",
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
                    TeamCode = "",
                    DepartID = department.DepartmentCode,
                    DepartName = department.DepartmentName,
                    TeamName = ""
                },
                idField = "ID"
            };

            return View(model);
        }

        public ActionResult Edit(string id = "")
        {
            var data = new SYS_WorkGroupDetailApiController().GetPageData(id);
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }
            if (string.IsNullOrEmpty(code))
            {
                return JavaScript("请先维护所属车间!");
            }
            var model = new
            {
                urls = new
                {
                    getdata = "/api/Mms/SYS_WorkGroupDetail/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/SYS_WorkGroupDetail/edit/",                      //数据保存api
                    audit = "/api/Mms/SYS_WorkGroupDetail/audit/",                    //审核api
                    newkey = "/api/Mms/SYS_WorkGroupDetail/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    defaults = new SYS_WorkGroup().Extend(new { ID = id, TeamCode = data.form == null ? "系统生成" : data.form.TeamCode, TeamName = "", DepartID = code, DepartName = name, IsEnable = 1 }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","UserCode","UserName","IsEnable"},
                          defaults = new {ID = "",MainID = id,UserCode = "",UserName = "",IsEnable = "1"}
                        }
}
            };
            return View(model);
        }
    }

    public class SYS_WorkGroupApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new SYS_WorkGroupService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>SYS_WorkGroup</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='TeamCode'		cp='equal'></field>   
        <field name='TeamName'		cp='equal'></field>   
        <field name='DepartName'		cp='equal'></field>   
    </where>
</settings>");
            var service = new SYS_WorkGroupService();
            //dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            //if (depart == null)
            //{
            //    return null;
            //}
            //var pQuery = query.ToParamQuery().AndWhere("DepartName", depart.DepartmentName);
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }
    }

    public class SYS_WorkGroupDetailApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new SYS_WorkGroupService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);
            var pQuery2 = ParamQuery.Instance().AndWhere("MainID", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据
                tab0 = new SYS_WorkGroupDetailService().GetDynamicList(pQuery2)
            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("SYS_WorkGroup")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new SYS_WorkGroupService();
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
                    var service0 = new SYS_WorkGroupDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));
                default:
                    return "";
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            dynamic depart = new SYS_BN_UserService().GetDepartment(MmsHelper.GetUserCode());
            string code = string.Empty;
            string name = string.Empty;
            if (depart != null)
            {
                code = depart.DepartmentCode;
                name = depart.DepartmentName;
            }

            if (data.form["TeamCode"] == "系统生成")
            {
                if (data.form["TeamName"]=="")
                {
                     MmsHelper.ThrowHttpExceptionWhen(true, "班组名称不能为空！", 0);
                                return;
                }
                string documentNo = MmsHelper.GetOrderNumber("SYS_WorkGroup", "BillCode", code, "", "");
                data.form["BillCode"] = documentNo;
                documentNo = documentNo.Replace(DateTime.Now.ToString("yyMMdd")+"0","");
                data.form["TeamCode"] = documentNo;
                
            }
            foreach (JToken tab in data["tabs"].Children())
            {
                foreach (JProperty item in tab.Children())
                {
                    if(item.Name=="deleted")
                    {
                        continue;
                    }
                    foreach (var row in item.Value.Children())
                    {
                        var pQuery = ParamQuery.Instance().Select("ID").AndWhere("UserCode", row["UserCode"]).AndWhere("IsEnable", 1);
                        var re = new SYS_WorkGroupDetailService().GetModel(pQuery);
                        if (re != null)
                        {
                            if (re.ID > 0)
                            {
                                MmsHelper.ThrowHttpExceptionWhen(true, "员工" + row["UserName"] + "已在别的班组！", 0);
                                return;
                            }
                        }
                       
                    }
                }
            }
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        SYS_WorkGroup
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>SYS_WorkGroupDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new SYS_WorkGroupService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
        }
        public dynamic GetWorkGroupByTCode(string teamCode)
        {
            return new SYS_WorkGroupService().GetWorkGroupByTCode(teamCode);
        }
        public int GetDelete(string id)
        {
            return new SYS_WorkGroupService().GetDelete2(id);
        }
    }
}
