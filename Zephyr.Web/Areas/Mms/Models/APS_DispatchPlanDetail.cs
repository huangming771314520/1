using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_DispatchPlanDetailService : ServiceBase<APS_DispatchPlanDetail>
    {
       
    }

    public class APS_DispatchPlanDetail : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string PlanCode { get; set; }
        public DateTime? EarliestStartTime { get; set; }
        public DateTime? EarliestFinishTime { get; set; }
        public DateTime? LatestStartTime { get; set; }
        public DateTime? LatestFinishTime { get; set; }
        public DateTime? PlanStartTime { get; set; }
        public DateTime? PlanFinishTime { get; set; }
        public int? FloatHour { get; set; }
        public int? WorkHour { get; set; }
        public int? Seq { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
