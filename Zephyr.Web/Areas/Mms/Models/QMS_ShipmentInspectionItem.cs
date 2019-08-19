using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_ShipmentInspectionItemService : ServiceBase<QMS_ShipmentInspectionItem>
    {

    }

    public class QMS_ShipmentInspectionItem : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int? MainID { get; set; }
      
        public string InspectionItemCode { get; set; }
        public string InspectionItemName { get; set; }
        public string InspectionStandard { get; set; }
        public string ShipMentCheckItemCode { get; set; }
        public string CheckRecord { get; set; }
        public string InspectionCode { get; set; }
        public string Inspector { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
