using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Areas.CommonWrap;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_MonthPlanService : ServiceBase<APS_MonthPlan>
    {
        public List<dynamic> GetProductPlanList(List<dynamic> plan_list)
        {
            string ContractCode = plan_list[0].ContractCode;
            int ProductID = plan_list[0].ProductID;
            string ProcessType = plan_list[0].ProcessType;
            string PartCode = plan_list[0].PartCode;

            var MonthPlanList = plan_list.GroupBy(
                a => new
                {
                    a.ContractCode,
                    a.ProductID,
                    a.PartCode,
                    a.PartName,
                    a.RootPartCode,
                    a.ProcessType,
                    a.PartFigureCode,
                    a.MaterialCode,
                    a.PartQuantity
                }).Select(
                a => a.Aggregate(
                    (current, next) => new
                    {
                        MonthPlanCode = Convert.ToString(current.MonthPlanCode) + "," + Convert.ToString(next.MonthPlanCode),
                        a.Key.ContractCode,
                        a.Key.ProductID,
                        a.Key.PartCode,
                        a.Key.PartName,
                        a.Key.RootPartCode,
                        a.Key.ProcessType,
                        a.Key.PartFigureCode,
                        a.Key.MaterialCode,
                        a.Key.PartQuantity,
                        Quantity = a.Sum(b => Convert.ToInt32(b.Quantity)),
                    }
                    )
                //a => new
                //{
                //    a.Key.ContractCode,
                //    a.Key.ProductID,
                //    a.Key.PartCode,
                //    a.Key.PartName,
                //    a.Key.RootPartCode,
                //    a.Key.ProcessType,
                //    a.Key.PartFigureCode,
                //    a.Key.MaterialCode,
                //    a.Key.PartQuantity,
                //    Quantity = a.Sum(b => Convert.ToInt32(b.Quantity)),
                //    MonthPlanCode = a.Aggregate((current, next) => Convert.ToString(current) + "," + Convert.ToString(next))
                //}
                )
                .ToList<dynamic>();

            //工序表
            var ProcessRouteList =
                new MES_BN_ProductProcessRouteService()
                .GetDynamicList()
                .Where(p => p.IsEnable == 1 && p.ContractCode == ContractCode && p.PartCode == PartCode && p.ProcessModelType == ProcessType)
                .ToList<dynamic>();


            var ProductPlanList =
                (from a in MonthPlanList
                 join p in ProcessRouteList
                 on Convert.ToString(a.PartCode) equals Convert.ToString(p.PartCode)
                 orderby p.ProcessModelType, p.ProcessLineCode
                 select new
                 {
                     a.ContractCode,
                     a.ProductID,
                     p.ProcessModelType,
                     p.PartCode,
                     a.RootPartCode,
                     a.MonthPlanCode,
                     p.ProcessCode,
                     p.ProcessName,
                     a.PartFigureCode,
                     a.PartName,
                     a.MaterialCode,
                     //Quantity = a.Quantity,
                     Quantity = Convert.ToInt32(a.PartQuantity) * Convert.ToInt32(a.Quantity),
                     BomQty = a.PartQuantity,
                     PlanType = 1,
                     p.ManHour,
                     p.WorkshopID,
                     p.WorkshopName,
                     p.EquipmentID,
                     p.EquipmentName,
                     p.WorkGroupID,
                     p.WorkGroupName
                 }).ToList<dynamic>();

            return ProductPlanList;
        }
    }

    public class APS_MonthPlan : ModelBase
    {
        [PrimaryKey]
        public string PlanCode { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public string ProjectName { get; set; }
        public decimal? ProductiveValue { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string Specifications { get; set; }
        public int? Quantity { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime? PlanStartTime { get; set; }
        public DateTime? PlanFinishTime { get; set; }
        public int? PlanMonth { get; set; }
        public int? PlanType { get; set; }
    }
}
