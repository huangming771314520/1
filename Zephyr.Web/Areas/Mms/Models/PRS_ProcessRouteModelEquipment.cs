using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_ProcessRouteModelEquipmentService : ServiceBase<PRS_ProcessRouteModelEquipment>
    {
       
    }

    public class PRS_ProcessRouteModelEquipment : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string EquipmentCode { get; set; }
        public string EquitmentType { get; set; }
        public string AffiliatedWorkshopID { get; set; }
        public string EquiptmentParms { get; set; }
        public string EquipmentState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string EquipmentName { get; set; }
        public string epqID { get; set; }
    }
}
