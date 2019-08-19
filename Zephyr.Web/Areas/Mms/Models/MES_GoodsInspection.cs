using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_GoodsInspectionService : ServiceBase<MES_GoodsInspection>
    {
       
    }

    public class MES_GoodsInspection : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string UserCode { get; set; }
        public string SaleMan { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
