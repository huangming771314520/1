using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_BN_ProjectPartService : ServiceBase<PMS_BN_ProjectPart>
    {

    }

    public class PMS_BN_ProjectPart : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public int ProjectDetailID { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string PartCode { get; set; }
        public string VersionCode { get; set; }
        public string PartName { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
