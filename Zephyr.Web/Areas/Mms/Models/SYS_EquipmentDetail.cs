using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_EquipmentDetailService : ServiceBase<SYS_EquipmentDetail>
    {
       
    }

    public class SYS_EquipmentDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string MainID { get; set; }
        public int? ProcessType { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
