using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BN_InspectionEquipmentQualificationService : ServiceBase<QMS_BN_InspectionEquipmentQualification>
    {
       
    }

    public class QMS_BN_InspectionEquipmentQualification : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string InspectionEquipmentCode { get; set; }
        public string InspectionEquipmentName { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationName { get; set; }
        public string CertificateCode { get; set; }
        public DateTime? EffectiveBeginDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTiime { get; set; }
    }
}
