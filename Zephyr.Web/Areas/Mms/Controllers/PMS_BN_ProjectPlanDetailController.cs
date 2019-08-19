

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
    public class PMS_BN_ProjectPlanDetailController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {
                    dsPlanType = code.GetValueTextListByType("PlanType")
                },
                urls = new
                {
                    query = "/api/Mms/PMS_BN_ProjectPlanDetail",
                    newkey = "/api/Mms/PMS_BN_ProjectPlanDetail/getnewkey",
                    edit = "/api/Mms/PMS_BN_ProjectPlanDetail/edit"
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
                    PlanType = "",
                    IsEnable = "",
                    ProductName = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ProjectDetailID", "ContractCode", "PlanType", "PlanDate", "FinishDate", "ApprovalStatus", "IsEnable" }
                }
            };

            return View(model);
        }

        public ActionResult Index2()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/PMS_ProjectPlanSearch",
                },
                resx = new
                {
                   
                },
                form = new
                {
                    ContractCode = ""
                }
            };

            return View(model);
        }
    }




    public class PMS_BN_ProjectPlanDetailApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>* </select>
        <from>V_PMS_BN_ProjectPlanDetail </from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
 <field name='ProductName' 		 cp='like'></field>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='PlanType' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectPlanDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PMS_BN_ProjectPlanDetailService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public string GetProjectDetailID(string ID, string ProjectDetailID, string ContractCode)
        {
            string Sql2 = string.Format(@" select Top 1 ApprovalStatus from PMS_BN_ProjectPlanDetail where ProjectDetailID = '{0}' and ContractCode = '{1}'", ProjectDetailID, ContractCode);
            var db = Db.Context("MMs");
            string res1 = db.Sql(Sql2).QuerySingle<string>();
            if (res1 == "1")
            {
                return "0";
            }

            //            string Sql = string.Format(@"  
            //insert into PMS_BN_ProjectPlanDetailUpdate (ProjectDetailID,ContractCode,PlanType,PlanDate,FinishDate,IsEnable) 
            //select ProjectDetailID,ContractCode,PlanType,PlanDate,FinishDate,IsEnable from PMS_BN_ProjectPlanDetail where ProjectDetailID='{0}'
            //and ContractCode ='{1}'", ProjectDetailID, ContractCode);
            //            int res = db.Sql(Sql).Execute();
            string sql = string.Format(@"select count(1) from PMS_BN_ProjectPlanDetail 
 WHERE  ContractCode = '{0}' AND PlanDate IS NULL ", ContractCode);
            string res = db.Sql(sql).QuerySingle<string>();
            if (res == "0")
            {
                string Sql1 = string.Format(@"update PMS_BN_ProjectPlanDetail set ApprovalStatus = '1' where ProjectDetailID = '{0}' AND ContractCode = '{1}'", ProjectDetailID, ContractCode);
                int res2 = db.Sql(Sql1).Execute();
                return res2.ToString();
                //return "1";
            }
            else
            {
                return "2";
            }
            //string Sql1 = string.Format(@"update PMS_BN_ProjectPlanDetail set ApprovalStatus = '1' where ProjectDetailID = '{0}' AND ContractCode = '{1}'", ProjectDetailID, ContractCode);
            //res = db.Sql(Sql1).Execute();
            //return res.ToString();
        }

        public string GetContractCode(string ContractCode)
        {
            var db = Db.Context("MMs");
            //判断当前合同是否已经结束
            string sql = string.Format(@"select count(1) from PMS_BN_ProjectPlanDetail 
 WHERE  ContractCode = '{0}' AND FinishDate IS NULL  and IsEnable = '1'", ContractCode);
            string res1 = db.Sql(sql).QuerySingle<string>();
            //如果res1=0，此合同为结束状态，反之未结束
            if (res1 == "0")
            {
                return "0";
            }
            else
            {
                return "1";
            }

        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {

            if (data.list.updated.ToString() != "[]")
            {
                foreach (var item in data.list.updated)
                {
                    if (item.ApprovalStatus == "1")
                    {
                        string Sql2 = string.Format(@" select  * from PMS_BN_ProjectPlanDetail where ID = '{0}' ", item.ID);
                        var db = Db.Context("MMs");
                        var projectPlanDetail = db.Sql(Sql2).QuerySingle<dynamic>();

                        string sql1 = "insert into PMS_BN_ProjectPlanDetailUpdate (ProjectDetailID,ContractCode,PlanType,IsEnable ";
                        string args1 = "values('" + projectPlanDetail.ProjectDetailID + "','" + projectPlanDetail.ContractCode + "','" + projectPlanDetail.PlanType + "','" + projectPlanDetail.IsEnable + "'";

                        if (projectPlanDetail.PlanDate == null)
                        {
                        }
                        else
                        {
                            sql1 += ",PlanDate";
                            args1 += ",'" + projectPlanDetail.PlanDate + "'";
                        }
                        if (projectPlanDetail.FinishDate == null)
                        {
                        }
                        else
                        {
                            sql1 += ",FinishDate";
                            args1 += ",'" + projectPlanDetail.FinishDate + "'";
                        }
                        if (item.PlanDate == null || item.PlanDate == "")
                        {
                        }
                        else
                        {
                            sql1 += ",UpdatedPlanDate";
                            args1 += ",'" + item.PlanDate + "'";
                        }
                        if (item.FinishDate == null || item.FinishDate == "")
                        {
                        }
                        else
                        {
                            sql1 += ",UpdatedFinishDate";
                            args1 += ",'" + item.FinishDate + "'";
                        }
                        sql1 += ")";
                        args1 += ")";

                        string Sql = sql1 + args1;


                        int res = db.Sql(Sql).Execute();
                    }
                }


            }


            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PMS_BN_ProjectPlanDetail
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_BN_ProjectPlanDetailService();
            var result = service.Edit(null, listWrapper, data);
            if (((Newtonsoft.Json.Linq.JContainer)(data.list.updated)).Count != 0)
            {
                var db = Db.Context("MMs");
                string ProjectDetailID = data.list.updated[0].ProjectDetailID;
                string ContractCode = data.list.updated[0].ContractCode;
                string sql = string.Format(@"select count(1) from PMS_BN_ProjectPlanDetail WHERE ProjectDetailID = '{0}' AND ContractCode = '{1}' AND FinishDate IS NULL", ProjectDetailID, ContractCode);
                string res = db.Sql(sql).QuerySingle<string>();
                if (res == "0")
                {
                    String sql1 = string.Format(@"update PMS_BN_ProjectDetail set ProjectState = 5 where MainID = (select ProjectID from PMS_BN_Project
where ContractCode='{0}')", ContractCode);
                    db.Sql(sql1).Execute();
                }
            }
        }
    }

    public class PMS_ProjectPlanSearchApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            string contractCode = string.Empty;
            if (query["ContractCode"] != null)
            {
                contractCode = query["contractCode"].ToString();
            }
            return new PMS_BN_ProjectPlanDetailService().GetProjectPlan(contractCode);
        }
        public int GetIsExsitsPlan(string ContractCode, string ProjectDetailID)
        {
            return new PMS_BN_ProjectPlanDetailService().IsExsitsPlan(ContractCode, ProjectDetailID);
        }
        public string PostGetColumns()
        {
            List<dynamic> columns = new PMS_BN_ProjectPlanDetailService().GetColumns();
            string jaon = "[[";
            jaon += @"{#field#:#ContractCode#,#title#:#合同号#,#width#:80},
                   {#field#:#ProductName#,#title#:#产品名称#,#width#:80},
                    {#field#:#ProductType#,#title#:#产品类型#,#width#:80},
                    {#field#:#Model#,#title#:#型号#,#width#:80},
                    {#field#:#Specifications#,#title#:#规格#,#width#:80},";
            foreach (var item in columns)
            {
                jaon += "{#field#: #" + item.text + "考核日期" + "#,#title#: #" + item.text + "考核日期" + "#,#width#: " + (item.text.Length + 4) * 13 + ", #align#:#center#},";
                jaon += "{#field#: #" + item.text + "完成日期" + "#,#title#: #" + item.text + "完成日期" + "#,#width#: " + (item.text.Length + 4) * 13 + ", #align#:#center#},";
            }

            jaon = jaon.Remove(jaon.Length - 1, 1);
            jaon += "]]";
            return jaon;
        }
    }
}
