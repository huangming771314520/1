using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BlankingResultService : ServiceBase<PRS_BlankingResult>
    {
       
    }

    public class PRS_BlankingResult : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string ResultSize { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string MdifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int PartBlankingQuntity { get; set; }
        public string SavantCode { get; set; }
    }
}
