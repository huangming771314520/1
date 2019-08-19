
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Sys.Controllers
{
    public class UserStatusController : Controller
    {
        public ActionResult Index()
        {
            var codeService = new sys_codeService();
            var model = new
            {
                urls = new {
                    query = "/api/Sys/UserStatus",
                    remove = "/api/Sys/UserStatus/",
                    billno = "/api/Sys/UserStatus/getnewbillno",
                    audit = "/api/Sys/UserStatus/audit/",
                    edit = "/Sys/UserStatus/edit/"
                },
                resx = new {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new{
                    status = codeService.GetValueTextListByType_Xttcqw("OnlineStatus")
                },
                form = new{
                    UserCode = "" ,
                    UserName = "" ,
                    UserStatus = ""
                },
                idField="ID"
            };

            return View(model);
        }
    }

    public class UserStatusApiController : ApiController
    {
        

        public string GetNewBillNo()
        {
            return new sys_userStatusService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ID'>
    <select>ID,UserCode,UserName
      ,CONVERT(varchar(50), LastHandleDate,120) as LastHandleDate 
      ,HostName,HostIP,LoginCity,UserStatus</select>
    <from>sys_userStatus</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='UserCode'		cp='like'></field>   
        <field name='UserName'		cp='like'></field>   
        <field name='UserStatus'		cp='equal'></field>   
    </where>
</settings>");
            var service = new sys_userStatusService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
    }
}
