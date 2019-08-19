using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkingTicket_ExtendService : ServiceBase<MES_WorkingTicket_Extend>
    {
       
    }

    public class MES_WorkingTicket_Extend : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public long ID { get; set; }
        public string WorkTicketCode { get; set; }
        public string ApsCode { get; set; }
        public long? WorkStepsID { get; set; }
        public int? IsEnable { get; set; }
        public string WorkshopCode { get; set; }
        public string WorkshopName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualFinishTime { get; set; }
        public int? ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveTime { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string WorkStepsName { get; set; }
        public int? WorkStepsLineCode { get; set; }
        public string WorkStepsContent { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public string TurnTargetName { get; set; }
        public string TurnTargetCode { get; set; }
        public string PartCode { get; set; }
        public int? WorkQuantity { get; set; }

        public int? ProjectDetailID { get; set; }
        public string ContractCode { get; set; }
        public string ProcessLineCode { get; set; }
        public string Unit { get; set; }

        public string ApproveRemark { get; set; }
        public int? TicketLevel { get; set; }
        public int? TicketStatus { get; set; }
    }
}
