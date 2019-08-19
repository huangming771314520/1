using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_MaterialReturnMainService : ServiceBase<MES_MaterialReturnMain>
    {
       
    }

    public class MES_MaterialReturnMain : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }

        public string BillCode { get; set; }
        public string PickBillCode { get; set; }
        public int? BillState { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
