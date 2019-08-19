using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_PurchaseOrderMainService : ServiceBase<MES_PurchaseOrderMain>
    {
       
    }

    public class MES_PurchaseOrderMain : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ProductPurchaseCode { get; set; }
        public string ContractCode { get; set; }
        //public string ProiectName { get; set; }
        public string PurchaseContract { get; set; }

        public DateTime? OrderDate { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string UserCode { get; set; }
        public string SaleMan { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        //public  int CheckedQuantity { get; set; }
    }

}
