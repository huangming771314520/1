using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProduceAndMonthPlanService : ServiceBase<APS_ProduceAndMonthPlan>
    {
       
        
    }

    public class APS_ProduceAndMonthPlan : ModelBase
    {
        [PrimaryKey]   
        [Identity]
        public long ID { get; set; }
        public string ProducePlanCode { get; set; }
        public string MonthPlanCode { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
