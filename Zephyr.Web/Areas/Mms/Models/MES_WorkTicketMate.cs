using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkTicketMateService : ServiceBase<MES_WorkTicketMate>
    {
       
    }

    public class MES_WorkTicketMate : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public long ID { get; set; }
        public string WorkTicketCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public int? RequiredQuantity { get; set; }
        public int? TotalQuantity { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string WorkshopCode { get; set; }
        public string WorkshopName { get; set; }
        public string PartCode { get; set; }
        public string ParentCode { get; set; }
        public int? IsCrux { get; set; }
    }
}
