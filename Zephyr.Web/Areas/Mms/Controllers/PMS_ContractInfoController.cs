
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Areas.Areas.Mms.Common;
using System.Linq;
using System.Web;
using Zephyr.Utils;

namespace Zephyr.Areas.Mms.Controllers
{
    public class PMS_ContractInfoController : Controller
    {
        public ActionResult Edit(string id = "")
        {

            //            var model = new
            //            {
            //                urls = new {
            //                    getdata = "/api/Mms/PMS_ContractInfo/GetPageData/",        //获取主表数据及数据滚动数据api
            //                    edit = "/api/Mms/PMS_ContractInfo/edit/",                      //数据保存api
            //                    audit = "/api/Mms/PMS_ContractInfo/audit/",                    //审核api
            //                    newkey = "/api/Mms/PMS_ContractInfo/GetNewRowId/"            //获取新的明细数据的主键(日语叫采番)
            //                },
            //                resx = new {
            //                    rejected = "已撤消修改！",
            //                    editSuccess = "保存成功！",
            //                    auditPassed ="单据已通过审核！",
            //                    auditReject = "单据已取消审核！"
            //                },
            //                dataSource = new{
            //                    pageData=new PMS_ContractInfoApiController().GetPageData(id)
            //                    //payKinds = codeService.GetValueTextListByType("PayType")
            //                },
            //                form = new{
            //                    defaults = new PMS_ContractInfo().Extend(new {  }),
            //                    primaryKeys = new string[]{"ID"}
            //                },
            //                tabs = new object[]{
            //                        new{
            //                          type = "form",
            //                          primaryKeys = new string[]{"ID"},
            //                          defaults = new {ID = "",ContractCode = "",ProjectName = "",ProjectForShort = "",AdvancePaymentDate = "",Is0tSartProduct = "",IsEnable = "",Remark = "",ProductReport = ""}
            //                        }
            //}
            //            };
            //            return View(model);
            var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("IsEnable", "1").OrderBy("ProjectID desc");
            var model = new PMS_BN_ProjectService().GetModel(pQuery);
            ViewData["cinfo"] = model;
            return View();
        }


        //合同信息管理
        public ActionResult ContractInfoIndex()
        {
            return View();
        }

        //产品信息管理
        public ActionResult ProductInfoIndex(int pID, string cCode, string pName, int pState = 0, int projectDetailID = 0, string sousid = "")
        {
            if (projectDetailID != 0)
            {
                ViewData["projectDetailID"] = projectDetailID;
            }
            ViewData["pID"] = pID;
            ViewData["cCode"] = cCode;
            ViewData["pState"] = pState;

            return View();
        }
        //任务信息管理
        //string pName, string pModel, string pQuantity, string pDeliveryTime, string pSpecifications
        public ActionResult TaskInfoIndex(string pID, string contractCode, string productID, string pName, string pModel, string pSpecifications, string DeliveryTime, string Quantity, int pState = 0)
        {
            ViewData["pID"] = pID;
            ViewData["projectDetailID"] = productID;
            ViewData["productID"] = productID;
            ViewData["contractCode"] = contractCode;
            ViewData["pName"] = pName;
            ViewData["pModel"] = pModel;
            ViewData["DeliveryTime"] = DeliveryTime;
            ViewData["Quantity"] = Quantity;
            ViewData["pSpecifications"] = pSpecifications;
            ViewData["pState"] = pState;
            return View();
        }

        //设计任务明细管理
        public ActionResult DesignTaskDetailedIndex(string pID, string contractCode, string productID, string pName, string pModel, string pSpecifications, string MainID, int id = 0, int pState = 0)
        {
            ViewData["pID"] = pID;
            ViewData["projectDetailID"] = productID;
            ViewData["contractCode"] = contractCode;
            if (id > 0)
            {
                var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("ID", id);
                var result = new PMS_DesignTaskDetailService().GetModel(pQuery);
                ViewData["DesignTask"] = result == null ? new PMS_DesignTaskDetail() : result;
            }
            else
            {
                var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("IsEnable", "1").AndWhere("ContractCode", contractCode).AndWhere("ProductID", productID).OrderBy("ID desc");
                var result = new PMS_DesignTaskDetailService().GetModel(pQuery);
                ViewData["DesignTask"] = result == null ? new PMS_DesignTaskDetail() : result;
            }

            ViewData["pName"] = pName;
            ViewData["pModel"] = pModel;
            ViewData["ProductID"] = productID;
            ViewData["pSpecifications"] = pSpecifications;
            ViewData["contractCode"] = contractCode;
            ViewData["MainID"] = MainID;
            ViewData["pState"] = pState;
            return View();
        }

