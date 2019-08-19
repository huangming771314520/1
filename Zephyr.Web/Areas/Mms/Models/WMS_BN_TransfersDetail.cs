using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_TransfersDetailService : ServiceBase<WMS_BN_TransfersDetail>
    {
       
    }

    public class WMS_BN_TransfersDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public int? MateNum { get; set; }
        public int? ConfirmNum { get; set; }
        public string TotalPrice { get; set; }
        public string UnitPrice { get; set; }
        public string BatchCode { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
