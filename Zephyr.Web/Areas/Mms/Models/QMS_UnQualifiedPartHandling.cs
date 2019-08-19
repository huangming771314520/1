using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_UnQualifiedPartHandlingService : ServiceBase<QMS_UnQualifiedPartHandling>
    {
       
    }

    public class QMS_UnQualifiedPartHandling : ModelBase
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
        public int? RejectQuantity { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string RejectDescription { get; set; }
        public int? HandlingType { get; set; }
        public string AnalysisReason { get; set; }
        public string ApprovedPerson { get; set; }
        public DateTime? ApprovedTime { get; set; }
        //public DateTime? ApprovedState { get; set; }
        public int? HandlingResult { get; set; }
        public string InspectorCode { get; set; }
        public string InspectorName { get; set; }
        public int? ApproveState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
