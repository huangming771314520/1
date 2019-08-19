using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_BN_DispatchModelMainService : ServiceBase<APS_BN_DispatchModelMain>
    {
       
    }

    public class APS_BN_DispatchModelMain : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
