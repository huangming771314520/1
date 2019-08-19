using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_RealStockService : ServiceBase<WMS_BN_RealStock>
    {
       
    }

    public class WMS_BN_RealStock : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public double? RealStock { get; set; }
        public double? TotalStock { get; set; }
        public double LockStock { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string BatchCode { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
