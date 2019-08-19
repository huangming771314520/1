using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_ProcessModelWorkStepsService : ServiceBase<PRS_ProcessModelWorkSteps>
    {
       
    }

    public class PRS_ProcessModelWorkSteps : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int? MainID { get; set; }
        public int? WorkStepsLineCode { get; set; }
        public string WorkStepsName { get; set; }
        public string WorkStepsContent { get; set; }
        public int? ManHours { get; set; }
        public int? IsEnable { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModifyPerson { get; set; }
        public int? Unit { get; set; }
    }
}
