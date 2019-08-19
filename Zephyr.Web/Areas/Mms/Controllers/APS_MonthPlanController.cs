
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.CommonWrap;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Utils;
using System.Dynamic;

namespace Zephyr.Areas.Mms.Controllers
{
    public class APS_MonthPlanController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/APS_MonthPlan",
                    newkey = "/api/Mms/APS_MonthPlan/getnewkey",
                    edit = "/api/Mms/APS_MonthPlan/edit"
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
                    ProductName = ""
                },
                defaultRow = new
                {
                    PlanMonth = DateTime.Now.Month
                },
                setting = new
                {
                    idField = "PlanCode",
                    postListFields = new string[] { "PlanCode", "ContractCode", "ProjectName", "ProductID", "ProductName", "Model", "Specifications", "Quantity", "ProductiveValue", "Remark", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "PlanFinishTime", "PlanStartTime", "PlanMonth", "PlanType" }
                }
            };

            return View(model);
        }
    }

    public class APS_MonthPlanApiController : ApiController
    {
        public dynamic GetMonthPlanList(RequestWrapper data)
        {
            data.LoadSettingXmlString(@"
                <settings defaultOrderBy='ContractCode,ProductID'>
                <select>*</select>
                <from>PRS_Process_BOM</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                    <field name='ContractCode' cp='equal'></field>
                    <field name='ProductID' cp='equal'></field>
                </where>
                </settings>");
            var pQuery = data.ToParamQuery();
            var month_plan_list = new APS_MonthPlanService().GetDynamicList(pQuery.AndWhere("IsEnable", 1).AndWhere("ISNULL(ParentCode,'')", ""));
            return month_plan_list;
        }

        public dynamic GetMonthPlanRootPartCode(RequestWrapper data)
        {
            data.LoadSettingXmlString(@"
                <settings defaultOrderBy='ContractCode,ProductID'>
                <select>a.*,b.PartCode,b.PartName,b.PartFigureCode,b.PartQuantity</select>
                <from>dbo.APS_MonthPlan a JOIN dbo.PRS_Process_BOM b ON a.ContractCode=b.ContractCode AND a.ProductID=b.ProductID AND ISNULL(b.ParentCode,'')=''</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                    <field name='b.PartCode' cp='equal'></field>
                </where>
                </settings>");
            var pQuery = data.ToParamQuery();
            var month_plan_list = new APS_MonthPlanService().GetDynamicList(pQuery);
            return month_plan_list;
        }

        //public dynamic PostProductPlanRate(dynamic data)
        //{
        //    string ContractCode = data.ContractCode;
        //    int ProductID = data.ProductID;
        //    string PartCode = data.PartCode;
        //    int ProcessType = data.ProcessType;
        //    var Quantity = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("ID", ProductID)).Quantity ?? 0;
        //    var PartQuantity = new PRS_Process_BOMService().GetModel(ParamQuery.Instance().AndWhere("PartCode", PartCode)).PartQuantity ?? 0;
        //    var ProductTotal = Quantity * PartQuantity;//生产总数=产品数量*单台数量
        //    //已生产数量
        //    int? ProcuctList = new APS_ProjectProduceDetialService().GetField<int>(
        //        ParamQuery.Instance()
        //        .Select("SUM(Quantity) TotalQuantity")
        //        .From("APS_ProjectProduceDetial")
        //        .AndWhere("ContractCode", ContractCode)
        //        .AndWhere("ProjectDetailID", ProductID)
        //        .AndWhere("PartCode", PartCode)
        //        .AndWhere("ProcessModelType", 1));
        //    return "(" + (ProcuctList ?? 0).ToString() + "/" + ProductTotal.ToString() + ")";
        //}

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='PlanCode'>
        <select>*</select>
        <from>APS_MonthPlan</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='PlanCode' 		 cp='like'></field>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='ProductName' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new APS_MonthPlanService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public dynamic GetProductTreeList([FromUri]TreeNodeModel model)
        {
            var PMS_BN_ProjectDetailList = new PMS_BN_ProjectDetailService().GetModelList();
            var PRS_Process_BOMList = new PRS_Process_BOMService().GetModelList();
            var APS_ProjectProduceDetialList = new APS_ProjectProduceDetialService().GetModelList();

            var list = TreeNodeManage.GetTreeNodeList<dynamic>(
                  TreeNodeManage.Instance()
                  .SetNode(model.NodeField)
                  .SetParentNode(model.ParentNodeField, model.ParentNodeValue)
                  .SetTableName(model.TableName)
                  .SetNodeLevel(model.IsLevel)
                  .SetTreeSetting(model.TreeSetting)
                  .SetWhereSql(model.WhereSql));

            var new_list = new List<dynamic>();
            list.ForEach(item =>
            {
                dynamic item_old = item;

                string ContractCode = item_old.ContractCode;
                int ProductID = item_old.ProductID;
                string PartCode = item_old.PartCode;
                var Quantity = PMS_BN_ProjectDetailList.Where(a => a.ID == ProductID).FirstOrDefault().Quantity ?? 0;//合同台数
                var PartQuantity = PRS_Process_BOMList.Where(a => a.PartCode == PartCode && a.ContractCode == ContractCode && a.ProductID == ProductID).FirstOrDefault().PartQuantity ?? 0;//单台数量
                int ProductTotal = Quantity * PartQuantity;//生产总数=合同台数*单台数量                                  
                //已生产数量
                //var ProcuctList = APS_ProjectProduceDetialList
                //.Where(a => a.ContractCode == ContractCode && a.ProjectDetailID == ProductID && a.PartCode == PartCode)
                //.GroupBy(a => new { a.ProcessModelType, a.MonthPlanCode })
                //.Select(a => new { ProcessType = a.Key.ProcessModelType, a.Key.MonthPlanCode, Quantity = a.Max(b => b.Quantity) });
                var ProcuctList = APS_ProjectProduceDetialList
                .Where(a => a.ContractCode == ContractCode && a.ProjectDetailID == ProductID && a.PartCode == PartCode)
                .GroupBy(a => new { a.ProcessModelType, a.ProcessCode })
                .Select(a => new { ProcessType = a.Key.ProcessModelType, ProcessCode = a.Key.ProcessCode, Quantity = a.Sum(b => b.Quantity) });

                int BlankingTotal = ProcuctList.Where(a => a.ProcessType == "1").FirstOrDefault() == null ? 0 : ProcuctList.Where(a => a.ProcessType == "1").FirstOrDefault().Quantity ?? 0;
                int WeldingTotal = ProcuctList.Where(a => a.ProcessType == "2").FirstOrDefault() == null ? 0 : ProcuctList.Where(a => a.ProcessType == "2").FirstOrDefault().Quantity ?? 0;
                int MachiningTotal = ProcuctList.Where(a => a.ProcessType == "3").FirstOrDefault() == null ? 0 : ProcuctList.Where(a => a.ProcessType == "3").FirstOrDefault().Quantity ?? 0;
                int AssemblingTotal = ProcuctList.Where(a => a.ProcessType == "4").FirstOrDefault() == null ? 0 : ProcuctList.Where(a => a.ProcessType == "4").FirstOrDefault().Quantity ?? 0;

                item_old.BlankingTotal = BlankingTotal.ToString() + "/" + ProductTotal.ToString();
                item_old.WeldingTotal = WeldingTotal.ToString() + "/" + ProductTotal.ToString();
                item_old.MachiningTotal = MachiningTotal.ToString() + "/" + ProductTotal.ToString();
                item_old.AssemblingTotal = AssemblingTotal.ToString() + "/" + ProductTotal.ToString();

                new_list.Add(item_old);
            });
            return new_list;
        }

        /// <summary>
        /// 点击datagrid checkbox时判断是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public dynamic PostCurrentProductList(dynamic data)
        {
            var result = new { status = 0 };
            string ProductRate = data.ProductRate;
            string ContractCode = data.ContractCode;
            int ProductID = data.ProductID;
            string PlanCode = data.PlanCode;
            string ProcessType = data.ProcessType;
            string PartCode = data.PartCode;
            var PRS_Process_BOMList = new PRS_Process_BOMService().GetModelList();
            var PartQuantity = PRS_Process_BOMList.Where(a => a.PartCode == PartCode && a.ContractCode == ContractCode && a.ProductID == ProductID).FirstOrDefault().PartQuantity ?? 0;//单台数量
            var APS_ProjectProduceDetialList = new APS_ProjectProduceDetialService().GetModelList();
            var ProcuctList = APS_ProjectProduceDetialList
                .Where(a =>
                a.ContractCode == ContractCode &&
                a.ProjectDetailID == ProductID &&
                a.PartCode == PartCode &&
                a.ProcessModelType == ProcessType &&
                a.MonthPlanCode == PlanCode).Count();
            if (ProcuctList > 0)
            {
                result = new { status = 0 };
            }
            else
            {
                result = new { status = 1 };
            }
            return result;
        }

        public string GetNewKey()
        {
            return new APS_MonthPlanService().GetNewKey("PlanCode", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            dynamic insert_list = data.list.inserted;
            if (data.list.inserted.ToString() != "[]")
            {
                var PlanCode = MmsHelper.GetOrderNumber("APS_MonthPlan", "PlanCode", "YDJH", "", "");
                string PreCode = PlanCode.Substring(0, PlanCode.Length - 3);
                int StartNumber = Convert.ToInt32(PlanCode.Substring(PlanCode.Length - 3));
                foreach (dynamic item in data.list.inserted)
                {
                    item["PlanCode"] = PreCode + StartNumber.ToString().PadLeft(3, '0');
                    StartNumber++;
                }
            }

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            APS_MonthPlan
        </table>
        <where>
            <field name='PlanCode' cp='equal'></field>
        </where>
    </settings>");
            var service = new APS_MonthPlanService();
            var result = service.Edit(null, listWrapper, data);
        }

        //生产计划弹窗点【确定】
        public dynamic PostProductPlanList(dynamic data)
        {
            List<dynamic> plan_list = data.ToObject<List<dynamic>>();
            var ProductPlanList = new APS_MonthPlanService().GetProductPlanList(plan_list);
            return new { list = ProductPlanList };
        }

        public dynamic PostProductPlanList_Blanking(dynamic data)
        {
            var ContractCode = data[0].ContractCode;
            var SavantCode = data[0].SavantCode;
            List<APS_ProjectProduceDetial> IsExistSavant = new APS_ProjectProduceDetialService().GetModelList(
                  ParamQuery.Instance()
                  .AndWhere("ContractCode", ContractCode).AndWhere("SavantCode", SavantCode));

            if (IsExistSavant.Count > 0)
            {
                return new { list = new List<dynamic>(), status = 2 };//已存在该中间件的计划
            }

            //中间件表
            List<dynamic> MES_SavantList = data.ToObject<List<dynamic>>();

            //工序表
            var ProcessRouteList =
                new MES_BN_ProductProcessRouteService()
                .GetDynamicList()
                .Where(p => p.IsEnable == 1 && p.ProcessModelType == "1")
                .ToList<dynamic>();
            var ProductPlanList =
                (from a in MES_SavantList
                 join p in ProcessRouteList
                 on new { PartCode = Convert.ToString(a.PartCode), ContractCode = Convert.ToString(a.ContractCode) } equals new { PartCode = Convert.ToString(p.PartCode), ContractCode = Convert.ToString(p.ContractCode) }
                 orderby p.ProcessModelType, p.ProcessLineCode
                 select new
                 {
                     ContractCode = a.ContractCode,
                     SavantCode = a.SavantCode,
                     //a.ContractCode,
                     a.ProductID,
                     p.ProcessModelType,
                     p.PartCode,
                     p.ProcessCode,
                     p.ProcessName,
                     a.PartFigureCode,
                     a.PartName,
                     a.MaterialCode,
                     Quantity = a.SpareMateNum,
                     //BomQty = a.PartQuantity,
                     PlanType = 1,
                     p.ManHour,
                     p.WorkshopID,
                     p.WorkshopName,
                     p.EquipmentID,
                     p.EquipmentName,
                     p.WorkGroupID,
                     p.WorkGroupName
                 }).ToList<dynamic>();

            return new { list = ProductPlanList, status = 1 };
        }

        public dynamic PostCheckIsExist(dynamic data)
        {
            //List<dynamic> MonthPlanDetailList = data.MonthPlanDetailList.ToObject<List<dynamic>>();
            var ApsProduceAndMonthPlanList = new APS_ProduceAndMonthPlanService().GetModelList();
            var ApsProduceList = new APS_ProjectProduceDetialService().GetModelList();
            string PartCode = data.PartCode;
            var list = (from m in ApsProduceAndMonthPlanList
                        join p in ApsProduceList
                        on Convert.ToString(m.ProducePlanCode) equals Convert.ToString(p.ApsCode)
                        where p.PartCode == PartCode
                        select m.MonthPlanCode).Distinct().ToList();
            return list;
        }

        public dynamic GetIsExistMonthPlan(string ContractCode,int ProductID)
        {
            return new APS_MonthPlanService().GetModelList(ParamQuery.Instance()
                 .AndWhere("ContractCode", ContractCode)
                 .AndWhere("ProductID", ProductID)).Count() == 0 ? true : false;
        }
    }
}
