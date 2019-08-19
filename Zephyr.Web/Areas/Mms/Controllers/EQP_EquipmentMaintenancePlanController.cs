
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
    public class EQP_EquipmentMaintenancePlanController : Controller
    {
        public static sys_code uc = new sys_code();
    public ActionResult Index(int id=0)
    {
    var loginer = FormsAuth.GetUserData<LoginerBase>();
    uc = new sys_codeService().Getsys_codeByTypeAndID("MaintenanceType", id);
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/EQP_EquipmentMaintenancePlan",
    newkey = "/api/Mms/EQP_EquipmentMaintenancePlan/getnewkey",
    edit = "/api/Mms/EQP_EquipmentMaintenancePlan/edit"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            PlanedStartTime = "" ,
            EquipmentMaintenanceID = "" ,
            EquipmentCode = "" ,
            EquipmentName="",
            MaintenanceType = id,
            MaintenanceName = uc.Text,
            //MaintenanceMan = loginer.UserName,
    },
    defaultRow = new {
        MaintenanceState=1,
        MaintenanceType=id,
        MaintenanceName = uc.Text,
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "MaintenancePlanCode", "EquipmentMaintenanceID", "EquipmentCode", "EquipmentName", "MaintenanceType", "MaintenanceName", "PlanedStartTime", "PlanedFinishTime", "ActualStartTime", "ActualFinishTime", "PlanedContent", "ActualContent", "MaintenanceMan", "MaintenanceState", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class EQP_EquipmentMaintenancePlanApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>EQP_EquipmentMaintenancePlan</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='PlanedStartTime' 		 cp='daterange'></field>
                <field name='EquipmentMaintenanceID' 		 cp='equal'></field>
                <field name='EquipmentCode' 		 cp='equal'></field>
                <field name='MaintenanceType' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_EquipmentMaintenancePlanService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new EQP_EquipmentMaintenancePlanService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }



    public string GetMaintenancePlanCode()
    {
        string documentNo = MmsHelper.GetOrderNumber("EQP_EquipmentMaintenancePlan", "MaintenancePlanCode", "SBWH", "", "");
        return documentNo;
    }
  
    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            EQP_EquipmentMaintenancePlan
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_EquipmentMaintenancePlanService();
    var result = service.Edit(null, listWrapper, data);
    }
        public int GetIsExsitsPlan(string MaintenanceType,int? EquipmentMaintenanceID,string EquipmentCode ,DateTime startTime,DateTime endTime)
    {
        return new EQP_EquipmentMaintenancePlanService().IsExsitsPlan(MaintenanceType, EquipmentMaintenanceID, EquipmentCode, startTime, endTime);
    }

        public int GetIsExsitsWX(string EquipmentCode, int? MaintenanceType,int? Year)
    {
        return new EQP_EquipmentMaintenancePlanService().IsExsitsWX(EquipmentCode, MaintenanceType, Year);
    }
        public string PostChangePlanState(int id)
    {
        string msg = "";
        var result = new EQP_EquipmentMaintenancePlanService().ChangePlanState(id, out msg);
        return msg;
    }

        public string PostFinishPlanState(int id)
    {
        string msg = "";
        var result = new EQP_EquipmentMaintenancePlanService().FinishPlanState(id, out msg);
        return msg;
    }
    }
    }
