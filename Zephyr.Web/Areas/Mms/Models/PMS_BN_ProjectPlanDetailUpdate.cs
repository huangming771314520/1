using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_ProjectPlanDetailUpdateService : ServiceBase<PMS_BN_ProjectPlanDetailUpdate>
    {
       
    }

    public class PMS_BN_ProjectPlanDetailUpdate : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public int ProjectDetailID { get; set; }
        public int PlanType { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime UpdatedPlanDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime UpdatedFinishDate { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
