using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_DrawingApplicationService : ServiceBase<SYS_DrawingApplication>
    {
       
    }

    public class SYS_DrawingApplication : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public long ID { get; set; }
        public string RequestCode { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string RootPartCode { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string FigureCode { get; set; }
        public string ApplicationReason { get; set; }
        public int? RequestStatus { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
