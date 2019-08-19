using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_BN_ProductProcessRouteDetailService : ServiceBase<MES_BN_ProductProcessRouteDetail>
    {
       
    }

    public class MES_BN_ProductProcessRouteDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int ProcessRouteID { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public int ToolNum { get; set; }
        public int? ProcessType { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
