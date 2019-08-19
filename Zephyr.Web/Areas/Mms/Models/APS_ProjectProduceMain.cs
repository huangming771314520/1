using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProjectProduceMainService : ServiceBase<APS_ProjectProduceMain>
    {
       
    }

    public class APS_ProjectProduceMain : ModelBase
    {
        [PrimaryKey]   
        [Identity]
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public int? ContractQuantity { get; set; }
        public int? ProduceQuantity { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string DesignTaskCode { get; set; }
    }
}