        //生产任务明细管理
        public ActionResult ProductionTaskDetailedIndex(string pID, string contractCode, string productID, string pName, string pModel, string pSpecifications, string DeliveryTime, string Quantity, string MainID, int id = 0, int pState = 0)
        {
            ViewData["pID"] = pID;
            ViewData["projectDetailID"] = productID;
            ViewData["contractCode"] = contractCode;
            if (id > 0)
            {
                var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("ID", id);
                var result = new PMS_ProductPlanDetailService().GetModel(pQuery);
                ViewData["PMS_ProductPlanDetail"] = result == null ? new PMS_ProductPlanDetail() : result;
            }
            else
            {
                var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("IsEnable", "1").AndWhere("ContractCode", contractCode).AndWhere("ProductID", productID).AndWhere("MainID", MainID).OrderBy("ID desc");
                var result = new PMS_ProductPlanDetailService().GetModel(pQuery);
                ViewData["PMS_ProductPlanDetail"] = result == null ? new PMS_ProductPlanDetail() : result;
            }


            ViewData["pName"] = pName;
            ViewData["pModel"] = pModel;
            ViewData["ProductID"] = productID;
            ViewData["pSpecifications"] = pSpecifications;
            ViewData["contractCode"] = contractCode;
            ViewData["Quantity"] = Quantity;
            ViewData["DeliveryTime"] = DeliveryTime;
            ViewData["MainID"] = MainID;
            ViewData["pState"] = pState;
            return View();
        }

