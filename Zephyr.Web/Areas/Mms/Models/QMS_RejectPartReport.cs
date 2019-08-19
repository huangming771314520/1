using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_RejectPartReportService : ServiceBase<QMS_RejectPartReport>
    {
       
    }

    public class QMS_RejectPartReport : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string Model { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public int? RejectQuantity { get; set; }
        public string WorkTeamCode { get; set; }
        public string WorkTeamName { get; set; }
        public string TechCommand { get; set; }
        public string InspectionRecord { get; set; }
        public string LeaderRemark { get; set; }
        public string InspectorCode { get; set; }
        public string InspectorName { get; set; }
        public string ApproveState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
