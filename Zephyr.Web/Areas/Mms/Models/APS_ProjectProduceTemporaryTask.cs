using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProjectProduceTemporaryTaskService : ServiceBase<APS_ProjectProduceTemporaryTask>
    {
       
    }

    public class APS_ProjectProduceTemporaryTask : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string TemporaryTaskDec { get; set; }
        public string ContractCode { get; set; }
        public int? ProjectDetailID { get; set; }
        public int? ProductPlanMainID { get; set; }
        public string PartCode { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessLineCode { get; set; }
        public string WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public string WorkGroupID { get; set; }
        public string WorkGroupName { get; set; }
        public int? Quantity { get; set; }
        public int? ManHour { get; set; }
        public int? Unit { get; set; }
        public DateTime? EarliestStartTime { get; set; }
        public DateTime? LatestStartTime { get; set; }
        public DateTime? PlanedStartTime { get; set; }
        public DateTime? EarliestFinishTime { get; set; }
        public DateTime? LatestFinishTime { get; set; }
        public DateTime? PlanedFinishTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualFinishTime { get; set; }
        public DateTime? FloatingHour { get; set; }
        public string PlanColor { get; set; }
        public int? PlanState { get; set; }
        public int? PlanType { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string ApsCode { get; set; }
        public string BillCode { get; set; }
        public string LaunchWorkshopID { get; set; }
        public string LaunchWorkshopName { get; set; }
    }
}
