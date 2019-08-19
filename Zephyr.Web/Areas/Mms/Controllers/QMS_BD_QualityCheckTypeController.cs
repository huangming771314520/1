
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
    public class QMS_BD_QualityCheckTypeController : Controller
    {
    public ActionResult Index()
    {

    var code = new sys_codeService();
    var model = new
    {
        dataSource = new
        {
            pageData = new QMS_BD_QualityCheckTypeApiController().GetPageData(),
        },
   
    urls = new{
    query = "/api/Mms/QMS_BD_QualityCheckType",
    newkey = "/api/Mms/QMS_BD_QualityCheckType/getnewkey",
    edit = "/api/Mms/QMS_BD_QualityCheckType/edit",
    edit_toplist = "/api/Mms/QMS_BD_QualityCheckType/Edit_TopList"
    },
    resx = new{
    noneSelect = "请先选择一条数据！",
    editSuccess = "保存成功！",
    auditSuccess = "单据已审核！"
    },
    form = new{
            InspectionTypeCode = "" ,
            InspectionTypeName = "" ,
            PInspectionType="",
            PInspectionName="",
            defaults = new QMS_BD_CheckItems().Extend(new { }),
            primaryKeys = new string[] { "ID" }
    },
        defaultRow = new
        {
            PInspectionType="0",
            PInspectionName="顶级质检类型",
            IsEnable = 1
        },
        tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",

                          postFields = new string[] { "ID","CheckItemCode","CheckItemName","InspectionTypeCode","","Remark","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {
                          IsEnable=1,
                          }
                        },
                },
    setting = new{
    idField = "ID",
    postListFields = new string[] { "ID", "InspectionTypeCode", "InspectionTypeName", "PInspectionType", "PInspectionName", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
    }
    };

    return View(model);
    }
    }

    public class QMS_BD_QualityCheckTypeApiController : ApiController
    {
        public dynamic GetPageData(string id = "")
        {
            var pQuery = ParamQuery.Instance();
            if (id == "")
            {
                var result = new
                {
                    tab0 = "",
                };
                return result;
            }
            else
            {
                List<dynamic> data = new List<dynamic>();

                data = new QMS_BD_CheckItemsService().GetDynamicList(pQuery.AndWhere("InspectionTypeCode", id));
               
                var result = new
                {
                    rows = data,
                    total = data.Count
                };
                return result;
            }
        }
    public dynamic Get(RequestWrapper query)
    {
    query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>QMS_BD_QualityCheckType</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='InspectionTypeCode' 		 cp='like'></field>
                <field name='InspectionTypeName' 		 cp='like'></field>
        </where>
    </settings>");
    var service = new QMS_BD_QualityCheckTypeService();
    var pQuery = query.ToParamQuery();
    var result = service.GetDynamicListWithPaging(pQuery);
    return result;
    }

    public string GetNewKey()
    {
    return new QMS_BD_QualityCheckTypeService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
    }

    [System.Web.Http.HttpPost]
//    public void Edit(dynamic data)
//    {
//    var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
//    <settings>
//        <table>
//            QMS_BD_CheckItems
//        </table>
//        <where>
//            <field name='ID' cp='equal'></field>
//        </where>
//    </settings>");
//    var service = new QMS_BD_CheckItemsService();
//    var result = service.Edit(null, listWrapper, data);
//        //service.EditPage(data, null, tabsWrapper);
//    }


    public void Edit(dynamic data)
    {
        var tabsWrapper = new List<RequestWrapper>();
        tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>QMS_BD_CheckItems</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

        var service = new QMS_BD_CheckItemsService();
        var result = service.EditPage(data, null, tabsWrapper);
    }
    public void Edit_TopList(dynamic data)
    {
        var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        QMS_BD_QualityCheckType
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");
        var service = new QMS_BD_QualityCheckTypeService();
        var result = service.Edit(null, listWrapper, data);
      
    }
    public dynamic GetCheckItemTree(string code)
    {
        //return new SYS_BOMService().GetBoomTree(code);

        return new QMS_BD_CheckItemsService().GetCheckItemTree(code);
    }
    public dynamic GetInspectionTypeTree()
    {
        //return new SYS_BOMService().GetBoomTree(code);

        return new QMS_BD_QualityCheckTypeService().GetInspectionTypeTree();
    }
    public dynamic GetInspectionTypeByCode(string code)
    {
        //return new SYS_BOMService().GetBoomTree(code);

        return new QMS_BD_QualityCheckTypeService().GetInspectionTypeByCode(code);
    }
    }
    }
