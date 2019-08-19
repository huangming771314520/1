using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class EQP_EquipmentMaintenanceService : ServiceBase<EQP_EquipmentMaintenance>
    {
       
    }

    public class EQP_EquipmentMaintenance : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }

        public string MaintenanceCode { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public int? WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public int? MaintenanceType { get; set; }
       
        public string MaintenanceStandard { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
