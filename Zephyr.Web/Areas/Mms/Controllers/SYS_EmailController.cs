
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
using System.Linq;

namespace Zephyr.Areas.Mms.Controllers
{
    public class SYS_EmailController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/SYS_Email",
                    remove = "/api/Mms/SYS_Email/",
                    billno = "/api/Mms/SYS_Email/getnewbillno",
                    audit = "/api/Mms/SYS_Email/audit/",
                    edit1 = "/Mms/SYS_Email/edit/"
                },
                resx = new
                {
                    detailTitle = "发邮件",
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
                    EmailCode = "",
                    EmailName = "",
                    SendTime = ""
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
                    getdata = "/api/Mms/SYS_Email1/GetPageData/",        //获取主表数据及数据滚动数据api
                    edit = "/api/Mms/SYS_Email1/edit/",                      //数据保存api
                    audit = "/api/Mms/SYS_Email1/audit/",                    //审核api
                    newkey = "/api/Mms/SYS_Email1/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
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
                    pageData = new SYS_Email1ApiController().GetPageData(id)
                    //payKinds = codeService.GetValueTextListByType("PayType")
                },
                form = new
                {
                    defaults = new SYS_Email().Extend(new { ID = "", EmailCode = DateTime.Now.ToString("yyyyMMddHHmmss"), EmailName = "", Sender = "", CarbonCopy = "", Context = "", SendTime = DateTime.Now.ToString("yyyy-MM-dd"), Addressee = "", IsSend = "0" }),
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

    public class SYS_EmailApiController : ApiController
    {


        public string GetNewBillNo()
        {
            return new SYS_EmailService().GetNewKey("ID", "maxplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>*</select>
    <from>SYS_Email</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='EmailCode'		cp='like'></field>   
        <field name='EmailName'		cp='like'></field>   
        <field name='SendTime'		cp='equal'></field>   
    </where>
</settings>");
            var service = new SYS_EmailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }

    public class SYS_Email1ApiController : ApiController
    {
        public dynamic GetPageData(string id)
        {
            var masterService = new SYS_EmailService();
            var pQuery = ParamQuery.Instance().AndWhere("ID", id);

            var result = new
            {
                //主表数据
                form = masterService.GetModel(pQuery),
                scrollKeys = masterService.ScrollKeys("ID", id),

                //明细数据

            };
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Audit(string id, JObject data)
        {
            var pUpdate = ParamUpdate.Instance()
                .Update("SYS_Email")
                .Column("ApproveState", data["status"])
                .Column("ApproveRemark", data["comment"])
                .Column("ApprovePerson", FormsAuth.GetUserData().UserName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("ID", id);

            var service = new SYS_EmailService();
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
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        SYS_Email
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");

            var tabsWrapper = new List<RequestWrapper>();

            var service = new SYS_EmailService();
            var result = service.EditPage(data, formWrapper, tabsWrapper);
            string EmailCode = data.form.EmailCode;
            string sql = string.Format(@"update SYS_Email set IsSend=1 where EmailCode='{0}'", EmailCode);
            var db = Db.Context("MMs");
            db.Sql(sql).Execute();
        }
        public dynamic GetEmailTree()
        {
            //return new SYS_BOMService().GetBoomTree(code);

            return new SYS_EmailService().GetEmailTree();
        }
        public dynamic GetUserCode(string code)
        {
            //return new SYS_BOMService().GetBoomTree(code);

            return new SYS_EmailService().GetUserCode(code);
        }
    }
}
