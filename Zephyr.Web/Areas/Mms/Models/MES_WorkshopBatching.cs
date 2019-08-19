using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkshopBatchingService : ServiceBase<MES_WorkshopBatching>
    {

    }

    public class MES_WorkshopBatching : ModelBase
    {
        [PrimaryKey]
        public string BatchingCode { get; set; }
        public string RootPartCode { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public string WorkshopCode { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string PartCode { get; set; }
        public int? IsEnable { get; set; }
    }
}
