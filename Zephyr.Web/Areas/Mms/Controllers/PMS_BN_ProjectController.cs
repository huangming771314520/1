
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
    public class PMS_BN_ProjectController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {
                    pageData = new PMS_BN_ProjectApiController().GetPageData(),
                },
                urls = new
                {
                    query = "/api/Mms/PMS_BN_Project",
                    newkey = "/api/Mms/PMS_BN_Project/getnewkey",
                    edit = "/api/Mms/PMS_BN_Project/edit",
                    edit_toplist = "/api/Mms/PMS_BN_Project/PostEdit" 
                    
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = "",
                    ProjectName = "",
                    //  defaults = new QMS_BN_InspectionEquipment().Extend(new { InspectionEquipmentCode = "", InspectionEquipmentName = "", InspectionEquipmenState = "", IsEnable = "" }),
                    //primaryKeys = new string[] { "ID" }
                    defaults = new PMS_BN_Project().Extend(new {}),
                    primaryKeys = new string[] { "ID" },
                    
                },

               
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","MainID","ProductName","ProductType","Model","Specifications","Quantity","FigureNumber","DeliveryTime","Remark","Urgent","Remark","ProjectState","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {}

                        }
                },
                setting = new
                {
                    idField = "ProjectID",
                    postListFields = new string[] { "ProjectID", "ContractCode", "ProjectName", "ProjectForShort", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
    }

    public class PMS_BN_ProjectApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ProjectID'>
        <select>*</select>
        <from>PMS_BN_Project</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='ProjectName' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

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
                data = new PMS_BN_ProjectDetailService().GetDynamicList(pQuery.AndWhere("MainID", id));
                var result = new
                {
                    rows = data,
                    total = data.Count
                };
                return result;
            }
        }

        public string GetNewRowId(string type, string key, int qty = 1)
        {

            var service0 = new PMS_BN_ProjectDetailService();
            return service0.GetNewKey("ID", "maxplus", qty, ParamQuery.Instance().AndWhere("ID", key, Cp.Equal));

        }

        public string GetNewKey()
        {
            return new PMS_BN_ProjectService().GetNewKey("ProjectID", "maxplus").PadLeft(6, '0'); ;

        }
        public string GetProjectState(string ID, string ProjectState)
        {
            string Sql2 = string.Format(@" select Top 1 ProjectState from PMS_BN_ProjectDetail where ID = '{0}'", ID);
            var db = Db.Context("MMs");
            string res1 = db.Sql(Sql2).QuerySingle<string>();
            if (res1 == "1")//如果是未启动状态
            {
                if (ProjectState == "2")//如果是未启动状态
                {
                    string Sql = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = '{1}' where ID = '{0}'", ID, ProjectState);
                    int res = db.Sql(Sql).Execute();
                    return "1";

                }
                else
                {
                    return "0";
                }
            }
            if (res1 == "2")//如果是启动状态
            {
                if (ProjectState == "2" || ProjectState == "4" || ProjectState == "5" || ProjectState == "6")
                {
                    return "0";
                }
                if (ProjectState == "3" )
                {
                    string Sql = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = '{1}' where ID = '{0}'", ID, ProjectState);
                    int res = db.Sql(Sql).Execute();
                    return "1";
                }
            }
            if (res1=="3")
            {
                if (ProjectState == "2" || ProjectState == "3" || ProjectState == "6")
                {
                    return "0";
                }
                if (ProjectState == "4" )
                {
                    string Sql = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = '{1}' where ID = '{0}'", ID, ProjectState);
                    int res = db.Sql(Sql).Execute();
                    return "1";
                }
                if (ProjectState == "5")
                {
                    string Sql = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = '2' where ID = '{0}'", ID);
                    int res = db.Sql(Sql).Execute();
                    return "1";
                }
            }
            if (res1 == "4")
            {
                if (ProjectState == "6")
                {
                    string Sql = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = '2' where ID = '{0}'", ID);
                    int res = db.Sql(Sql).Execute();
                    return "1";
                }
                else
                {
                    return "0";

                }
            }
            if (res1 == "5")
            {
                return "0";
            }

            return "";
        }
       

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {

            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>PMS_BN_ProjectDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            var service = new PMS_BN_ProjectDetailService();
            var result = service.EditPage(data, null, tabsWrapper);
        }

         [System.Web.Http.HttpPost]
        public void PostEdit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PMS_BN_Project
        </table>
        <where>
            <field name='ProjectID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectService();
            var result = service.Edit(null, listWrapper, data);
        }

        public dynamic GetProjectByCCode(string contractCode)
        {
            return new PMS_BN_ProjectService().GetProjectByCCode(contractCode);
        }
    }
}
