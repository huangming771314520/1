using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BN_InspectorQualificationService : ServiceBase<QMS_BN_InspectorQualification>
    {
       
    }

    public class QMS_BN_InspectorQualification : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string InspectorName { get; set; }
        public string QualificationCode { get; set; }
        public string CertificateCode { get; set; }
        public DateTime? EffectiveBegainDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModiftTime { get; set; }
    }
}
