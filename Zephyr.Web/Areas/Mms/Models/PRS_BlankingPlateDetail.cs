using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BlankingPlateDetailService : ServiceBase<PRS_BlankingPlateDetail>
    {
       
    }

    public class PRS_BlankingPlateDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string PlateSize { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public decimal? Weight { get; set; }
        public decimal? LineLength { get; set; }
        public int? BlankingType { get; set; }

        public string InventoryCode { get; set; }
    }
}