        //任务查询
        public ActionResult TaskSearchIndex()
        {
            var model = new
            {
                urls = new
                {

                },
                resx = new
                {

                },
                dataSource = new
                {
                    pageData = ""
                },
                form = new
                {
                    ContractCode = "",
                    PruductID = "",
                    IsEnable = 1
                },

                //    <th id="ID" sortable="tru
                //<th id="TaskType" sortabl
                //<th id="TaskDescription" 
                //<th id="TaskStartDate" so
                //<th id="TaskFinishDate" s
                //<th id="BillState" sortab
                //<th id="IsEnable" sortabl

                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",

                          postFields = new string[] { "ID","TaskType","TaskDescription","TaskStartDate","TaskFinishDate","BillState","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime"},
                          defaults = new {}
                        }
}
            };
            return View(model);
        }

        //合同信息查询
        public ActionResult ContractInfoSearchIndex()
        {
            return View();
        }

        //应交工程明细表
        public ActionResult ProjectDetailedIndex()
        {
            return View();
        }

        ////项目任务查询报表
        //public ActionResult ProjectTaskData()
        //{
        //    return View();
        //}

    }

    public class PMS_ContractInfoApiController : ApiController
    {
        //任务查询
        public dynamic GetTaskTree(string cCode)
        {
            return new PMS_BN_ProjectService().GetTaskTree(cCode);
        }

        ////任务报表查询方法
        //public dynamic GetTaskData(string projectName,DateTime? receiveTime)
        //{
        //    //return new SYS_BOMService().GetBoomTree(code);

        //    return new PMS_BN_ProjectService().GetTaskData(projectName, receiveTime);
        //}

        /// <summary>
        /// 合同信息页面保存/删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int PostOnSave(List<PMS_BN_Project> model)
        {
            List<string> strList = new PMS_BN_ProjectService().GetCnotractCode();
            if (model.Count < 0)
            {
                return 0;
            }
            else
            {
                int result = 0;
                if (model[0].ProjectID <= 0)
                {
                    //if (model[0].ContractCode)
                    bool isContain = strList.Contains(model[0].ContractCode);
                    if (!isContain)
                    {
                        model[0].CreateTime = DateTime.Now;
                        model[0].ModifyTime = model[0].CreateTime;
                        model[0].CreatePerson = MmsHelper.GetUserCode();
                        model[0].ModifyPerson = model[0].CreatePerson;
                        result = new PMS_BN_ProjectService().Insert(model[0]);
                    }
                    else
                    {
                        MmsHelper.ThrowHttpExceptionWhen(false, "合同编号重复，请确认数据！", 0);
                    }
                }
                else
                {
                    result = new PMS_BN_ProjectService().Update(model[0]);
                }
                return result;
            }
        }

        /// <summary>
        /// 删除项目信息及后续处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic PostOnDelete(List<PMS_BN_Project> model)
        {
            bool result = false;

            using (var db = Db.Context("Mms"))
            {
                db.UseTransaction(true);
                string sql1 = string.Format("update PMS_BN_ProjectDetail set IsEnable=0 where MainID={0} ", model[0].ProjectID);
                string sql2 = string.Format("update PMS_ProductTask set IsEnable=0 where ContractCode='{0}'", model[0].ContractCode);
                string sql3 = string.Format("update PMS_DesignTaskDetail set IsEnable=0 where ContractCode='{0}'", model[0].ContractCode);
                string sql4 = string.Format("update PMS_ProductPlanDetail set IsEnable=0 where ContractCode='{0}'", model[0].ContractCode);
                string sql5 = string.Format("update PMS_BN_Project set IsEnable=0 where ProjectID='{0}'", model[0].ProjectID);
                bool resProjectDetail = db.Sql(sql1).Execute() >= 0;
                bool resProductTask = db.Sql(sql2).Execute() >= 0;
                bool resDesignTaskDetail = db.Sql(sql3).Execute() >= 0;
                bool resProductPlanDetail = db.Sql(sql4).Execute() >= 0;
                bool resProject = db.Sql(sql5).Execute() >= 0;
                if (resProject)
                {
                    db.Commit();
                    return new
                    {
                        msg = "删除成功",
                        result
                    };
                }
                else
                {
                    db.Rollback();
                    return new
                    {
                        msg = "删除失败",
                        result
                    };
                }

            }


            //return result;


        }


        [System.Web.Http.HttpPost]
        public int PostUpdate(string ID, string newFileName, string oldFileName, string path, string tableName)
        {
            newFileName = newFileName + "." + oldFileName.Substring(oldFileName.LastIndexOf('.') + 1, oldFileName.Length - oldFileName.LastIndexOf('.') - 1);
            string filePath = HttpContext.Current.Server.MapPath("~/Upload/" + path) + newFileName;
            string InsertSql = string.Format(@"update " + tableName + " set FileAddress='{0}',FileName='{1}',DocName='{2}' where ProjectID ='{3}'", filePath, newFileName, oldFileName, ID);
            using (var db = Db.Context("Mms"))
            {
                return db.Sql(InsertSql).Execute();
            }
        }

        public dynamic GetProjectDetailData(int pID, int projectDetailID = 0)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = "";
                if (projectDetailID != 0)
                {
                    sql = string.Format(@"SELECT top 1 * FROM dbo.PMS_BN_ProjectDetail WHERE IsEnable = 1  and ID={0} order by id desc", projectDetailID);
                }
                else
                    sql = string.Format(@"SELECT top 1 * FROM dbo.PMS_BN_ProjectDetail WHERE IsEnable = 1 AND MainID = {0} order by id desc", pID);

                var result = db.Sql(sql).QueryMany<PMS_BN_ProjectDetail>();

                return new
                {
                    result = result.Count > 0,
                    data = result
                };
            }
        }
        public dynamic GetProjectDetailData2(int id)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = "";

                sql = string.Format(@"SELECT top 1 * FROM dbo.PMS_BN_ProjectDetail WHERE id = {0}", id);
                var result = db.Sql(sql).QueryMany<PMS_BN_ProjectDetail>();

                return new
                {
                    result = result.Count > 0,
                    data = result
                };
            }
        }
        public dynamic GetProductTaskDetailData(string productID, string pID, int dkdID = -1)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select * from PMS_DesignTaskDetail  t1 left join PMS_BN_ProjectDetail t2 on t1.ProductID =t2.ID
