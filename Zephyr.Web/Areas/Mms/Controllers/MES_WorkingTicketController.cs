
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_WorkingTicketController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var WorkshopCodeList = new SYS_WorkGroupService()
                .GetModelList(ParamQuery.Instance()
                .Select("DISTINCT b.DepartID")
                .From("SYS_WorkGroupDetail a LEFT JOIN SYS_WorkGroup b ON a.MainID=b.ID")
                .AndWhere("a.UserCode", MmsHelper.GetUserCode()));
            var WorkshopCode = "";

            var deptInfo = new SYS_BN_DepartmentService().GetDeptInfoByUCode(MmsHelper.GetUserCode());

            if (WorkshopCodeList.Count > 0)
            {
                WorkshopCode = WorkshopCodeList.FirstOrDefault().DepartID;
            }
            //ViewBag.WorkshopCode = WorkshopCode;
            ViewBag.WorkshopCode = deptInfo.DepartmentCode;
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/MES_WorkingTicket",
                    newkey = "/api/Mms/MES_WorkingTicket/getnewkey",
                    edit = "/api/Mms/MES_WorkingTicket/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WorkTicketCode = "",
                    ApsCode = "",
                    TeamName = "",
                    WorkshopName = deptInfo==null?"": deptInfo.DepartmentName,
                    ProcessName = "",
                    WorkStepsName = "",
                    EquipmentName = "",
                    ApproveState = ""
                },
                defaultRow = new
                {
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "WorkTicketCode", "TicketLevel", "ApsCode", "WorkStepsID", "IsEnable", "WorkshopCode", "WorkshopName", "TeamCode", "TeamName", "CreateTime", "CreatePerson", "ModifyTime", "ModifyPerson", "ActualStartTime", "ActualFinishTime", "ApproveState", "ApprovePerson", "ApproveTime", "ProcessCode", "ProcessName", "WorkStepsName", "WorkStepsLineCode", "WorkStepsContent", "EquipmentCode", "EquipmentName", "TurnTargetName", "TurnTargetCode", "PartCode", "WorkQuantity", "ApproveRemark" }
                }
            };

            return View(model);
        }
    }

    public class MES_WorkingTicketApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*,(select TOP 1 PartFigureCode from PRS_Process_BOM where PartCode=MES_WorkingTicket.PartCode) PartFigureCode</select>
        <from>MES_WorkingTicket</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='WorkTicketCode' 		 cp='like'></field>
                <field name='ApsCode' 		 cp='like'></field>
                <field name='TeamName' 		 cp='like'></field>
                <field name='WorkshopName' 		 cp='like'></field>
                <field name='ProcessName' 		 cp='like'></field>
                <field name='WorkStepsName' 		 cp='like'></field>
                <field name='EquipmentName' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new MES_WorkingTicketService();
            //string WorkshopCodeStr = "'nodata'";
            //var WorkshopCodeList = new SYS_WorkGroupService()
            //    .GetModelList(ParamQuery.Instance()
            //    .Select("DISTINCT b.DepartID")
            //    .From("SYS_WorkGroupDetail a LEFT JOIN SYS_WorkGroup b ON a.MainID=b.ID")
            //    .AndWhere("a.UserCode", MmsHelper.GetUserCode()));
            var user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
            //if (WorkshopCodeList.Count > 0)
            //{
            //    WorkshopCodeStr = "'" + string.Join("','", WorkshopCodeList.Select(a => a.DepartID)) + "'";
            //}
            var pQuery = query.ToParamQuery();
            //var result = service.GetDynamicListWithPaging(pQuery.AndWhere("WorkshopCode", user.DepartmentCode));
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new MES_WorkingTicketService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            string TurnTargetCode = "", TurnTargetName = "";
            var ApsList = new APS_ProjectProduceDetialService().GetModelList();
            dynamic insert_list = data.list.inserted;
            if (data.list.inserted.ToString() != "[]")
            {
                var PlanCode = MmsHelper.GetOrderNumber("MES_WorkingTicket", "WorkTicketCode", "ZYGP", "", "");
                string PreCode = PlanCode.Substring(0, PlanCode.Length - 3);
                int StartNumber = Convert.ToInt32(PlanCode.Substring(PlanCode.Length - 3));
                foreach (dynamic item in data.list.inserted)
                {
                    item["ApproveState"] = 1;
                    item["WorkTicketCode"] = PreCode + StartNumber.ToString().PadLeft(3, '0');
                    string ApsCode = item["ApsCode"];
                    var ApsItem = ApsList.Where(a => a.ApsCode == ApsCode).FirstOrDefault();
                    var TurnTargetModel = new WinFormClientService().GetTurnTarget(ApsItem.ID);
                    bool Result = TurnTargetModel.Result;
                    if (Result)
                    {
                        TurnTargetCode = TurnTargetModel.Data.ID;
                        TurnTargetName = TurnTargetModel.Data.Name;
                    }
                    item["TurnTargetCode"] = TurnTargetCode;
                    item["TurnTargetName"] = TurnTargetName;
                    StartNumber++;
                }
            }

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            MES_WorkingTicket
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_WorkingTicketService();
            var result = service.Edit(null, listWrapper, data);
        }

        public void PostWorkingTicketLower(dynamic data)
        {
            string WTList = data.WTList;
            new MES_WorkingTicketService()
                .Update(ParamUpdate.Instance()
                .Update("MES_WorkingTicket")
                .Column("ApproveState", 2)
                .Column("ApproveTime", DateTime.Now)
                .Column("ApprovePerson", MmsHelper.GetUserName())
                .AndWhere("ID", WTList, Cp.In));
        }

    }
}
