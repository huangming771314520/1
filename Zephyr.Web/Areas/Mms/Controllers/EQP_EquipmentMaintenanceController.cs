
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
    public class EQP_EquipmentMaintenanceController : Controller
    {
        public static sys_code uc = new sys_code();
    public ActionResult Index(int id=0)
    {
    uc = new sys_codeService().Getsys_codeByTypeAndID("MaintenanceType", id);
    var model = new
    {
    dataSource = new{
   
    },
    urls = new{
    query = "/api/Mms/EQP_EquipmentMaintenance",
    newkey = "/api/Mms/EQP_EquipmentMaintenance/getnewkey",
    edit = "/api/Mms/EQP_EquipmentMaintenance/edit"
    },
    resx = new{

    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            EquipmentCode = "" ,
            WorkshopName = "",
            MaintenanceType=id
    },
    defaultRow = new {
        MaintenanceType=id,
        IsEnable=1,
    },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "MaintenanceCode", "EquipmentCode", "EquipmentName", "WorkshopID", "WorkshopName", "MaintenanceType", "MaintenanceName", "MaintenanceStandard", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class EQP_EquipmentMaintenanceApiController : ApiController
    {
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>EQP_EquipmentMaintenance</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='EquipmentCode' 		 cp='like'></field>
                <field name='WorkshopName' 		 cp='like'></field>
                <field name='MaintenanceType' 		 cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_EquipmentMaintenanceService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new EQP_EquipmentMaintenanceService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
    public void Edit(dynamic data)
    {
    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            EQP_EquipmentMaintenance
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
    var service = new EQP_EquipmentMaintenanceService();
    var result = service.Edit(null, listWrapper, data);
    }
    }
    }