left join  PMS_BN_Project t3 on t1.ContractCode=t3.ContractCode WHERE t1.IsEnable = 1 AND t1.ProductID =  '{0}' and t1.IsEnable=1 AND t3.ContractCode = '{1}' order by t1.ID desc", productID, pID);

                var result = db.Sql(sql).QueryMany<dynamic>().Where(item => item.ID == dkdID).ToList();

                return new
                {
                    result = result.Count > 0,
                    data = result
                };
            }
        }


        //        public dynamic GetProductTaskDetailData(string contract,string pID)
        //        {
        //            using (var db = Db.Context("Mms"))
        //            {
        //                string sql = string.Format(@"    select * from PMS_ProductPlanDetail  t1 left join PMS_BN_ProjectDetail t2 on t1.ProductID =t2.ID
        //  left join  PMS_BN_Project t3 on t1.ContractCode=t3.ContractCode WHERE t1.IsEnable = 1 and t2.IsEnable=1 and t3.IsEnable=1 AND t1.ContractCode = '{0}' and t2.ProductID='{1}' order by t1.ID desc", contract,pID);

        //                var result = db.Sql(sql).QueryMany<dynamic>();

        //                return new
        //                {
        //                    result = result.Count > 0,
        //                    data = result
        //                };
        //            }
        //        }

        //public dynamic GetDesignTaskData(int pID)
        //{
        //    using (var db = Db.Context("Mms"))
        //    {
        //        string sql = string.Format(@"SELECT * FROM dbo.PMS_BN_ProjectDetail WHERE IsEnable = 1 AND MainID = {0}", pID);

        //        var result = db.Sql(sql).QueryMany<PMS_BN_ProjectDetail>();

        //        return new
        //        {
        //            result = result.Count > 0,
        //            data = result
        //        };
        //    }
        //}


        public dynamic GetProducTaskData(string contractCode, string productID)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select t1.*,t2.ProductName,t2.Model,t2.Specifications,t2.Quantity,t2.DeliveryTime 
     from PMS_ProductTask t1 left join PMS_BN_ProjectDetail t2 
     on t1.ProductID=t2.ID WHERE t1.IsEnable = 1 and t1.IsEnable = 1 AND t1.ContractCode = '{0}' and ProductID='{1}'", contractCode, productID);

                var result = db.Sql(sql).QueryMany<dynamic>();

                return result;

            }
        }

        [System.Web.Http.HttpPost]
        public dynamic PostProjectDetailOnSave(List<PMS_BN_ProjectDetail> model)
        {
            int result = 0;

            if (model.Count > 0)
            {
                if (model[0].ID <= 0)
                {
                    model[0].CreateTime = DateTime.Now;
                    model[0].ModifyTime = model[0].CreateTime;
                    model[0].CreatePerson = MmsHelper.GetUserCode();
                    model[0].ModifyPerson = model[0].CreatePerson;
                    result = new PMS_BN_ProjectDetailService().Insert(model[0]);
                }
                else
                {

                    result = new PMS_BN_ProjectDetailService().Update(model[0]);
                }
            }

            return new
            {
                result = result
            };
        }

        public dynamic PostProjectDetailOnDelete(List<PMS_BN_ProjectDetail> model)
        {
            bool result = false;
            model[0].ModifyTime = DateTime.Now;
            model[0].ModifyPerson = MmsHelper.GetUserName();
            using (var db = Db.Context("Mms"))
            {
                db.UseTransaction(true);
                string sql1 = string.Format("update PMS_BN_ProjectDetail set IsEnable=0 where ID={0} ", model[0].ID);
                string sql2 = string.Format("update PMS_ProductTask set IsEnable=0 where ProductID='{0}'", model[0].ID);
                string sql3 = string.Format("update PMS_DesignTaskDetail set IsEnable=0 where ProductID='{0}'", model[0].ID);
                string sql4 = string.Format("update PMS_ProductPlanDetail set IsEnable=0 where ProductID='{0}'", model[0].ID);
                bool resProjectDetail = db.Sql(sql1).Execute() >= 0;
                bool resProductTask = db.Sql(sql2).Execute() >= 0;
                bool resDesignTaskDetail = db.Sql(sql3).Execute() >= 0;
                bool resProductPlanDetail = db.Sql(sql4).Execute() >= 0;
                if (resProjectDetail)
                {
                    db.Commit();
                    return new
                    {
                        msg = "删除成功",
                        result
                    };
                }
                else
                {
                    db.Rollback();
                    return new
                    {
                        msg = "删除失败",
                        result
                    };
                }

            }

        }

        [System.Web.Http.HttpPost]
        public dynamic PostDesignDetailOnSave(List<PMS_DesignTaskDetail> model)
        {
            int result = 0;

            if (model.Count > 0)
            {
                if (model[0].ID <= 0)
                {
                    model[0].DesignTaskCode = MmsHelper.GetOrderNumber("PMS_DesignTaskDetail", "DesignTaskCode", "SJRW", "", "");
                    model[0].CreateTime = DateTime.Now;
                    model[0].ModifyTime = model[0].CreateTime;
                    model[0].CreatePerson = MmsHelper.GetUserCode();
                    model[0].ModifyPerson = model[0].CreatePerson;
                    result = new PMS_DesignTaskDetailService().Insert(model[0]);
                }
                else
                {

                    result = new PMS_DesignTaskDetailService().Update(model[0]);
                }
            }

            return new
            {
                result = result > 0
            };
        }
        [System.Web.Http.HttpPost]
        public dynamic PostProductionTaskDetailOnSave(List<PMS_ProductPlanDetail> model)
        {
            int result = 0;

            if (model.Count > 0)
            {
                if (model[0].ID <= 0)
                {
                    model[0].ProductPlanCode = MmsHelper.GetOrderNumber("PMS_ProductPlanDetail", "ProductPlanCode", "PCSC", "", "");
                    model[0].CreateTime = DateTime.Now;
                    model[0].ModifyTime = model[0].CreateTime;
                    model[0].CreatePerson = MmsHelper.GetUserCode();
                    model[0].ModifyPerson = model[0].CreatePerson;
                    result = new PMS_ProductPlanDetailService().Insert(model[0]);
                }
                else
                {

                    result = new PMS_ProductPlanDetailService().Update(model[0]);
                }
            }

            return new
            {
                result = result > 0
            };
        }
        [System.Web.Http.HttpPost]
        //
        public dynamic PostProductSaveData(List<PMS_ProductTask> model)
        {
            int result = 0;

            result = new PMS_ProductTaskService().SaveData(model);

            return result;
        }

        public dynamic PostProjectUpOrDownData(dynamic data)//(string table, int tID, string type, string whereSql = "")
        {
            string table = data["table"] == null ? null : data["table"].ToString();
            string whereSql = data["whereSql"] == null ? null : data["whereSql"].ToString();
            string type = data["type"] == null ? null : data["type"].ToString();
            int tID = data["tid"] == "" ? 0 : Convert.ToInt32(data["tid"]);

            using (var db = Db.Context("Mms"))
            {
                string sql = string.Empty;

                if (type.Equals("up"))
                {
                    sql = string.Format(@"SELECT * FROM " + table + " WHERE  IsEnable = 1 AND ID < {0} ", tID);
                    if (table == "PMS_BN_Project")
                    {
                        sql = string.Format(@"SELECT * FROM " + table + " WHERE  IsEnable = 1 AND ProjectID < {0} ", tID);
                    }
                    sql += whereSql;
                    if (table == "PMS_BN_Project")
                    {
                        sql += " ORDER BY ProjectID DESC ";
                    }
                    else
                        sql += " ORDER BY ID DESC";
                }
                if (type.Equals("down"))
                {
                    sql = string.Format(@"SELECT * FROM " + table + " WHERE  IsEnable = 1 AND ID > {0} ", tID);
                    if (table == "PMS_BN_Project")
                    {
                        sql = string.Format(@"SELECT * FROM " + table + " WHERE  IsEnable = 1 AND ProjectID > {0} ", tID);
                    }
                    sql += whereSql;
                    if (table == "PMS_BN_Project")
                    {
                        sql += " ORDER BY ProjectID asc";
                    }
                    else
                        sql += " ORDER BY ID asc";
                }

                return db.Sql(sql).QueryMany<dynamic>();
            }
        }


        public dynamic GetProjectByCCode(string contractCode)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"SELECT * FROM PMS_BN_Project WHERE  IsEnable = 1 AND ContractCode = '{0}'", contractCode);

                return db.Sql(sql).QueryMany<dynamic>();
            }
        }
        public dynamic GetProject(int projectID = 0)
        {
            if (projectID != 0)
            {
                var pQuery = ParamQuery.Instance().AndWhere("ProjectID", projectID);
                return new PMS_BN_ProjectService().GetModelList(pQuery);
            }
            else
            {
                var pQuery = ParamQuery.Instance().Select("top 1 *").AndWhere("IsEnable", "1").OrderBy("ProjectID desc");
                return new PMS_BN_ProjectService().GetModelList(pQuery);
            }
        }

        public dynamic GetDeleteProductTaskDetailData(int? id)
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Empty;

                sql = string.Format(@"update PMS_DesignTaskDetail set IsEnable=0 WHERE  ID = {0}", id);

                var result = db.Sql(sql).Execute();

                return new
                {
                    result = result > 0,
                    data = result
                };
            }
        }

        //合同详细信息报表

        [System.Web.Http.HttpGet]
        public dynamic GetContractInfoData(DateTime? beginDate, DateTime? endDate, int? isEnable)
        {
            using (var db = Db.Context("Mms"))
            {
                string where = string.Empty;

                if (beginDate != null && endDate != null)
                {
                    if (beginDate > endDate)
                    {
                        DateTime? temp = endDate;
                        endDate = beginDate;
                        beginDate = temp;
                    }

                    string strBeginData = Convert.ToDateTime(beginDate.ToString()).ToString("yyyy-MM-dd 00:00:00");
                    string strEndData = Convert.ToDateTime(endDate.ToString()).ToString("yyyy-MM-dd 23:59:59");

                    where += string.Format(@" and DeliveryTime between '{0}' and '{1}'", strBeginData, strEndData);
                }
                if (beginDate != null)
                {
                    where += string.Format(@" and DATEDIFF(day,DeliveryTime,'{0}') <= 0", beginDate);
                }
                if (endDate != null)
                {
                    where += string.Format(@" and DATEDIFF(day,DeliveryTime,'{0}') >= 0", endDate);
                }
                if (isEnable != null)
                {
                    where += string.Format(@" and B.IsEnable={0}", isEnable);
                }

                string sql = string.Format(@"
SELECT A.ContractCode, A.ProjectName, B.ProductUnit, C.ProductUnitName, B.ProductType
	, D.ProductTypeName, B.ProductName, B.Model, B.Specifications, B.Quantity
	, B.TotalWeight, B.PlanPrice, A.ContractReceiveTime, A.AdvancePaymentDate, B.DeliveryTime
	, B.IsEnable, B.CompleteQuantity, b.Remark
FROM [dbo].[PMS_BN_Project] A
	INNER JOIN [dbo].[PMS_BN_ProjectDetail] B
	ON A.ProjectID = B.MainID
		AND B.IsEnable = 1
	LEFT JOIN (
		SELECT [Value], [Text] AS ProductUnitName
		FROM [HBHC_Sys].[dbo].[sys_code]
		WHERE IsEnable = 1
			AND CodeType = 'ProductUnit'
	) C
	ON B.ProductUnit = C.[Value]
	LEFT JOIN (
		SELECT [Value], [Text] AS ProductTypeName
		FROM [HBHC_Sys].[dbo].[sys_code]
		WHERE IsEnable = 1
			AND CodeType = 'ProductType'
	) D
	ON B.ProductType = D.[Value]
WHERE A.IsEnable = 1 {0};
", where);

                var list = db.Sql(sql).QueryMany<dynamic>();
                foreach (var item in list)
                {
                    double quantity = string.IsNullOrEmpty(Convert.ToString(item.Quantity)) ? 0 : Convert.ToDouble(item.Quantity);
                    double completeQuantity = string.IsNullOrEmpty(Convert.ToString(item.CompleteQuantity)) ? 0 : Convert.ToDouble(item.CompleteQuantity);
                    double totalWeight = string.IsNullOrEmpty(Convert.ToString(item.TotalWeight)) ? 0 : Convert.ToDouble(item.TotalWeight);
                    double planPrice = string.IsNullOrEmpty(Convert.ToString(item.PlanPrice)) ? 0 : Convert.ToDouble(item.PlanPrice);


                    item.ActualQuantity = quantity - completeQuantity;
                    item.ActualWeight = (quantity - completeQuantity) * totalWeight;
                    item.ActualPlanPrice = (quantity - completeQuantity) * planPrice;
                    item.ContractReceiveTime = item.ContractReceiveTime == null ? "" : Convert.ToDateTime(item.ContractReceiveTime.ToString()).ToString("yyyy-MM-dd");
                    item.AdvancePaymentDate = item.AdvancePaymentDate == null ? "" : Convert.ToDateTime(item.AdvancePaymentDate.ToString()).ToString("yyyy-MM-dd");
                    item.DeliveryTime = item.DeliveryTime == null ? "" : Convert.ToDateTime(item.DeliveryTime.ToString()).ToString("yyyy-MM-dd");
                }

                return list;
            }
        }

        public dynamic GetValueAndText(string billType)
        {

            var data = new sys_codeService().GetValueTextListByType(billType);
            return data;
        }

        //应交工程明细报表
        [System.Web.Http.HttpGet]
        public dynamic GetProjectDetailedData(DateTime? beginDate, DateTime? endDate, int? pruductUnit)
        {
            using (var db = Db.Context("Mms"))
            {
                string where = string.Format(@" AND tbA.IsEnable = 1 ");
                string where1 = string.Empty;
                if (beginDate != null)
                {
                    string strBeginData = Convert.ToDateTime(beginDate.ToString()).ToString("yyyy-MM-dd 00:00:00");
                    where += string.Format(@" and datepart(mm,'{0}') = datepart(mm,getdate())", strBeginData);
                }
                if (pruductUnit != null)
                {
                    //string strBeginData = Convert.ToDateTime(beginDate.ToString()).ToString("yyyy-MM-dd 00:00:00");
                    where += string.Format(@" and tbA.ProductUnit={0}", pruductUnit);
                    where1 = string.Format(@" and tbA.ProductUnit={0}", pruductUnit);
                }
                //                string sql1 = string.Format(@"SELECT 
                //       A.IsEnable,
                //       C.ID,
                //       A.ProjectName,
                //       A.ContractCode,
                //       B.Model,
                //       B.Specifications,       
                //       B.TotalWeight,
                //       B.PlanPrice,
                //       B.Quantity totalQuantity ,
                //       A.AdvancePaymentDate,
                //       B.DeliveryTime,
                //       A.ContractReceiveTime CreateTime, 
                //       D.ProductQuantity Quantity,
                //       D.CompleteQuantity,      
                //       D.PlanFinishDate, 
                //       B.Remark
                //FROM [dbo].[PMS_BN_Project] A
                //    INNER JOIN [dbo].[PMS_BN_ProjectDetail] B
                //        ON A.ProjectID = B.MainID
                //           AND B.IsEnable = '1'
                //    INNER JOIN [dbo].[PMS_ProductTask] C
                //        ON B.ID = C.ProductID
                //           AND C.TaskType = '3'
                //           AND C.IsEnable = '1'
                //    INNER JOIN [dbo].[PMS_ProductPlanDetail] D
                //        ON C.ID = D.MainID
                //           AND D.IsEnable = '1'
                //WHERE (D.PlanFinishDate <= DATEADD(DAY, -1, DATEADD(MM, DATEDIFF(MM, 0, '{0}') + 1, 0))
                //      OR
                //      (
                //          D.CompleteQuantity < D.ProductQuantity
                //          AND D.PlanFinishDate < DATEADD(MM, DATEDIFF(MM, 0,'{0}'), 0)
                //      )) and B.ProductUnit='{1}'
                //      ", beginDate, pruductUnit);
                string sql1 = string.Format(@"SELECT 
      max( A.ProjectName)ProjectName,
      max(A.ContractCode)ContractCode,
      max(B.Model)Model,
      max(B.Specifications)Specifications,       
      max(B.TotalWeight)TotalWeight,
      max(B.PlanPrice)PlanPrice,
      max(B.Quantity) totalQuantity ,
      max(A.AdvancePaymentDate)AdvancePaymentDate,
      max(B.DeliveryTime)DeliveryTime,
      max(A.ContractReceiveTime) CreateTime, 
      max(D.ProductQuantity) Quantity,
      SUM(D.CompleteQuantity)CompleteQuantity,      
      max(D.PlanFinishDate)PlanFinishDate, 
      max(B.Remark)Remark
FROM [dbo].[PMS_BN_Project] A
    INNER JOIN [dbo].[PMS_BN_ProjectDetail] B
        ON A.ProjectID = B.MainID
           AND B.IsEnable = '1'
    INNER JOIN [dbo].[PMS_ProductTask] C
        ON B.ID = C.ProductID
           AND C.TaskType = '3'
           AND C.IsEnable = '1'
    INNER JOIN [dbo].[PMS_ProductPlanDetail] D
        ON C.ID = D.MainID
           AND D.IsEnable = '1' 
WHERE (D.PlanFinishDate <= DATEADD(DAY, -1, DATEADD(MM, DATEDIFF(MM, 0, '{0}') + 1, 0))
      OR
      (
          D.CompleteQuantity < D.ProductQuantity
          AND D.PlanFinishDate < DATEADD(MM, DATEDIFF(MM, 0,'{0}'), 0)
      )) and B.ProductUnit='{1}'
      group by B.ID
      ", beginDate, pruductUnit);

                List<GetContractInfModel> list1 = db.Sql(sql1).QueryMany<GetContractInfModel>();

                var data = list1.Select(item =>
                {
                    double quantity = string.IsNullOrEmpty(item.Quantity) ? 0 : Convert.ToDouble(item.Quantity);
                    double completeQuantity = string.IsNullOrEmpty(item.CompleteQuantity) ? 0 : Convert.ToDouble(item.CompleteQuantity);
                    double totalWeight = string.IsNullOrEmpty(item.TotalWeight) ? 0 : Convert.ToDouble(item.TotalWeight);
                    double planPrice = string.IsNullOrEmpty(item.PlanPrice) ? 0 : Convert.ToDouble(item.PlanPrice);

                    return new
                    {
                        ProjectName = item.ProjectName,
                        ContractCode = item.ContractCode,
                        Model = item.Model,
                        Specifications = item.Specifications,
                        TotalQuantity = item.TotalQuantity,
                        Quantity = item.Quantity,
                        CompleteQuantity = completeQuantity,
                        //ActualQuantity = quantity - completeQuantity,
                        //ActualWeight = (quantity - completeQuantity) * totalWeight,
                        //ActualPlanPrice = (quantity - completeQuantity) * planPrice,
                        CreateTime = item.CreateTime == null ? "" : Convert.ToDateTime(item.CreateTime.ToString()).ToString("yyyy-MM-dd"),
                        AdvancePaymentDate = item.AdvancePaymentDate == null ? "" : Convert.ToDateTime(item.AdvancePaymentDate.ToString()).ToString("yyyy-MM-dd"),
                        DeliveryTime = item.DeliveryTime == null ? "" : Convert.ToDateTime(item.DeliveryTime.ToString()).ToString("yyyy-MM-dd"),
                        PlanFinishDate = item.PlanFinishDate == null ? "" : Convert.ToDateTime(item.PlanFinishDate.ToString()).ToString("yyyy-MM-dd"),
                        Remark = item.Remark,
                        IsEnable = item.IsEnable
                    };
                }).ToList();

                return data;
            }
        }

        //设计任务详细查询
        public dynamic GetDesignDetailedData(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>PMS_DesignTaskDetail</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='IsEnable' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PMS_DesignTaskDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;

            //return null;
            //string where="";
            //if(string.IsNullOrEmpty(contractCode)){
            //    where+=string.Format(" and ContractCode='{0}'",contractCode);
            //}
            // if(string.IsNullOrEmpty(productID)){
            //    where+=string.Format(" and PrductID='{0}'",productID);
            //}

            //using (var db = Db.Context("Mms"))
            //{
            //    string sql = string.Format(@"select * from PMS_DesignTaskDetail where 1=1 {0}", where);
            //    dynamic res = db.Sql(sql).QueryMany<dynamic>();
            //    return res;
            //}
        }

        public class GetContractInfModel
        {
            public int ID { get; set; }
            //工程项目
            public string ProjectName { get; set; }
            //合同编号
            public string ContractCode { get; set; }
            //合同接受时间
            //public DateTime? ContractReceiveTime { get; set; }
            //型号
            public string Model { get; set; }
            //规格
            public string Specifications { get; set; }
            //批次数量
            public string Quantity { get; set; }
            //合同数量
            public string TotalQuantity { get; set; }
            //批次完成数量
            public string CompleteQuantity { get; set; }
            //单台重量
            public string TotalWeight { get; set; }
            //单台计划价
            public string PlanPrice { get; set; }

            public DateTime? CreateTime { get; set; }
            //到预付款时间
            public DateTime? AdvancePaymentDate { get; set; }
            //交货期
            public DateTime? DeliveryTime { get; set; }
            //批次计划完成日期
            public DateTime? PlanFinishDate { get; set; }
            //备注
            public string Remark { get; set; }
            public int? IsEnable { get; set; }

        }
        //合同信息文件
        public dynamic GetProcessRouteFile(string id)
        {
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("SourceID", id).AndWhere("FileType", 4); ;
            var re = new QMS_QualityReportDocService().GetModelList(pQuery);
            return re;
        }

        public dynamic GetOneKeyAuditResult(int pID)
        {
            using (var db = Db.Context("Mms"))
            {
                string sqlA = string.Format(@"select * from PMS_BN_Project where IsEnable = 1 and ProjectID = '{0}'", pID);
                var project = db.Sql(sqlA).QuerySingle<PMS_BN_Project>();

                if (project == null)
                {
                    return new
                    {
                        Result = false,
                        Msg = @"差不到该合同信息！"
                    };
                }
                else
                {
                    if (project.ProjectState != null && project.ProjectState.Equals("1"))
                    {
                        return new
                        {
                            Result = false,
                            Msg = @"已审核数据不能重复审核！"
                        };
                    }
                    else
                    {
                        string sqlB = string.Format(@"UPDATE dbo.PMS_BN_Project SET ProjectState = '1' WHERE IsEnable = 1 AND ProjectID = '{0}'", pID);
                        var tempB = db.Sql(sqlB).Execute();

                        if (tempB >= 0)
                        {
                            return new
                            {
                                Result = true
                            };
                        }
                        else
                        {
                            return new
                            {
                                Result = false,
                                Msg = @"审核失败！"
                            };
                        }
                    }
                }
            }


        }


    }
}
