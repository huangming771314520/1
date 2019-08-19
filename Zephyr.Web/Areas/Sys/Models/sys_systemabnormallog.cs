using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Sys")]
    public class sys_systemabnormallogService : ServiceBase<sys_systemabnormallog>
    {
       
    }

    public class sys_systemabnormallog : ModelBase
    {
        [PrimaryKey]   
        public string Object_ID { get; set; }
        [Identity]
        public int RowID { get; set; }
        public string UnabnormalReason { get; set; }
        public string UnabnormalDescription { get; set; }
        public string Remark { get; set; }
        public DateTime? Modify_Time { get; set; }
        public string Modify_Person { get; set; }
        public DateTime? Create_Time { get; set; }
        public string Create_Person { get; set; }
    }
}
