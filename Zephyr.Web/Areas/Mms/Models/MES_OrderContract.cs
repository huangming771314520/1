using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_OrderContractService : ServiceBase<MES_OrderContract>
    {
       
    }

    public class MES_OrderContract : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string OrderContractCode { get; set; }
        public string OrderContractName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public string DocName { get; set; }
    }
}
