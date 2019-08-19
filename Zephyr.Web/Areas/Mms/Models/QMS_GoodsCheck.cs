using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_GoodsCheckService : ServiceBase<QMS_GoodsCheck>
    {
        //public List<dynamic> GetCheckItem(string InspectionTypeCode)
        //{
        //    string sql = String.Format(@"select * from QMS_BD_CheckItems where InspectionTypeCode='{0}' ", InspectionTypeCode);
        //    var list = db.Sql(sql).QueryMany<dynamic>();
        //    return list;
        //}

        public List<QMS_BD_CheckItems> GetCheckItem(string InspectionTypeCode)
        {
            List<QMS_BD_QualityCheckType> CheckTypeList = new List<QMS_BD_QualityCheckType>();
            string sql = String.Format(@"select * from QMS_BD_QualityCheckType where InspectionTypeCode='{0}'  ", InspectionTypeCode);
            var list = db.Sql(sql).QueryMany<QMS_BD_QualityCheckType>();
            while (list.Count != 0)
            {
                CheckTypeList.AddRange(list);
                var part = "(";
                foreach (var item in list)
                {
                    part += "'" + item.InspectionTypeCode + "',";
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                sql = String.Format(@"select * from QMS_BD_QualityCheckType where PInspectionType in {0}  ", part);
                list = db.Sql(sql).QueryMany<QMS_BD_QualityCheckType>();
            }
            //return CheckTypeList;
            List<QMS_BD_CheckItems> CheckItemList = new List<QMS_BD_CheckItems>();
            foreach(var item in CheckTypeList)
            {
                string sql1 = String.Format(@"select * from QMS_BD_CheckItems where InspectionTypeCode='{0}' ",item.InspectionTypeCode);
                var ItemList = db.Sql(sql1).QueryMany<QMS_BD_CheckItems>();
                CheckItemList.AddRange(ItemList);
            }
            return CheckItemList;
        }

    }

    public class QMS_GoodsCheck : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }

      
        public string PBillCode { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseID { get; set; }
        public string ProductionUnits { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        
        public DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// 报检数量
        /// </summary>
        public int? CheckQuantity { get; set; }
        /// <summary>
        /// 检验数量
        /// </summary>
        public int? InspectionQuantity { get; set; }
        public int? QualifiedQuantity { get; set; }

        public string Unit { get; set; }
        public string SalesmanCode { get; set; }
        public string Salesman { get; set; }
        public string DocName { get; set; }
        public int? BillState { get; set; }
        public string BatchCode { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string OuterFactoryBatch { get; set; }
    }
}
