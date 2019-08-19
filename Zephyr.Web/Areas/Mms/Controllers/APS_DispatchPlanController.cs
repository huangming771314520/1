using Newtonsoft.Json;
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
    [MvcMenuFilter(false)]
    public class APS_DispatchPlanController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_DispatchPlan",
                    newkey = "/api/Mms/APS_DispatchPlan/getnewkey",
                    edit = "/api/Mms/APS_DispatchPlan/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanCode = "",
                    ContractCode = "",
                    ProductID = "",
                    PlanQuantity = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "PlanCode", "TaskName", "EarliestStartTime", "EarliestFinishTime", "LatestStartTime", "LatestFinishTime", "PlanStartTime", "PlanFinishTime", "WorkHour", "FloatHour" }
                }
            };

            return View(model);
        }

        public ActionResult ChooseDispatchModel()
        {
            return View();
        }
    }

    public class APS_DispatchPlanApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='PlanCode'>
    <select>*</select>
    <from>APS_DispatchPlanDetail</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='PlanCode' cp='equal'></field>   
    </where>
</settings>");
            var service = new APS_DispatchPlanDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public dynamic GetModelType()
        {
            var service = new APS_BN_DispatchModelMainService();
            var pQuery = ParamQuery.Instance().Paging(1, 100).AndWhere("IsEnable", 1);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public dynamic GetModelItem(int MainID)
        {
            var service = new APS_BN_DispatchModelDetailService();
            var pQuery = ParamQuery.Instance().Paging(1, 100).AndWhere("MainID", MainID);
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }
        public int GetPlanedQuantity(string contractCode,string ProductID,string mainID)
        {
            //int MainID = data.MainID;
            var service = new APS_DispatchPlanMainService();
            
            return service.GetPlanedQuantity(contractCode, ProductID, mainID);
        }

        public dynamic PostCreateDispatchPlanList(dynamic data)
        {
            int DispatchModelCode = data.DispatchModelCode;
            string ContractCode = data.ContractCode;
            int ProductID = data.ProductID;
            int PlanQuantity = data.PlanQuantity;
            string PlanCode = MmsHelper.GetLSNumber("APS_DispatchPlanMain", "PlanCode", "DDJH", "", "");
            var plan_list = new List<APS_DispatchPlanDetail>();
            var model_list = new APS_BN_DispatchModelDetailService().GetModelList(ParamQuery.Instance().AndWhere("MainID", DispatchModelCode).OrderBy("ID"));
            int Seq = 1;
            for (int i = 0; i < PlanQuantity; i++)
            {
                model_list.ForEach(model =>
                {
                    APS_DispatchPlanDetail plan = new APS_DispatchPlanDetail();
                    plan.PlanCode = PlanCode;
                    plan.TaskName = model.Name;
                    plan.WorkHour = model.TaskCycle;
                    plan.Seq = Seq;
                    plan_list.Add(plan);
                    Seq++;
                });
            }
            return new
            {
                plan_list = plan_list
            };
        }

        public dynamic PostUpdateDispatchPlanList(dynamic data)
        {
            DateTime PlanStartTime = data.PlanStartTime;
            DateTime PlanFinishTime = data.PlanFinishTime;
            string ContractCode = data.ContractCode;
            string PlanCode = data.PlanCode;
            int ProductID = data.ProductID;
            int PlanQuantity = data.PlanQuantity;
            List<dynamic> DispatchPlanDetailList = data.rows.ToObject<List<dynamic>>();

            int TotalWorkHour = DispatchPlanDetailList.Sum(d => d.WorkHour);//调度计划总工时

            //更正开始时间
            int TotalMinute = PlanStartTime.Hour * 60 + PlanStartTime.Minute;
            int MinMinute = 8 * 60, MaxMinute = 17 * 60;
            if (TotalMinute < MinMinute)
            {
                PlanStartTime = Convert.ToDateTime(PlanStartTime.ToShortDateString() + " 08:00");
            }
            if (TotalMinute > MaxMinute)
            {
                PlanStartTime = Convert.ToDateTime(PlanStartTime.ToShortDateString() + " 17:00");
            }

            PlanFinishTime = Convert.ToDateTime(PlanFinishTime.ToString("yyyy-MM-dd hh:mm"));

            //正排：填充EarliestStartTime、EarliestFinishTime
            foreach (dynamic item in DispatchPlanDetailList)
            {
                double WorkHour = item.WorkHour;//工时定额
                var EarliestStartTime = PlanStartTime;
                var EarliestFinishTime = PlanStartTime.AddHours(WorkHour);
                item.EarliestStartTime = EarliestStartTime;
                item.EarliestFinishTime = EarliestFinishTime;
                PlanStartTime = PlanStartTime.AddHours(WorkHour);
            }
            //倒排：填充LatestStartTime、LatestFinishTime
            foreach (dynamic item in DispatchPlanDetailList.OrderByDescending(p => p.Seq))
            {
                double WorkHour = item.WorkHour;//工时定额
                item.LatestStartTime = PlanFinishTime.AddHours(-WorkHour);
                item.LatestFinishTime = PlanFinishTime;
                DateTime EarliestStartTime = item.EarliestStartTime;
                DateTime LatestStartTime = item.LatestStartTime;
                item.FloatHour = Convert.ToInt32((LatestStartTime - EarliestStartTime).TotalHours);
                PlanFinishTime = PlanFinishTime.AddHours(-WorkHour);
            }
            return new
            {
                plan_list = DispatchPlanDetailList
            };
        }

        public string GetNewKey()
        {
            return new APS_DispatchPlanDetailService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            APS_DispatchPlanDetail
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_DispatchPlanDetailService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}
