using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_WorkGroupDetailService : ServiceBase<SYS_WorkGroupDetail>
    {
       
    }

    public class SYS_WorkGroupDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
