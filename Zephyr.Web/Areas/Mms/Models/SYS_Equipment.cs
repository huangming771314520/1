using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_EquipmentService : ServiceBase<SYS_Equipment>
    {
       
    }

    public class SYS_Equipment : ModelBase
    {
        
        [PrimaryKey]   
        public int ID { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string TeamCode { get; set; } 
        public string AffiliatedWorkshopID { get; set; }
        public string AffiliatedWorkshopName { get; set; }
        public string EquipmentParms { get; set; }
        public int? EquipmentState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ProcessType { get; set; }
    }
}
