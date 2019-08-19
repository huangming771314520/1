using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_BN_QualificationService : ServiceBase<QMS_BN_Qualification>
    {
       
    }

    public class QMS_BN_Qualification : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationName { get; set; }
        public int? IdentificationCycle { get; set; }
        public int? QualificationState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public DateTime? InspectionBeginDate { get; set; }

        public DateTime? InspectionFinishDate { get; set; }
    }
}
